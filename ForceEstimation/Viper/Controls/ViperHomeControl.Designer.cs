
namespace ForceEstimation
{
    partial class ViperHomeControl
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
            lblJoints = new Label();
            lblCart = new Label();
            radX = new RadioButton();
            radQ = new RadioButton();
            lblQ1 = new Label();
            tbxQ1 = new TextBox();
            lblQ2 = new Label();
            tbxQ2 = new TextBox();
            lblQ3 = new Label();
            tbxQ3 = new TextBox();
            lblQ4 = new Label();
            tbxQ4 = new TextBox();
            lblQ5 = new Label();
            tbxQ5 = new TextBox();
            lblQ6 = new Label();
            tbxQ6 = new TextBox();
            lblX = new Label();
            tbxX = new TextBox();
            lblY = new Label();
            tbxY = new TextBox();
            lblZ = new Label();
            tbxZ = new TextBox();
            lblVx = new Label();
            tbxVx = new TextBox();
            lblVy = new Label();
            tbxVy = new TextBox();
            lblVz = new Label();
            tbxVz = new TextBox();
            lblPsi = new Label();
            tbxPsi = new TextBox();
            bttnGoHome = new Button();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTitle.Location = new Point(13, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(68, 15);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Home";
            //
            // radX
            //
            radX.AutoSize = true;
            radX.Location = new Point(69, 8);
            radX.Name = "radX";
            radX.Size = new Size(32, 19);
            radX.TabIndex = 1;
            radX.Text = "X";
            radX.UseVisualStyleBackColor = true;
            radX.CheckedChanged += radMode_CheckedChanged;
            //
            // radQ
            //
            radQ.AutoSize = true;
            radQ.Checked = true;
            radQ.Location = new Point(107, 8);
            radQ.Name = "radQ";
            radQ.Size = new Size(34, 19);
            radQ.TabIndex = 2;
            radQ.TabStop = true;
            radQ.Text = "Q";
            radQ.UseVisualStyleBackColor = true;
            radQ.CheckedChanged += radMode_CheckedChanged;
            //
            // lblJoints
            //
            lblJoints.AutoSize = true;
            lblJoints.ForeColor = SystemColors.GrayText;
            lblJoints.Location = new Point(13, 30);
            lblJoints.Name = "lblJoints";
            lblJoints.Size = new Size(80, 15);
            lblJoints.TabIndex = 3;
            lblJoints.Text = "Articular [deg]";
            //
            // lblCart
            //
            lblCart.AutoSize = true;
            lblCart.ForeColor = SystemColors.GrayText;
            lblCart.Location = new Point(13, 231);
            lblCart.Name = "lblCart";
            lblCart.Size = new Size(90, 15);
            lblCart.TabIndex = 4;
            lblCart.Text = "Cartesiano [cm]";
            //
            // lblQ1
            //
            lblQ1.AutoSize = true;
            lblQ1.Location = new Point(20, 54);
            lblQ1.Name = "lblQ1";
            lblQ1.Size = new Size(24, 15);
            lblQ1.TabIndex = 3;
            lblQ1.Text = "q1";
            //
            // tbxQ1
            //
            tbxQ1.Location = new Point(43, 50);
            tbxQ1.Name = "tbxQ1";
            tbxQ1.Size = new Size(100, 23);
            tbxQ1.TabIndex = 4;
            tbxQ1.Text = "0.00";
            //
            // lblQ2
            //
            lblQ2.AutoSize = true;
            lblQ2.Location = new Point(20, 84);
            lblQ2.Name = "lblQ2";
            lblQ2.Size = new Size(24, 15);
            lblQ2.TabIndex = 5;
            lblQ2.Text = "q2";
            //
            // tbxQ2
            //
            tbxQ2.Location = new Point(43, 80);
            tbxQ2.Name = "tbxQ2";
            tbxQ2.Size = new Size(100, 23);
            tbxQ2.TabIndex = 6;
            tbxQ2.Text = "0.00";
            //
            // lblQ3
            //
            lblQ3.AutoSize = true;
            lblQ3.Location = new Point(20, 114);
            lblQ3.Name = "lblQ3";
            lblQ3.Size = new Size(24, 15);
            lblQ3.TabIndex = 7;
            lblQ3.Text = "q3";
            //
            // tbxQ3
            //
            tbxQ3.Location = new Point(43, 110);
            tbxQ3.Name = "tbxQ3";
            tbxQ3.Size = new Size(100, 23);
            tbxQ3.TabIndex = 8;
            tbxQ3.Text = "0.00";
            //
            // lblQ4
            //
            lblQ4.AutoSize = true;
            lblQ4.Location = new Point(20, 144);
            lblQ4.Name = "lblQ4";
            lblQ4.Size = new Size(24, 15);
            lblQ4.TabIndex = 9;
            lblQ4.Text = "q4";
            //
            // tbxQ4
            //
            tbxQ4.Location = new Point(43, 140);
            tbxQ4.Name = "tbxQ4";
            tbxQ4.Size = new Size(100, 23);
            tbxQ4.TabIndex = 10;
            tbxQ4.Text = "0.00";
            //
            // lblQ5
            //
            lblQ5.AutoSize = true;
            lblQ5.Location = new Point(20, 174);
            lblQ5.Name = "lblQ5";
            lblQ5.Size = new Size(24, 15);
            lblQ5.TabIndex = 11;
            lblQ5.Text = "q5";
            //
            // tbxQ5
            //
            tbxQ5.Location = new Point(43, 170);
            tbxQ5.Name = "tbxQ5";
            tbxQ5.Size = new Size(100, 23);
            tbxQ5.TabIndex = 12;
            tbxQ5.Text = "0.00";
            //
            // lblQ6
            //
            lblQ6.AutoSize = true;
            lblQ6.Location = new Point(20, 204);
            lblQ6.Name = "lblQ6";
            lblQ6.Size = new Size(24, 15);
            lblQ6.TabIndex = 13;
            lblQ6.Text = "q6";
            //
            // tbxQ6
            //
            tbxQ6.Location = new Point(43, 200);
            tbxQ6.Name = "tbxQ6";
            tbxQ6.Size = new Size(100, 23);
            tbxQ6.TabIndex = 14;
            tbxQ6.Text = "0.00";
            //
            // lblX
            //
            lblX.AutoSize = true;
            lblX.Location = new Point(20, 255);
            lblX.Name = "lblX";
            lblX.Size = new Size(24, 15);
            lblX.TabIndex = 15;
            lblX.Text = "x";
            //
            // tbxX
            //
            tbxX.Location = new Point(43, 251);
            tbxX.Name = "tbxX";
            tbxX.Size = new Size(100, 23);
            tbxX.TabIndex = 16;
            tbxX.Text = "0.00";
            //
            // lblY
            //
            lblY.AutoSize = true;
            lblY.Location = new Point(20, 285);
            lblY.Name = "lblY";
            lblY.Size = new Size(24, 15);
            lblY.TabIndex = 17;
            lblY.Text = "y";
            //
            // tbxY
            //
            tbxY.Location = new Point(43, 281);
            tbxY.Name = "tbxY";
            tbxY.Size = new Size(100, 23);
            tbxY.TabIndex = 18;
            tbxY.Text = "0.00";
            //
            // lblZ
            //
            lblZ.AutoSize = true;
            lblZ.Location = new Point(20, 315);
            lblZ.Name = "lblZ";
            lblZ.Size = new Size(24, 15);
            lblZ.TabIndex = 19;
            lblZ.Text = "z";
            //
            // tbxZ
            //
            tbxZ.Location = new Point(43, 311);
            tbxZ.Name = "tbxZ";
            tbxZ.Size = new Size(100, 23);
            tbxZ.TabIndex = 20;
            tbxZ.Text = "0.00";
            //
            // lblVx
            //
            lblVx.AutoSize = true;
            lblVx.Location = new Point(20, 345);
            lblVx.Name = "lblVx";
            lblVx.Size = new Size(24, 15);
            lblVx.TabIndex = 21;
            lblVx.Text = "vx";
            //
            // tbxVx
            //
            tbxVx.Location = new Point(43, 341);
            tbxVx.Name = "tbxVx";
            tbxVx.Size = new Size(100, 23);
            tbxVx.TabIndex = 22;
            tbxVx.Text = "0.00";
            //
            // lblVy
            //
            lblVy.AutoSize = true;
            lblVy.Location = new Point(20, 375);
            lblVy.Name = "lblVy";
            lblVy.Size = new Size(24, 15);
            lblVy.TabIndex = 23;
            lblVy.Text = "vy";
            //
            // tbxVy
            //
            tbxVy.Location = new Point(43, 371);
            tbxVy.Name = "tbxVy";
            tbxVy.Size = new Size(100, 23);
            tbxVy.TabIndex = 24;
            tbxVy.Text = "0.00";
            //
            // lblVz
            //
            lblVz.AutoSize = true;
            lblVz.Location = new Point(20, 405);
            lblVz.Name = "lblVz";
            lblVz.Size = new Size(24, 15);
            lblVz.TabIndex = 25;
            lblVz.Text = "vz";
            //
            // tbxVz
            //
            tbxVz.Location = new Point(43, 401);
            tbxVz.Name = "tbxVz";
            tbxVz.Size = new Size(100, 23);
            tbxVz.TabIndex = 26;
            tbxVz.Text = "-1.00";
            //
            // lblPsi
            //
            lblPsi.AutoSize = true;
            lblPsi.Location = new Point(20, 435);
            lblPsi.Name = "lblPsi";
            lblPsi.Size = new Size(24, 15);
            lblPsi.TabIndex = 27;
            lblPsi.Text = "ψ";
            //
            // tbxPsi
            //
            tbxPsi.Location = new Point(43, 431);
            tbxPsi.Name = "tbxPsi";
            tbxPsi.Size = new Size(100, 23);
            tbxPsi.TabIndex = 28;
            tbxPsi.Text = "0.00";
            //
            // bttnGoHome
            //
            bttnGoHome.Location = new Point(20, 466);
            bttnGoHome.Name = "bttnGoHome";
            bttnGoHome.Size = new Size(126, 23);
            bttnGoHome.TabIndex = 29;
            bttnGoHome.Text = "Ir a Home";
            bttnGoHome.UseVisualStyleBackColor = true;
            bttnGoHome.Click += bttnGoHome_Click;
            //
            // ViperHomeControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblTitle);
            Controls.Add(lblJoints);
            Controls.Add(lblCart);
            Controls.Add(radX);
            Controls.Add(radQ);
            Controls.Add(lblQ1);
            Controls.Add(tbxQ1);
            Controls.Add(lblQ2);
            Controls.Add(tbxQ2);
            Controls.Add(lblQ3);
            Controls.Add(tbxQ3);
            Controls.Add(lblQ4);
            Controls.Add(tbxQ4);
            Controls.Add(lblQ5);
            Controls.Add(tbxQ5);
            Controls.Add(lblQ6);
            Controls.Add(tbxQ6);
            Controls.Add(lblX);
            Controls.Add(tbxX);
            Controls.Add(lblY);
            Controls.Add(tbxY);
            Controls.Add(lblZ);
            Controls.Add(tbxZ);
            Controls.Add(lblVx);
            Controls.Add(tbxVx);
            Controls.Add(lblVy);
            Controls.Add(tbxVy);
            Controls.Add(lblVz);
            Controls.Add(tbxVz);
            Controls.Add(lblPsi);
            Controls.Add(tbxPsi);
            Controls.Add(bttnGoHome);
            Name = "ViperHomeControl";
            Size = new Size(159, 501);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label   lblTitle;
        private Label   lblJoints;
        private Label   lblCart;
        private RadioButton radX;
        private RadioButton radQ;
        private Label   lblQ1;
        private TextBox tbxQ1;
        private Label   lblQ2;
        private TextBox tbxQ2;
        private Label   lblQ3;
        private TextBox tbxQ3;
        private Label   lblQ4;
        private TextBox tbxQ4;
        private Label   lblQ5;
        private TextBox tbxQ5;
        private Label   lblQ6;
        private TextBox tbxQ6;
        private Label   lblX;
        private TextBox tbxX;
        private Label   lblY;
        private TextBox tbxY;
        private Label   lblZ;
        private TextBox tbxZ;
        private Label   lblVx;
        private TextBox tbxVx;
        private Label   lblVy;
        private TextBox tbxVy;
        private Label   lblVz;
        private TextBox tbxVz;
        private Label   lblPsi;
        private TextBox tbxPsi;
        private Button  bttnGoHome;
    }
}
