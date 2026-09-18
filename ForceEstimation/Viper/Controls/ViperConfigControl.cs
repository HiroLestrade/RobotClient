using System.ComponentModel;

namespace ForceEstimation
{
    /// <summary>
    /// Configuration panel for the Viper X-300S: controller, trajectory and
    /// G-code interpreter. Mirrors GeomagicConfigControl, minus the tabs whose
    /// hardware the Viper does not have yet.
    ///
    /// Layout only for now: the controls are laid out and expose their values,
    /// but nothing is wired to ViperCore.
    /// </summary>
    public partial class ViperConfigControl : UserControl
    {
        /// <summary>Raised when the user asks for the G-code to run.</summary>
        public event EventHandler<string>? ExecuteGCodeRequested;

        public ViperConfigControl()
        {
            InitializeComponent();
            cmbController.SelectedIndex = 0;
            cmbTrajectory.SelectedIndex = 0;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedController => cmbController.SelectedItem?.ToString() ?? "";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedTrajectory => cmbTrajectory.SelectedItem?.ToString() ?? "";

        /// <summary>Motion duration, in seconds. Falls back to 3 s if unparsable.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TrajectoryTime =>
            double.TryParse(tbxTf.Text, out double tf) && tf > 0 ? tf : 3.0;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string GCode => txtGCode.Text;

        private void btnExecuteGCode_Click(object sender, EventArgs e) =>
            ExecuteGCodeRequested?.Invoke(this, txtGCode.Text);
    }
}
