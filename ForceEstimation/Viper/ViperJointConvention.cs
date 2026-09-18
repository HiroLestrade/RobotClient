namespace ForceEstimation
{
    /// <summary>
    /// Translates between the two angle conventions this project has to keep
    /// straight, and keeps the translation in exactly one place.
    ///
    /// <list type="bullet">
    /// <item><b>Dynamixel degrees</b> — 0 to 360 with 180° at the motor centre.
    /// What the motors report, what Dynamixel Wizard shows, and what every
    /// ViperCore device call speaks. <b>The user never sees these.</b></item>
    /// <item><b>Real degrees</b> — the same angles with the centre at zero, so
    /// Home is six zeros instead of six 180s and a negative number means the
    /// joint is on the other side of centre. This is what the panel shows and
    /// what its boxes accept.</item>
    /// <item><b>Model radians</b> — the q of the article's D-H table, zero at the
    /// reference configuration. What <see cref="ViperCore.ViperKinematics"/> and
    /// the dynamic model speak.</item>
    /// </list>
    ///
    /// <para>All three differ by the 180° centre and the degree-radian factor:
    /// <c>real = dxl − 180</c> and <c>q = real·π/180</c>. Nothing else — real
    /// degrees and model radians are the same convention in different units.
    /// Both offsets that make the arm's reference pose what it is — 0.437π on q2
    /// and 0.063π on q3 — live inside the D-H table, not here, so they are
    /// applied once rather than in every caller.</para>
    ///
    /// <para><b>Direction is assumed, not verified.</b> This takes a rising
    /// Dynamixel reading to mean a rising q on every joint. That holds only if
    /// each primary motor's Drive Mode is normal; a joint mounted reversed would
    /// need its sign flipped here. Checking it is one move per joint against the
    /// Cartesian readout.</para>
    /// </summary>
    internal static class ViperJointConvention
    {
        /// <summary>Dynamixel degrees at the motor centre, where model q is zero.</summary>
        public const double CentreDeg = 180.0;

        private const double DegToRad = Math.PI / 180.0;
        private const double RadToDeg = 180.0 / Math.PI;

        public static double ToModel(double dxlDeg) => (dxlDeg - CentreDeg) * DegToRad;

        public static double ToDynamixel(double qRad) => qRad * RadToDeg + CentreDeg;

        // ── Real degrees: the panel's unit ───────────────────────────────────
        //
        // Only the centre moves, so a difference between two angles is the same
        // number in either convention. That is why velocities, accelerations and
        // tracking errors need no conversion at all — the 180° cancels — and only
        // absolute positions do.

        /// <summary>Dynamixel degrees to real degrees: centre becomes zero.</summary>
        public static double ToRealDeg(double dxlDeg) => dxlDeg - CentreDeg;

        /// <summary>Real degrees back to Dynamixel degrees, for ViperCore.</summary>
        public static double ToDynamixelDeg(double realDeg) => realDeg + CentreDeg;

        /// <summary>Dynamixel to real degrees into a fresh array.</summary>
        public static double[] ToRealDeg(double[] dxlDeg)
        {
            var real = new double[dxlDeg.Length];
            for (int i = 0; i < dxlDeg.Length; i++) real[i] = ToRealDeg(dxlDeg[i]);
            return real;
        }

        /// <summary>Real to Dynamixel degrees into a fresh array.</summary>
        public static double[] ToDynamixelDeg(double[] realDeg)
        {
            var dxl = new double[realDeg.Length];
            for (int i = 0; i < realDeg.Length; i++) dxl[i] = ToDynamixelDeg(realDeg[i]);
            return dxl;
        }

        // Real degrees and model radians are the same convention in different
        // units — no centre to shift, just the factor. These exist so the
        // inverse kinematics' output reaches the panel without anyone being
        // tempted to route it through the Dynamixel form and back.

        /// <summary>Model radians, as the kinematics returns them, to the panel's degrees.</summary>
        public static double[] ModelRadToRealDeg(double[] qRad)
        {
            var real = new double[qRad.Length];
            for (int i = 0; i < qRad.Length; i++) real[i] = qRad[i] * RadToDeg;
            return real;
        }

        /// <summary>The panel's degrees to model radians, as the kinematics wants them.</summary>
        public static double[] RealDegToModelRad(double[] realDeg)
        {
            var q = new double[realDeg.Length];
            for (int i = 0; i < realDeg.Length; i++) q[i] = realDeg[i] * DegToRad;
            return q;
        }

        /// <summary>Dynamixel degrees to model radians, joint by joint.</summary>
        public static double[] ToModel(double[] dxlDeg)
        {
            var q = new double[dxlDeg.Length];
            for (int i = 0; i < dxlDeg.Length; i++) q[i] = ToModel(dxlDeg[i]);
            return q;
        }

        /// <summary>Into a caller-supplied array, for use inside a control loop.</summary>
        public static void ToModel(double[] dxlDeg, double[] qRad)
        {
            for (int i = 0; i < dxlDeg.Length && i < qRad.Length; i++)
                qRad[i] = ToModel(dxlDeg[i]);
        }

        /// <summary>
        /// Model radians back to Dynamixel degrees. Everything handed to
        /// ViperCore goes through here first.
        /// </summary>
        public static double[] ToDynamixel(double[] qRad)
        {
            var dxl = new double[qRad.Length];
            for (int i = 0; i < qRad.Length; i++) dxl[i] = ToDynamixel(qRad[i]);
            return dxl;
        }
    }
}
