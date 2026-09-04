namespace Teleoperation
{
    /// <summary>
    /// Experiment 2 — bilateral teleoperation with two Geomagic Touch devices,
    /// running the position/force law of Guajardo, Section 3.1.
    ///
    /// Both devices come up under the single shared HD scheduler; a single
    /// <see cref="GeomagicBilateralLoop"/> then drives the pair, computing both
    /// control laws (3.1)/(3.2) from one position snapshot and running both
    /// estimators (3.35)-(3.43) afterwards, all on one 2 ms thread.
    /// </summary>
    public partial class TeleoperationForm : Form
    {
        private GeomagicDevice? _device1;
        private GeomagicDevice? _device2;

        // One loop drives both robots. Robot 1 is the local side (3.1),
        // robot 2 the remote one (3.2).
        private GeomagicBilateralLoop? _loop;
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
        /// Creates the single control loop over both devices and loads the gains.
        /// The loop owns both controllers and both estimators and ticks them from
        /// one thread, so the channel of (3.9)/(3.13) is broken by exactly one
        /// sample. Its transport delay T_j(t) is zero — both robots are on this
        /// host and the loop reads them from the same snapshot.
        /// </summary>
        private void BuildControlStack()
        {
            _loop = new GeomagicBilateralLoop(_device1!.NativeHandle, _device2!.NativeHandle);
            _loop.SetVelocitySource(BilateralGains.UseLevantVelocity
                ? GeomagicBilateralLoop.VelocitySource.Levant
                : GeomagicBilateralLoop.VelocitySource.Dirty);
            _loop.SetDirtyLambda(BilateralGains.DirtyLambda);
            _loop.SetVelocityFilter(BilateralGains.VelocityFilterOwn,
                                    BilateralGains.VelocityFilterPeer);

            ApplySideConfig(_loop.Local,  BilateralGains.StartLocal);
            ApplySideConfig(_loop.Remote, BilateralGains.StartRemote);

            plotsControl.ExportLog = path => _loop.ExportLog(path);
        }

        private static void ApplySideConfig(GeomagicBilateralView side, BilateralGainSet g)
        {
            side.SetControlGains(g.Ka, g.Kp, g.Kf, g.Lambda, g.Kbeta, g.Kgamma);
            side.SetForceChannelGain(BilateralGains.ForceChannelGain);
            side.SetEstimatorGains(BilateralGains.EstimatorK, BilateralGains.EstimatorKu);
            side.SetDifferentiatorBounds(BilateralGains.DifferentiatorL,
                                         BilateralGains.DifferentiatorM);
        }

        private void DisposeDevices()
        {
            // The loop first: its timer thread touches both devices, so it has to
            // be stopped and freed before they are.
            plotsControl.ExportLog = null;
            _loop?.Dispose(); _loop = null;

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
            plotsControl.Clear();
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

        // Runs on the UI thread — both devices are read within the same tick, so
        // the two plot traces share a time base.
        private void EncTimer_Tick(object? sender, EventArgs e)
        {
            double[]? qLocal  = UpdateEncoders(_device1, encoders1);
            double[]? qRemote = UpdateEncoders(_device2, encoders2);

            if (qLocal != null && qRemote != null)
                plotsControl.AddSample(qLocal, qRemote);
        }

        /// <summary>
        /// Refreshes one readout and returns that robot's joint angles in
        /// degrees, or null if the device could not be read.
        /// </summary>
        private static double[]? UpdateEncoders(GeomagicDevice? device,
                                                GeomagicEncodersControl display)
        {
            if (device == null) return null;
            try
            {
                double[] q = device.GetJointAngles();
                double[] p = GeomagicModel.ForwardKinematics(q);
                double[] deg =
                [
                    q[0] * 180.0 / Math.PI,
                    q[1] * 180.0 / Math.PI,
                    q[2] * 180.0 / Math.PI,
                ];
                display.UpdateDisplay(deg[0], deg[1], deg[2],
                                      p[0] * 100.0, p[1] * 100.0, p[2] * 100.0);
                return deg;
            }
            catch { return null; }
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
            if (_loop == null ||
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

            // One call: resets both controllers and both estimators, then starts
            // the single 2 ms loop that drives the pair.
            _loop.Start();

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

            // Kills the timer and zeroes the torques on both devices.
            try { _loop?.Stop(); } catch { }

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
