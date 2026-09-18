
namespace ForceEstimation
{
    partial class ViperConfigControl
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
            tabConfig          = new TabControl();
            tabPageControl     = new TabPage();
            lblController      = new Label();
            cmbController      = new ComboBox();
            lblMode            = new Label();
            lblModeValue       = new Label();
            tabPageTrajectory  = new TabPage();
            lblTrajectory      = new Label();
            cmbTrajectory      = new ComboBox();
            lblTf              = new Label();
            tbxTf              = new TextBox();
            tabPageInterpreter = new TabPage();
            txtGCode           = new TextBox();
            btnExecuteGCode    = new Button();
            tabConfig.SuspendLayout();
            tabPageControl.SuspendLayout();
            tabPageTrajectory.SuspendLayout();
            tabPageInterpreter.SuspendLayout();
            SuspendLayout();
            //
            // tabConfig
            //
            tabConfig.Controls.Add(tabPageControl);
            tabConfig.Controls.Add(tabPageTrajectory);
            tabConfig.Controls.Add(tabPageInterpreter);
            tabConfig.Dock = DockStyle.Fill;
            tabConfig.Location = new Point(0, 0);
            tabConfig.Name = "tabConfig";
            tabConfig.SelectedIndex = 0;
            tabConfig.Size = new Size(519, 264);
            tabConfig.TabIndex = 0;
            //
            // tabPageControl
            //
            tabPageControl.Controls.Add(lblController);
            tabPageControl.Controls.Add(cmbController);
            tabPageControl.Controls.Add(lblMode);
            tabPageControl.Controls.Add(lblModeValue);
            tabPageControl.Location = new Point(4, 24);
            tabPageControl.Name = "tabPageControl";
            tabPageControl.Padding = new Padding(3);
            tabPageControl.Size = new Size(511, 236);
            tabPageControl.TabIndex = 0;
            tabPageControl.Text = "Control";
            tabPageControl.UseVisualStyleBackColor = true;
            //
            // lblController
            //
            lblController.AutoSize = true;
            lblController.Location = new Point(14, 20);
            lblController.Name = "lblController";
            lblController.Size = new Size(75, 15);
            lblController.TabIndex = 0;
            lblController.Text = "Controlador";
            //
            // cmbController
            //
            cmbController.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbController.Items.AddRange(new object[] { "Básico (posición)" });
            cmbController.Location = new Point(110, 16);
            cmbController.Name = "cmbController";
            cmbController.Size = new Size(180, 23);
            cmbController.TabIndex = 1;
            //
            // lblMode
            //
            // The operating mode is not a separate setting: it follows the
            // controller, because IController declares the mode its output is
            // expressed in. Shown here as a read-only consequence.
            lblMode.AutoSize = true;
            lblMode.Location = new Point(14, 54);
            lblMode.Name = "lblMode";
            lblMode.Size = new Size(120, 15);
            lblMode.TabIndex = 2;
            lblMode.Text = "Modo de operación";
            //
            // lblModeValue
            //
            lblModeValue.AutoSize = true;
            lblModeValue.ForeColor = SystemColors.GrayText;
            lblModeValue.Location = new Point(140, 54);
            lblModeValue.Name = "lblModeValue";
            lblModeValue.Size = new Size(60, 15);
            lblModeValue.TabIndex = 3;
            lblModeValue.Text = "Position";
            //
            // tabPageTrajectory
            //
            tabPageTrajectory.Controls.Add(lblTrajectory);
            tabPageTrajectory.Controls.Add(cmbTrajectory);
            tabPageTrajectory.Controls.Add(lblTf);
            tabPageTrajectory.Controls.Add(tbxTf);
            tabPageTrajectory.Location = new Point(4, 24);
            tabPageTrajectory.Name = "tabPageTrajectory";
            tabPageTrajectory.Padding = new Padding(3);
            tabPageTrajectory.Size = new Size(511, 236);
            tabPageTrajectory.TabIndex = 1;
            tabPageTrajectory.Text = "Trayectoria";
            tabPageTrajectory.UseVisualStyleBackColor = true;
            //
            // lblTrajectory
            //
            lblTrajectory.AutoSize = true;
            lblTrajectory.Location = new Point(14, 20);
            lblTrajectory.Name = "lblTrajectory";
            lblTrajectory.Size = new Size(68, 15);
            lblTrajectory.TabIndex = 0;
            lblTrajectory.Text = "Trayectoria";
            //
            // cmbTrajectory
            //
            cmbTrajectory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTrajectory.Items.AddRange(new object[] { "Polinomio quíntico" });
            cmbTrajectory.Location = new Point(110, 16);
            cmbTrajectory.Name = "cmbTrajectory";
            cmbTrajectory.Size = new Size(200, 23);
            cmbTrajectory.TabIndex = 1;
            //
            // lblTf
            //
            lblTf.AutoSize = true;
            lblTf.Location = new Point(14, 54);
            lblTf.Name = "lblTf";
            lblTf.Size = new Size(70, 15);
            lblTf.TabIndex = 2;
            lblTf.Text = "Duración [s]";
            //
            // tbxTf
            //
            tbxTf.Location = new Point(110, 50);
            tbxTf.Name = "tbxTf";
            tbxTf.Size = new Size(60, 23);
            tbxTf.TabIndex = 3;
            tbxTf.Text = "3.0";
            //
            // tabPageInterpreter
            //
            tabPageInterpreter.Controls.Add(txtGCode);
            tabPageInterpreter.Controls.Add(btnExecuteGCode);
            tabPageInterpreter.Location = new Point(4, 24);
            tabPageInterpreter.Name = "tabPageInterpreter";
            tabPageInterpreter.Padding = new Padding(3);
            tabPageInterpreter.Size = new Size(511, 236);
            tabPageInterpreter.TabIndex = 2;
            tabPageInterpreter.Text = "Intérprete";
            tabPageInterpreter.UseVisualStyleBackColor = true;
            //
            // txtGCode
            //
            txtGCode.Location = new Point(6, 6);
            txtGCode.Multiline = true;
            txtGCode.Lines = new string[]
            {
                "G1 X0 Y-20 Z0.5 I0 J0 K-1",
                "G4 P1000",
                "G1 X0 Y-40 Z0.5 I0 J0 K-1",
                "G4 P1000",
                "G1 X0 Y-20 Z0.5 I0 J0 K-1",
                "G4 P1000",
                "G0 A0 B0 C0 D0 E0 F0",
                "G4 P1000"
            };
            txtGCode.Name = "txtGCode";
            txtGCode.ScrollBars = ScrollBars.Vertical;
            txtGCode.Size = new Size(499, 180);
            txtGCode.TabIndex = 0;
            //
            // btnExecuteGCode
            //
            btnExecuteGCode.Location = new Point(6, 194);
            btnExecuteGCode.Name = "btnExecuteGCode";
            btnExecuteGCode.Size = new Size(80, 26);
            btnExecuteGCode.TabIndex = 1;
            btnExecuteGCode.Text = "Ejecutar";
            btnExecuteGCode.UseVisualStyleBackColor = true;
            btnExecuteGCode.Click += btnExecuteGCode_Click;
            //
            // ViperConfigControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabConfig);
            Name = "ViperConfigControl";
            Size = new Size(519, 264);
            tabConfig.ResumeLayout(false);
            tabPageControl.ResumeLayout(false);
            tabPageControl.PerformLayout();
            tabPageTrajectory.ResumeLayout(false);
            tabPageTrajectory.PerformLayout();
            tabPageInterpreter.ResumeLayout(false);
            tabPageInterpreter.PerformLayout();
            ResumeLayout(false);
        }

        private TabControl tabConfig;
        private TabPage    tabPageControl;
        private Label      lblController;
        private ComboBox   cmbController;
        private Label      lblMode;
        private Label      lblModeValue;
        private TabPage    tabPageTrajectory;
        private Label      lblTrajectory;
        private ComboBox   cmbTrajectory;
        private Label      lblTf;
        private TextBox    tbxTf;
        private TabPage    tabPageInterpreter;
        private TextBox    txtGCode;
        private Button     btnExecuteGCode;
    }
}
