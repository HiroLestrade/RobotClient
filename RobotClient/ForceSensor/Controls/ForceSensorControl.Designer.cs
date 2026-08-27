
namespace RobotClient
{
    partial class ForceSensorControl
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            lblDeviceName  = new Label();
            tbxDeviceName  = new TextBox();
            btnConnect     = new Button();
            lblStatus      = new Label();
            btnTare        = new Button();
            btnRead        = new Button();
            lblFx          = new Label();
            tbxFx          = new TextBox();
            lblFy          = new Label();
            tbxFy          = new TextBox();
            lblFz          = new Label();
            tbxFz          = new TextBox();
            SuspendLayout();
            //
            // lblDeviceName
            //
            lblDeviceName.AutoSize = true;
            lblDeviceName.Location = new Point(12, 18);
            lblDeviceName.Name = "lblDeviceName";
            lblDeviceName.Size = new Size(177, 15);
            lblDeviceName.TabIndex = 0;
            lblDeviceName.Text = "Nombre del dispositivo (NI-MAX):";
            //
            // tbxDeviceName
            //
            tbxDeviceName.Location = new Point(12, 36);
            tbxDeviceName.Name = "tbxDeviceName";
            tbxDeviceName.Size = new Size(150, 23);
            tbxDeviceName.TabIndex = 1;
            tbxDeviceName.Text = "Dev1";
            //
            // btnConnect
            //
            btnConnect.Location = new Point(172, 35);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(100, 23);
            btnConnect.TabIndex = 2;
            btnConnect.Text = "Conectar";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(12, 72);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(115, 15);
            lblStatus.TabIndex = 3;
            lblStatus.Text = "Estado: Desconectado";
            //
            // btnTare
            //
            btnTare.Enabled = false;
            btnTare.Location = new Point(12, 104);
            btnTare.Name = "btnTare";
            btnTare.Size = new Size(100, 23);
            btnTare.TabIndex = 4;
            btnTare.Text = "Tarar";
            btnTare.UseVisualStyleBackColor = true;
            btnTare.Click += btnTare_Click;
            //
            // btnRead
            //
            btnRead.Enabled = false;
            btnRead.Location = new Point(118, 104);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(100, 23);
            btnRead.TabIndex = 5;
            btnRead.Text = "Leer";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += btnRead_Click;
            //
            // lblFx
            //
            lblFx.AutoSize = true;
            lblFx.Location = new Point(12, 150);
            lblFx.Name = "lblFx";
            lblFx.Size = new Size(45, 15);
            lblFx.TabIndex = 6;
            lblFx.Text = "Fx [N]:";
            //
            // tbxFx
            //
            tbxFx.Location = new Point(90, 147);
            tbxFx.Name = "tbxFx";
            tbxFx.ReadOnly = true;
            tbxFx.Size = new Size(100, 23);
            tbxFx.TabIndex = 7;
            tbxFx.Text = "0.0000";
            //
            // lblFy
            //
            lblFy.AutoSize = true;
            lblFy.Location = new Point(12, 180);
            lblFy.Name = "lblFy";
            lblFy.Size = new Size(45, 15);
            lblFy.TabIndex = 8;
            lblFy.Text = "Fy [N]:";
            //
            // tbxFy
            //
            tbxFy.Location = new Point(90, 177);
            tbxFy.Name = "tbxFy";
            tbxFy.ReadOnly = true;
            tbxFy.Size = new Size(100, 23);
            tbxFy.TabIndex = 9;
            tbxFy.Text = "0.0000";
            //
            // lblFz
            //
            lblFz.AutoSize = true;
            lblFz.Location = new Point(12, 210);
            lblFz.Name = "lblFz";
            lblFz.Size = new Size(45, 15);
            lblFz.TabIndex = 10;
            lblFz.Text = "Fz [N]:";
            //
            // tbxFz
            //
            tbxFz.Location = new Point(90, 207);
            tbxFz.Name = "tbxFz";
            tbxFz.ReadOnly = true;
            tbxFz.Size = new Size(100, 23);
            tbxFz.TabIndex = 11;
            tbxFz.Text = "0.0000";
            //
            // ForceSensorControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblDeviceName);
            Controls.Add(tbxDeviceName);
            Controls.Add(btnConnect);
            Controls.Add(lblStatus);
            Controls.Add(btnTare);
            Controls.Add(btnRead);
            Controls.Add(lblFx);
            Controls.Add(tbxFx);
            Controls.Add(lblFy);
            Controls.Add(tbxFy);
            Controls.Add(lblFz);
            Controls.Add(tbxFz);
            Name = "ForceSensorControl";
            Size = new Size(505, 269);
            ResumeLayout(false);
            PerformLayout();
        }

        private Label   lblDeviceName;
        private TextBox tbxDeviceName;
        private Button  btnConnect;
        private Label   lblStatus;
        private Button  btnTare;
        private Button  btnRead;
        private Label   lblFx;
        private TextBox tbxFx;
        private Label   lblFy;
        private TextBox tbxFy;
        private Label   lblFz;
        private TextBox tbxFz;
    }
}
