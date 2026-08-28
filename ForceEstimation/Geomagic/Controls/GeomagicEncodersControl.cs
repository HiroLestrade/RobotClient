using System.ComponentModel;

namespace ForceEstimation
{
    public partial class GeomagicEncodersControl : UserControl
    {
        public event EventHandler? ReadClicked;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ReadButtonText
        {
            get => bttnReadEncoders.Text;
            set => bttnReadEncoders.Text = value;
        }

        public GeomagicEncodersControl() => InitializeComponent();

        public void UpdateDisplay(double q1deg, double q2deg, double q3deg,
                                  double xcm,  double ycm,  double zcm)
        {
            tbxArtQ1.Text = $"{q1deg:F2}";
            tbxArtQ2.Text = $"{q2deg:F2}";
            tbxArtQ3.Text = $"{q3deg:F2}";
            tbxCartX.Text = $"{xcm:F2}";
            tbxCartY.Text = $"{ycm:F2}";
            tbxCartZ.Text = $"{zcm:F2}";
        }

        private void bttnReadEncoders_Click(object sender, EventArgs e) =>
            ReadClicked?.Invoke(this, EventArgs.Empty);
    }
}
