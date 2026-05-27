namespace RobotClient
{
    public partial class GeomagicControl : UserControl, IRobotHost
    {
        private readonly IRobotAdapter    _adapter;
        private readonly GCodeInterpreter _interpreter;
        private CancellationTokenSource?  _gcodeCts;

        private bool     _showElapsed;
        private DateTime _motionStart;
        private System.Windows.Forms.Timer? _elapsedTimer;
        private bool     _interpreterRunning;
        private DateTime _interpreterRecordingStart;

        public GeomagicControl()
        {
            InitializeComponent();

            plotsControl.GetExportMetadata =
                () => (configControl.SelectedController, configControl.SelectedTrajectory);

            _adapter     = new GeomagicAdapter(this, configControl);
            _interpreter = new GCodeInterpreter(_adapter, configControl);

            encodersControl.ReadClicked         += (_, _)  => _adapter.ToggleReadEncoders();
            homeControl.GoHomeRequested         += (_, qf) => _adapter.GoHome(qf);
            destinationControl.GoFinalRequested += (_, qf) => _adapter.GoFinal(qf);
            configControl.ExecuteGCodeRequested += OnExecuteGCodeRequested;

            _elapsedTimer = new System.Windows.Forms.Timer { Interval = 50 };
            _elapsedTimer.Tick += (_, _) =>
            {
                if (_showElapsed)
                    timeLabelValue.Text = $"t = {(DateTime.UtcNow - _motionStart).TotalSeconds:F2} [s]";
            };
            _elapsedTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _gcodeCts?.Cancel();
                _gcodeCts?.Dispose();
                _elapsedTimer?.Dispose();
                _adapter.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        // ── IRobotHost ───────────────────────────────────────────────────────────

        bool IRobotHost.IsCalibrated => calibrationLabelValue.Text == "Calibrado";

        void IRobotHost.SetConnectionStatus(bool connected)
        {
            statusLabelValue.Text      = connected ? "Conectado"   : "Desconectado";
            statusLabelValue.ForeColor = connected ? Color.Green    : Color.Red;
        }

        void IRobotHost.SetCalibrationStatus(bool calibrated)
        {
            calibrationLabelValue.Text      = calibrated ? "Calibrado"  : "No Calibrado";
            calibrationLabelValue.ForeColor = calibrated ? Color.Green   : Color.Red;
        }

        void IRobotHost.SetMotionState(string text, Color color)
        {
            robotStateLabelValue.Text      = text;
            robotStateLabelValue.ForeColor = color;
        }

        void IRobotHost.NotifyMotionStarted()
        {
            _motionStart = DateTime.UtcNow;
            _showElapsed = true;
        }

        void IRobotHost.NotifyGoFinalStarted()
        {
            _motionStart = DateTime.UtcNow;
            _showElapsed = true;
            if (!_interpreterRunning)
                plotsControl.BeginRecording();
            else
                plotsControl.SetDesiredTimeOffset(
                    (DateTime.UtcNow - _interpreterRecordingStart).TotalSeconds);
        }

        void IRobotHost.NotifyMotionCompleted(string completedState)
        {
            _showElapsed = false;
            if (!_interpreterRunning)
            {
                plotsControl.StopRecording();
                robotStateLabelValue.Text      = completedState;
                robotStateLabelValue.ForeColor = Color.Green;
            }
        }

        void IRobotHost.RecordDesiredTrajectory(double t, double[] qd, double[] qpd, double[] qppd) =>
            plotsControl.RecordDesiredPoint(t, qd, qpd, qppd);

        void IRobotHost.UpdateEncoderDisplay(double[] q, double[] p)
        {
            encodersControl.UpdateDisplay(
                q[0] * 180.0 / Math.PI,
                q[1] * 180.0 / Math.PI,
                q[2] * 180.0 / Math.PI,
                p[0] * 100.0,
                p[1] * 100.0,
                p[2] * 100.0);
            plotsControl.RecordSample(q, p);
        }

        void IRobotHost.SetEncoderButtonText(string text) =>
            encodersControl.ReadButtonText = text;

        void IRobotHost.ShowMessage(string message, string title, MessageBoxIcon icon) =>
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);

        void IRobotHost.InvokeOnUI(Action action) => BeginInvoke(action);

        // ── Button handlers ──────────────────────────────────────────────────────

        private void bttnConnect_Click(object sender, EventArgs e)
        {
            string? deviceName = string.IsNullOrWhiteSpace(tbDeviceName.Text)
                ? null : tbDeviceName.Text.Trim();
            _adapter.Connect(deviceName);
        }

        private void bttnCalibrate_Click(object sender, EventArgs e) =>
            _adapter.Calibrate();

        private void bttnDisconnect_Click(object sender, EventArgs e) =>
            _adapter.Disconnect();

        private async void OnExecuteGCodeRequested(object? sender, string code)
        {
            _gcodeCts?.Cancel();
            _gcodeCts = new CancellationTokenSource();

            _interpreterRunning            = true;
            _interpreterRecordingStart     = DateTime.UtcNow;
            robotStateLabelValue.Text      = "Ejecutando";
            robotStateLabelValue.ForeColor = Color.Orange;
            plotsControl.BeginRecording();
            try
            {
                await _interpreter.RunAsync(code, null, _gcodeCts.Token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _interpreterRunning            = false;
                _showElapsed                   = false;
                plotsControl.StopRecording();
                robotStateLabelValue.Text      = "En destino";
                robotStateLabelValue.ForeColor = Color.Green;
            }
        }
    }
}
