
namespace ForceEstimation
{
    partial class RobotSelector
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
            lblRobot = new Label();
            cmbRobot = new ComboBox();
            SuspendLayout();
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
            // RobotSelector
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(cmbRobot);
            Controls.Add(lblRobot);
            Name = "RobotSelector";
            Size = new Size(1347, 40);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label    lblRobot;
        private ComboBox cmbRobot;
    }
}
