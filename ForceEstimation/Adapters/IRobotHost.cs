namespace ForceEstimation
{
    /// <summary>
    /// UI callbacks that robot adapters use to report state changes to the Form.
    /// The Form implements this; adapters receive it via constructor injection.
    /// </summary>
    internal interface IRobotHost
    {
        bool IsCalibrated { get; }

        void SetConnectionStatus(bool connected);
        void SetCalibrationStatus(bool calibrated);
        void SetMotionState(string text, Color color);

        void NotifyMotionStarted();
        void NotifyGoFinalStarted();
        void NotifyMotionCompleted(string completedState);

        void UpdateEncoderDisplay(double[] q, double[] p);
        void RecordDesiredTrajectory(double t, double[] qd, double[] qpd, double[] qppd);
        void SetEncoderButtonText(string text);

        void ShowMessage(string message, string title, MessageBoxIcon icon);

        // Marshals an action onto the UI thread (BeginInvoke equivalent).
        void InvokeOnUI(Action action);
    }
}
