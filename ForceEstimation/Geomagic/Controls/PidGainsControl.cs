using System.ComponentModel;

namespace ForceEstimation
{
    public partial class PidGainsControl : UserControl
    {
        public PidGainsControl() => InitializeComponent();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SampleTimeMs => ParsePositiveInt(tbxSampleTime.Text, 1);

        public double[] GetKp() => ParseRow(tbxKp1.Text, tbxKp2.Text, tbxKp3.Text);
        public double[] GetKi() => ParseRow(tbxKi1.Text, tbxKi2.Text, tbxKi3.Text);
        public double[] GetKd() => ParseRow(tbxKd1.Text, tbxKd2.Text, tbxKd3.Text);

        private static double[] ParseRow(string v1, string v2, string v3)
        {
            double.TryParse(v1, out double g1);
            double.TryParse(v2, out double g2);
            double.TryParse(v3, out double g3);
            return [g1, g2, g3];
        }

        private static int ParsePositiveInt(string text, int defaultValue)
        {
            if (int.TryParse(text, out int v) && v > 0) return v;
            return defaultValue;
        }
    }
}
