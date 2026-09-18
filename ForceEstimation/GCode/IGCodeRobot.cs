namespace ForceEstimation
{
    /// <summary>
    /// Everything <see cref="GCodeInterpreter"/> needs from a robot, and nothing
    /// else.
    ///
    /// <para>It used to need <c>IRobotAdapter</c> plus a concrete
    /// <c>GeomagicConfigControl</c> plus a direct call into the Geomagic's own
    /// inverse kinematics, which is three reasons the Viper could not run a
    /// program. The parts that actually differ between the two arms are the ones
    /// below: how many axes there are, how a Cartesian line becomes joints, and
    /// how long to wait afterwards.</para>
    ///
    /// <para><b>Joint values are in the robot's own panel convention</b> —
    /// whatever its boxes show. The interpreter never converts; it only moves
    /// numbers from a line of text to the panel that owns them.</para>
    /// </summary>
    internal interface IGCodeRobot
    {
        /// <summary>
        /// The letters <c>G0</c> reads, in joint order: "ABC" for a three-axis
        /// arm, "ABCDEF" for six. Its length is the joint count.
        /// </summary>
        string AxisLetters { get; }

        /// <summary>
        /// Seconds to wait after <see cref="MoveToJoints"/> before the next line.
        /// Each robot answers for itself: it has to cover the trajectory <i>and</i>
        /// whatever settling the controller does afterwards, or the next line
        /// arrives while the arm is still moving and gets refused.
        /// </summary>
        double MoveWaitSecs { get; }

        /// <summary>Seconds to wait after <see cref="GoHome"/>.</summary>
        double HomeWaitSecs { get; }

        /// <summary>
        /// Commands a joint-space move. False means it did not start — the robot
        /// has already said why — and the program stops there rather than running
        /// the rest of the lines against an arm that never moved.
        /// </summary>
        bool MoveToJoints(double[] q);

        /// <summary>Sends the arm home. False if it did not start.</summary>
        bool GoHome();

        /// <summary>
        /// Turns a <c>G1</c> line into a joint target. The whole word set is
        /// handed over so each arm reads what it understands: a three-axis arm
        /// takes X, Y and Z, while the Viper also takes a tool direction and a
        /// spin. Letters that are absent are the implementation's to default.
        /// </summary>
        bool TryCartesianToJoints(
            IReadOnlyDictionary<char, double> words, out double[] q, out string reason);

        /// <summary>The <c>M64</c> / <c>M65</c> pair.</summary>
        void SetEndEffectorState(bool on);
    }
}
