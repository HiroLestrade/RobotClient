using System.ComponentModel;

namespace ForceEstimation
{
    /// <summary>
    /// The strip at the top of the experiment window: which robot is in use.
    ///
    /// Owns nothing but the choice. It does not know what a Geomagic or a Viper
    /// is — the host reacts to <see cref="SelectionChanged"/> and swaps whatever
    /// control panel belongs to that robot.
    /// </summary>
    public partial class RobotSelector : UserControl
    {
        /// <summary>Raised when the selection changes. Carries the robot's name.</summary>
        public event EventHandler<string>? SelectionChanged;

        public RobotSelector() => InitializeComponent();

        /// <summary>Currently selected robot, or null before anything is chosen.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? SelectedRobot => cmbRobot.SelectedItem?.ToString();

        /// <summary>
        /// Replaces the list of robots. Selects the first entry, which raises
        /// <see cref="SelectionChanged"/>.
        /// </summary>
        public void SetRobots(params string[] names)
        {
            cmbRobot.Items.Clear();
            cmbRobot.Items.AddRange(names);
            if (cmbRobot.Items.Count > 0)
                cmbRobot.SelectedIndex = 0;
        }

        /// <summary>
        /// Fires <see cref="SelectionChanged"/> for whatever is selected now.
        /// Lets the host build the initial panel without duplicating the logic
        /// of the event handler.
        /// </summary>
        public void RaiseCurrentSelection()
        {
            if (SelectedRobot is string name)
                SelectionChanged?.Invoke(this, name);
        }

        private void cmbRobot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedRobot is string name)
                SelectionChanged?.Invoke(this, name);
        }
    }
}
