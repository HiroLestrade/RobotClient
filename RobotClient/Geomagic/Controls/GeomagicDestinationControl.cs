namespace RobotClient
{
    public partial class GeomagicDestinationControl : UserControl
    {
        public event EventHandler<double[]>? GoFinalRequested;

        public GeomagicDestinationControl() => InitializeComponent();

        private void bttnGoFinal_Click(object sender, EventArgs e)
        {
            double[]? qf = ParseTarget(
                radFinalQ.Checked,
                tbxFinalQf1.Text, tbxFinalQf2.Text, tbxFinalQf3.Text,
                tbxFinalXf.Text,  tbxFinalYf.Text,  tbxFinalZf.Text);
            if (qf != null)
                GoFinalRequested?.Invoke(this, qf);
        }

        private void radMode_CheckedChanged(object sender, EventArgs e)
        {
            bool q = radFinalQ.Checked;
            tbxFinalQf1.Enabled = q;  tbxFinalQf2.Enabled = q;  tbxFinalQf3.Enabled = q;
            tbxFinalXf.Enabled  = !q; tbxFinalYf.Enabled  = !q; tbxFinalZf.Enabled  = !q;
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
