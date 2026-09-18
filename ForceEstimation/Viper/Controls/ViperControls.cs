using ViperCore;

namespace ForceEstimation
{
    /// <summary>
    /// Control panel for the Viper X-300S, the counterpart of
    /// <see cref="GeomagicControls"/>. Laid out the same way: connection strip
    /// across the top, measurements and configuration down the left, plots on
    /// the right, status bar along the bottom.
    ///
    /// <para><b>Wired so far:</b> connection, torque, the encoder readout, and
    /// three motions. Home and the rest pose are fixed at
    /// <see cref="FixedMotionTime"/> seconds through <see cref="BasicController"/>
    /// and are deliberately not plotted; "Ir a Destino" takes its controller,
    /// trajectory and duration from the Configuration tabs and records every
    /// joint signal. Emergency stop is on its button and on <b>Ctrl+Space</b>,
    /// from anywhere in the panel.</para>
    ///
    /// <para><b>Units.</b> Everything the user reads or types is in <b>real
    /// degrees</b>, 0 at centre — so Home is six zeros, not six 180s. The motors'
    /// own 0–360 convention never leaves this class: targets cross into it in
    /// <see cref="StartMotion"/> and readings cross out of it where they are
    /// displayed or plotted. <see cref="ViperJointConvention"/> holds both
    /// directions.</para>
    ///
    /// <para>Two differences from the Geomagic panel worth noting, both from the
    /// hardware: six joints instead of three, and a serial port plus baud rate
    /// instead of a device name.</para>
    ///
    /// <para>This panel owns the connection, so <see cref="ForceEstimationForm"/>
    /// disposing it on a robot change is what closes the serial port.</para>
    /// </summary>
    public partial class ViperControls : UserControl
    {
        /// <summary>
        /// Encoder sampling period. 20 ms is 50 Hz, and each tick costs a single
        /// sync read of the ten-byte block at 126.
        /// </summary>
        private const int EncoderIntervalMs = 20;

        /// <summary>Duration of Home and the rest pose, in seconds.</summary>
        private const double FixedMotionTime = 3.0;

        /// <summary>
        /// Seconds the loop keeps holding the final point after the trajectory
        /// ends. Long enough for a servo lag of ~100 ms to decay several times
        /// over, so what is left on the error plot at the end is what the joint
        /// genuinely cannot close.
        /// </summary>
        private const double SettleSeconds = 0.5;

        /// <summary>
        /// Period the control loop asks for, in seconds — 1 ms.
        ///
        /// <para>This is a floor, not a promise. Two serial transactions per
        /// tick over a USB adapter rarely fit in a millisecond, so the loop runs
        /// at whatever the bus sustains and reports it live in the status bar.
        /// Asking for 1 ms simply means nothing in software is holding it back:
        /// the loop is the bottleneck nowhere, and the rate you see is the bus's
        /// answer, not the scheduler's.</para>
        /// </summary>
        private const double MotionSampleTime = 0.001;

        /// <summary>
        /// How far above the trajectory's peak velocity the motor's own profile
        /// limit is set. Margin for the loop's jitter, without so much slack
        /// that the motor finishes each step early and waits.
        /// </summary>
        private const double ProfileMargin = 1.2;

        /// <summary>
        /// Floor for the profile velocity, deg/s. Guards the degenerate move —
        /// a target equal to the present pose — from asking for a limit of zero,
        /// which the register reads as "no limit".
        /// </summary>
        private const double MinProfileVelocity = 5.0;

        /// <summary>
        /// Shortest gap between samples kept for the plots, in seconds — 200 Hz.
        /// Decimating on elapsed time rather than on a tick count keeps the plot
        /// density the same whatever rate the loop turns out to achieve.
        /// </summary>
        private const double PlotSamplePeriod = 0.005;

        /// <summary>How often the UI drains samples and refreshes the status bar.</summary>
        private const int PumpIntervalMs = 50;

        private const string BasicControllerName = "Básico (posición)";
        private const string QuinticName         = "Polinomio quíntico";

        /// <summary>
        /// Rest pose, in <b>real degrees</b>: low and resting on itself, which is
        /// the safe place to leave the arm before cutting torque.
        ///
        /// <para>The same pose in Dynamixel degrees is
        /// <c>[180, 68, 272, 180, 218, 180]</c> — every entry here is that minus
        /// the 180° centre. It is written in the panel's convention, like every
        /// other target, so there is one place that converts and it is
        /// <see cref="StartMotion"/>.</para>
        /// </summary>
        private static readonly double[] RestPoseDeg =
            [0.0, -112.0, 92.0, 0.0, 38.0, 0.0];

        private ViperDevice? _device;

        // The control loop. Rebuilt when a different controller is asked for,
        // because installing one can force an operating-mode change, and that
        // costs an EEPROM write with torque off.
        private JointController? _motion;
        private string?          _installedController;

        /// <summary>Drains samples and polls the loop. Touches no hardware.</summary>
        private readonly System.Windows.Forms.Timer _pump;

        private string _motionName = string.Empty;

        /// <summary>
        /// Last fault reported by the loop. Written on the loop thread and read
        /// by the pump on the UI thread, so a loop faulting every tick cannot
        /// flood the UI with marshalled calls.
        /// </summary>
        private volatile string? _motionFault;

        // Samples waiting to reach the plots. The loop appends under the lock
        // and the pump drains: at a thousand ticks a second, one repaint per
        // tick would spend the whole period painting.
        private readonly List<PlotSample> _pending     = [];
        private readonly object           _pendingLock = new();
        private volatile bool             _recording;
        private double                    _lastPlotT;

        /// <summary>
        /// Guards <see cref="EmergencyStop"/> against re-entering itself. UI
        /// thread only, so no volatile: the re-entry it stops is a nested call
        /// through the message pump, not another thread.
        /// </summary>
        private bool _stopping;

        /// <summary>Cancels a running G-code program when a new one is launched.</summary>
        private CancellationTokenSource? _gcodeCts;

        /// <summary>
        /// True while a G-code program runs. It makes the individual moves stop
        /// owning the plots and the status bar: a program is one recording with
        /// several moves in it, not several recordings.
        /// </summary>
        private bool _gcodeRunning;

        /// <summary>When the running program started, for the plots' time axis.</summary>
        private DateTime _gcodeStart;

        /// <summary>
        /// The last commanded joint target that reached the plots, real degrees.
        /// Held flat by <see cref="RecordIdleSample"/> while no move is running.
        /// </summary>
        private double[]? _lastDesiredReal;

        /// <summary>
        /// Seconds added to every sample's timestamp so the segments of a program
        /// lay end to end instead of on top of each other.
        ///
        /// <para>The loop's clock restarts at zero on every <c>MoveTo</c>, so
        /// without this the second move of a program would draw over the first
        /// across the same 0..tf. Written on the UI thread in
        /// <see cref="StartMotion"/> before the loop thread exists, read on the
        /// loop thread afterwards — starting the thread is the barrier between
        /// the two.</para>
        /// </summary>
        private double _plotTimeOffset;

        // Scratch for the kinematics inside OnSampled, reserved once so the loop
        // thread does not allocate a pose per sample. Touched by that thread and
        // no other — ShowState runs the same maths on the UI thread and keeps the
        // allocating overload, because sharing these would be a data race.
        private readonly double[] _loopQRad = new double[ViperKinematics.JointCount];
        private readonly double[] _loopPM   = new double[3];

        /// <summary>One tick's worth of signals, copied out of the loop's buffers.</summary>
        private sealed record PlotSample(
            double T,
            double[] Q,  double[] Qp,  double[] Qpp,
            double[] Qd, double[] Qpd, double[] Qppd,
            double[] PCm);

        // ── Encoder readout ──────────────────────────────────────────────────

        // A threading timer, not a WinForms one: the sync read blocks on the
        // serial port, and doing that on the UI thread 50 times a second makes
        // the window stutter. The hop back to the UI thread is explicit below.
        private System.Threading.Timer? _encoderTimer;
        private volatile bool           _encoderPending;

        /// <summary>
        /// Whether the last snapshot came back valid, so the status bar only
        /// changes when the reading actually starts or stops working.
        /// </summary>
        private bool _lastReadValid = true;

        /// <summary>
        /// When the fault registers were last read, so a reading that flaps
        /// between valid and invalid cannot turn every transition into nine more
        /// serial transactions on the UI thread.
        /// </summary>
        private DateTime _lastFaultProbe = DateTime.MinValue;

        private static readonly TimeSpan FaultProbeInterval = TimeSpan.FromSeconds(1);

        public ViperControls()
        {
            InitializeComponent();

            homeControl.GoHomeRequested         += (_, qf) => _ = GoHome(qf);
            destinationControl.GoFinalRequested += (_, qf) => _ = GoFinal(qf);
            configControl.ExecuteGCodeRequested += OnExecuteGCodeRequested;
            encodersControl.ReadClicked         += (_, _)  => ToggleReadEncoders();

            _pump = new System.Windows.Forms.Timer { Interval = PumpIntervalMs };
            _pump.Tick += PumpTick;

            ShowDisconnected();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Straight at the timer rather than through StopEncoderTimer:
                // that one touches child controls, which may already be gone.
                _encoderTimer?.Dispose();
                _encoderTimer   = null;
                _encoderPending = false;

                _pump.Dispose();
                _motion?.Dispose();
                _motion = null;

                _device?.Dispose();
                _device = null;

                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        // ── Connection ───────────────────────────────────────────────────────

        private void bttnConnect_Click(object sender, EventArgs e)
        {
            if (_device != null) Disconnect();
            else                 Connect();
        }

        private void Connect()
        {
            string port = tbxPort.Text.Trim();
            if (port.Length == 0)
            {
                Warn("Escriba el puerto serial del brazo, por ejemplo COM3.");
                return;
            }

            if (!int.TryParse(cmbBaud.SelectedItem?.ToString(), out int baud))
                baud = 1_000_000;

            var device = new ViperDevice();
            try
            {
                // Connect only opens the port, pings the motors and adopts the
                // operating mode they are already in. It deliberately does NOT
                // energise anything: torque is its own button, because an arm
                // with no brakes should not come alive on connect.
                if (!device.Connect(port, baud))
                {
                    device.Dispose();
                    MessageBox.Show(
                        $"No se pudo conectar al Viper X-300S en {port} a {baud} baud." +
                        $"{Environment.NewLine}{Environment.NewLine}{device.LastError}",
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (DllNotFoundException)
            {
                device.Dispose();
                MessageBox.Show(
                    "No se encontró dxl_x86_c.dll." + Environment.NewLine + Environment.NewLine +
                    "Descarga el Dynamixel SDK de ROBOTIS y coloca dxl_x86_c.dll en " +
                    "ViperCore\\ViperCore\\, de donde la compilación la copia junto al " +
                    "ejecutable.",
                    "Dynamixel SDK no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                device.Dispose();
                MessageBox.Show($"Error al conectar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _device = device;
            ShowConnected(port, baud);

            RefreshHardwareErrors(force: true);
            WarnIfCouplingIsWrong();
        }

        private void Disconnect()
        {
            StopEncoderTimer();

            _pump.Stop();
            _recording = false;
            _motion?.Dispose();
            _motion              = null;
            _installedController = null;
            _motionFault         = null;
            lock (_pendingLock) _pending.Clear();

            // Closing the port leaves torque exactly as it was. That is on
            // purpose: cutting it here would drop the arm.
            _device?.Dispose();
            _device = null;

            ShowDisconnected();
        }

        /// <summary>
        /// Checks the dual-motor joints and speaks up only when something is
        /// off. Joints 2 and 3 have two motors each, and the second follows the
        /// first through its Secondary ID; if it does not, the pair is driven
        /// independently and the two motors fight each other.
        /// </summary>
        private void WarnIfCouplingIsWrong()
        {
            List<CouplingStatus>? coupling = _device?.VerifyCoupling();
            if (coupling == null)
            {
                MessageBox.Show(
                    "No se pudo verificar el acoplamiento de los motores duales." +
                    Environment.NewLine + Environment.NewLine + _device?.LastError,
                    "Acoplamiento sin verificar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<CouplingStatus> loose = coupling.Where(c => !c.IsShadowing).ToList();
            if (loose.Count == 0) return;

            MessageBox.Show(
                "Los motores secundarios de las articulaciones duales no están " +
                "configurados para seguir a su primario:" +
                Environment.NewLine + Environment.NewLine +
                string.Join(Environment.NewLine, loose.Select(c => c.ToString())) +
                Environment.NewLine + Environment.NewLine +
                "Cada una de las articulaciones 2 y 3 tiene dos motores enfrentados. " +
                "Si el secundario no sigue al primario por su Secondary ID, los dos " +
                "pelean entre sí y se sobrecalientan." +
                Environment.NewLine + Environment.NewLine +
                "No habilite el par hasta corregirlo.",
                "Acoplamiento incorrecto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // ── Encoder readout ──────────────────────────────────────────────────

        private void ToggleReadEncoders()
        {
            if (_device == null)
            {
                Warn("Conecte el Viper X-300S antes de leer los encoders.");
                return;
            }

            if (_encoderTimer != null)
            {
                StopEncoderTimer();
                return;
            }

            _encoderPending = false;
            _lastReadValid  = true;
            _encoderTimer   = new System.Threading.Timer(
                EncoderTick, null, 0, EncoderIntervalMs);

            encodersControl.ReadButtonText = "Detener lectura";
            if (_motion?.IsRunning != true) SetRobotState("Leyendo encoders", Color.Orange);
        }

        private void EncoderTick(object? _)
        {
            // A tick that arrives while the previous sync read is still on the
            // wire is dropped rather than queued: the serial port sets the pace.
            if (_encoderPending) return;
            _encoderPending = true;

            try
            {
                ViperDevice? device = _device;
                if (device == null || !device.IsConnected) return;

                // Who touches the bus depends on whether the loop is running.
                //
                // While it runs, it owns the port: it already sync-reads every
                // tick and publishes the snapshot, so this reads LatestState,
                // which touches no hardware. Issuing our own transaction here
                // would put two threads on one serial port, and they would read
                // each other's reply packets.
                //
                // With the loop stopped nobody refreshes that snapshot, so the
                // readout has to do the sync read itself.
                JointState state = _motion?.IsRunning == true
                    ? device.LatestState
                    : device.ReadState();

                if (IsDisposed || !IsHandleCreated) return;
                BeginInvoke(() => ShowState(state));
            }
            catch (ObjectDisposedException)   { /* panel went away mid-tick */ }
            catch (InvalidOperationException) { /* handle died before BeginInvoke */ }
            finally
            {
                _encoderPending = false;
            }
        }

        /// <summary>Runs on the UI thread with one snapshot of the arm.</summary>
        private void ShowState(JointState state)
        {
            if (_device == null) return;

            // Only a valid snapshot reaches the boxes. An invalid one carries
            // the previous values, and painting those would show stale numbers
            // as if they were fresh.
            if (state.Valid)
            {
                encodersControl.UpdateDisplay(
                    ViperJointConvention.ToRealDeg(state.Position),
                    CartesianCm(state.Position),
                    OrientationDirPsi(state.Position));

                // Between the lines of a program — a G4, or the gap before the
                // next move starts — the control loop is stopped, so this timer
                // is the only thing still reading the arm. Feeding the plots from
                // here is what makes a routine come out as one continuous trace
                // instead of one island per move with holes between them.
                if (_gcodeRunning && _motion?.IsRunning != true)
                    RecordIdleSample(state);
            }

            if (state.Valid == _lastReadValid) return;
            _lastReadValid = state.Valid;

            // A read that stops working is what a latched fault looks like from
            // here: a motor that trips overload cuts its own torque and stops
            // answering. Worth checking rather than guessing.
            if (_motion?.IsRunning != true)
                SetRobotState(
                    state.Valid ? "Leyendo encoders" : "Lectura fallida",
                    state.Valid ? Color.Orange       : Color.Red);

            RefreshHardwareErrors();
        }

        private void StopEncoderTimer()
        {
            _encoderTimer?.Dispose();
            _encoderTimer   = null;
            _encoderPending = false;

            encodersControl.ReadButtonText = "Leer encoders";
            if (_motion?.IsRunning != true)
                SetRobotState(_device != null ? "Inactivo" : "—", Color.Gray);
        }

        // ── Torque ───────────────────────────────────────────────────────────

        private void bttnTorque_Click(object sender, EventArgs e)
        {
            if (_device == null) return;

            if (_motion?.IsRunning == true)
            {
                Warn("Hay un movimiento en curso. Espere a que termine antes de " +
                     "cambiar el par.");
                return;
            }

            bool? on = _device.ReadTorqueEnabled();
            if (on == null)
            {
                Warn("No se pudo leer el estado del par." +
                     Environment.NewLine + Environment.NewLine + _device.LastError);
                RefreshTorqueStatus();
                return;
            }

            if (on.Value) DisableTorque();
            else          EnableTorque();

            RefreshTorqueStatus();
        }

        /// <summary>
        /// Energises the arm on the pose it is already in. Never a bare
        /// EnableTorque: that would servo every motor to whatever stale goal its
        /// register holds, and after any spell with torque off the arm has
        /// sagged away from it.
        /// </summary>
        private void EnableTorque()
        {
            if (_device!.EnableTorqueHolding()) return;

            MessageBox.Show(
                "No se pudo habilitar el par." +
                Environment.NewLine + Environment.NewLine + _device.LastError,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Cuts torque, which on this arm means letting go of it. Confirmed
        /// first because the consequence is physical and immediate.
        /// </summary>
        private void DisableTorque()
        {
            DialogResult answer = MessageBox.Show(
                "El Viper X-300S no tiene frenos: al quitar el par, el brazo se " +
                "desploma bajo su propio peso." +
                Environment.NewLine + Environment.NewLine +
                "Hágalo sólo con el brazo en una pose baja y apoyada, o " +
                "sosteniéndolo con la mano." +
                Environment.NewLine + Environment.NewLine +
                "¿Quitar el par?",
                "Quitar par", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (answer != DialogResult.Yes) return;

            if (_device!.EnableTorque(false)) return;

            MessageBox.Show(
                "No se pudo quitar el par en todos los motores." +
                Environment.NewLine + Environment.NewLine + _device.LastError,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // ── Motion ───────────────────────────────────────────────────────────

        /// <summary>
        /// Home and the rest pose are fixed: the basic controller, a quintic and
        /// three seconds, whatever the Configuration tabs say. They are utility
        /// moves, not experiments, so they are also not plotted.
        /// </summary>
        private bool GoHome(double[] qf) =>
            StartMotion(qf, "Ir a Home", BasicControllerName, QuinticName,
                        FixedMotionTime, record: false);

        private void bttnRest_Click(object sender, EventArgs e) =>
            _ = StartMotion(RestPoseDeg, "Posición de descanso", BasicControllerName,
                        QuinticName, FixedMotionTime, record: false);

        /// <summary>
        /// The experiment. Takes its controller, trajectory and duration from
        /// the Configuration tabs, and records every joint signal.
        /// </summary>
        private bool GoFinal(double[] qf) =>
            StartMotion(qf, "Ir a Destino",
                        configControl.SelectedController,
                        configControl.SelectedTrajectory,
                        configControl.TrajectoryTime,
                        record: true);

        private static IController? CreateController(string name) => name switch
        {
            BasicControllerName => new BasicController(),
            _                   => null,
        };

        private static ITrajectory? CreateTrajectory(string name) => name switch
        {
            QuinticName => new PolyTrajectory(),
            _           => null,
        };

        /// <summary>
        /// Builds the control loop for <paramref name="controllerName"/>, reusing
        /// the one already installed when it is the same.
        ///
        /// <para>Rebuilt rather than reconfigured because installing a controller
        /// applies its <see cref="IController.RequiredMode"/>, and a mode change
        /// lives in EEPROM — it needs torque off, which on this arm means letting
        /// go of it.</para>
        /// </summary>
        private bool EnsureController(string controllerName, string trajectoryName)
        {
            IController? controller = CreateController(controllerName);
            if (controller == null)
            {
                Warn($"El controlador \"{controllerName}\" todavía no está " +
                     "implementado en ViperCore.");
                return false;
            }

            ITrajectory? trajectory = CreateTrajectory(trajectoryName);
            if (trajectory == null)
            {
                Warn($"La trayectoria \"{trajectoryName}\" todavía no está " +
                     "implementada en ViperCore.");
                return false;
            }

            if (_motion != null && _installedController == controllerName)
            {
                // Same controller, fresh trajectory: a new motion re-initialises
                // it from the arm's present pose anyway.
                _motion.SetTrajectory(trajectory);
                return true;
            }

            // A different controller may want a different mode. If the arm is up
            // and energised, the switch drops it.
            if (_device!.Mode != controller.RequiredMode &&
                _device.ReadTorqueEnabled() == true)
            {
                DialogResult answer = MessageBox.Show(
                    $"El brazo está en modo {_device.Mode} y el controlador " +
                    $"\"{controllerName}\" requiere modo {controller.RequiredMode}." +
                    Environment.NewLine + Environment.NewLine +
                    "El modo de operación vive en EEPROM, así que cambiarlo exige " +
                    "apagar el par. El brazo no tiene frenos: se va a desplomar " +
                    "durante el cambio." +
                    Environment.NewLine + Environment.NewLine +
                    "¿Continuar?",
                    "Cambio de modo", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (answer != DialogResult.Yes) return false;
            }

            _motion?.Dispose();
            _motion              = null;
            _installedController = null;

            var motion = new JointController(_device, MotionSampleTime);
            if (!motion.SetController(controller))
            {
                MessageBox.Show(
                    $"No se pudo instalar el controlador \"{controllerName}\"." +
                    Environment.NewLine + Environment.NewLine + _device.LastError,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                motion.Dispose();
                return false;
            }

            motion.SetTrajectory(trajectory);
            motion.SettleTime = SettleSeconds;
            motion.Fault     += OnMotionFault;
            motion.Sampled   += OnSampled;

            // Profile acceleration stays at zero — no limit. The trajectory
            // already shapes the acceleration across the whole move; limiting it
            // again inside each tick would only ramp the motor up and down
            // within the tick and fight the reference.
            //
            // Profile velocity is the opposite case and is set per motion, in
            // SetProfileForMove: it is what carries the motor smoothly from one
            // setpoint to the next.
            if (!_device.SetProfileAcceleration(0))
            {
                MessageBox.Show(
                    "No se pudo configurar la aceleración de perfil." +
                    Environment.NewLine + Environment.NewLine + _device.LastError,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                motion.Dispose();
                return false;
            }

            _motion              = motion;
            _installedController = controllerName;

            // SetController may have changed the mode, so the bar is refreshed
            // rather than left showing what the arm was in at connect.
            modeLabelValue.Text = _device.Mode.ToString();
            return true;
        }

        private bool StartMotion(double[] qfReal, string what, string controllerName,
                                string trajectoryName, double tf, bool record)
        {
            if (_device == null)
            {
                Warn($"Conecte el Viper X-300S antes de usar \"{what}\".");
                return false;
            }

            if (_motion?.IsRunning == true)
            {
                Warn("Hay un movimiento en curso. Espere a que termine.");
                return false;
            }

            if (qfReal.Length != ViperDevice.JointMotorIds.Length)
            {
                Warn($"\"{what}\" espera {ViperDevice.JointMotorIds.Length} valores " +
                     $"articulares, recibió {qfReal.Length}.");
                return false;
            }

            // ── The one place real degrees become Dynamixel degrees ──────────
            //
            // Every target reaches the arm through here — Home, the rest pose and
            // Destination all call this — so this single line is the whole
            // boundary. Below it everything is the motor's convention, including
            // the profile calculation, which compares against a reading that
            // never left it.
            double[] qf = ViperJointConvention.ToDynamixelDeg(qfReal);

            if (!EnsureController(controllerName, trajectoryName)) return false;
            if (!SetProfileForMove(qf, tf)) return false;

            _motionFault = null;
            _motionName  = what;

            // The loop's clock restarts here, so the decimator's watermark has to
            // as well or the first samples of the move are dropped.
            _lastPlotT = double.NegativeInfinity;

            // Inside a program the plots belong to the program, not to this move:
            // clearing them, or restarting their time axis, is what made each line
            // of a routine erase the one before it. Outside one, this move owns
            // them and starts them fresh.
            if (_gcodeRunning)
            {
                _plotTimeOffset = (DateTime.UtcNow - _gcodeStart).TotalSeconds;
            }
            else
            {
                _plotTimeOffset = 0.0;
                lock (_pendingLock) _pending.Clear();
                if (record) plotsControl.BeginRecording();
            }

            _recording = record;

            // MoveTo energises the arm holding its present pose before the first
            // tick, so this does not need the torque button to have been used.
            if (!_motion!.MoveTo(qf, tf))
            {
                _recording = false;
                plotsControl.StopRecording();
                MessageBox.Show(
                    $"No se pudo iniciar \"{what}\"." +
                    Environment.NewLine + Environment.NewLine +
                    (_motionFault ?? _device.LastError),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetRobotState("Movimiento no iniciado", Color.Red);
                RefreshTorqueStatus();
                return false;
            }

            SetRobotState($"{what}...", Color.Orange);

            // MoveTo energised the arm, so the state is known without asking —
            // and from here on the loop owns the bus until it stops.
            SetTorqueLabel(true);
            _pump.Start();
            return true;
        }

        /// <summary>
        /// Sets the motor's own profile velocity to just above the peak of the
        /// trajectory about to be run.
        ///
        /// <para>This is what makes the motion continuous. The loop hands the
        /// motor a new Goal Position every tick, and the profile limit decides
        /// what it does in between: left at zero — "no limit" — the motor slams
        /// to each setpoint as fast as it can and then waits out the rest of the
        /// period, which is a staircase you can feel. Set near the speed the
        /// trajectory actually asks for, the motor is still travelling when the
        /// next setpoint lands.</para>
        ///
        /// <para>A quintic peaks at <c>1.875·d/tf</c>, in the middle of the move
        /// and near double its own average, so the peak is what the limit has to
        /// clear — sizing it off the average would saturate the motor exactly
        /// where it is moving fastest.</para>
        /// </summary>
        private bool SetProfileForMove(double[] qf, double tf)
        {
            // Fresh, not LatestState: the loop is not running, so nothing has
            // refreshed that snapshot since the last motion ended.
            JointState now = _device!.ReadState();
            if (!now.Valid)
            {
                MessageBox.Show(
                    "No se pudo leer la posición actual para calcular el perfil." +
                    Environment.NewLine + Environment.NewLine + _device.LastError,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // The joint that travels furthest sets the pace: every joint runs
            // the same tf, so that one peaks highest and the others sit below
            // the limit anyway.
            double furthest = 0.0;
            for (int i = 0; i < qf.Length; i++)
                furthest = Math.Max(furthest, Math.Abs(qf[i] - now.Position[i]));

            double peak    = 1.875 * furthest / tf;
            double profile = Math.Max(peak * ProfileMargin, MinProfileVelocity);

            if (_device.SetProfileVelocity(profile)) return true;

            MessageBox.Show(
                "No se pudo configurar la velocidad de perfil." +
                Environment.NewLine + Environment.NewLine + _device.LastError,
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        /// <summary>
        /// Tool-tip position in centimetres for a pose given in Dynamixel
        /// degrees — the panel's unit on the way in, the readout's on the way
        /// out. The conversion to the model's radians happens here, once,
        /// because that is the only convention the kinematics speaks.
        ///
        /// <para>For the UI thread: allocates freely, and at 50 Hz on a thread
        /// that is already painting, that costs nothing worth saving. The loop
        /// thread uses <see cref="CartesianCmOnLoop"/> instead.</para>
        /// </summary>
        private static double[] CartesianCm(double[] qDxlDeg)
        {
            double[] qRad = ViperJointConvention.ToModel(qDxlDeg);
            double[] pM   = ViperKinematics.Forward(qRad);
            return [pM[0] * 100.0, pM[1] * 100.0, pM[2] * 100.0];
        }

        /// <summary>
        /// Tool orientation the way the panel states it — unit direction then the
        /// spin ψ about it, four numbers — for a pose given in Dynamixel degrees.
        /// Same trip through the kinematics as <see cref="CartesianCm"/>, reading
        /// the rotation instead of the last column.
        ///
        /// <para>UI thread only, and allocating like its sibling: 50 Hz on a
        /// thread that is already painting.</para>
        /// </summary>
        private static double[] OrientationDirPsi(double[] qDxlDeg)
        {
            double[] qRad = ViperJointConvention.ToModel(qDxlDeg);
            double[,] t   = ViperKinematics.Pose(qRad);

            // Pose is 4x4; the orientation convention wants the rotation alone.
            var r = new double[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    r[i, j] = t[i, j];

            (double[] dir, double psi) = ViperOrientationConvention.ToDirection(r);

            return [dir[0], dir[1], dir[2], psi];
        }

        /// <summary>
        /// The same conversion on the loop thread, through the buffers reserved
        /// once in the fields. Only the returned three doubles are allocated, and
        /// those have to be: the sample outlives the tick.
        ///
        /// <para><b>Loop thread only</b> — the buffers are not guarded.</para>
        /// </summary>
        private double[] CartesianCmOnLoop(double[] qDxlDeg)
        {
            ViperJointConvention.ToModel(qDxlDeg, _loopQRad);
            ViperKinematics.Forward(_loopQRad, _loopPM);
            return [_loopPM[0] * 100.0, _loopPM[1] * 100.0, _loopPM[2] * 100.0];
        }

        /// <summary>Fires on the loop thread — only stores the message.</summary>
        private void OnMotionFault(string message) => _motionFault = message;

        /// <summary>
        /// Fires on the loop thread, inside the control period.
        ///
        /// <para>The argument's arrays are reused between ticks, so everything
        /// kept is copied here and now. Decimated on elapsed time so the plots
        /// hold the same density whatever rate the loop achieves.</para>
        /// </summary>
        private void OnSampled(ControlInput input)
        {
            if (!_recording) return;
            if (input.T - _lastPlotT < PlotSamplePeriod) return;
            _lastPlotT = input.T;

            // Positions are shifted to real degrees so the plots read in the same
            // unit as the boxes; the copy the shift makes is the copy the sample
            // needed anyway. Velocities, accelerations and the error curve are
            // differences, so the 180° cancels and they are cloned untouched.
            var sample = new PlotSample(
                input.T + _plotTimeOffset,
                ViperJointConvention.ToRealDeg(input.Q),
                (double[])input.Qp.Clone(),
                (double[])input.Qpp.Clone(),
                ViperJointConvention.ToRealDeg(input.Qd),
                (double[])input.Qpd.Clone(),
                (double[])input.Qppd.Clone(),
                CartesianCmOnLoop(input.Q));

            lock (_pendingLock) _pending.Add(sample);
        }

        /// <summary>
        /// Drains samples into the plots, keeps the rate on the status bar, and
        /// notices when the loop has finished. Runs on the UI thread.
        /// </summary>
        private void PumpTick(object? sender, EventArgs e)
        {
            DrainSamples();

            JointController? motion = _motion;
            string?          fault  = _motionFault;

            if (motion?.IsRunning == true)
            {
                string rate = motion.ActualRateHz > 0.0
                    ? $" ({motion.ActualRateHz:F0} Hz)"
                    : string.Empty;

                SetRobotState(
                    fault != null ? $"{_motionName}: {fault}" : $"{_motionName}...{rate}",
                    fault != null ? Color.Red : Color.Orange);
                return;
            }

            // Finished. One last drain, because the loop may have appended
            // between the drain above and its exit.
            _recording = false;
            DrainSamples();

            // Inside a program this was one line of several. The pump stays
            // running so the encoder tick's samples keep reaching the plots
            // through the G4 pauses, and the recording and the status bar stay
            // the program's to close.
            if (_gcodeRunning) return;

            _pump.Stop();
            RefreshTorqueStatus();
            plotsControl.StopRecording();

            string achieved = motion is { ActualRateHz: > 0.0 }
                ? $" — {motion.ActualRateHz:F0} Hz, peor tick {motion.WorstTickMs:F1} ms, " +
                  $"{motion.TickCount} ticks" +
                  (motion.FaultCount > 0 ? $", {motion.FaultCount} fallidos" : string.Empty)
                : string.Empty;

            if (fault != null)
                SetRobotState($"{_motionName} terminó con fallos: {fault}", Color.Red);
            else if (motion?.IsCompleted == true)
                SetRobotState($"{_motionName}: completado{achieved}", Color.Green);
            else
                SetRobotState("Movimiento detenido", Color.Gray);
        }

        /// <summary>
        /// One plot sample built from an encoder read instead of a control tick,
        /// for the stretches of a program where no move is running.
        ///
        /// <para>The arm is at rest here, so the accelerations are zero and so are
        /// the desired derivatives — that is not a filler value, it is what the
        /// trajectory is doing. The <b>desired position holds its last commanded
        /// value</b> for the same reason: the loop stopped writing goals, so the
        /// motors are still holding the last one it wrote. Keeping it flat is what
        /// the arm is actually being told.</para>
        ///
        /// <para>Runs on the UI thread at 50 Hz. It appends to the same queue the
        /// loop uses and lets the pump drain it, so nothing repaints here.</para>
        /// </summary>
        private void RecordIdleSample(JointState state)
        {
            if (!plotsControl.IsRecording) return;

            double[] qReal = ViperJointConvention.ToRealDeg(state.Position);

            // Before the first move of a program there is nothing commanded yet,
            // so the arm's own pose is the honest stand-in.
            double[] qd = _lastDesiredReal ?? qReal;

            var zeros = new double[qReal.Length];

            var sample = new PlotSample(
                (DateTime.UtcNow - _gcodeStart).TotalSeconds,
                qReal,
                state.Velocity.Length == qReal.Length
                    ? (double[])state.Velocity.Clone()
                    : zeros,
                zeros,
                qd,
                new double[qReal.Length],
                new double[qReal.Length],
                CartesianCm(state.Position));

            lock (_pendingLock) _pending.Add(sample);
        }

        private void DrainSamples()
        {
            PlotSample[] batch;
            lock (_pendingLock)
            {
                if (_pending.Count == 0) return;
                batch = [.. _pending];
                _pending.Clear();
            }

            foreach (PlotSample s in batch)
                plotsControl.AddSample(s.T, s.Q, s.Qp, s.Qpp, s.Qd, s.Qpd, s.Qppd, s.PCm);

            // What the last tick was told to aim for, so the stretches between
            // the moves of a program can hold it instead of leaving the desired
            // curve blank. UI thread only, like everything else here.
            _lastDesiredReal = batch[^1].Qd;

            plotsControl.RefreshPlots();
        }

        // ── G-code ───────────────────────────────────────────────────────────

        /// <summary>
        /// The panel's own <see cref="IGCodeRobot"/>. Nested rather than a
        /// separate class because every line of it is a call back into this
        /// panel: the Viper has no adapter between the UI and the arm, which is
        /// exactly why the interpreter had to stop asking for one.
        /// </summary>
        private sealed class GCodeRobot(ViperControls owner) : IGCodeRobot
        {
            /// <summary>Slack over the motion so the next line is never refused.</summary>
            private const double Margin = 0.25;

            /// <summary>Six axes, so six letters.</summary>
            public string AxisLetters => "ABCDEF";

            // The loop keeps holding the target for SettleSeconds after the
            // trajectory ends, and StartMotion refuses while it runs. Waiting
            // only tf would put the next line into that refusal every time.
            public double MoveWaitSecs =>
                owner.configControl.TrajectoryTime + SettleSeconds + Margin;

            public double HomeWaitSecs => FixedMotionTime + SettleSeconds + Margin;

            public bool MoveToJoints(double[] q) =>
                owner.StartMotion(q, "G-code", owner.configControl.SelectedController,
                                  owner.configControl.SelectedTrajectory,
                                  owner.configControl.TrajectoryTime, record: true);

            public bool GoHome() => owner.GoHome([0.0, 0.0, 0.0, 0.0, 0.0, 0.0]);

            /// <summary>
            /// <c>X Y Z</c> in cm, and the orientation the panel's own boxes take:
            /// <c>I J K</c> for the tool direction in the base frame, <c>R</c> for
            /// the spin about it.
            ///
            /// <para>Both are optional and default to <b>pointing straight down
            /// with no spin</b> — the common case, and the one a line that names
            /// only a point almost certainly means.</para>
            /// </summary>
            public bool TryCartesianToJoints(
                IReadOnlyDictionary<char, double> words, out double[] q, out string reason)
            {
                q = [];

                double[] dir =
                [
                    words.GetValueOrDefault('I',  0.0),
                    words.GetValueOrDefault('J',  0.0),
                    words.GetValueOrDefault('K', -1.0),
                ];

                if (!ViperOrientationConvention.TryFromDirection(
                        dir, words.GetValueOrDefault('R', 0.0), out double[,] r06, out reason))
                    return false;

                double[] pTip =
                [
                    words['X'] / 100.0, words['Y'] / 100.0, words['Z'] / 100.0,
                ];

                var qRad = new double[ViperKinematics.JointCount];
                if (!ViperKinematics.Inverse(pTip, r06, qRad, out reason)) return false;

                q = ViperJointConvention.ModelRadToRealDeg(qRad);
                return true;
            }

            /// <summary>
            /// The gripper is not wired as an end effector yet, so M64 and M65
            /// would silently do nothing. Saying so is the whole point of the
            /// change that brought the interpreter here.
            /// </summary>
            public void SetEndEffectorState(bool on) =>
                throw new InvalidOperationException(
                    "M64/M65: el efector final del Viper todavía no está conectado.");
        }

        private async void OnExecuteGCodeRequested(object? sender, string code)
        {
            if (_device == null)
            {
                Warn("Conecte el Viper X-300S antes de ejecutar un programa.");
                return;
            }

            _gcodeCts?.Cancel();
            _gcodeCts = new CancellationTokenSource();

            var log = new List<string>();
            SetRobotState("Ejecutando instrucciones...", Color.Orange);

            // One recording for the whole program. Every move inside it appends,
            // offset by how long the program has been running, so the routine
            // ends up as a single continuous trace instead of the last move
            // having erased all the ones before it.
            _gcodeRunning = true;
            _gcodeStart   = DateTime.UtcNow;
            _plotTimeOffset = 0.0;
            lock (_pendingLock) _pending.Clear();
            plotsControl.BeginRecording();

            try
            {
                await new GCodeInterpreter(new GCodeRobot(this))
                    .RunAsync(code, log.Add, _gcodeCts.Token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _gcodeRunning = false;
                _recording    = false;
                DrainSamples();
                plotsControl.StopRecording();
                plotsControl.RefreshPlots();
            }

            // The interpreter reports failures through the log instead of
            // throwing, so the last line is what actually happened.
            string last = log.Count > 0 ? log[^1] : "Programa vacío.";
            bool ok = last.StartsWith("Programa completado", StringComparison.Ordinal);

            SetRobotState(ok ? "Programa completado" : last,
                          ok ? Color.Green : Color.Red);
        }

        // ── Emergency stop ───────────────────────────────────────────────────

        private void bttnStop_Click(object sender, EventArgs e) => EmergencyStop();

        /// <summary>
        /// <b>Ctrl+Space</b> is the emergency stop, wherever the focus is inside
        /// this panel.
        ///
        /// <para><see cref="ProcessCmdKey"/> and not a <c>KeyDown</c> handler
        /// because this has to fire from the text boxes too: a stop that does not
        /// work while you are typing is a stop you cannot rely on. Returning true
        /// swallows the key so no button with the focus also fires on it.</para>
        ///
        /// <para><b>It was the bare space bar and that was wrong.</b> Swallowing
        /// space everywhere in the panel made the G-code box impossible to type
        /// in — <c>G0 A0 B0</c> needs spaces. Ctrl+Space collides with nothing
        /// here and is still a single gesture.</para>
        ///
        /// <para>Ctrl has to be down; the other modifiers are ignored. Someone
        /// reaching for this is not checking whether they are leaning on
        /// shift.</para>
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.KeyCode) == Keys.Space &&
                (keyData & Keys.Control) == Keys.Control)
            {
                EmergencyStop();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Stops the loop, then freezes the arm where it stands.
        ///
        /// <para><b>The order is the whole thing.</b> Freezing first would put
        /// this thread on the serial port while the loop is still ticking on it,
        /// which is §7.4 — two threads reading each other's replies. So the loop
        /// is stopped and joined first; only then is the bus ours to write the
        /// hold to. Stopping the loop alone is not a stop: it leaves the motors
        /// servoing to the last setpoint they were given.</para>
        /// </summary>
        private void EmergencyStop()
        {
            // Stop is allowed to take up to half a second joining the loop
            // thread, and Join pumps messages on an STA thread — so a second
            // second Ctrl+Space can land inside the first one.
            if (_stopping) return;
            if (_device == null)
            {
                SetRobotState("Paro de emergencia: no hay brazo conectado", Color.Gray);
                return;
            }

            _stopping = true;
            try
            {
                // Before anything else, because a program is a queue of moves:
                // stopping only the one in flight would let the next line fire
                // the moment its wait expired — the arm moving again seconds
                // after someone hit the emergency stop.
                _gcodeCts?.Cancel();
                _gcodeRunning = false;

                _motion?.Stop();

                bool frozen = _device.EmergencyStop();

                // The tidy-up PumpTick would have done, done here instead so the
                // status bar ends up saying that this was a stop and not that the
                // movement merely finished.
                _recording = false;
                DrainSamples();
                plotsControl.StopRecording();
                _pump.Stop();

                if (frozen)
                {
                    SetRobotState("PARO DE EMERGENCIA — brazo sujeto", Color.Red);
                    SetTorqueLabel(true);
                    RefreshHardwareErrors(force: true);
                    return;
                }

                // A refusal here is the §4.3 hole: outside the position modes
                // there is no pose to take hold of, and the loop having stopped
                // does not stop a motor that was given a current or a PWM. That
                // has to be loud — it is the one case where the arm may still be
                // moving after a stop.
                SetRobotState("PARO DE EMERGENCIA FALLIDO", Color.Red);
                RefreshTorqueStatus();

                MessageBox.Show(
                    "El brazo NO quedó sujeto." + Environment.NewLine + Environment.NewLine +
                    _device.LastError + Environment.NewLine + Environment.NewLine +
                    "Corte la alimentación del brazo si sigue moviéndose.",
                    "Paro de emergencia", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _stopping = false;
            }
        }

        // ── Status bar ───────────────────────────────────────────────────────

        private void ShowConnected(string port, int baud)
        {
            bttnConnect.Text = "Desconectar";
            tbxPort.Enabled  = false;
            cmbBaud.Enabled  = false;
            SetActionButtonsEnabled(true);

            statusLabelValue.Text      = "Conectado";
            statusLabelValue.ForeColor = Color.Green;

            portLabelValue.Text      = $"{port} @ {baud}";
            portLabelValue.ForeColor = SystemColors.ControlText;

            modeLabelValue.Text      = _device?.Mode.ToString() ?? "—";
            modeLabelValue.ForeColor = SystemColors.ControlText;

            // Read, not assumed: the motors may well have been left energised by
            // a previous session, and an arm you think is limp but is not is the
            // wrong surprise to hand someone reaching for it.
            RefreshTorqueStatus();

            SetRobotState("Inactivo", Color.Gray);
        }

        private void ShowDisconnected()
        {
            bttnConnect.Text = "Conectar";
            tbxPort.Enabled  = true;
            cmbBaud.Enabled  = true;
            SetActionButtonsEnabled(false);

            statusLabelValue.Text      = "Desconectado";
            statusLabelValue.ForeColor = Color.Red;

            portLabelValue.Text      = "—";
            portLabelValue.ForeColor = SystemColors.GrayText;

            modeLabelValue.Text      = "—";
            modeLabelValue.ForeColor = SystemColors.GrayText;

            // Disconnected says nothing about the motors: closing the port
            // leaves torque exactly as it was.
            torqueLabelValue.Text      = "—";
            torqueLabelValue.ForeColor = SystemColors.GrayText;
            bttnTorque.Text            = "Habilitar par";

            faultLabelValue.Text      = "—";
            faultLabelValue.ForeColor = SystemColors.GrayText;

            SetRobotState("—", Color.Gray);
            encodersControl.ReadButtonText = "Leer encoders";
            encodersControl.ClearDisplay();
        }

        private void SetActionButtonsEnabled(bool enabled)
        {
            bttnTorque.Enabled = enabled;
            bttnStop.Enabled   = enabled;
            bttnRest.Enabled   = enabled;
        }

        /// <summary>
        /// Reads Hardware Error Status on every motor and puts it on the bar.
        /// One transaction per motor, so this runs at connect and when a read
        /// starts or stops working — not on every tick, and no more than once a
        /// second unless <paramref name="force"/> says otherwise.
        /// </summary>
        private void RefreshHardwareErrors(bool force = false)
        {
            // The loop owns the bus while it runs; this would interleave its own
            // transactions with the loop's ticks.
            if (_motion?.IsRunning == true) return;

            DateTime now = DateTime.UtcNow;
            if (!force && now - _lastFaultProbe < FaultProbeInterval) return;
            _lastFaultProbe = now;

            Dictionary<byte, HardwareError>? errors = _device?.ReadHardwareErrors();
            if (errors == null)
            {
                faultLabelValue.Text      = "Sin leer";
                faultLabelValue.ForeColor = SystemColors.GrayText;
                return;
            }

            List<string> latched = errors
                .Where(e => e.Value != HardwareError.None)
                .Select(e => $"ID {e.Key}: {e.Value}")
                .ToList();

            if (latched.Count == 0)
            {
                faultLabelValue.Text      = "Ninguno";
                faultLabelValue.ForeColor = SystemColors.GrayText;
            }
            else
            {
                faultLabelValue.Text      = string.Join("; ", latched);
                faultLabelValue.ForeColor = Color.Red;
            }
        }

        private void RefreshTorqueStatus() => SetTorqueLabel(_device?.ReadTorqueEnabled());

        /// <summary>
        /// Paints a torque state already known, without asking the bus. Used
        /// while the control loop is running: it owns the port, and reads from
        /// the UI thread would be transactions interleaved with its ticks.
        /// </summary>
        private void SetTorqueLabel(bool? on)
        {
            if (on == null)
            {
                torqueLabelValue.Text      = "Desconocido";
                torqueLabelValue.ForeColor = Color.Gray;
                bttnTorque.Text            = "Habilitar par";
                return;
            }

            torqueLabelValue.Text      = on.Value ? "Habilitado" : "Deshabilitado";
            torqueLabelValue.ForeColor = on.Value ? Color.Green : Color.Gray;
            bttnTorque.Text            = on.Value ? "Quitar par" : "Habilitar par";
        }

        private void SetRobotState(string text, Color color)
        {
            stateLabelValue.Text      = text;
            stateLabelValue.ForeColor = color;
        }

        private static void Warn(string message) =>
            MessageBox.Show(message, "Viper X-300S",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
