
namespace RobotClient
{
    partial class GeomagicEncodersControl
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
            lblEnc           = new Label();
            tbxArtQ1         = new Label();
            lblArtQ1         = new Label();
            tbxArtQ2         = new Label();
            lblArtQ2         = new Label();
            tbxArtQ3         = new Label();
            lblArtqQ3        = new Label();
            tbxCartX         = new Label();
            lblCartX         = new Label();
            tbxCartY         = new Label();
            lblCartY         = new Label();
            tbxCartZ         = new Label();
            lblCartZ         = new Label();
            bttnReadEncoders = new Button();
            SuspendLayout();
            //
            // lblEnc
            //
            lblEnc.AutoSize = true;
            lblEnc.Location = new Point(13, 12);
            lblEnc.Name = "lblEnc";
            lblEnc.Size = new Size(68, 15);
            lblEnc.TabIndex = 0;
            lblEnc.Text = "Mediciones";
            //
            // tbxArtQ1
            //
            tbxArtQ1.BackColor = SystemColors.Window;
            tbxArtQ1.BorderStyle = BorderStyle.FixedSingle;
            tbxArtQ1.Location = new Point(43, 40);
            tbxArtQ1.Name = "tbxArtQ1";
            tbxArtQ1.Size = new Size(100, 23);
            tbxArtQ1.TabIndex = 2;
            tbxArtQ1.Text = "0.00";
            //
            // lblArtQ1
            //
            lblArtQ1.AutoSize = true;
            lblArtQ1.Location = new Point(20, 43);
            lblArtQ1.Name = "lblArtQ1";
            lblArtQ1.Size = new Size(20, 15);
            lblArtQ1.TabIndex = 1;
            lblArtQ1.Text = "q1";
            //
            // tbxArtQ2
            //
            tbxArtQ2.BackColor = SystemColors.Window;
            tbxArtQ2.BorderStyle = BorderStyle.FixedSingle;
            tbxArtQ2.Location = new Point(43, 84);
            tbxArtQ2.Name = "tbxArtQ2";
            tbxArtQ2.Size = new Size(100, 23);
            tbxArtQ2.TabIndex = 4;
            tbxArtQ2.Text = "0.00";
            //
            // lblArtQ2
            //
            lblArtQ2.AutoSize = true;
            lblArtQ2.Location = new Point(20, 87);
            lblArtQ2.Name = "lblArtQ2";
            lblArtQ2.Size = new Size(20, 15);
            lblArtQ2.TabIndex = 3;
            lblArtQ2.Text = "q2";
            //
            // tbxArtQ3
            //
            tbxArtQ3.BackColor = SystemColors.Window;
            tbxArtQ3.BorderStyle = BorderStyle.FixedSingle;
            tbxArtQ3.Location = new Point(43, 128);
            tbxArtQ3.Name = "tbxArtQ3";
            tbxArtQ3.Size = new Size(100, 23);
            tbxArtQ3.TabIndex = 6;
            tbxArtQ3.Text = "0.00";
            //
            // lblArtqQ3
            //
            lblArtqQ3.AutoSize = true;
            lblArtqQ3.Location = new Point(20, 131);
            lblArtqQ3.Name = "lblArtqQ3";
            lblArtqQ3.Size = new Size(20, 15);
            lblArtqQ3.TabIndex = 5;
            lblArtqQ3.Text = "q3";
            //
            // tbxCartX
            //
            tbxCartX.BackColor = SystemColors.Window;
            tbxCartX.BorderStyle = BorderStyle.FixedSingle;
            tbxCartX.Location = new Point(43, 183);
            tbxCartX.Name = "tbxCartX";
            tbxCartX.Size = new Size(100, 23);
            tbxCartX.TabIndex = 9;
            tbxCartX.Text = "0.00";
            //
            // lblCartX
            //
            lblCartX.AutoSize = true;
            lblCartX.Location = new Point(20, 186);
            lblCartX.Name = "lblCartX";
            lblCartX.Size = new Size(13, 15);
            lblCartX.TabIndex = 8;
            lblCartX.Text = "x";
            //
            // tbxCartY
            //
            tbxCartY.BackColor = SystemColors.Window;
            tbxCartY.BorderStyle = BorderStyle.FixedSingle;
            tbxCartY.Location = new Point(43, 227);
            tbxCartY.Name = "tbxCartY";
            tbxCartY.Size = new Size(100, 23);
            tbxCartY.TabIndex = 11;
            tbxCartY.Text = "0.00";
            //
            // lblCartY
            //
            lblCartY.AutoSize = true;
            lblCartY.Location = new Point(20, 230);
            lblCartY.Name = "lblCartY";
            lblCartY.Size = new Size(13, 15);
            lblCartY.TabIndex = 10;
            lblCartY.Text = "y";
            //
            // tbxCartZ
            //
            tbxCartZ.BackColor = SystemColors.Window;
            tbxCartZ.BorderStyle = BorderStyle.FixedSingle;
            tbxCartZ.Location = new Point(43, 271);
            tbxCartZ.Name = "tbxCartZ";
            tbxCartZ.Size = new Size(100, 23);
            tbxCartZ.TabIndex = 13;
            tbxCartZ.Text = "0.00";
            //
            // lblCartZ
            //
            lblCartZ.AutoSize = true;
            lblCartZ.Location = new Point(20, 274);
            lblCartZ.Name = "lblCartZ";
            lblCartZ.Size = new Size(12, 15);
            lblCartZ.TabIndex = 12;
            lblCartZ.Text = "z";
            //
            // bttnReadEncoders
            //
            bttnReadEncoders.Location = new Point(20, 327);
            bttnReadEncoders.Name = "bttnReadEncoders";
            bttnReadEncoders.Size = new Size(126, 23);
            bttnReadEncoders.TabIndex = 7;
            bttnReadEncoders.Text = "Leer encoders";
            bttnReadEncoders.UseVisualStyleBackColor = true;
            bttnReadEncoders.Click += bttnReadEncoders_Click;
            //
            // GeomagicEncodersControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblEnc);
            Controls.Add(tbxArtQ1);
            Controls.Add(lblArtQ1);
            Controls.Add(tbxArtQ2);
            Controls.Add(lblArtQ2);
            Controls.Add(tbxArtQ3);
            Controls.Add(lblArtqQ3);
            Controls.Add(tbxCartX);
            Controls.Add(lblCartX);
            Controls.Add(tbxCartY);
            Controls.Add(lblCartY);
            Controls.Add(tbxCartZ);
            Controls.Add(lblCartZ);
            Controls.Add(bttnReadEncoders);
            Name = "GeomagicEncodersControl";
            Size = new Size(159, 368);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblEnc;
        private Label tbxArtQ1;
        private Label lblArtQ1;
        private Label tbxArtQ2;
        private Label lblArtQ2;
        private Label tbxArtQ3;
        private Label lblArtqQ3;
        private Label tbxCartX;
        private Label lblCartX;
        private Label tbxCartY;
        private Label lblCartY;
        private Label tbxCartZ;
        private Label lblCartZ;
        private Button bttnReadEncoders;
    }
}
