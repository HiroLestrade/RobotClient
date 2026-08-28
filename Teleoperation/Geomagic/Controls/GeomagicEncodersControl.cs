using System.ComponentModel;

namespace Teleoperation
{
    /// <summary>
    /// Read-only joint/Cartesian readout for one Geomagic Touch. Unlike the
    /// single-robot experiment there is no "Leer encoders" button here: both
    /// robots are sampled together from <see cref="TeleoperationForm"/> so the
    /// two readings share the same tick.
    /// </summary>
    public partial class GeomagicEncodersControl : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get => lblTitle.Text;
            set => lblTitle.Text = value;
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
    }
}
