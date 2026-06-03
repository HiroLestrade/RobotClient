
namespace RobotClient
{
    partial class GeomagicConfigControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serialPort?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabConfig           = new TabControl();
            tabPageControl      = new TabPage();
            lblController       = new Label();
            cmbController       = new ComboBox();
            pidGainsControl     = new PidGainsControl();
            tabPageTrajectory   = new TabPage();
            lblTrajectory       = new Label();
            cmbTrajectory       = new ComboBox();
            lblTf               = new Label();
            tbxTf               = new TextBox();
            tabPageEndEffector  = new TabPage();
            lblEndEffector      = new Label();
            cmbEndEffector      = new ComboBox();
            lblComPort          = new Label();
            tbxComPort          = new TextBox();
            lblBaudRate         = new Label();
            cmbBaudRate         = new ComboBox();
            btnConnect          = new Button();
            lblStatus           = new Label();
            chkElectromagnet    = new CheckBox();
            tabPageInterpreter  = new TabPage();
            txtGCode            = new TextBox();
            btnExecuteGCode     = new Button();
            tabConfig.SuspendLayout();
            tabPageControl.SuspendLayout();
            tabPageTrajectory.SuspendLayout();
            tabPageEndEffector.SuspendLayout();
            SuspendLayout();
            //
            // tabConfig
            //
            tabConfig.Controls.Add(tabPageControl);
            tabConfig.Controls.Add(tabPageTrajectory);
            tabConfig.Controls.Add(tabPageEndEffector);
            tabConfig.Controls.Add(tabPageInterpreter);
            tabConfig.Dock = DockStyle.Fill;
            tabConfig.Location = new Point(0, 0);
            tabConfig.Name = "tabConfig";
            tabConfig.SelectedIndex = 0;
            tabConfig.Size = new Size(519, 303);
            tabConfig.TabIndex = 0;
            //
            // tabPageControl
            //
            tabPageControl.Controls.Add(pidGainsControl);
            tabPageControl.Controls.Add(cmbController);
            tabPageControl.Controls.Add(lblController);
            tabPageControl.Location = new Point(4, 24);
            tabPageControl.Name = "tabPageControl";
            tabPageControl.Padding = new Padding(3);
            tabPageControl.Size = new Size(511, 275);
            tabPageControl.TabIndex = 0;
            tabPageControl.Text = "Control";
            tabPageControl.UseVisualStyleBackColor = true;
            //
            // lblController
            //
            lblController.AutoSize = true;
            lblController.Location = new Point(12, 18);
            lblController.Name = "lblController";
            lblController.TabIndex = 0;
            lblController.Text = "Controlador:";
            //
            // cmbController
            //
            cmbController.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbController.Items.AddRange(new object[] { "PID" });
            cmbController.Location = new Point(12, 36);
            cmbController.Name = "cmbController";
            cmbController.Size = new Size(100, 23);
            cmbController.TabIndex = 1;
            cmbController.SelectedIndex = 0;
            cmbController.SelectedIndexChanged += cmbController_SelectedIndexChanged;
            //
            // pidGainsControl
            //
            pidGainsControl.Location = new Point(3, 68);
            pidGainsControl.Name = "pidGainsControl";
            pidGainsControl.Size = new Size(340, 150);
            pidGainsControl.TabIndex = 2;
            pidGainsControl.Visible = true;
            //
            // tabPageTrajectory
            //
            tabPageTrajectory.Controls.Add(tbxTf);
            tabPageTrajectory.Controls.Add(lblTf);
            tabPageTrajectory.Controls.Add(cmbTrajectory);
            tabPageTrajectory.Controls.Add(lblTrajectory);
            tabPageTrajectory.Location = new Point(4, 24);
            tabPageTrajectory.Name = "tabPageTrajectory";
            tabPageTrajectory.Padding = new Padding(3);
            tabPageTrajectory.Size = new Size(511, 275);
            tabPageTrajectory.TabIndex = 1;
            tabPageTrajectory.Text = "Trayectoria";
            tabPageTrajectory.UseVisualStyleBackColor = true;
            //
            // lblTrajectory
            //
            lblTrajectory.AutoSize = true;
            lblTrajectory.Location = new Point(12, 18);
            lblTrajectory.Name = "lblTrajectory";
            lblTrajectory.TabIndex = 0;
            lblTrajectory.Text = "Trayectoria:";
            //
            // cmbTrajectory
            //
            cmbTrajectory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTrajectory.Items.AddRange(new object[] { "Polinomio de 5° grado", "Trayectoria MPC" });
            cmbTrajectory.Location = new Point(12, 36);
            cmbTrajectory.Name = "cmbTrajectory";
            cmbTrajectory.Size = new Size(200, 23);
            cmbTrajectory.TabIndex = 1;
            cmbTrajectory.SelectedIndex = 0;
            //
            // lblTf
            //
            lblTf.AutoSize = true;
            lblTf.Location = new Point(12, 78);
            lblTf.Name = "lblTf";
            lblTf.TabIndex = 2;
            lblTf.Text = "Tiempo [s]:";
            //
            // tbxTf
            //
            tbxTf.Location = new Point(90, 75);
            tbxTf.Name = "tbxTf";
            tbxTf.Size = new Size(60, 23);
            tbxTf.TabIndex = 3;
            tbxTf.Text = "2.0";
            //
            // tabPageEndEffector
            //
            tabPageEndEffector.Controls.Add(chkElectromagnet);
            tabPageEndEffector.Controls.Add(lblStatus);
            tabPageEndEffector.Controls.Add(btnConnect);
            tabPageEndEffector.Controls.Add(cmbBaudRate);
            tabPageEndEffector.Controls.Add(lblBaudRate);
            tabPageEndEffector.Controls.Add(tbxComPort);
            tabPageEndEffector.Controls.Add(lblComPort);
            tabPageEndEffector.Controls.Add(cmbEndEffector);
            tabPageEndEffector.Controls.Add(lblEndEffector);
            tabPageEndEffector.Location = new Point(4, 24);
            tabPageEndEffector.Name = "tabPageEndEffector";
            tabPageEndEffector.Padding = new Padding(3);
            tabPageEndEffector.Size = new Size(511, 275);
            tabPageEndEffector.TabIndex = 2;
            tabPageEndEffector.Text = "Efector Final";
            tabPageEndEffector.UseVisualStyleBackColor = true;
            //
            // lblEndEffector
            //
            lblEndEffector.AutoSize = true;
            lblEndEffector.Location = new Point(12, 18);
            lblEndEffector.Name = "lblEndEffector";
            lblEndEffector.TabIndex = 0;
            lblEndEffector.Text = "Efector Final:";
            //
            // cmbEndEffector
            //
            cmbEndEffector.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEndEffector.Items.AddRange(new object[] { "Electroimán" });
            cmbEndEffector.Location = new Point(12, 36);
            cmbEndEffector.Name = "cmbEndEffector";
            cmbEndEffector.Size = new Size(200, 23);
            cmbEndEffector.TabIndex = 1;
            cmbEndEffector.SelectedIndex = 0;
            //
            // lblComPort
            //
            lblComPort.AutoSize = true;
            lblComPort.Location = new Point(12, 78);
            lblComPort.Name = "lblComPort";
            lblComPort.TabIndex = 2;
            lblComPort.Text = "Puerto COM:";
            //
            // tbxComPort
            //
            tbxComPort.Location = new Point(90, 75);
            tbxComPort.Name = "tbxComPort";
            tbxComPort.Size = new Size(70, 23);
            tbxComPort.TabIndex = 3;
            tbxComPort.Text = "COM3";
            //
            // lblBaudRate
            //
            lblBaudRate.AutoSize = true;
            lblBaudRate.Location = new Point(175, 78);
            lblBaudRate.Name = "lblBaudRate";
            lblBaudRate.TabIndex = 4;
            lblBaudRate.Text = "Baud Rate:";
            //
            // cmbBaudRate
            //
            cmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBaudRate.Items.AddRange(new object[] { "9600", "19200", "38400", "57600", "115200" });
            cmbBaudRate.Location = new Point(248, 75);
            cmbBaudRate.Name = "cmbBaudRate";
            cmbBaudRate.Size = new Size(85, 23);
            cmbBaudRate.TabIndex = 5;
            cmbBaudRate.SelectedIndex = 4; // 115200 por defecto
            //
            // btnConnect
            //
            btnConnect.Location = new Point(12, 112);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(110, 23);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Conectar";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            //
            // lblStatus
            //
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(12, 148);
            lblStatus.Name = "lblStatus";
            lblStatus.TabIndex = 7;
            lblStatus.Text = "Estado: Desconectado";
            //
            // chkElectromagnet
            //
            chkElectromagnet.Appearance = Appearance.Button;
            chkElectromagnet.AutoSize = false;
            chkElectromagnet.Enabled = false;
            chkElectromagnet.Location = new Point(12, 185);
            chkElectromagnet.Name = "chkElectromagnet";
            chkElectromagnet.Size = new Size(120, 26);
            chkElectromagnet.TabIndex = 8;
            chkElectromagnet.Text = "Electroimán: OFF";
            chkElectromagnet.TextAlign = ContentAlignment.MiddleCenter;
            chkElectromagnet.UseVisualStyleBackColor = true;
            chkElectromagnet.CheckedChanged += chkElectromagnet_CheckedChanged;
            //
            // tabPageInterpreter
            //
            tabPageInterpreter.Controls.Add(txtGCode);
            tabPageInterpreter.Controls.Add(btnExecuteGCode);
            tabPageInterpreter.Location = new Point(4, 24);
            tabPageInterpreter.Name = "tabPageInterpreter";
            tabPageInterpreter.Padding = new Padding(3);
            tabPageInterpreter.Size = new Size(511, 275);
            tabPageInterpreter.TabIndex = 3;
            tabPageInterpreter.Text = "Intérprete";
            tabPageInterpreter.UseVisualStyleBackColor = true;
            //
            // txtGCode
            //
            txtGCode.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point);
            txtGCode.Location = new Point(6, 6);
            txtGCode.Multiline = true;
            txtGCode.Name = "txtGCode";
            txtGCode.ScrollBars = ScrollBars.Vertical;
            txtGCode.Size = new Size(499, 220);
            txtGCode.TabIndex = 0;
            txtGCode.Text = "G0 A0 B15 C-80\r\nG4 P1000\r\nG0 A0 B20 C-120\r\nG4 P1000\r\nG1 X10 Y0 Z0\r\nG4 P1000\r\nG1 X20 Y0 Z0\r\nG4 P1000";
            //
            // btnExecuteGCode
            //
            btnExecuteGCode.Location = new Point(6, 236);
            btnExecuteGCode.Name = "btnExecuteGCode";
            btnExecuteGCode.Size = new Size(80, 26);
            btnExecuteGCode.TabIndex = 1;
            btnExecuteGCode.Text = "Ejecutar";
            btnExecuteGCode.UseVisualStyleBackColor = true;
            btnExecuteGCode.Click += btnExecuteGCode_Click;
            //
            // GeomagicConfigControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabConfig);
            Name = "GeomagicConfigControl";
            Size = new Size(519, 303);
            tabConfig.ResumeLayout(false);
            tabPageControl.ResumeLayout(false);
            tabPageControl.PerformLayout();
            tabPageTrajectory.ResumeLayout(false);
            tabPageTrajectory.PerformLayout();
            tabPageEndEffector.ResumeLayout(false);
            tabPageEndEffector.PerformLayout();
            ResumeLayout(false);
        }

        private TabControl      tabConfig;
        private TabPage         tabPageControl;
        private Label           lblController;
        private ComboBox        cmbController;
        private PidGainsControl pidGainsControl;
        private TabPage         tabPageTrajectory;
        private Label           lblTrajectory;
        private ComboBox        cmbTrajectory;
        private Label           lblTf;
        private TextBox         tbxTf;
        private TabPage         tabPageEndEffector;
        private Label           lblEndEffector;
        private ComboBox        cmbEndEffector;
        private Label           lblComPort;
        private TextBox         tbxComPort;
        private Label           lblBaudRate;
        private ComboBox        cmbBaudRate;
        private Button          btnConnect;
        private Label           lblStatus;
        private CheckBox        chkElectromagnet;
        private TabPage         tabPageInterpreter;
        private TextBox         txtGCode;
        private Button          btnExecuteGCode;
    }
}
