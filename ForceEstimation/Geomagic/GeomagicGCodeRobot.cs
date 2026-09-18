namespace ForceEstimation
{
    /// <summary>
    /// The Geomagic side of <see cref="IGCodeRobot"/>, over the adapter and config
    /// control it already has.
    ///
    /// <para>This is a shim on purpose. The interpreter used to reach into
    /// <c>IRobotAdapter</c>, <c>GeomagicConfigControl</c> and
    /// <c>GeomagicModel.InverseKinematics</c> directly; all three couplings now
    /// live here, where they are the Geomagic's business and nobody else's.
    /// Behaviour is unchanged line for line — this arm works and there is no
    /// reason for a refactor to find out otherwise.</para>
    /// </summary>
    internal sealed class GeomagicGCodeRobot : IGCodeRobot
    {
        /// <summary>Margin over the trajectory time, seconds. As it always was.</summary>
        private const double MoveMargin = 0.15;

        /// <summary>Home takes a fixed 2 s here, plus the same margin.</summary>
        private const double HomeSecs = 2.0;

        private readonly IRobotAdapter         _adapter;
        private readonly GeomagicConfigControl _config;

        internal GeomagicGCodeRobot(IRobotAdapter adapter, GeomagicConfigControl config)
        {
            _adapter = adapter;
            _config  = config;
        }

        public string AxisLetters => "ABC";

        public double MoveWaitSecs => _config.TrajectoryTimeSecs + MoveMargin;

        public double HomeWaitSecs => HomeSecs + MoveMargin;

        /// <summary>
        /// G0 gives degrees; the adapter speaks radians, so the conversion is
        /// here. Returns true unconditionally because <c>GoFinal</c> reports
        /// nothing back — it shows its own message and returns void.
        /// </summary>
        public bool MoveToJoints(double[] q)
        {
            var qRad = new double[q.Length];
            for (int i = 0; i < q.Length; i++) qRad[i] = q[i] * Math.PI / 180.0;

            _adapter.GoFinal(qRad);
            return true;
        }

        public bool GoHome()
        {
            _adapter.GoHome([0.0, 0.0, 0.0]);
            return true;
        }

        /// <summary>
        /// X, Y and Z in centimetres through the Geomagic's own inverse
        /// kinematics. It is a three-degree-of-freedom arm, so there is no
        /// orientation to read and any other letters on the line are ignored.
        /// </summary>
        public bool TryCartesianToJoints(
            IReadOnlyDictionary<char, double> words, out double[] q, out string reason)
        {
            q      = [];
            reason = string.Empty;

            double x = words['X'], y = words['Y'], z = words['Z'];

            double[]? qf = GeomagicModel.InverseKinematics([x / 100.0, y / 100.0, z / 100.0]);
            if (qf == null)
            {
                reason = $"Posición ({x}, {y}, {z}) cm fuera del espacio de trabajo.";
                return false;
            }

            // Straight to the adapter's convention: MoveToJoints converts from
            // degrees, and this is already radians, so hand back degrees.
            q = new double[qf.Length];
            for (int i = 0; i < qf.Length; i++) q[i] = qf[i] * 180.0 / Math.PI;

            return true;
        }

        public void SetEndEffectorState(bool on) => _config.SetEndEffectorState(on);
    }
}
