namespace Teleoperation
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (_, args) =>
                MessageBox.Show(args.Exception.ToString(), "Unhandled UI Exception",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            AppDomain.CurrentDomain.UnhandledException += (_, args) =>
                MessageBox.Show(args.ExceptionObject?.ToString(), "Unhandled Exception",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            ApplicationConfiguration.Initialize();
            Application.Run(new TeleoperationForm());
        }
    }
}
