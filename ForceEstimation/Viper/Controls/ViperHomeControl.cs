namespace ForceEstimation
{
    /// <summary>
    /// Home target for the Viper X-300S. Six joints instead of the Geomagic's
    /// three, and the joint values are <b>real degrees</b>: 0 at centre, ±180 at
    /// the ends of the motor's travel, negative on the other side of centre. So
    /// Home is six zeros. The motors' own 0–360 convention stops at
    /// <see cref="ViperControls"/>, which converts on the way out.
    ///
    /// <para>The Cartesian mode is laid out but not usable yet: ViperCore has no
    /// inverse kinematics, so the X option reports that rather than pretending
    /// to convert.</para>
    /// </summary>
    public partial class ViperHomeControl : UserControl
    {
        /// <summary>Raised with six joint targets in real degrees, 0 at centre.</summary>
        public event EventHandler<double[]>? GoHomeRequested;

        public ViperHomeControl()
        {
            InitializeComponent();
            radMode_CheckedChanged(this, EventArgs.Empty);
        }

        private void bttnGoHome_Click(object sender, EventArgs e)
        {
            if (ViperTargetParser.TryParse(
                    radQ.Checked,
                    [tbxQ1, tbxQ2, tbxQ3, tbxQ4, tbxQ5, tbxQ6],
                    [tbxX, tbxY, tbxZ, tbxVx, tbxVy, tbxVz, tbxPsi],
                    out double[] qf))
            {
                GoHomeRequested?.Invoke(this, qf);
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
}
