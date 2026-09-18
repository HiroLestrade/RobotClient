namespace ForceEstimation
{
    /// <summary>
    /// Experiment 1. The window is two pieces: a <see cref="RobotSelector"/>
    /// across the top and, below it, whichever control panel belongs to the
    /// robot that is selected.
    ///
    /// The form's only job is that swap. Everything a given robot needs lives in
    /// its own panel, so adding a robot means writing a panel and adding a case
    /// to <see cref="BuildPanel"/> — not touching this window's layout.
    /// </summary>
    public partial class ForceEstimationForm : Form
    {
        private const string GeomagicTouch = "Geomagic Touch";
        private const string ViperX300S    = "Viper X-300S";

        /// <summary>The panel currently in <c>pnlHost</c>, if any.</summary>
        private UserControl? _panel;

        public ForceEstimationForm()
        {
            InitializeComponent();

            robotSelector.SelectionChanged += (_, robot) => BuildPanel(robot);
            robotSelector.SetRobots(GeomagicTouch, ViperX300S);
        }

        /// <summary>
        /// Tears down the panel that is up and builds the one for
        /// <paramref name="robot"/>. Disposing matters: the panel owns the
        /// device connection and its control loop.
        /// </summary>
        private void BuildPanel(string robot)
        {
            pnlHost.Controls.Clear();
            _panel?.Dispose();
            _panel = null;

            _panel = robot switch
            {
                GeomagicTouch => new GeomagicControls(),
                ViperX300S    => new ViperControls(),
                _             => null,
            };

            if (_panel == null) return;

            _panel.Dock = DockStyle.Fill;
            pnlHost.Controls.Add(_panel);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _panel?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
