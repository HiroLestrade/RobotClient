

namespace ForceEstimation
{
    partial class ForceEstimationForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            robotSelector = new RobotSelector();
            pnlHost       = new Panel();
            SuspendLayout();
            //
            // robotSelector
            //
            robotSelector.Dock = DockStyle.Top;
            robotSelector.Location = new Point(0, 0);
            robotSelector.Name = "robotSelector";
            robotSelector.Size = new Size(1347, 40);
            robotSelector.TabIndex = 0;
            //
            // pnlHost
            //
            // Holds the control panel of whichever robot is selected.
            pnlHost.Dock = DockStyle.Fill;
            pnlHost.Location = new Point(0, 40);
            pnlHost.Name = "pnlHost";
            pnlHost.Size = new Size(1347, 819);
            pnlHost.TabIndex = 1;
            //
            // ForceEstimationForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1347, 979);
            // The Viper panel's left column ends at y=917, under a 40 px selector,
            // plus the window chrome — below this the Configuration tabs get clipped.
            // Width can shrink: only the plots are anchored, and they take the slack.
            MinimumSize = new Size(1000, 1020);
            WindowState = FormWindowState.Maximized;
            Controls.Add(pnlHost);
            Controls.Add(robotSelector);
            Name = "ForceEstimationForm";
            Text = "Estimación de Fuerza";
            ResumeLayout(false);
        }

        private RobotSelector robotSelector;
        private Panel         pnlHost;
    }
}
