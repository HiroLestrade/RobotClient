

namespace Teleoperation
{
    partial class TeleoperationForm
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
            panelTop           = new Panel();
            lblRobot1          = new Label();
            lblDeviceName1     = new Label();
            tbDevice1          = new TextBox();
            bttnCalibrate1     = new Button();
            lblRobot2          = new Label();
            lblDeviceName2     = new Label();
            tbDevice2          = new TextBox();
            bttnCalibrate2     = new Button();
            bttnConnect        = new Button();
            bttnReadEncoders   = new Button();
            bttnStartTeleop    = new Button();
            encoders1          = new GeomagicEncodersControl();
            encoders2          = new GeomagicEncodersControl();
            grpForceSensor     = new GroupBox();
            forceSensorControl = new ForceSensorControl();
            plotsControl       = new TeleoperationPlotsControl();
            statusStrip1       = new StatusStrip();
            status1LabelPrefix = new ToolStripStatusLabel();
            status1LabelValue  = new ToolStripStatusLabel();
            calib1LabelPrefix  = new ToolStripStatusLabel();
            calib1LabelValue   = new ToolStripStatusLabel();
            robotSeparator     = new ToolStripStatusLabel();
            status2LabelPrefix = new ToolStripStatusLabel();
            status2LabelValue  = new ToolStripStatusLabel();
            calib2LabelPrefix  = new ToolStripStatusLabel();
            calib2LabelValue   = new ToolStripStatusLabel();
            readingSeparator   = new ToolStripStatusLabel();
            readingLabelPrefix = new ToolStripStatusLabel();
            readingLabelValue  = new ToolStripStatusLabel();
            forceSeparator     = new ToolStripStatusLabel();
            forceLabelPrefix   = new ToolStripStatusLabel();
            forceLabelValue    = new ToolStripStatusLabel();
            teleopSeparator    = new ToolStripStatusLabel();
            teleopLabelPrefix  = new ToolStripStatusLabel();
            teleopLabelValue   = new ToolStripStatusLabel();
            panelTop.SuspendLayout();
            grpForceSensor.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            //
            // panelTop
            //
            panelTop.Controls.Add(lblRobot1);
            panelTop.Controls.Add(lblDeviceName1);
            panelTop.Controls.Add(tbDevice1);
            panelTop.Controls.Add(bttnCalibrate1);
            panelTop.Controls.Add(lblRobot2);
            panelTop.Controls.Add(lblDeviceName2);
            panelTop.Controls.Add(tbDevice2);
            panelTop.Controls.Add(bttnCalibrate2);
            panelTop.Controls.Add(bttnConnect);
            panelTop.Controls.Add(bttnReadEncoders);
            panelTop.Controls.Add(bttnStartTeleop);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1347, 110);
            panelTop.TabIndex = 0;
            //
            // lblRobot1
            //
            lblRobot1.AutoSize = true;
            lblRobot1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRobot1.Location = new Point(12, 22);
            lblRobot1.Name = "lblRobot1";
            lblRobot1.Size = new Size(110, 15);
            lblRobot1.TabIndex = 0;
            lblRobot1.Text = "Robot 1 (maestro)";
            //
            // lblDeviceName1
            //
            lblDeviceName1.AutoSize = true;
            lblDeviceName1.Location = new Point(140, 22);
            lblDeviceName1.Name = "lblDeviceName1";
            lblDeviceName1.Size = new Size(130, 15);
            lblDeviceName1.TabIndex = 1;
            lblDeviceName1.Text = "Nombre del dispositivo";
            //
            // tbDevice1
            //
            tbDevice1.Location = new Point(280, 18);
            tbDevice1.Name = "tbDevice1";
            tbDevice1.Size = new Size(200, 23);
            tbDevice1.TabIndex = 2;
            tbDevice1.Text = "Left Device";
            //
            // bttnCalibrate1
            //
            bttnCalibrate1.Location = new Point(496, 18);
            bttnCalibrate1.Name = "bttnCalibrate1";
            bttnCalibrate1.Size = new Size(90, 23);
            bttnCalibrate1.TabIndex = 3;
            bttnCalibrate1.Text = "Calibrar";
            bttnCalibrate1.UseVisualStyleBackColor = true;
            bttnCalibrate1.Click += bttnCalibrate1_Click;
            //
            // lblRobot2
            //
            lblRobot2.AutoSize = true;
            lblRobot2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblRobot2.Location = new Point(12, 62);
            lblRobot2.Name = "lblRobot2";
            lblRobot2.Size = new Size(110, 15);
            lblRobot2.TabIndex = 4;
            lblRobot2.Text = "Robot 2 (esclavo)";
            //
            // lblDeviceName2
            //
            lblDeviceName2.AutoSize = true;
            lblDeviceName2.Location = new Point(140, 62);
            lblDeviceName2.Name = "lblDeviceName2";
            lblDeviceName2.Size = new Size(130, 15);
            lblDeviceName2.TabIndex = 5;
            lblDeviceName2.Text = "Nombre del dispositivo";
            //
            // tbDevice2
            //
            tbDevice2.Location = new Point(280, 58);
            tbDevice2.Name = "tbDevice2";
            tbDevice2.Size = new Size(200, 23);
            tbDevice2.TabIndex = 6;
            tbDevice2.Text = "Right Device";
            //
            // bttnCalibrate2
            //
            bttnCalibrate2.Location = new Point(496, 58);
            bttnCalibrate2.Name = "bttnCalibrate2";
            bttnCalibrate2.Size = new Size(90, 23);
            bttnCalibrate2.TabIndex = 7;
            bttnCalibrate2.Text = "Calibrar";
            bttnCalibrate2.UseVisualStyleBackColor = true;
            bttnCalibrate2.Click += bttnCalibrate2_Click;
            //
            // bttnConnect
            //
            // Both devices come up together: OpenHaptics needs every hdInitDevice
            // to happen before the shared scheduler starts.
            bttnConnect.Location = new Point(626, 18);
            bttnConnect.Name = "bttnConnect";
            bttnConnect.Size = new Size(130, 63);
            bttnConnect.TabIndex = 8;
            bttnConnect.Text = "Conectar ambos";
            bttnConnect.UseVisualStyleBackColor = true;
            bttnConnect.Click += bttnConnect_Click;
            //
            // bttnReadEncoders
            //
            bttnReadEncoders.Location = new Point(772, 18);
            bttnReadEncoders.Name = "bttnReadEncoders";
            bttnReadEncoders.Size = new Size(130, 63);
            bttnReadEncoders.TabIndex = 9;
            bttnReadEncoders.Text = "Leer encoders";
            bttnReadEncoders.UseVisualStyleBackColor = true;
            bttnReadEncoders.Click += bttnReadEncoders_Click;
            //
            // bttnStartTeleop
            //
            bttnStartTeleop.Location = new Point(918, 18);
            bttnStartTeleop.Name = "bttnStartTeleop";
            bttnStartTeleop.Size = new Size(160, 63);
            bttnStartTeleop.TabIndex = 10;
            bttnStartTeleop.Text = "Iniciar teleoperación";
            bttnStartTeleop.UseVisualStyleBackColor = true;
            bttnStartTeleop.Click += bttnStartTeleop_Click;
            //
            // encoders1
            //
            encoders1.Location = new Point(12, 130);
            encoders1.Name = "encoders1";
            encoders1.Size = new Size(200, 340);
            encoders1.TabIndex = 1;
            //
            // encoders2
            //
            encoders2.Location = new Point(232, 130);
            encoders2.Name = "encoders2";
            encoders2.Size = new Size(200, 340);
            encoders2.TabIndex = 2;
            //
            // grpForceSensor
            //
            // Same panel as experiment 1; the ATI sensor is mounted on the remote
            // (slave) robot. Sits under the two encoder readouts, spanning them.
            grpForceSensor.Controls.Add(forceSensorControl);
            grpForceSensor.Location = new Point(12, 482);
            grpForceSensor.Name = "grpForceSensor";
            grpForceSensor.Size = new Size(420, 300);
            grpForceSensor.TabIndex = 3;
            grpForceSensor.TabStop = false;
            grpForceSensor.Text = "Sensor de fuerza (robot remoto)";
            //
            // forceSensorControl
            //
            forceSensorControl.Dock = DockStyle.Fill;
            forceSensorControl.Location = new Point(3, 19);
            forceSensorControl.Name = "forceSensorControl";
            forceSensorControl.Size = new Size(414, 278);
            forceSensorControl.TabIndex = 0;
            //
            // plotsControl
            //
            plotsControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            plotsControl.Location = new Point(452, 130);
            plotsControl.Name = "plotsControl";
            plotsControl.Size = new Size(883, 652);
            plotsControl.TabIndex = 4;
            //
            // statusStrip1
            //
            statusStrip1.Dock = DockStyle.Bottom;
            statusStrip1.Items.AddRange(new ToolStripItem[] { status1LabelPrefix, status1LabelValue, calib1LabelPrefix, calib1LabelValue, robotSeparator, status2LabelPrefix, status2LabelValue, calib2LabelPrefix, calib2LabelValue, readingSeparator, readingLabelPrefix, readingLabelValue, forceSeparator, forceLabelPrefix, forceLabelValue, teleopSeparator, teleopLabelPrefix, teleopLabelValue });
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1347, 22);
            statusStrip1.TabIndex = 5;
            //
            // status1LabelPrefix
            //
            status1LabelPrefix.Name = "status1LabelPrefix";
            status1LabelPrefix.Size = new Size(55, 17);
            status1LabelPrefix.Text = "Robot 1: ";
            //
            // status1LabelValue
            //
            status1LabelValue.ForeColor = Color.Red;
            status1LabelValue.Name = "status1LabelValue";
            status1LabelValue.Size = new Size(82, 17);
            status1LabelValue.Text = "Desconectado";
            //
            // calib1LabelPrefix
            //
            calib1LabelPrefix.Name = "calib1LabelPrefix";
            calib1LabelPrefix.Size = new Size(85, 17);
            calib1LabelPrefix.Text = "   |   Calibración: ";
            //
            // calib1LabelValue
            //
            calib1LabelValue.ForeColor = Color.Red;
            calib1LabelValue.Name = "calib1LabelValue";
            calib1LabelValue.Size = new Size(77, 17);
            calib1LabelValue.Text = "No Calibrado";
            //
            // robotSeparator
            //
            robotSeparator.Name = "robotSeparator";
            robotSeparator.Size = new Size(34, 17);
            robotSeparator.Text = "     ||     ";
            //
            // status2LabelPrefix
            //
            status2LabelPrefix.Name = "status2LabelPrefix";
            status2LabelPrefix.Size = new Size(55, 17);
            status2LabelPrefix.Text = "Robot 2: ";
            //
            // status2LabelValue
            //
            status2LabelValue.ForeColor = Color.Red;
            status2LabelValue.Name = "status2LabelValue";
            status2LabelValue.Size = new Size(82, 17);
            status2LabelValue.Text = "Desconectado";
            //
            // calib2LabelPrefix
            //
            calib2LabelPrefix.Name = "calib2LabelPrefix";
            calib2LabelPrefix.Size = new Size(85, 17);
            calib2LabelPrefix.Text = "   |   Calibración: ";
            //
            // calib2LabelValue
            //
            calib2LabelValue.ForeColor = Color.Red;
            calib2LabelValue.Name = "calib2LabelValue";
            calib2LabelValue.Size = new Size(77, 17);
            calib2LabelValue.Text = "No Calibrado";
            //
            // readingSeparator
            //
            readingSeparator.Name = "readingSeparator";
            readingSeparator.Size = new Size(34, 17);
            readingSeparator.Text = "     ||     ";
            //
            // readingLabelPrefix
            //
            readingLabelPrefix.Name = "readingLabelPrefix";
            readingLabelPrefix.Size = new Size(52, 17);
            readingLabelPrefix.Text = "Lectura: ";
            //
            // readingLabelValue
            //
            readingLabelValue.ForeColor = Color.Gray;
            readingLabelValue.Name = "readingLabelValue";
            readingLabelValue.Size = new Size(55, 17);
            readingLabelValue.Text = "Detenida";
            //
            // forceSeparator
            //
            forceSeparator.Name = "forceSeparator";
            forceSeparator.Size = new Size(34, 17);
            forceSeparator.Text = "     ||     ";
            //
            // forceLabelPrefix
            //
            forceLabelPrefix.Name = "forceLabelPrefix";
            forceLabelPrefix.Size = new Size(114, 17);
            forceLabelPrefix.Text = "Sensor de fuerza: ";
            //
            // forceLabelValue
            //
            forceLabelValue.ForeColor = Color.Red;
            forceLabelValue.Name = "forceLabelValue";
            forceLabelValue.Size = new Size(82, 17);
            forceLabelValue.Text = "Desconectado";
            //
            // teleopSeparator
            //
            teleopSeparator.Name = "teleopSeparator";
            teleopSeparator.Size = new Size(34, 17);
            teleopSeparator.Text = "     ||     ";
            //
            // teleopLabelPrefix
            //
            teleopLabelPrefix.Name = "teleopLabelPrefix";
            teleopLabelPrefix.Size = new Size(93, 17);
            teleopLabelPrefix.Text = "Teleoperación: ";
            //
            // teleopLabelValue
            //
            teleopLabelValue.ForeColor = Color.Gray;
            teleopLabelValue.Name = "teleopLabelValue";
            teleopLabelValue.Size = new Size(55, 17);
            teleopLabelValue.Text = "Detenida";
            //
            // TeleoperationForm
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1347, 859);
            Controls.Add(plotsControl);
            Controls.Add(grpForceSensor);
            Controls.Add(encoders2);
            Controls.Add(encoders1);
            Controls.Add(panelTop);
            Controls.Add(statusStrip1);
            Name = "TeleoperationForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Teleoperación Bilateral";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            grpForceSensor.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel   panelTop;
        private Label   lblRobot1;
        private Label   lblDeviceName1;
        private TextBox tbDevice1;
        private Button  bttnCalibrate1;
        private Label   lblRobot2;
        private Label   lblDeviceName2;
        private TextBox tbDevice2;
        private Button  bttnCalibrate2;
        private Button  bttnConnect;
        private Button  bttnReadEncoders;
        private Button  bttnStartTeleop;
        private GeomagicEncodersControl encoders1;
        private GeomagicEncodersControl encoders2;
        private GroupBox                   grpForceSensor;
        private ForceSensorControl         forceSensorControl;
        private TeleoperationPlotsControl  plotsControl;
        private StatusStrip          statusStrip1;
        private ToolStripStatusLabel status1LabelPrefix;
        private ToolStripStatusLabel status1LabelValue;
        private ToolStripStatusLabel calib1LabelPrefix;
        private ToolStripStatusLabel calib1LabelValue;
        private ToolStripStatusLabel robotSeparator;
        private ToolStripStatusLabel status2LabelPrefix;
        private ToolStripStatusLabel status2LabelValue;
        private ToolStripStatusLabel calib2LabelPrefix;
        private ToolStripStatusLabel calib2LabelValue;
        private ToolStripStatusLabel readingSeparator;
        private ToolStripStatusLabel readingLabelPrefix;
        private ToolStripStatusLabel readingLabelValue;
        private ToolStripStatusLabel forceSeparator;
        private ToolStripStatusLabel forceLabelPrefix;
        private ToolStripStatusLabel forceLabelValue;
        private ToolStripStatusLabel teleopSeparator;
        private ToolStripStatusLabel teleopLabelPrefix;
        private ToolStripStatusLabel teleopLabelValue;
    }
}
