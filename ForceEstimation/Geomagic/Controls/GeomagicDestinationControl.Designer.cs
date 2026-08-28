
namespace ForceEstimation
{
    partial class GeomagicDestinationControl
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
            lblDestino    = new Label();
            radFinalQ     = new RadioButton();
            radFinalX     = new RadioButton();
            tbxFinalQf1   = new TextBox();
            lblFinalQf1   = new Label();
            tbxFinalQf2   = new TextBox();
            lblFinalQf2   = new Label();
            tbxFinalQf3   = new TextBox();
            lblFinalQf3   = new Label();
            tbxFinalXf    = new TextBox();
            lblFinalXf    = new Label();
            tbxFinalYf    = new TextBox();
            lblFinalYf    = new Label();
            tbxFinalZf    = new TextBox();
            lblFinalZf    = new Label();
            bttnGoFinal   = new Button();
            SuspendLayout();
            //
            // lblDestino
            //
            lblDestino.AutoSize = true;
            lblDestino.Location = new Point(13, 12);
            lblDestino.Name = "lblDestino";
            lblDestino.Size = new Size(47, 15);
            lblDestino.TabIndex = 0;
            lblDestino.Text = "Destino";
            //
            // radFinalQ
            //
            radFinalQ.AutoSize = true;
            radFinalQ.Checked = true;
            radFinalQ.Location = new Point(109, 10);
            radFinalQ.Name = "radFinalQ";
            radFinalQ.Size = new Size(34, 19);
            radFinalQ.TabIndex = 19;
            radFinalQ.TabStop = true;
            radFinalQ.Text = "Q";
            radFinalQ.UseVisualStyleBackColor = true;
            radFinalQ.CheckedChanged += radMode_CheckedChanged;
            //
            // radFinalX
            //
            radFinalX.AutoSize = true;
            radFinalX.Location = new Point(71, 10);
            radFinalX.Name = "radFinalX";
            radFinalX.Size = new Size(32, 19);
            radFinalX.TabIndex = 18;
            radFinalX.Text = "X";
            radFinalX.UseVisualStyleBackColor = true;
            radFinalX.CheckedChanged += radMode_CheckedChanged;
            //
            // tbxFinalQf1
            //
            tbxFinalQf1.Location = new Point(43, 40);
            tbxFinalQf1.Name = "tbxFinalQf1";
            tbxFinalQf1.Size = new Size(100, 23);
            tbxFinalQf1.TabIndex = 2;
            tbxFinalQf1.Text = "0.00";
            //
            // lblFinalQf1
            //
            lblFinalQf1.AutoSize = true;
            lblFinalQf1.Location = new Point(20, 43);
            lblFinalQf1.Name = "lblFinalQf1";
            lblFinalQf1.Size = new Size(24, 15);
            lblFinalQf1.TabIndex = 1;
            lblFinalQf1.Text = "qf1";
            //
            // tbxFinalQf2
            //
            tbxFinalQf2.Location = new Point(43, 84);
            tbxFinalQf2.Name = "tbxFinalQf2";
            tbxFinalQf2.Size = new Size(100, 23);
            tbxFinalQf2.TabIndex = 4;
            tbxFinalQf2.Text = "20.00";
            //
            // lblFinalQf2
            //
            lblFinalQf2.AutoSize = true;
            lblFinalQf2.Location = new Point(20, 87);
            lblFinalQf2.Name = "lblFinalQf2";
            lblFinalQf2.Size = new Size(24, 15);
            lblFinalQf2.TabIndex = 3;
            lblFinalQf2.Text = "qf2";
            //
            // tbxFinalQf3
            //
            tbxFinalQf3.Location = new Point(43, 128);
            tbxFinalQf3.Name = "tbxFinalQf3";
            tbxFinalQf3.Size = new Size(100, 23);
            tbxFinalQf3.TabIndex = 6;
            tbxFinalQf3.Text = "-120.00";
            //
            // lblFinalQf3
            //
            lblFinalQf3.AutoSize = true;
            lblFinalQf3.Location = new Point(20, 131);
            lblFinalQf3.Name = "lblFinalQf3";
            lblFinalQf3.Size = new Size(24, 15);
            lblFinalQf3.TabIndex = 5;
            lblFinalQf3.Text = "qf3";
            //
            // tbxFinalXf
            //
            tbxFinalXf.Enabled = false;
            tbxFinalXf.Location = new Point(43, 183);
            tbxFinalXf.Name = "tbxFinalXf";
            tbxFinalXf.Size = new Size(100, 23);
            tbxFinalXf.TabIndex = 9;
            tbxFinalXf.Text = "20.00";
            //
            // lblFinalXf
            //
            lblFinalXf.AutoSize = true;
            lblFinalXf.Location = new Point(20, 186);
            lblFinalXf.Name = "lblFinalXf";
            lblFinalXf.Size = new Size(17, 15);
            lblFinalXf.TabIndex = 8;
            lblFinalXf.Text = "xf";
            //
            // tbxFinalYf
            //
            tbxFinalYf.Enabled = false;
            tbxFinalYf.Location = new Point(43, 227);
            tbxFinalYf.Name = "tbxFinalYf";
            tbxFinalYf.Size = new Size(100, 23);
            tbxFinalYf.TabIndex = 11;
            tbxFinalYf.Text = "0.00";
            //
            // lblFinalYf
            //
            lblFinalYf.AutoSize = true;
            lblFinalYf.Location = new Point(20, 230);
            lblFinalYf.Name = "lblFinalYf";
            lblFinalYf.Size = new Size(17, 15);
            lblFinalYf.TabIndex = 10;
            lblFinalYf.Text = "yf";
            //
            // tbxFinalZf
            //
            tbxFinalZf.Enabled = false;
            tbxFinalZf.Location = new Point(43, 271);
            tbxFinalZf.Name = "tbxFinalZf";
            tbxFinalZf.Size = new Size(100, 23);
            tbxFinalZf.TabIndex = 13;
            tbxFinalZf.Text = "0.00";
            //
            // lblFinalZf
            //
            lblFinalZf.AutoSize = true;
            lblFinalZf.Location = new Point(20, 274);
            lblFinalZf.Name = "lblFinalZf";
            lblFinalZf.Size = new Size(16, 15);
            lblFinalZf.TabIndex = 12;
            lblFinalZf.Text = "zf";
            //
            // bttnGoFinal
            //
            bttnGoFinal.Location = new Point(20, 327);
            bttnGoFinal.Name = "bttnGoFinal";
            bttnGoFinal.Size = new Size(126, 23);
            bttnGoFinal.TabIndex = 7;
            bttnGoFinal.Text = "Ir a Destino";
            bttnGoFinal.UseVisualStyleBackColor = true;
            bttnGoFinal.Click += bttnGoFinal_Click;
            //
            // GeomagicDestinationControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblDestino);
            Controls.Add(radFinalQ);
            Controls.Add(radFinalX);
            Controls.Add(tbxFinalQf1);
            Controls.Add(lblFinalQf1);
            Controls.Add(tbxFinalQf2);
            Controls.Add(lblFinalQf2);
            Controls.Add(tbxFinalQf3);
            Controls.Add(lblFinalQf3);
            Controls.Add(tbxFinalXf);
            Controls.Add(lblFinalXf);
            Controls.Add(tbxFinalYf);
            Controls.Add(lblFinalYf);
            Controls.Add(tbxFinalZf);
            Controls.Add(lblFinalZf);
            Controls.Add(bttnGoFinal);
            Name = "GeomagicDestinationControl";
            Size = new Size(159, 368);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label     lblDestino;
        private RadioButton radFinalQ;
        private RadioButton radFinalX;
        private TextBox   tbxFinalQf1;
        private Label     lblFinalQf1;
        private TextBox   tbxFinalQf2;
        private Label     lblFinalQf2;
        private TextBox   tbxFinalQf3;
        private Label     lblFinalQf3;
        private TextBox   tbxFinalXf;
        private Label     lblFinalXf;
        private TextBox   tbxFinalYf;
        private Label     lblFinalYf;
        private TextBox   tbxFinalZf;
        private Label     lblFinalZf;
        private Button    bttnGoFinal;
    }
}
