namespace ForceEstimation
{
    /// <summary>
    /// The Geomagic's split between its panel and its device code.
    ///
    /// <para><b>This is not the project's robot abstraction.</b> It was written as
    /// one — there was a factory that picked an adapter by robot name — but the
    /// Viper panel was built without it and talks to <c>ViperDevice</c> directly,
    /// so the factory and the Viper's adapter were deleted rather than left
    /// looking current. What remains is used by exactly one arm, and the shape
    /// below is that arm's: three joints, radians, and a Cartesian force of three
    /// components.</para>
    ///
    /// <para>Anything that needs to serve both arms should take a narrow
    /// interface of its own instead, the way <see cref="IGCodeRobot"/> does.</para>
    /// </summary>
    internal interface IRobotAdapter : IDisposable
    {
        void Connect(string? deviceName);
        void Calibrate();
        void Disconnect();
        void ToggleReadEncoders();
        void GoHome(double[] qf);
        void GoFinal(double[] qf);

        /// <summary>
        /// Stops any active motion command or trajectory. Does not affect encoder
        /// reading and does not disconnect from the robot.
        /// </summary>
        void Stop();

        /// <summary>
        /// Returns the last computed external-torque estimate {τ_e1, τ_e2, τ_e3} (N-m).
        /// Returns zeros if no estimator is running.
        /// </summary>
        double[] GetLastEstimate();

        /// <summary>
        /// Returns the last estimated Cartesian force {Fx, Fy, Fz} (N), expressed
        /// in the robot base frame. Returns zeros if no estimator is running.
        /// </summary>
        double[] GetEstimatedForce();
    }
}
