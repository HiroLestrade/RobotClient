namespace Teleoperation
{
    /// <summary>
    /// Experiment 2 — bilateral teleoperation with two Geomagic Touch devices,
    /// running the position/force law of Guajardo, Section 3.1.
    ///
    /// Both devices come up under the single shared HD scheduler; each then gets
    /// its own estimator (3.35)-(3.43) and bilateral controller (3.1)/(3.2),
    /// cross-linked so each reads the other's position and estimated torque at
    /// the 2 ms control rate.
    /// </summary>
    public partial class TeleoperationForm : Form
    {
        private GeomagicDevice? _device1;
        private GeomagicDevice? _device2;

        // Control stack of the bilateral law, one per robot. Robot 1 is the
        // local side (3.1), robot 2 the remote one (3.2).
        private GeomagicMomentumEstimator?   _estimator1, _estimator2;
        private GeomagicBilateralController? _bilateral1, _bilateral2;
        private GeomagicJointController?     _controller1, _controller2;
        private bool _teleopRunning;

        // Largest joint mismatch tolerated when starting teleoperation, in rad.
        // Not part of the law: with dq at 0.35 rad the term Kp·Lambda·dq of
        // (3.1)/(3.2) already saturates the ±1 N·m of the Touch, so the arms
        // would snap together on the first tick.
        private const double MaxStartMismatch = 0.35;

        // One UI timer drives both readouts, so robot 1 and robot 2 are always
        // sampled on the same tick and their values stay comparable.
        private System.Windows.Forms.Timer? _encTimer;
        private bool _reading;

        public TeleoperationForm()
        {
            InitializeComponent();
            encoders1.Title = "Robot 1 (maestro)";
            encoders2.Title = "Robot 2 (esclavo)";

            forceSensorControl.ConnectionChanged += (_, connected) =>
            {
                forceLabelValue.Text      = connected ? "Conectado" : "Desconectado";
                forceLabelValue.ForeColor = connected ? Color.Green : Color.Red;
            };
        }

        // ── Conexión ─────────────────────────────────────────────────────────────

        private void bttnConnect_Click(object sender, EventArgs e)
        {
            StopTeleop();
            StopReading();
            SetCalibrationStatus(1, false);
            SetCalibrationStatus(2, false);
            ConnectBoth();
        }

        /// <summary>
        /// Initializes both devices before starting the shared HD scheduler.
        /// OpenHaptics only accepts hdInitDevice while the scheduler is stopped,
        /// so both calls complete first and StartScheduler() runs once at the end.
        /// </summary>
        private void ConnectBoth()
        {
            DisposeDevices();

            string? name1 = string.IsNullOrWhiteSpace(tbDevice1.Text) ? null : tbDevice1.Text.Trim();
            string? name2 = string.IsNullOrWhiteSpace(tbDevice2.Text) ? null : tbDevice2.Text.Trim();

            try
            {
                _device1 = new GeomagicDevice();
                if (!_device1.Initialize(name1))
                    throw new InvalidOperationException("No se pudo conectar al Robot 1.");

                _device2 = new GeomagicDevice();
                if (!_device2.Initialize(name2))
                    throw new InvalidOperationException("No se pudo conectar al Robot 2.");

                // Both devices are registered — start the scheduler once.
                GeomagicDevice.StartScheduler();

                BuildControlStack();

                SetConnectionStatus(1, true);
                SetConnectionStatus(2, true);
            }
            catch (Exception ex)
            {
                DisposeDevices();
                SetConnectionStatus(1, false);
                SetConnectionStatus(2, false);
                MessageBox.Show(
                    $"Error al conectar dispositivos Geomagic:\n{ex.Message}",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Builds one estimator + bilateral controller + joint controller per
        /// robot and cross-links them: each controller reads q_di and tau_di
        /// straight from the other robot's device and estimator, at the control
        /// rate. The channel delay T_j(t) of (3.9) and (3.13) is therefore zero,
        /// both robots being on this host.
        /// </summary>
        private void BuildControlStack()
        {
            _estimator1 = new GeomagicMomentumEstimator(isLocal: true);
            _estimator2 = new GeomagicMomentumEstimator(isLocal: false);
            _estimator1.SetGains(BilateralGains.EstimatorK, BilateralGains.EstimatorKu);
            _estimator2.SetGains(BilateralGains.EstimatorK, BilateralGains.EstimatorKu);
            _estimator1.SetDifferentiatorBounds(BilateralGains.DifferentiatorL,
                                                BilateralGains.DifferentiatorM);
            _estimator2.SetDifferentiatorBounds(BilateralGains.DifferentiatorL,
                                                BilateralGains.DifferentiatorM);

            _bilateral1 = new GeomagicBilateralController(isLocal: true);
            _bilateral2 = new GeomagicBilateralController(isLocal: false);
            ApplyGains(_bilateral1, BilateralGains.StartLocal);
            ApplyGains(_bilateral2, BilateralGains.StartRemote);
            _bilateral1.SetDifferentiatorBounds(BilateralGains.DifferentiatorL,
                                                BilateralGains.DifferentiatorM);
            _bilateral2.SetDifferentiatorBounds(BilateralGains.DifferentiatorL,
                                                BilateralGains.DifferentiatorM);

            _bilateral1.SetEstimator(_estimator1.NativeHandle);
            _bilateral2.SetEstimator(_estimator2.NativeHandle);
            _bilateral1.SetPeer(_device2!.NativeHandle, _estimator2.NativeHandle);
            _bilateral2.SetPeer(_device1!.NativeHandle, _estimator1.NativeHandle);

            _controller1 = new GeomagicJointController(_device1.NativeHandle);
            _controller2 = new GeomagicJointController(_device2.NativeHandle);
            _controller1.SetController(_bilateral1.NativeHandle);
            _controller2.SetController(_bilateral2.NativeHandle);
            _controller1.SetEstimator(_estimator1.NativeHandle);
            _controller2.SetEstimator(_estimator2.NativeHandle);
        }

        private static void ApplyGains(GeomagicBilateralController ctrl, BilateralGainSet g) =>
            ctrl.SetGains(g.Ka, g.Kp, g.Kf, g.Lambda, g.Kbeta, g.Kgamma);

        private void DisposeDevices()
        {
            // Controllers first: they hold pointers into the devices and the
            // estimators, and their timer thread must be stopped before either
            // is freed.
            _controller1?.Dispose(); _controller1 = null;
            _controller2?.Dispose(); _controller2 = null;
            _bilateral1?.Dispose();  _bilateral1  = null;
            _bilateral2?.Dispose();  _bilateral2  = null;
            _estimator1?.Dispose();  _estimator1  = null;
            _estimator2?.Dispose();  _estimator2  = null;

            _device1?.Dispose(); _device1 = null;
            _device2?.Dispose(); _device2 = null;
        }

        // ── Calibración ──────────────────────────────────────────────────────────

        private void bttnCalibrate1_Click(object sender, EventArgs e) => Calibrate(1, _device1);
        private void bttnCalibrate2_Click(object sender, EventArgs e) => Calibrate(2, _device2);

        private void Calibrate(int robotNum, GeomagicDevice? device)
        {
            if (device == null || !device.IsInitialized)
            {
                MessageBox.Show(
                    $"Conecte el Robot {robotNum} antes de calibrar.",
                    "Dispositivo no conectado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool ok = device.Calibrate();
            SetCalibrationStatus(robotNum, ok);
            if (!ok)
            {
                MessageBox.Show(
                    $"La calibración del Robot {robotNum} requiere intervención manual.\n" +
                    "Coloque el brazo en la posición de inicio (inkwell) e intente de nuevo.",
                    "Calibración incompleta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ── Lectura conjunta de encoders ─────────────────────────────────────────

        private void bttnReadEncoders_Click(object sender, EventArgs e)
        {
            if (_reading)
            {
                StopReading();
                return;
            }

            if (_device1 == null || !_device1.IsInitialized ||
                _device2 == null || !_device2.IsInitialized)
            {
                MessageBox.Show(
                    "Conecte ambos robots antes de leer los encoders.",
                    "Dispositivos no conectados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StartReading();
        }

        private void StartReading()
        {
            if (_reading) return;

            _reading = true;
            bttnReadEncoders.Text = "Detener lectura";
            readingLabelValue.Text      = "Activa";
            readingLabelValue.ForeColor = Color.Green;

            _encTimer = new System.Windows.Forms.Timer { Interval = 16 }; // ~60 fps
            _encTimer.Tick += EncTimer_Tick;
            _encTimer.Start();
        }

        private void StopReading()
        {
            if (!_reading) return;
            _reading = false;
            _encTimer?.Stop();
            _encTimer?.Dispose();
            _encTimer = null;
            bttnReadEncoders.Text = "Leer encoders";
            readingLabelValue.Text      = "Detenida";
            readingLabelValue.ForeColor = Color.Gray;
        }

        // Runs on the UI thread — both devices are read within the same tick.
        private void EncTimer_Tick(object? sender, EventArgs e)
        {
            UpdateEncoders(_device1, encoders1);
            UpdateEncoders(_device2, encoders2);
        }

        private static void UpdateEncoders(GeomagicDevice? device, GeomagicEncodersControl display)
        {
            if (device == null) return;
            try
            {
                double[] q = device.GetJointAngles();
                double[] p = GeomagicModel.ForwardKinematics(q);
                display.UpdateDisplay(
                    q[0] * 180.0 / Math.PI,
                    q[1] * 180.0 / Math.PI,
                    q[2] * 180.0 / Math.PI,
                    p[0] * 100.0,
                    p[1] * 100.0,
                    p[2] * 100.0);
            }
            catch { }
        }

        // ── Teleoperación ────────────────────────────────────────────────────────

        private void bttnStartTeleop_Click(object sender, EventArgs e)
        {
            if (_teleopRunning)
            {
                StopTeleop();
                return;
            }
            StartTeleop();
        }

        private void StartTeleop()
        {
            if (_controller1 == null || _controller2 == null ||
                _device1 == null || !_device1.IsInitialized ||
                _device2 == null || !_device2.IsInitialized)
            {
                MessageBox.Show(
                    "Conecte ambos robots antes de iniciar la teleoperación.",
                    "Dispositivos no conectados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (calib1LabelValue.Text != "Calibrado" || calib2LabelValue.Text != "Calibrado")
            {
                MessageBox.Show(
                    "Calibre ambos robots antes de iniciar la teleoperación.",
                    "Dispositivos no calibrados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            double[] q1 = _device1.GetJointAngles();
            double[] q2 = _device2.GetJointAngles();
            for (int k = 0; k < 3; k++)
            {
                double dq = Math.Abs(q1[k] - q2[k]);
                if (dq <= MaxStartMismatch) continue;

                MessageBox.Show(
                    $"Los brazos están demasiado separados en q{k + 1}: " +
                    $"{dq * 180.0 / Math.PI:F1}° (máximo {MaxStartMismatch * 180.0 / Math.PI:F0}°).\n" +
                    "Acérquelos a la misma postura antes de iniciar; de lo contrario " +
                    "el primer tick satura el par y los brazos se juntan de golpe.",
                    "Posturas desalineadas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Each Start() resets its controller and its estimator, then ticks
            // every 2 ms. Between the two calls robot 1 already tracks robot 2;
            // dq is under MaxStartMismatch, so that first interval is harmless.
            _controller1.Start();
            _controller2.Start();

            _teleopRunning = true;
            bttnStartTeleop.Text        = "Detener teleoperación";
            teleopLabelValue.Text       = "Activa";
            teleopLabelValue.ForeColor  = Color.Green;

            // The encoder readout is the only view of what the law is doing.
            if (!_reading) StartReading();
        }

        private void StopTeleop()
        {
            if (!_teleopRunning) return;
            _teleopRunning = false;

            // Stop() kills the timer and zeroes the torques on both devices.
            try { _controller1?.Stop(); } catch { }
            try { _controller2?.Stop(); } catch { }

            bttnStartTeleop.Text       = "Iniciar teleoperación";
            teleopLabelValue.Text      = "Detenida";
            teleopLabelValue.ForeColor = Color.Gray;
        }

        // ── Barra de estado ──────────────────────────────────────────────────────

        private void SetConnectionStatus(int robotNum, bool connected)
        {
            var label = robotNum == 1 ? status1LabelValue : status2LabelValue;
            label.Text      = connected ? "Conectado" : "Desconectado";
            label.ForeColor = connected ? Color.Green  : Color.Red;
        }

        private void SetCalibrationStatus(int robotNum, bool calibrated)
        {
            var label = robotNum == 1 ? calib1LabelValue : calib2LabelValue;
            label.Text      = calibrated ? "Calibrado" : "No Calibrado";
            label.ForeColor = calibrated ? Color.Green : Color.Red;
        }

        // ── Ciclo de vida ────────────────────────────────────────────────────────

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            StopTeleop();
            StopReading();
            DisposeDevices();
            base.OnFormClosed(e);
        }
    }
}
