

namespace RobotClient
{
    partial class Form1
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
            pnlSelector = new Panel();
            cmbRobot = new ComboBox();
            lblRobot = new Label();
            pnlHost = new Panel();
            pnlSelector.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSelector
            // 
            pnlSelector.Controls.Add(cmbRobot);
            pnlSelector.Controls.Add(lblRobot);
            pnlSelector.Dock = DockStyle.Top;
            pnlSelector.Location = new Point(0, 0);
            pnlSelector.Name = "pnlSelector";
            pnlSelector.Size = new Size(1347, 40);
            pnlSelector.TabIndex = 0;
            // 
            // cmbRobot
            // 
            cmbRobot.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRobot.Items.AddRange(new object[] { "Geomagic Touch", "Viper X-300S" });
            cmbRobot.Location = new Point(70, 8);
            cmbRobot.Name = "cmbRobot";
            cmbRobot.Size = new Size(150, 23);
            cmbRobot.TabIndex = 1;
            cmbRobot.SelectedIndexChanged += cmbRobot_SelectedIndexChanged;
            // 
            // lblRobot
            // 
            lblRobot.AutoSize = true;
            lblRobot.Location = new Point(12, 12);
            lblRobot.Name = "lblRobot";
            lblRobot.Size = new Size(39, 15);
            lblRobot.TabIndex = 0;
            lblRobot.Text = "Robot";
            // 
            // pnlHost
            // 
            pnlHost.Dock = DockStyle.Fill;
            pnlHost.Location = new Point(0, 40);
            pnlHost.Name = "pnlHost";
            pnlHost.Size = new Size(1347, 819);
            pnlHost.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1347, 859);
            Controls.Add(pnlHost);
            Controls.Add(pnlSelector);
            Name = "Form1";
            Text = "Robot Client";
            pnlSelector.ResumeLayout(false);
            pnlSelector.PerformLayout();
            ResumeLayout(false);
        }

        private Panel pnlSelector;
        private Label lblRobot;
        private ComboBox cmbRobot;
        private Panel pnlHost;
    }
}
