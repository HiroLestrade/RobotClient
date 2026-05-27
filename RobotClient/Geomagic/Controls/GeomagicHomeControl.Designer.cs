
namespace RobotClient
{
    partial class GeomagicHomeControl
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
            lblHome     = new Label();
            radHomeQ    = new RadioButton();
            radHomeX    = new RadioButton();
            tbxHomeQi1  = new TextBox();
            lblHomeQi1  = new Label();
            tbxHomeQi2  = new TextBox();
            lblHomeQ2   = new Label();
            tbxHomeQi3  = new TextBox();
            lblHomeQ3   = new Label();
            tbxHomeXi   = new TextBox();
            lblHomeXi   = new Label();
            tbxHomeYi   = new TextBox();
            lblHomeYi   = new Label();
            tbxHomeZi   = new TextBox();
            lblHomeZi   = new Label();
            bttnGoHome  = new Button();
            SuspendLayout();
            //
            // lblHome
            //
            lblHome.AutoSize = true;
            lblHome.Location = new Point(13, 12);
            lblHome.Name = "lblHome";
            lblHome.Size = new Size(40, 15);
            lblHome.TabIndex = 0;
            lblHome.Text = "Home";
            //
            // radHomeQ
            //
            radHomeQ.AutoSize = true;
            radHomeQ.Checked = true;
            radHomeQ.Location = new Point(107, 10);
            radHomeQ.Name = "radHomeQ";
            radHomeQ.Size = new Size(34, 19);
            radHomeQ.TabIndex = 17;
            radHomeQ.TabStop = true;
            radHomeQ.Text = "Q";
            radHomeQ.UseVisualStyleBackColor = true;
            radHomeQ.CheckedChanged += radMode_CheckedChanged;
            //
            // radHomeX
            //
            radHomeX.AutoSize = true;
            radHomeX.Location = new Point(69, 10);
            radHomeX.Name = "radHomeX";
            radHomeX.Size = new Size(32, 19);
            radHomeX.TabIndex = 16;
            radHomeX.Text = "X";
            radHomeX.UseVisualStyleBackColor = true;
            radHomeX.CheckedChanged += radMode_CheckedChanged;
            //
            // tbxHomeQi1
            //
            tbxHomeQi1.Location = new Point(43, 40);
            tbxHomeQi1.Name = "tbxHomeQi1";
            tbxHomeQi1.Size = new Size(100, 23);
            tbxHomeQi1.TabIndex = 2;
            tbxHomeQi1.Text = "0.00";
            //
            // lblHomeQi1
            //
            lblHomeQi1.AutoSize = true;
            lblHomeQi1.Location = new Point(20, 43);
            lblHomeQi1.Name = "lblHomeQi1";
            lblHomeQi1.Size = new Size(23, 15);
            lblHomeQi1.TabIndex = 1;
            lblHomeQi1.Text = "qi1";
            //
            // tbxHomeQi2
            //
            tbxHomeQi2.Location = new Point(43, 84);
            tbxHomeQi2.Name = "tbxHomeQi2";
            tbxHomeQi2.Size = new Size(100, 23);
            tbxHomeQi2.TabIndex = 4;
            tbxHomeQi2.Text = "15.00";
            //
            // lblHomeQ2
            //
            lblHomeQ2.AutoSize = true;
            lblHomeQ2.Location = new Point(20, 87);
            lblHomeQ2.Name = "lblHomeQ2";
            lblHomeQ2.Size = new Size(23, 15);
            lblHomeQ2.TabIndex = 3;
            lblHomeQ2.Text = "qi2";
            //
            // tbxHomeQi3
            //
            tbxHomeQi3.Location = new Point(43, 128);
            tbxHomeQi3.Name = "tbxHomeQi3";
            tbxHomeQi3.Size = new Size(100, 23);
            tbxHomeQi3.TabIndex = 6;
            tbxHomeQi3.Text = "-80.00";
            //
            // lblHomeQ3
            //
            lblHomeQ3.AutoSize = true;
            lblHomeQ3.Location = new Point(20, 131);
            lblHomeQ3.Name = "lblHomeQ3";
            lblHomeQ3.Size = new Size(23, 15);
            lblHomeQ3.TabIndex = 5;
            lblHomeQ3.Text = "qi3";
            //
            // tbxHomeXi
            //
            tbxHomeXi.Enabled = false;
            tbxHomeXi.Location = new Point(43, 183);
            tbxHomeXi.Name = "tbxHomeXi";
            tbxHomeXi.Size = new Size(100, 23);
            tbxHomeXi.TabIndex = 9;
            tbxHomeXi.Text = "10.00";
            //
            // lblHomeXi
            //
            lblHomeXi.AutoSize = true;
            lblHomeXi.Location = new Point(20, 186);
            lblHomeXi.Name = "lblHomeXi";
            lblHomeXi.Size = new Size(16, 15);
            lblHomeXi.TabIndex = 8;
            lblHomeXi.Text = "xi";
            //
            // tbxHomeYi
            //
            tbxHomeYi.Enabled = false;
            tbxHomeYi.Location = new Point(43, 227);
            tbxHomeYi.Name = "tbxHomeYi";
            tbxHomeYi.Size = new Size(100, 23);
            tbxHomeYi.TabIndex = 11;
            tbxHomeYi.Text = "0.00";
            //
            // lblHomeYi
            //
            lblHomeYi.AutoSize = true;
            lblHomeYi.Location = new Point(20, 230);
            lblHomeYi.Name = "lblHomeYi";
            lblHomeYi.Size = new Size(16, 15);
            lblHomeYi.TabIndex = 14;
            lblHomeYi.Text = "yi";
            //
            // tbxHomeZi
            //
            tbxHomeZi.Enabled = false;
            tbxHomeZi.Location = new Point(43, 271);
            tbxHomeZi.Name = "tbxHomeZi";
            tbxHomeZi.Size = new Size(100, 23);
            tbxHomeZi.TabIndex = 13;
            tbxHomeZi.Text = "0.00";
            //
            // lblHomeZi
            //
            lblHomeZi.AutoSize = true;
            lblHomeZi.Location = new Point(20, 274);
            lblHomeZi.Name = "lblHomeZi";
            lblHomeZi.Size = new Size(15, 15);
            lblHomeZi.TabIndex = 15;
            lblHomeZi.Text = "zi";
            //
            // bttnGoHome
            //
            bttnGoHome.Location = new Point(20, 327);
            bttnGoHome.Name = "bttnGoHome";
            bttnGoHome.Size = new Size(126, 23);
            bttnGoHome.TabIndex = 7;
            bttnGoHome.Text = "Ir a Home";
            bttnGoHome.UseVisualStyleBackColor = true;
            bttnGoHome.Click += bttnGoHome_Click;
            //
            // GeomagicHomeControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblHome);
            Controls.Add(radHomeQ);
            Controls.Add(radHomeX);
            Controls.Add(tbxHomeQi1);
            Controls.Add(lblHomeQi1);
            Controls.Add(tbxHomeQi2);
            Controls.Add(lblHomeQ2);
            Controls.Add(tbxHomeQi3);
            Controls.Add(lblHomeQ3);
            Controls.Add(tbxHomeXi);
            Controls.Add(lblHomeXi);
            Controls.Add(tbxHomeYi);
            Controls.Add(lblHomeYi);
            Controls.Add(tbxHomeZi);
            Controls.Add(lblHomeZi);
            Controls.Add(bttnGoHome);
            Name = "GeomagicHomeControl";
            Size = new Size(159, 368);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label     lblHome;
        private RadioButton radHomeQ;
        private RadioButton radHomeX;
        private TextBox   tbxHomeQi1;
        private Label     lblHomeQi1;
        private TextBox   tbxHomeQi2;
        private Label     lblHomeQ2;
        private TextBox   tbxHomeQi3;
        private Label     lblHomeQ3;
        private TextBox   tbxHomeXi;
        private Label     lblHomeXi;
        private TextBox   tbxHomeYi;
        private Label     lblHomeYi;
        private TextBox   tbxHomeZi;
        private Label     lblHomeZi;
        private Button    bttnGoHome;
    }
}
