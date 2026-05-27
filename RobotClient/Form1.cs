namespace RobotClient
{
    public partial class Form1 : Form
    {
        private GeomagicControl? _geomagicControl;

        public Form1()
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
