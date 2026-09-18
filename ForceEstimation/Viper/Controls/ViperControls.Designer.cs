
namespace ForceEstimation
{
    partial class ViperControls
    {
        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            pnlConnection      = new Panel();
            lblPort            = new Label();
            tbxPort            = new TextBox();
            lblBaud            = new Label();
            cmbBaud            = new ComboBox();
            bttnConnect        = new Button();
            bttnTorque         = new Button();
            encodersControl    = new ViperEncodersControl();
            homeControl        = new ViperHomeControl();
            destinationControl = new ViperDestinationControl();
            configControl      = new ViperConfigControl();
            plotsControl       = new ViperPlotsControl();
            bttnStop           = new Button();
            bttnRest           = new Button();
            statusStrip1       = new StatusStrip();
            statusLabelPrefix  = new ToolStripStatusLabel();
            statusLabelValue   = new ToolStripStatusLabel();
            portSeparator      = new ToolStripStatusLabel();
            portLabelPrefix    = new ToolStripStatusLabel();
            portLabelValue     = new ToolStripStatusLabel();
            torqueSeparator    = new ToolStripStatusLabel();
            torqueLabelPrefix  = new ToolStripStatusLabel();
            torqueLabelValue   = new ToolStripStatusLabel();
            modeSeparator      = new ToolStripStatusLabel();
            modeLabelPrefix    = new ToolStripStatusLabel();
            modeLabelValue     = new ToolStripStatusLabel();
            faultSeparator     = new ToolStripStatusLabel();
            faultLabelPrefix   = new ToolStripStatusLabel();
            faultLabelValue    = new ToolStripStatusLabel();
            stateSeparator     = new ToolStripStatusLabel();
            stateLabelPrefix   = new ToolStripStatusLabel();
            stateLabelValue    = new ToolStripStatusLabel();
            pnlConnection.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            //
            // pnlConnection
            //
            // Full width across the top. The Viper is reached over a serial
            // port, so this asks for port and baud rate rather than a device
            // name as the Geomagic panel does.
            pnlConnection.Controls.Add(lblPort);
            pnlConnection.Controls.Add(tbxPort);
            pnlConnection.Controls.Add(lblBaud);
            pnlConnection.Controls.Add(cmbBaud);
            pnlConnection.Controls.Add(bttnConnect);
            pnlConnection.Controls.Add(bttnTorque);
            pnlConnection.Dock = DockStyle.Top;
            pnlConnection.Location = new Point(0, 0);
            pnlConnection.Name = "pnlConnection";
            pnlConnection.Size = new Size(1347, 67);
            pnlConnection.TabIndex = 0;
            //
            // lblPort
            //
            lblPort.AutoSize = true;
            lblPort.Location = new Point(12, 26);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(75, 15);
            lblPort.TabIndex = 0;
            lblPort.Text = "Puerto serial";
            //
            // tbxPort
            //
            tbxPort.Location = new Point(100, 22);
            tbxPort.Name = "tbxPort";
            tbxPort.Size = new Size(90, 23);
            tbxPort.TabIndex = 1;
            tbxPort.Text = "COM3";
            //
            // lblBaud
            //
            lblBaud.AutoSize = true;
            lblBaud.Location = new Point(210, 26);
            lblBaud.Name = "lblBaud";
            lblBaud.Size = new Size(65, 15);
            lblBaud.TabIndex = 2;
            lblBaud.Text = "Baud rate";
            //
            // cmbBaud
            //
            cmbBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBaud.Items.AddRange(new object[] { "1000000", "2000000", "115200", "57600" });
            cmbBaud.Location = new Point(285, 22);
            cmbBaud.Name = "cmbBaud";
            cmbBaud.SelectedIndex = 0;
            cmbBaud.Size = new Size(110, 23);
            cmbBaud.TabIndex = 3;
            //
            // bttnConnect
            //
            bttnConnect.Location = new Point(420, 22);
            bttnConnect.Name = "bttnConnect";
            bttnConnect.Size = new Size(100, 23);
            bttnConnect.TabIndex = 4;
            bttnConnect.Text = "Conectar";
            bttnConnect.UseVisualStyleBackColor = true;
            bttnConnect.Click += bttnConnect_Click;
            //
            // bttnTorque
            //
            // Separate from Connect on purpose: connecting must not energise the
            // motors, and de-energising them drops an arm that has no brakes.
            bttnTorque.Location = new Point(530, 22);
            bttnTorque.Name = "bttnTorque";
            bttnTorque.Size = new Size(120, 23);
            bttnTorque.TabIndex = 5;
            bttnTorque.Text = "Habilitar par";
            bttnTorque.UseVisualStyleBackColor = true;
            bttnTorque.Click += bttnTorque_Click;
            //
            // encodersControl
            //
            encodersControl.Location = new Point(8, 76);
            encodersControl.Name = "encodersControl";
            encodersControl.Size = new Size(159, 501);
            encodersControl.TabIndex = 1;
            //
            // homeControl
            //
            homeControl.Location = new Point(188, 76);
            homeControl.Name = "homeControl";
            homeControl.Size = new Size(159, 501);
            homeControl.TabIndex = 2;
            //
            // destinationControl
            //
            destinationControl.Location = new Point(368, 76);
            destinationControl.Name = "destinationControl";
            destinationControl.Size = new Size(159, 501);
            destinationControl.TabIndex = 3;
            //
            // bttnStop
            //
            bttnStop.BackColor = Color.FromArgb(255, 220, 220);
            bttnStop.Location = new Point(8, 585);
            bttnStop.Name = "bttnStop";
            bttnStop.Size = new Size(519, 28);
            bttnStop.TabIndex = 4;
            bttnStop.Text = "PARO DE EMERGENCIA  (Ctrl+Espacio)";
            bttnStop.UseVisualStyleBackColor = false;
            bttnStop.Click += bttnStop_Click;
            //
            // bttnRest
            //
            // Moving to a rest pose is the safe way to end a session: park the
            // arm low and supported, and only then cut torque. Cutting torque
            // anywhere else drops it.
            bttnRest.Location = new Point(8, 617);
            bttnRest.Name = "bttnRest";
            bttnRest.Size = new Size(519, 28);
            bttnRest.TabIndex = 5;
            bttnRest.Text = "Posición de descanso";
            bttnRest.UseVisualStyleBackColor = true;
            bttnRest.Click += bttnRest_Click;
            //
            // configControl
            //
            configControl.Location = new Point(8, 653);
            configControl.Name = "configControl";
            configControl.Size = new Size(519, 264);
            configControl.TabIndex = 6;
            //
            // plotsControl
            //
            plotsControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            plotsControl.Location = new Point(548, 76);
            plotsControl.Name = "plotsControl";
            plotsControl.Size = new Size(785, 721);
            plotsControl.TabIndex = 7;
            //
            // statusStrip1
            //
            statusStrip1.Dock = DockStyle.Bottom;
            statusStrip1.Items.AddRange(new ToolStripItem[] {
                statusLabelPrefix, statusLabelValue,
                portSeparator, portLabelPrefix, portLabelValue,
                torqueSeparator, torqueLabelPrefix, torqueLabelValue,
                modeSeparator, modeLabelPrefix, modeLabelValue,
                faultSeparator, faultLabelPrefix, faultLabelValue,
                stateSeparator, stateLabelPrefix, stateLabelValue });
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1347, 22);
            statusStrip1.TabIndex = 8;
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
            // portSeparator
            //
            portSeparator.Name = "portSeparator";
            portSeparator.Size = new Size(28, 17);
            portSeparator.Text = "   |   ";
            //
            // portLabelPrefix
            //
            portLabelPrefix.Name = "portLabelPrefix";
            portLabelPrefix.Size = new Size(50, 17);
            portLabelPrefix.Text = "Puerto: ";
            //
            // portLabelValue
            //
            portLabelValue.ForeColor = SystemColors.GrayText;
            portLabelValue.Name = "portLabelValue";
            portLabelValue.Size = new Size(20, 17);
            portLabelValue.Text = "—";
            //
            // torqueSeparator
            //
            torqueSeparator.Name = "torqueSeparator";
            torqueSeparator.Size = new Size(28, 17);
            torqueSeparator.Text = "   |   ";
            //
            // torqueLabelPrefix
            //
            torqueLabelPrefix.Name = "torqueLabelPrefix";
            torqueLabelPrefix.Size = new Size(35, 17);
            torqueLabelPrefix.Text = "Par: ";
            //
            // torqueLabelValue
            //
            torqueLabelValue.ForeColor = Color.Gray;
            torqueLabelValue.Name = "torqueLabelValue";
            torqueLabelValue.Size = new Size(82, 17);
            torqueLabelValue.Text = "Deshabilitado";
            //
            // modeSeparator
            //
            modeSeparator.Name = "modeSeparator";
            modeSeparator.Size = new Size(28, 17);
            modeSeparator.Text = "   |   ";
            //
            // modeLabelPrefix
            //
            modeLabelPrefix.Name = "modeLabelPrefix";
            modeLabelPrefix.Size = new Size(45, 17);
            modeLabelPrefix.Text = "Modo: ";
            //
            // modeLabelValue
            //
            modeLabelValue.ForeColor = SystemColors.GrayText;
            modeLabelValue.Name = "modeLabelValue";
            modeLabelValue.Size = new Size(20, 17);
            modeLabelValue.Text = "—";
            //
            // faultSeparator
            //
            faultSeparator.Name = "faultSeparator";
            faultSeparator.Size = new Size(28, 17);
            faultSeparator.Text = "   |   ";
            //
            // faultLabelPrefix
            //
            // Hardware Error Status. A motor that latches overload or
            // overheating disables its own torque, and the arm drops — worth a
            // permanent place on the bar rather than a surprise.
            faultLabelPrefix.Name = "faultLabelPrefix";
            faultLabelPrefix.Size = new Size(55, 17);
            faultLabelPrefix.Text = "Errores: ";
            //
            // faultLabelValue
            //
            faultLabelValue.ForeColor = SystemColors.GrayText;
            faultLabelValue.Name = "faultLabelValue";
            faultLabelValue.Size = new Size(50, 17);
            faultLabelValue.Text = "Ninguno";
            //
            // stateSeparator
            //
            stateSeparator.Name = "stateSeparator";
            stateSeparator.Size = new Size(28, 17);
            stateSeparator.Text = "   |   ";
            //
            // stateLabelPrefix
            //
            stateLabelPrefix.Name = "stateLabelPrefix";
            stateLabelPrefix.Size = new Size(96, 17);
            stateLabelPrefix.Text = "Estado de robot: ";
            //
            // stateLabelValue
            //
            stateLabelValue.Name = "stateLabelValue";
            stateLabelValue.Size = new Size(55, 17);
            stateLabelValue.Text = "Detenido";
            //
            // ViperControls
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(plotsControl);
            Controls.Add(configControl);
            Controls.Add(bttnRest);
            Controls.Add(bttnStop);
            Controls.Add(destinationControl);
            Controls.Add(homeControl);
            Controls.Add(encodersControl);
            Controls.Add(pnlConnection);
            Controls.Add(statusStrip1);
            Name = "ViperControls";
            Size = new Size(1347, 939);
            pnlConnection.ResumeLayout(false);
            pnlConnection.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private Panel                pnlConnection;
        private Label                lblPort;
        private TextBox              tbxPort;
        private Label                lblBaud;
        private ComboBox             cmbBaud;
        private Button               bttnConnect;
        private Button               bttnTorque;
        private ViperEncodersControl    encodersControl;
        private ViperHomeControl        homeControl;
        private ViperDestinationControl destinationControl;
        private ViperConfigControl   configControl;
        private ViperPlotsControl    plotsControl;
        private Button               bttnStop;
        private Button               bttnRest;
        private StatusStrip          statusStrip1;
        private ToolStripStatusLabel statusLabelPrefix;
        private ToolStripStatusLabel statusLabelValue;
        private ToolStripStatusLabel portSeparator;
        private ToolStripStatusLabel portLabelPrefix;
        private ToolStripStatusLabel portLabelValue;
        private ToolStripStatusLabel torqueSeparator;
        private ToolStripStatusLabel torqueLabelPrefix;
        private ToolStripStatusLabel torqueLabelValue;
        private ToolStripStatusLabel modeSeparator;
        private ToolStripStatusLabel modeLabelPrefix;
        private ToolStripStatusLabel modeLabelValue;
        private ToolStripStatusLabel faultSeparator;
        private ToolStripStatusLabel faultLabelPrefix;
        private ToolStripStatusLabel faultLabelValue;
        private ToolStripStatusLabel stateSeparator;
        private ToolStripStatusLabel stateLabelPrefix;
        private ToolStripStatusLabel stateLabelValue;
    }
}
