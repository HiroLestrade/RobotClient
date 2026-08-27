using ViperCore;

namespace RobotClient
{
    internal sealed class ViperAdapter : IRobotAdapter
    {
        private readonly IRobotHost _host;

        private ViperDevice?       _device;
        private JointController?   _controller;
        private BasicController?   _basicCtrl;
        private PolyTrajectory?    _polyTraj;

        private System.Threading.Timer? _encoderTimer;
        private volatile bool           _encoderPending;
        private System.Threading.Timer? _motionTimer;

        private const int    EncoderIntervalMs = 20;
        private const double Tf                = 3.0;

        // Fixed Dynamixel-degree positions for joints the UI does not control.
        // 0-360°, where 180° = motor center (neutral).
        private static readonly double[] FixedJointsDeg = [180.0, 180.0, 180.0, 300.0];

        // Converts radians (host convention) to Dynamixel degrees.
        private static double RadToDxlDeg(double rad) => rad * 180.0 / Math.PI + 180.0;

        // Converts Dynamixel degrees to radians (host convention).
        private static double DxlDegToRad(double deg) => (deg - 180.0) * Math.PI / 180.0;

        // Expands the 3-value UI target [rad] to the full 7-joint array [Dynamixel degrees].
        private static double[] FullQfDeg(double[] qUiRad)
        {
            double[] qf = new double[qUiRad.Length + FixedJointsDeg.Length];
            for (int i = 0; i < qUiRad.Length; i++)
                qf[i] = RadToDxlDeg(qUiRad[i]);
            Array.Copy(FixedJointsDeg, 0, qf, qUiRad.Length, FixedJointsDeg.Length);
            return qf;
        }

        public ViperAdapter(IRobotHost host) => _host = host;

        // ── Connect / Disconnect ─────────────────────────────────────────────────

        public void Connect(string? deviceName)
        {
            StopEncoderTimer();
            DisposeController();
            _device?.Dispose();
            _device = null;
            _host.SetCalibrationStatus(false);

            string portName = string.IsNullOrWhiteSpace(deviceName) ? "COM3" : deviceName!;

            try
            {
                _device = new ViperDevice();
                if (!_device.Connect(portName))
                {
                    _host.ShowMessage(
                        $"No se pudo conectar al Viper X-300S en {portName}.\n{_device.LastError}",
                        "Error de conexión", MessageBoxIcon.Error);
                    _device.Dispose(); _device = null;
                    _host.SetConnectionStatus(false);
                    return;
                }

                _device.EnableTorque(true);

                _basicCtrl  = new BasicController();
                _polyTraj   = new PolyTrajectory();
                _controller = new JointController(_device);
                _controller.SetController(_basicCtrl);
                _controller.SetTrajectory(_polyTraj);

                _host.SetConnectionStatus(true);
                _host.SetCalibrationStatus(true);
            }
            catch (DllNotFoundException)
            {
                _device?.Dispose(); _device = null;
                _host.SetConnectionStatus(false);
                _host.ShowMessage(
                    "No se encontró dxl_x86_c.dll.\n" +
                    "Descarga el Dynamixel SDK de ROBOTIS y coloca dxl_x86_c.dll " +
                    "junto al ejecutable de RobotClient.",
                    "Dynamixel SDK no encontrado", MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                DisposeController();
                _device?.Dispose(); _device = null;
                _host.SetConnectionStatus(false);
                _host.ShowMessage($"Error al conectar: {ex.Message}", "Error", MessageBoxIcon.Error);
            }
        }

        public void Disconnect()
        {
            StopEncoderTimer();
            DisposeController();
            _device?.Dispose(); _device = null;
            _host.SetConnectionStatus(false);
            _host.SetCalibrationStatus(false);
        }

        // ── Calibrate ────────────────────────────────────────────────────────────

        public void Calibrate()
        {
            if (_device == null || !_device.IsConnected)
            {
                _host.ShowMessage("Conecte el Viper X-300S antes de calibrar.",
                    "Dispositivo no conectado", MessageBoxIcon.Warning);
                return;
            }
            _host.SetCalibrationStatus(true);
            _host.ShowMessage("El Viper X-300S usa encoders absolutos y no requiere calibración.",
                "Viper X-300S", MessageBoxIcon.Information);
        }

        // ── Encoder reading ──────────────────────────────────────────────────────

        public void ToggleReadEncoders()
        {
            if (_device == null || !_device.IsConnected)
            {
                _host.ShowMessage("Conecte el Viper X-300S antes de leer los encoders.",
                    "Dispositivo no conectado", MessageBoxIcon.Warning);
                return;
            }

            if (_encoderTimer != null)
                StopEncoderTimer();
            else
            {
                _encoderPending = false;
                _encoderTimer   = new System.Threading.Timer(EncoderTick, null, 0, EncoderIntervalMs);
                _host.SetEncoderButtonText("Detener lectura");
            }
        }

        // ── Motion ───────────────────────────────────────────────────────────────

        public void GoHome(double[] qf)
        {
            if (_device == null || !_device.IsConnected || _controller == null)
            {
                _host.ShowMessage("Conecte el Viper X-300S antes de ir a Home.",
                    "Dispositivo no conectado", MessageBoxIcon.Warning);
                return;
            }

            _device.EnableTorque(true);
            _controller.MoveTo(FullQfDeg(qf), Tf);
            _host.NotifyMotionStarted();
            _host.SetMotionState("Yendo a Home...", Color.Orange);
            StartMotionTimer("Home");
        }

        public void GoFinal(double[] qf)
        {
            if (_device == null || !_device.IsConnected || _controller == null)
            {
                _host.ShowMessage("Conecte el Viper X-300S antes de ir a destino.",
                    "Dispositivo no conectado", MessageBoxIcon.Warning);
                return;
            }

            _device.EnableTorque(true);
            _host.NotifyGoFinalStarted();
            _controller.MoveTo(FullQfDeg(qf), Tf);
            _host.SetMotionState("Yendo a destino...", Color.Orange);
            StartMotionTimer("En destino");
        }

        // ── IDisposable ──────────────────────────────────────────────────────────

        public void Stop()
        {
            _motionTimer?.Dispose(); _motionTimer = null;
            _controller?.Stop();
        }

        public void Dispose() => Disconnect();

        public double[] GetLastEstimate() => new double[3];

        public double[] GetEstimatedForce() => new double[3];

        // ── Helpers ──────────────────────────────────────────────────────────────

        private void StartMotionTimer(string completedState)
        {
            _motionTimer?.Dispose();
            _motionTimer = new System.Threading.Timer(_ =>
            {
                if (_controller?.IsCompleted == true)
                    _host.InvokeOnUI(() => _host.NotifyMotionCompleted(completedState));
            }, null, 100, 100);
        }

        private void EncoderTick(object? _)
        {
            if (_encoderPending) return;
            _encoderPending = true;

            if (_device == null || !_device.IsConnected) { _encoderPending = false; return; }

            double[] qDeg = _device.GetJointAngles();             // Dynamixel degrees (0-360)
            double[] qRad = qDeg.Select(DxlDegToRad).ToArray(); // → radians for host
            double[] p    = new double[3];

            _host.InvokeOnUI(() =>
            {
                _encoderPending = false;
                _host.UpdateEncoderDisplay(qRad, p);
            });
        }

        private void StopEncoderTimer()
        {
            _encoderTimer?.Dispose();
            _encoderTimer   = null;
            _encoderPending = false;
            _host.SetEncoderButtonText("Leer encoders");
        }

        private void DisposeController()
        {
            _motionTimer?.Dispose(); _motionTimer = null;
            _controller?.Dispose();  _controller  = null;
            _basicCtrl  = null;
            _polyTraj   = null;
        }
    }
}
