using ForceEstimation;

namespace RobotClient
{
    /// <summary>
    /// Entry view of the application. Each button opens one experiment as a
    /// separate window; the menu hides itself while an experiment is open and
    /// comes back when that window closes.
    ///
    /// Adding an experiment = adding a button here + a factory line in
    /// <see cref="OpenExperiment"/>'s caller.
    /// </summary>
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();

            btnExperiment1.Click += (_, _) => OpenExperiment(new ForceEstimationForm());
            btnExperiment2.Click += (_, _) => NotImplementedYet(btnExperiment2.Text);
            btnExperiment3.Click += (_, _) => NotImplementedYet(btnExperiment3.Text);
            btnExperiment4.Click += (_, _) => NotImplementedYet(btnExperiment4.Text);
            btnExperiment5.Click += (_, _) => NotImplementedYet(btnExperiment5.Text);
        }

        /// <summary>
        /// Shows <paramref name="experiment"/> and hides the menu until it closes.
        /// </summary>
        private void OpenExperiment(Form experiment)
        {
            experiment.FormClosed += (_, _) =>
            {
                Show();
                Activate();
            };

            Hide();
            experiment.Show();
        }

        private void NotImplementedYet(string experimentName) =>
            MessageBox.Show(
                $"El experimento \"{experimentName}\" todavía no está implementado.",
                "No disponible", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
