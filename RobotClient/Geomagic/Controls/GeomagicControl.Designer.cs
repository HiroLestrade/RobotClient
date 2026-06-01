
namespace RobotClient
{
    partial class GeomagicControl
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            encodersControl     = new GeomagicEncodersControl();
            homeControl         = new GeomagicHomeControl();
            destinationControl  = new GeomagicDestinationControl();
            configControl       = new GeomagicConfigControl();
            plotsControl        = new GeomagicPlotsControl();
            bttnStop            = new Button();
            panel1              = new Panel();
            bttnCalibrate       = new Button();
            tbDeviceName        = new TextBox();
            bttnConnect         = new Button();

            lblDeviceName       = new Label();
            statusStrip1        = new StatusStrip();
            statusLabelPrefix   = new ToolStripStatusLabel();
            statusLabelValue    = new ToolStripStatusLabel();
            statusSeparator     = new ToolStripStatusLabel();
            calibrationLabelPrefix = new ToolStripStatusLabel();
            calibrationLabelValue  = new ToolStripStatusLabel();
            robotStateSeparator    = new ToolStripStatusLabel();
            robotStateLabelPrefix  = new ToolStripStatusLabel();
            robotStateLabelValue   = new ToolStripStatusLabel();
            timeSeparator       = new ToolStripStatusLabel();
            timeLabelValue      = new ToolStripStatusLabel();
            configControl.SuspendLayout();
            panel1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            //
            // encodersControl
            //
            encodersControl.Location = new Point(8, 76);
            encodersControl.Name = "encodersControl";
            encodersControl.Size = new Size(159, 368);
            encodersControl.TabIndex = 1;
            //
            // homeControl
            //
            homeControl.Location = new Point(188, 76);
            homeControl.Name = "homeControl";
            homeControl.Size = new Size(159, 368);
            homeControl.TabIndex = 15;
            //
            // destinationControl
            //
            destinationControl.Location = new Point(368, 76);
            destinationControl.Name = "destinationControl";
            destinationControl.Size = new Size(159, 368);
            destinationControl.TabIndex = 16;
            //
            // bttnStop
            //
            bttnStop.BackColor = Color.FromArgb(255, 220, 220);
            bttnStop.Location = new Point(8, 450);
            bttnStop.Name = "bttnStop";
            bttnStop.Size = new Size(519, 28);
            bttnStop.TabIndex = 19;
            bttnStop.Text = "Detener";
            bttnStop.UseVisualStyleBackColor = false;
            bttnStop.Click += bttnStop_Click;
            //
            // configControl
            //
            configControl.Location = new Point(8, 484);
            configControl.Name = "configControl";
            configControl.Size = new Size(519, 303);
            configControl.TabIndex = 18;
            //
            // plotsControl
            //
            plotsControl.Location = new Point(548, 75);
            plotsControl.Name = "plotsControl";
            plotsControl.Size = new Size(785, 678);
            plotsControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            plotsControl.TabIndex = 17;
            //
            // panel1
            //
            panel1.Controls.Add(bttnCalibrate);
            panel1.Controls.Add(tbDeviceName);
            panel1.Controls.Add(bttnConnect);

            panel1.Controls.Add(lblDeviceName);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1347, 67);
            panel1.TabIndex = 9;
            //
            // bttnCalibrate
            //
            bttnCalibrate.Location = new Point(478, 22);
            bttnCalibrate.Name = "bttnCalibrate";
            bttnCalibrate.Size = new Size(75, 23);
            bttnCalibrate.TabIndex = 5;
            bttnCalibrate.Text = "Calibrar";
            bttnCalibrate.UseVisualStyleBackColor = true;
            bttnCalibrate.Click += bttnCalibrate_Click;
            //
            // tbDeviceName
            //
            tbDeviceName.Location = new Point(157, 22);
            tbDeviceName.Name = "tbDeviceName";
            tbDeviceName.Size = new Size(200, 23);
            tbDeviceName.TabIndex = 3;
            tbDeviceName.Text = "Default Device";
            //
            // bttnConnect
            //
            bttnConnect.Location = new Point(389, 22);
            bttnConnect.Name = "bttnConnect";
            bttnConnect.Size = new Size(75, 23);
            bttnConnect.TabIndex = 4;
            bttnConnect.Text = "Conectar";
            bttnConnect.UseVisualStyleBackColor = true;
            bttnConnect.Click += bttnConnect_Click;

            //
            // lblDeviceName
            //
            lblDeviceName.AutoSize = true;
            lblDeviceName.Location = new Point(12, 26);
            lblDeviceName.Name = "lblDeviceName";
            lblDeviceName.Size = new Size(130, 15);
            lblDeviceName.TabIndex = 2;
            lblDeviceName.Text = "Nombre del dispositivo";
            //
            // statusStrip1
            //
            statusStrip1.Dock = DockStyle.Bottom;
            statusStrip1.Items.AddRange(new ToolStripItem[] { statusLabelPrefix, statusLabelValue, statusSeparator, calibrationLabelPrefix, calibrationLabelValue, robotStateSeparator, robotStateLabelPrefix, robotStateLabelValue, timeSeparator, timeLabelValue });
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1347, 22);
            statusStrip1.TabIndex = 0;
            //
            // statusLabelPrefix
            //
            statusLabelPrefix.Name = "statusLabelPrefix";
            statusLabelPrefix.Size = new Size(116, 17);
            statusLabelPrefix.Text = "Estado de conexión: ";
            //
            // statusLabelValue
            //
            statusLabelValue.ForeColor = Color.Red;
            statusLabelValue.Name = "statusLabelValue";
            statusLabelValue.Size = new Size(82, 17);
            statusLabelValue.Text = "Desconectado";
            //
            // statusSeparator
            //
            statusSeparator.Name = "statusSeparator";
            statusSeparator.Size = new Size(28, 17);
            statusSeparator.Text = "   |   ";
            //
            // calibrationLabelPrefix
            //
            calibrationLabelPrefix.Name = "calibrationLabelPrefix";
            calibrationLabelPrefix.Size = new Size(73, 17);
            calibrationLabelPrefix.Text = "Calibración: ";
            //
            // calibrationLabelValue
            //
            calibrationLabelValue.ForeColor = Color.Red;
            calibrationLabelValue.Name = "calibrationLabelValue";
            calibrationLabelValue.Size = new Size(77, 17);
            calibrationLabelValue.Text = "No Calibrado";
            //
            // robotStateSeparator
            //
            robotStateSeparator.Name = "robotStateSeparator";
            robotStateSeparator.Size = new Size(28, 17);
            robotStateSeparator.Text = "   |   ";
            //
            // robotStateLabelPrefix
            //
            robotStateLabelPrefix.Name = "robotStateLabelPrefix";
            robotStateLabelPrefix.Size = new Size(96, 17);
            robotStateLabelPrefix.Text = "Estado de robot: ";
            //
            // robotStateLabelValue
            //
            robotStateLabelValue.Name = "robotStateLabelValue";
            robotStateLabelValue.Size = new Size(55, 17);
            robotStateLabelValue.Text = "Detenido";
            //
            // timeSeparator
            //
            timeSeparator.Name = "timeSeparator";
            timeSeparator.Size = new Size(28, 17);
            timeSeparator.Text = "   |   ";
            //
            // timeLabelValue
            //
            timeLabelValue.Name = "timeLabelValue";
            timeLabelValue.Size = new Size(0, 17);
            //
            // GeomagicControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(plotsControl);
            Controls.Add(configControl);
            Controls.Add(bttnStop);
            Controls.Add(destinationControl);
            Controls.Add(homeControl);
            Controls.Add(encodersControl);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Name = "GeomagicControl";
            Size = new Size(1347, 816);
            configControl.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private GeomagicEncodersControl    encodersControl;
        private GeomagicConfigControl      configControl;
        private GeomagicHomeControl        homeControl;
        private GeomagicDestinationControl destinationControl;
        private GeomagicPlotsControl       plotsControl;
        private Panel                      panel1;
        private Button                     bttnCalibrate;
        private TextBox                    tbDeviceName;
        private Button                     bttnConnect;
        private Label                      lblDeviceName;
        private StatusStrip                statusStrip1;
        private ToolStripStatusLabel       statusLabelPrefix;
        private ToolStripStatusLabel       statusLabelValue;
        private ToolStripStatusLabel       statusSeparator;
        private ToolStripStatusLabel       calibrationLabelPrefix;
        private ToolStripStatusLabel       calibrationLabelValue;
        private ToolStripStatusLabel       robotStateSeparator;
        private ToolStripStatusLabel       robotStateLabelPrefix;
        private ToolStripStatusLabel       robotStateLabelValue;
        private ToolStripStatusLabel       timeSeparator;
        private ToolStripStatusLabel       timeLabelValue;
        private Button                     bttnStop;
    }
}
