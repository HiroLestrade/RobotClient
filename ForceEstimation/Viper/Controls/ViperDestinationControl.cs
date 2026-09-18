using ViperCore;

namespace ForceEstimation
{
    /// <summary>
    /// Destination target for the Viper X-300S. Same shape as
    /// <see cref="ViperHomeControl"/>; kept as a separate control so the two
    /// targets can be edited side by side, which is how the Geomagic panel
    /// works.
    /// </summary>
    public partial class ViperDestinationControl : UserControl
    {
        /// <summary>Raised with six joint targets in real degrees, 0 at centre.</summary>
        public event EventHandler<double[]>? GoFinalRequested;

        public ViperDestinationControl()
        {
            InitializeComponent();
            radMode_CheckedChanged(this, EventArgs.Empty);
        }

        private void bttnGoFinal_Click(object sender, EventArgs e)
        {
            if (ViperTargetParser.TryParse(
                    radQ.Checked,
                    [tbxQ1, tbxQ2, tbxQ3, tbxQ4, tbxQ5, tbxQ6],
                    [tbxX, tbxY, tbxZ, tbxVx, tbxVy, tbxVz, tbxPsi],
                    out double[] qf))
            {
                GoFinalRequested?.Invoke(this, qf);
            }
        }

        private void radMode_CheckedChanged(object sender, EventArgs e)
        {
            bool q = radQ.Checked;
            foreach (var b in new[] { tbxQ1, tbxQ2, tbxQ3, tbxQ4, tbxQ5, tbxQ6 })
                b.Enabled = q;
            foreach (var b in new[] { tbxX, tbxY, tbxZ, tbxVx, tbxVy, tbxVz, tbxPsi })
                b.Enabled = !q;
        }
    }

    /// <summary>
    /// Shared parsing for the Home and Destination panels.
    /// </summary>
    internal static class ViperTargetParser
    {
        /// <summary>
        /// Produces six joint targets in <b>real degrees</b> — zero at centre,
        /// the only convention the panel speaks — from whichever mode is selected.
        /// Returns false and tells the user why if anything is wrong.
        /// </summary>
        /// <param name="joints">q1..q6, real degrees.</param>
        /// <param name="cartesian">
        /// x, y, z in cm, then the tool direction vx, vy, vz in the base frame,
        /// then the spin ψ about it in degrees.
        /// </param>
        public static bool TryParse(
            bool jointMode, TextBox[] joints, TextBox[] cartesian, out double[] qf)
        {
            qf = [];

            if (!jointMode)
                return TryParseCartesian(cartesian, out qf);

            var values = new double[joints.Length];
            for (int i = 0; i < joints.Length; i++)
            {
                if (!double.TryParse(joints[i].Text, out values[i]))
                {
                    MessageBox.Show($"El valor de q{i + 1} no es un número válido.",
                        "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // The motor's full range, 0-360 Dynamixel, is ±180 in real
                // degrees. Anything outside would be clamped silently by the
                // motor, so it is rejected here instead.
                if (values[i] < -180.0 || values[i] > 180.0)
                {
                    MessageBox.Show(
                        $"q{i + 1} = {values[i]:F2}° está fuera del rango del motor.\n" +
                        "El rango es de −180° a 180°, con 0° en el centro.",
                        "Fuera de rango", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            qf = values;
            return true;
        }

        /// <summary>Names of the seven Cartesian boxes, for the error messages.</summary>
        private static readonly string[] CartesianNames =
            ["x", "y", "z", "vx", "vy", "vz", "ψ"];

        /// <summary>
        /// Position and orientation through the inverse kinematics.
        ///
        /// <para><b>Orientation is a direction plus a spin</b>, not three Euler
        /// angles. The vector says where the tool points, in the base frame and at
        /// any length; ψ says how much it is rolled about that direction. That is
        /// the whole orientation, because a direction is two degrees of freedom
        /// and the roll is the third — and it is what a person actually means when
        /// they say "put it here pointing down".</para>
        ///
        /// <para>The panel works in centimetres and degrees, ViperCore in metres
        /// and radians, so both conversions happen here and nowhere else. The pose
        /// that comes back is <b>elbow up</b>; that is not a choice this makes, it
        /// is the one configuration <see cref="ViperKinematics.Inverse"/>
        /// returns.</para>
        /// </summary>
        private static bool TryParseCartesian(TextBox[] boxes, out double[] qf)
        {
            qf = [];

            var v = new double[CartesianNames.Length];
            for (int i = 0; i < v.Length && i < boxes.Length; i++)
            {
                if (!double.TryParse(boxes[i].Text, out v[i]))
                {
                    MessageBox.Show($"El valor de {CartesianNames[i]} no es un número válido.",
                        "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            double[] direction = [v[3], v[4], v[5]];
            if (!ViperOrientationConvention.TryFromDirection(
                    direction, v[6], out double[,] r06, out string bad))
            {
                MessageBox.Show(bad, "Dirección inválida",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            double[] pTip = [v[0] / 100.0, v[1] / 100.0, v[2] / 100.0];

            var qRad = new double[ViperKinematics.JointCount];
            if (!ViperKinematics.Inverse(pTip, r06, qRad, out string reason))
            {
                MessageBox.Show(
                    "Esa pose no es alcanzable." + Environment.NewLine + Environment.NewLine +
                    reason,
                    "Fuera de alcance", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            qf = ViperJointConvention.ModelRadToRealDeg(qRad);
            return true;
        }
    }
}
