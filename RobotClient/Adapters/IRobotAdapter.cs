namespace RobotClient
{
    /// <summary>
    /// Common contract for all robot implementations.
    /// Adding a new robot type = implementing this interface + registering in RobotAdapterFactory.
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
    }
}
