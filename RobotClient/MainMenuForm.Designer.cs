

namespace RobotClient
{
    partial class MainMenuForm
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
            lblTitle = new Label();
            lblSubtitle = new Label();
            btnExperiment1 = new Button();
            btnExperiment2 = new Button();
            btnExperiment3 = new Button();
            btnExperiment4 = new Button();
            btnExperiment5 = new Button();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(24, 22);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(140, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Experimentos";
            //
            // lblSubtitle
            //
            lblSubtitle.AutoSize = true;
            lblSubtitle.ForeColor = SystemColors.GrayText;
            lblSubtitle.Location = new Point(26, 54);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(240, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Selecciona un experimento para comenzar.";
            //
            // btnExperiment1
            //
            btnExperiment1.Font = new Font("Segoe UI", 10F);
            btnExperiment1.Location = new Point(24, 92);
            btnExperiment1.Name = "btnExperiment1";
            btnExperiment1.Padding = new Padding(16, 0, 0, 0);
            btnExperiment1.Size = new Size(572, 52);
            btnExperiment1.TabIndex = 2;
            btnExperiment1.Text = "1. Estimación de Fuerza";
            btnExperiment1.TextAlign = ContentAlignment.MiddleLeft;
            btnExperiment1.UseVisualStyleBackColor = true;
            //
            // btnExperiment2
            //
            btnExperiment2.Font = new Font("Segoe UI", 10F);
            btnExperiment2.Location = new Point(24, 152);
            btnExperiment2.Name = "btnExperiment2";
            btnExperiment2.Padding = new Padding(16, 0, 0, 0);
            btnExperiment2.Size = new Size(572, 52);
            btnExperiment2.TabIndex = 3;
            btnExperiment2.Text = "2. Teleoperación bilateral — dos Geomagic";
            btnExperiment2.TextAlign = ContentAlignment.MiddleLeft;
            btnExperiment2.UseVisualStyleBackColor = true;
            //
            // btnExperiment3
            //
            btnExperiment3.Font = new Font("Segoe UI", 10F);
            btnExperiment3.Location = new Point(24, 212);
            btnExperiment3.Name = "btnExperiment3";
            btnExperiment3.Padding = new Padding(16, 0, 0, 0);
            btnExperiment3.Size = new Size(572, 52);
            btnExperiment3.TabIndex = 4;
            btnExperiment3.Text = "3. ViperX300 — control y sensor de fuerza";
            btnExperiment3.TextAlign = ContentAlignment.MiddleLeft;
            btnExperiment3.UseVisualStyleBackColor = true;
            //
            // btnExperiment4
            //
            btnExperiment4.Font = new Font("Segoe UI", 10F);
            btnExperiment4.Location = new Point(24, 272);
            btnExperiment4.Name = "btnExperiment4";
            btnExperiment4.Padding = new Padding(16, 0, 0, 0);
            btnExperiment4.Size = new Size(572, 52);
            btnExperiment4.TabIndex = 5;
            btnExperiment4.Text = "4. Teleoperación heterogénea — Geomagic y ViperX300";
            btnExperiment4.TextAlign = ContentAlignment.MiddleLeft;
            btnExperiment4.UseVisualStyleBackColor = true;
            //
            // btnExperiment5
            //
            btnExperiment5.Font = new Font("Segoe UI", 10F);
            btnExperiment5.Location = new Point(24, 332);
            btnExperiment5.Name = "btnExperiment5";
            btnExperiment5.Padding = new Padding(16, 0, 0, 0);
            btnExperiment5.Size = new Size(572, 52);
            btnExperiment5.TabIndex = 6;
            btnExperiment5.Text = "5. ViperX300 — control de fuerza";
            btnExperiment5.TextAlign = ContentAlignment.MiddleLeft;
            btnExperiment5.UseVisualStyleBackColor = true;
            //
            // MainMenuForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(620, 420);
            Controls.Add(btnExperiment5);
            Controls.Add(btnExperiment4);
            Controls.Add(btnExperiment3);
            Controls.Add(btnExperiment2);
            Controls.Add(btnExperiment1);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainMenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menú principal";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblSubtitle;
        private Button btnExperiment1;
        private Button btnExperiment2;
        private Button btnExperiment3;
        private Button btnExperiment4;
        private Button btnExperiment5;
    }
}
