namespace ForceEstimation
{
    internal static class RobotAdapterFactory
    {
        public static IRobotAdapter Create(string robotName, IRobotHost host) =>
            robotName switch
            {
                "Viper X-300S" => new ViperAdapter(host),
                _              => new ViperAdapter(host)
            };
    }
}
