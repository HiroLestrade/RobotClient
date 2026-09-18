using OxyPlot.WindowsForms;

namespace ForceEstimation
{
    partial class ViperPlotsControl
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
            pnlButtons        = new Panel();
            btnClear          = new Button();
            tabPlots          = new TabControl();
            tabPagePosition   = new TabPage();
            tabPageError      = new TabPage();
            tabPageVelocity   = new TabPage();
            tabPageAccel      = new TabPage();
            tabPageCartesian  = new TabPage();
            chartCartesian    = new PlotView();

            chartPositionJoint = new PlotView[JointCount];
            chartErrorJoint    = new PlotView[JointCount];
            chartVelocityJoint = new PlotView[JointCount];
            chartAccelJoint    = new PlotView[JointCount];

            tabPositionJoints = BuildJointTabs("Position", chartPositionJoint);
            tabErrorJoints    = BuildJointTabs("Error",    chartErrorJoint);
            tabVelocityJoints = BuildJointTabs("Velocity", chartVelocityJoint);
            tabAccelJoints    = BuildJointTabs("Accel",    chartAccelJoint);

            pnlButtons.SuspendLayout();
            tabPlots.SuspendLayout();
            tabPagePosition.SuspendLayout();
            tabPageError.SuspendLayout();
            tabPageVelocity.SuspendLayout();
            tabPageAccel.SuspendLayout();
            tabPageCartesian.SuspendLayout();
            SuspendLayout();
            //
            // pnlButtons
            //
            pnlButtons.Controls.Add(btnClear);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(785, 34);
            pnlButtons.TabIndex = 1;
            //
            // btnClear
            //
            btnClear.Location = new Point(0, 5);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(100, 25);
            btnClear.TabIndex = 0;
            btnClear.Text = "Limpiar";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            //
            // tabPlots
            //
            tabPlots.Controls.Add(tabPagePosition);
            tabPlots.Controls.Add(tabPageError);
            tabPlots.Controls.Add(tabPageVelocity);
            tabPlots.Controls.Add(tabPageAccel);
            tabPlots.Controls.Add(tabPageCartesian);
            tabPlots.Dock = DockStyle.Fill;
            tabPlots.Location = new Point(0, 0);
            tabPlots.Name = "tabPlots";
            tabPlots.SelectedIndex = 0;
            tabPlots.Size = new Size(785, 644);
            tabPlots.TabIndex = 0;
            //
            // tabPagePosition
            //
            tabPagePosition.Controls.Add(tabPositionJoints);
            tabPagePosition.Location = new Point(4, 24);
            tabPagePosition.Name = "tabPagePosition";
            tabPagePosition.Padding = new Padding(3);
            tabPagePosition.Size = new Size(777, 616);
            tabPagePosition.TabIndex = 0;
            tabPagePosition.Text = "Posición Articular";
            tabPagePosition.UseVisualStyleBackColor = true;
            //
            // tabPageError
            //
            // Second, right after position: it is the same information as the
            // gap between those two curves, but read off an axis instead of
            // eyeballed between two lines that nearly overlap.
            tabPageError.Controls.Add(tabErrorJoints);
            tabPageError.Location = new Point(4, 24);
            tabPageError.Name = "tabPageError";
            tabPageError.Padding = new Padding(3);
            tabPageError.Size = new Size(777, 616);
            tabPageError.TabIndex = 1;
            tabPageError.Text = "Error de Posición";
            tabPageError.UseVisualStyleBackColor = true;
            //
            // tabPageVelocity
            //
            tabPageVelocity.Controls.Add(tabVelocityJoints);
            tabPageVelocity.Location = new Point(4, 24);
            tabPageVelocity.Name = "tabPageVelocity";
            tabPageVelocity.Padding = new Padding(3);
            tabPageVelocity.Size = new Size(777, 616);
            tabPageVelocity.TabIndex = 2;
            tabPageVelocity.Text = "Velocidad Articular";
            tabPageVelocity.UseVisualStyleBackColor = true;
            //
            // tabPageAccel
            //
            tabPageAccel.Controls.Add(tabAccelJoints);
            tabPageAccel.Location = new Point(4, 24);
            tabPageAccel.Name = "tabPageAccel";
            tabPageAccel.Padding = new Padding(3);
            tabPageAccel.Size = new Size(777, 616);
            tabPageAccel.TabIndex = 3;
            tabPageAccel.Text = "Aceleración Articular";
            tabPageAccel.UseVisualStyleBackColor = true;
            //
            // tabPageCartesian
            //
            tabPageCartesian.Controls.Add(chartCartesian);
            tabPageCartesian.Location = new Point(4, 24);
            tabPageCartesian.Name = "tabPageCartesian";
            tabPageCartesian.Padding = new Padding(3);
            tabPageCartesian.Size = new Size(777, 616);
            tabPageCartesian.TabIndex = 4;
            tabPageCartesian.Text = "Posición Cartesiana";
            tabPageCartesian.UseVisualStyleBackColor = true;
            //
            // chartCartesian
            //
            // The one chart still shared, and the only one with nothing to
            // share: x, y and z belong on common axes, and none of them is fed
            // until there is forward kinematics.
            chartCartesian.Dock = DockStyle.Fill;
            chartCartesian.Location = new Point(3, 3);
            chartCartesian.Name = "chartCartesian";
            chartCartesian.PanCursor = Cursors.Hand;
            chartCartesian.Size = new Size(771, 610);
            chartCartesian.TabIndex = 0;
            chartCartesian.ZoomHorizontalCursor = Cursors.SizeWE;
            chartCartesian.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartCartesian.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // ViperPlotsControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabPlots);
            Controls.Add(pnlButtons);
            Name = "ViperPlotsControl";
            Size = new Size(785, 678);
            pnlButtons.ResumeLayout(false);
            tabPlots.ResumeLayout(false);
            tabPagePosition.ResumeLayout(false);
            tabPageVelocity.ResumeLayout(false);
            tabPageAccel.ResumeLayout(false);
            tabPageCartesian.ResumeLayout(false);
            ResumeLayout(false);
        }

        /// <summary>
        /// Builds one joint tab strip: six dock-filled charts, q1 to q6, and
        /// fills <paramref name="target"/> with them in that order.
        ///
        /// <para>All three joint quantities are laid out this way. Six traces on
        /// one pair of axes hid the thing you actually read — the gap between a
        /// joint's measured and desired curves — behind five other joints.</para>
        ///
        /// <para>Built in a loop rather than spelled out eighteen times: the
        /// pages are identical and dock-filled, so there are no coordinates to
        /// get wrong, and every hand-written copy is another chance to.</para>
        /// </summary>
        private static TabControl BuildJointTabs(string namePrefix, PlotView[] target)
        {
            var tabs = new TabControl
            {
                Dock          = DockStyle.Fill,
                Location      = new Point(3, 3),
                Name          = $"tab{namePrefix}Joints",
                SelectedIndex = 0,
                Size          = new Size(771, 610),
                TabIndex      = 0,
            };

            for (int i = 0; i < target.Length; i++)
            {
                var view = new PlotView
                {
                    Dock                 = DockStyle.Fill,
                    Location             = new Point(3, 3),
                    Name                 = $"chart{namePrefix}Q{i + 1}",
                    PanCursor            = Cursors.Hand,
                    TabIndex             = 0,
                    ZoomHorizontalCursor = Cursors.SizeWE,
                    ZoomRectangleCursor  = Cursors.SizeNWSE,
                    ZoomVerticalCursor   = Cursors.SizeNS,
                };

                var page = new TabPage
                {
                    Location                = new Point(4, 24),
                    Name                    = $"tabPage{namePrefix}Q{i + 1}",
                    Padding                 = new Padding(3),
                    TabIndex                = i,
                    Text                    = $"q{i + 1}",
                    UseVisualStyleBackColor = true,
                };

                page.Controls.Add(view);
                tabs.TabPages.Add(page);
                target[i] = view;
            }

            return tabs;
        }

        private Panel      pnlButtons;
        private Button     btnClear;
        private TabControl tabPlots;
        private TabPage    tabPagePosition;
        private TabPage    tabPageError;
        private TabPage    tabPageVelocity;
        private TabPage    tabPageAccel;
        private TabPage    tabPageCartesian;

        private TabControl tabPositionJoints;
        private TabControl tabErrorJoints;
        private TabControl tabVelocityJoints;
        private TabControl tabAccelJoints;

        /// <summary>One chart per joint, q1..q6, for each joint quantity.</summary>
        private PlotView[] chartPositionJoint;
        private PlotView[] chartErrorJoint;
        private PlotView[] chartVelocityJoint;
        private PlotView[] chartAccelJoint;

        private PlotView   chartCartesian;
    }
}
