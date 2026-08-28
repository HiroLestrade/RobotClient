
namespace ForceEstimation
{
    partial class PidGainsControl
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
            lblQ1         = new Label();
            lblQ2         = new Label();
            lblQ3         = new Label();
            lblKp         = new Label();
            tbxKp1        = new TextBox();
            tbxKp2        = new TextBox();
            tbxKp3        = new TextBox();
            lblKi         = new Label();
            tbxKi1        = new TextBox();
            tbxKi2        = new TextBox();
            tbxKi3        = new TextBox();
            lblKd         = new Label();
            tbxKd1        = new TextBox();
            tbxKd2        = new TextBox();
            tbxKd3        = new TextBox();
            lblSampleTime = new Label();
            tbxSampleTime = new TextBox();
            SuspendLayout();
            //
            // column headers
            //
            lblQ1.AutoSize = true;
            lblQ1.Location = new Point(83, 10);
            lblQ1.Name = "lblQ1";
            lblQ1.TabIndex = 0;
            lblQ1.Text = "q₁";
            //
            lblQ2.AutoSize = true;
            lblQ2.Location = new Point(178, 10);
            lblQ2.Name = "lblQ2";
            lblQ2.TabIndex = 1;
            lblQ2.Text = "q₂";
            //
            lblQ3.AutoSize = true;
            lblQ3.Location = new Point(273, 10);
            lblQ3.Name = "lblQ3";
            lblQ3.TabIndex = 2;
            lblQ3.Text = "q₃";
            //
            // Kp row
            //
            lblKp.AutoSize = true;
            lblKp.Location = new Point(12, 33);
            lblKp.Name = "lblKp";
            lblKp.TabIndex = 3;
            lblKp.Text = "Kp";
            //
            tbxKp1.Location = new Point(55, 30);
            tbxKp1.Name = "tbxKp1";
            tbxKp1.Size = new Size(80, 23);
            tbxKp1.TabIndex = 4;
            tbxKp1.Text = "2.3";
            //
            tbxKp2.Location = new Point(150, 30);
            tbxKp2.Name = "tbxKp2";
            tbxKp2.Size = new Size(80, 23);
            tbxKp2.TabIndex = 5;
            tbxKp2.Text = "2.0";
            //
            tbxKp3.Location = new Point(245, 30);
            tbxKp3.Name = "tbxKp3";
            tbxKp3.Size = new Size(80, 23);
            tbxKp3.TabIndex = 6;
            tbxKp3.Text = "2.3";
            //
            // Ki row
            //
            lblKi.AutoSize = true;
            lblKi.Location = new Point(12, 61);
            lblKi.Name = "lblKi";
            lblKi.TabIndex = 7;
            lblKi.Text = "Ki";
            //
            tbxKi1.Location = new Point(55, 58);
            tbxKi1.Name = "tbxKi1";
            tbxKi1.Size = new Size(80, 23);
            tbxKi1.TabIndex = 8;
            tbxKi1.Text = "0.36";
            //
            tbxKi2.Location = new Point(150, 58);
            tbxKi2.Name = "tbxKi2";
            tbxKi2.Size = new Size(80, 23);
            tbxKi2.TabIndex = 9;
            tbxKi2.Text = "0.36";
            //
            tbxKi3.Location = new Point(245, 58);
            tbxKi3.Name = "tbxKi3";
            tbxKi3.Size = new Size(80, 23);
            tbxKi3.TabIndex = 10;
            tbxKi3.Text = "0.36";
            //
            // Kd row
            //
            lblKd.AutoSize = true;
            lblKd.Location = new Point(12, 89);
            lblKd.Name = "lblKd";
            lblKd.TabIndex = 11;
            lblKd.Text = "Kd";
            //
            tbxKd1.Location = new Point(55, 86);
            tbxKd1.Name = "tbxKd1";
            tbxKd1.Size = new Size(80, 23);
            tbxKd1.TabIndex = 12;
            tbxKd1.Text = "0.03";
            //
            tbxKd2.Location = new Point(150, 86);
            tbxKd2.Name = "tbxKd2";
            tbxKd2.Size = new Size(80, 23);
            tbxKd2.TabIndex = 13;
            tbxKd2.Text = "0.03";
            //
            tbxKd3.Location = new Point(245, 86);
            tbxKd3.Name = "tbxKd3";
            tbxKd3.Size = new Size(80, 23);
            tbxKd3.TabIndex = 14;
            tbxKd3.Text = "0.03";
            //
            // Sampling time
            //
            lblSampleTime.AutoSize = true;
            lblSampleTime.Location = new Point(12, 123);
            lblSampleTime.Name = "lblSampleTime";
            lblSampleTime.TabIndex = 15;
            lblSampleTime.Text = "T muestreo [ms]:";
            //
            tbxSampleTime.Location = new Point(130, 120);
            tbxSampleTime.Name = "tbxSampleTime";
            tbxSampleTime.Size = new Size(60, 23);
            tbxSampleTime.TabIndex = 16;
            tbxSampleTime.Text = "1";
            //
            // PidGainsControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblQ1);
            Controls.Add(lblQ2);
            Controls.Add(lblQ3);
            Controls.Add(lblKp);
            Controls.Add(tbxKp1);
            Controls.Add(tbxKp2);
            Controls.Add(tbxKp3);
            Controls.Add(lblKi);
            Controls.Add(tbxKi1);
            Controls.Add(tbxKi2);
            Controls.Add(tbxKi3);
            Controls.Add(lblKd);
            Controls.Add(tbxKd1);
            Controls.Add(tbxKd2);
            Controls.Add(tbxKd3);
            Controls.Add(lblSampleTime);
            Controls.Add(tbxSampleTime);
            Name = "PidGainsControl";
            Size = new Size(340, 150);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label   lblQ1;
        private Label   lblQ2;
        private Label   lblQ3;
        private Label   lblKp;
        private TextBox tbxKp1;
        private TextBox tbxKp2;
        private TextBox tbxKp3;
        private Label   lblKi;
        private TextBox tbxKi1;
        private TextBox tbxKi2;
        private TextBox tbxKi3;
        private Label   lblKd;
        private TextBox tbxKd1;
        private TextBox tbxKd2;
        private TextBox tbxKd3;
        private Label   lblSampleTime;
        private TextBox tbxSampleTime;
    }
}
