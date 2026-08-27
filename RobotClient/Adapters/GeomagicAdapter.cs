using System.Runtime.InteropServices;

namespace RobotClient
{
    internal sealed class GeomagicAdapter : IRobotAdapter
    {
        [DllImport("winmm.dll")]
        private static extern uint timeGetTime();
        private readonly IRobotHost            _host;
        private readonly GeomagicConfigControl _config;

        private GeomagicDevice?            _device;
        private GeomagicJointController?   _controller;
        private GeomagicPIDController?     _pid;
        private GeomagicPolyTrajectory?    _polyTraj;
        private GeomagicDirectEstimation?  _estimator;
        private System.Threading.Timer?    _encoderTimer;
        private volatile bool              _encoderPending;
        private System.Threading.Timer?    _motionTimer;
        private volatile bool              _recordingDesired;
        private int                        _motionGeneration;
        private double                     _desiredTf;

        private static readonly double[] KpHome = [0.66,  0.66,  0.66 ];
        private static readonly double[] KiHome = [0.16,  0.16,  0.16 ];
        private static readonly double[] KdHome = [0.003, 0.003, 0.003];

        private const double TfHome = 2.0; // GoHome always uses 2 s

        public GeomagicAdapter(IRobotHost host, GeomagicConfigControl config)
        {
            _host   = host;
            _config = config;
        }

        public void Connect(string? deviceName)
        {
            StopEncoderTimer();
            DisposeController();
            _device?.Dispose();
            _device = null;
            _host.SetCalibrationStatus(false);

            try
            {
                _device = new GeomagicDevice();
                if (!_device.Initialize(deviceName))
                {
                    _device.Dispose(); _device = null;
                    _host.SetConnectionStatus(false);
                    _host.ShowMessage(
                        "No se pudo conectar al dispositivo.\n" +
                        "Verifique que el Geomagic Touch esté encendido y conectado.",
                        "Error de conexión", MessageBoxIcon.Error);
                    return;
                }

                GeomagicDevice.StartScheduler();
                _pid        = new GeomagicPIDController();
                _polyTraj   = new GeomagicPolyTrajectory();
                _controller = new GeomagicJointController(_device.NativeHandle);
                _controller.SetController(_pid.NativeHandle);
                _controller.SetTrajectory(_polyTraj.NativeHandle);
                _host.SetConnectionStatus(true);
            }
            catch (Exception ex)
            {
                DisposeController();
                _device?.Dispose(); _device = null;
                _host.SetConnectionStatus(false);
                _host.ShowMessage($"Error al cargar GeomagicCore.dll:\n{ex.Message}",
                    "Error", MessageBoxIcon.Error);
            }
        }

        public void Calibrate()
        {
            if (_device == null || !_device.IsInitialized)
            {
                _host.ShowMessage("Conecte el Geomagic Touch antes de calibrar.",
                    "Dispositivo no conectado", MessageBoxIcon.Warning);
                return;
            }

            bool ok = _device.Calibrate();
            _host.SetCalibrationStatus(ok);
            if (!ok)
                _host.ShowMessage(
                    "La calibración requiere intervención manual.\n" +
                    "Coloque el brazo en la posición de inicio (inkwell) e intente de nuevo.",
                    "Calibración incompleta", MessageBoxIcon.Warning);
        }

        public void Disconnect()
        {
            StopEncoderTimer();
            DisposeController();
            _device?.Dispose(); _device = null;
            _host.SetConnectionStatus(false);
            _host.SetCalibrationStatus(false);
        }

        public void ToggleReadEncoders()
        {
            if (_device == null || !_device.IsInitialized)
            {
                _host.ShowMessage("Conecte el Geomagic Touch antes de leer los encoders.",
                    "Dispositivo no conectado", MessageBoxIcon.Warning);
                return;
            }

            if (_encoderTimer != null)
            {
                StopEncoderTimer();
            }
            else
            {
                _encoderPending = false;
                _encoderTimer   = new System.Threading.Timer(EncoderTick, null, 0, _config.SampleTimeMs);
                _host.SetEncoderButtonText("Detener lectura");
            }
        }

        public void GoHome(double[] qf)
        {
            if (!CheckReady("ir a Home")) return;

            int gen = ++_motionGeneration;
            _recordingDesired = false;
            _pid!.SetGains(KpHome, KiHome, KdHome);
            _controller!.SetController(_pid.NativeHandle);
            _controller.MoveTo(qf, TfHome);
            _host.NotifyMotionStarted();
            _host.SetMotionState("Yendo a Home...", Color.Orange);

            _motionTimer?.Dispose();
            _motionTimer = new System.Threading.Timer(_ =>
                _host.InvokeOnUI(() =>
                {
                    if (_motionGeneration != gen) return;
                    _host.NotifyMotionCompleted("Home");
                }),
                null, (int)(TfHome * 1000), Timeout.Infinite);
        }

        public void GoFinal(double[] qf)
        {
            if (!CheckReady("ir a destino")) return;

            // Attach estimator on first GoFinal (lazy).
            if (_estimator == null)
            {
                try
                {
                    _estimator = new GeomagicDirectEstimation();
                    _controller!.SetEstimator(_estimator.NativeHandle);
                }
                catch { _estimator = null; }
            }

            _pid!.SetGains(_config.GetKp(), _config.GetKi(), _config.GetKd());
            _controller!.SetController(_pid.NativeHandle);

            int gen = ++_motionGeneration;
            double tf = _config.TrajectoryTimeSecs;
            _desiredTf = tf;
            _host.NotifyGoFinalStarted();
            _controller.MoveTo(qf, tf);
            _recordingDesired = true;
            _host.SetMotionState("Yendo a destino...", Color.Orange);

            _motionTimer?.Dispose();
            _motionTimer = new System.Threading.Timer(_ =>
                _host.InvokeOnUI(() =>
                {
                    if (_motionGeneration != gen) return;
                    _recordingDesired = false;
                    _host.NotifyMotionCompleted("En destino");
                }),
                null, (int)(tf * 1000), Timeout.Infinite);
        }

        public void Stop()
        {
            if (_controller == null) return;
            ++_motionGeneration;
            _recordingDesired = false;
            _motionTimer?.Dispose(); _motionTimer = null;
            _controller.Stop();
        }

        public void Dispose() => Disconnect();

        private bool CheckReady(string action)
        {
            if (_device == null || !_device.IsInitialized || _controller == null || _pid == null)
            {
                _host.ShowMessage($"Conecte el Geomagic Touch antes de {action}.",
                    "Dispositivo no conectado", MessageBoxIcon.Warning);
                return false;
            }
            if (!_host.IsCalibrated)
            {
                _host.ShowMessage("Calibre el dispositivo antes de mover el robot.",
                    "Dispositivo no calibrado", MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void EncoderTick(object? _)
        {
            if (_encoderPending) return;
            _encoderPending = true;
            _host.InvokeOnUI(() =>
            {
                try
                {
                    if (_device == null) return;
                    double[] q = _device.GetJointAngles();
                    double[] p = GeomagicModel.ForwardKinematics(q);
                    _host.UpdateEncoderDisplay(q, p);

                    if (_recordingDesired && _controller != null && _polyTraj != null)
                    {
                        double tStart = _controller.MotionStartTime;
                        if (tStart > 0)
                        {
                            double t = (timeGetTime() - tStart) / 1000.0;
                            if (t >= 0 && t <= _desiredTf)
                            {
                                _polyTraj.Evaluate(t, out double[] qd, out double[] qpd, out double[] qppd);
                                _host.RecordDesiredTrajectory(t, qd, qpd, qppd);
                            }
                        }
                    }
                }
                finally
                {
                    _encoderPending = false;
                }
            });
        }

        private void StopEncoderTimer()
        {
            _encoderTimer?.Dispose(); _encoderTimer = null;
            _encoderPending = false;
            _host.SetEncoderButtonText("Leer encoders");
        }

        public double[] GetLastEstimate()
        {
            if (_controller == null) return new double[3];
            try   { return _controller.GetLastEstimate(); }
            catch { return new double[3]; }
        }

        public double[] GetEstimatedForce()
        {
            if (_estimator == null) return new double[3];
            try   { return _estimator.GetForce(); }
            catch { return new double[3]; }
        }

        private void DisposeController()
        {
            _motionTimer?.Dispose(); _motionTimer = null;
            _controller?.Dispose();  _controller  = null;
            _pid?.Dispose();         _pid         = null;
            _polyTraj?.Dispose();    _polyTraj    = null;
            _estimator?.Dispose();   _estimator   = null;
        }
    }
}
