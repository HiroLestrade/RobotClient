namespace ForceEstimation
{
    public partial class GeomagicHomeControl : UserControl
    {
        public event EventHandler<double[]>? GoHomeRequested;

        public GeomagicHomeControl() => InitializeComponent();

        private void bttnGoHome_Click(object sender, EventArgs e)
        {
            double[]? qf = ParseTarget(
                radHomeQ.Checked,
                tbxHomeQi1.Text, tbxHomeQi2.Text, tbxHomeQi3.Text,
                tbxHomeXi.Text,  tbxHomeYi.Text,  tbxHomeZi.Text);
            if (qf != null)
                GoHomeRequested?.Invoke(this, qf);
        }

        private void radMode_CheckedChanged(object sender, EventArgs e)
        {
            bool q = radHomeQ.Checked;
            tbxHomeQi1.Enabled = q;  tbxHomeQi2.Enabled = q;  tbxHomeQi3.Enabled = q;
            tbxHomeXi.Enabled  = !q; tbxHomeYi.Enabled  = !q; tbxHomeZi.Enabled  = !q;
        }

        private static double[]? ParseTarget(
            bool qMode,
            string q1, string q2, string q3,
            string x,  string y,  string z)
        {
            if (qMode)
            {
                if (!double.TryParse(q1, out double q1deg) ||
                    !double.TryParse(q2, out double q2deg) ||
                    !double.TryParse(q3, out double q3deg))
                {
                    MessageBox.Show("Los valores deben ser números válidos.",
                        "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                return [q1deg * Math.PI / 180.0, q2deg * Math.PI / 180.0, q3deg * Math.PI / 180.0];
            }
            else
            {
                if (!double.TryParse(x, out double xcm) ||
                    !double.TryParse(y, out double ycm) ||
                    !double.TryParse(z, out double zcm))
                {
                    MessageBox.Show("Los valores deben ser números válidos.",
                        "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                double[]? qf = GeomagicModel.InverseKinematics(
                    [xcm / 100.0, ycm / 100.0, zcm / 100.0]);
                if (qf == null)
                    MessageBox.Show("La posición cartesiana está fuera del espacio de trabajo.",
                        "Cinemática inversa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return qf;
            }
        }
    }
}
