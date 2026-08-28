namespace ForceEstimation
{
    public partial class ForceEstimationForm : Form
    {
        private GeomagicControl? _geomagicControl;

        public ForceEstimationForm()
        {
            InitializeComponent();
            cmbRobot.SelectedIndex = 0;
        }

        private void cmbRobot_SelectedIndexChanged(object sender, EventArgs e)
        {
            _geomagicControl?.Dispose();
            _geomagicControl = null;
            pnlHost.Controls.Clear();

            if (cmbRobot.SelectedItem?.ToString() == "Geomagic Touch")
            {
                _geomagicControl = new GeomagicControl { Dock = DockStyle.Fill };
                pnlHost.Controls.Add(_geomagicControl);
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _geomagicControl?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
