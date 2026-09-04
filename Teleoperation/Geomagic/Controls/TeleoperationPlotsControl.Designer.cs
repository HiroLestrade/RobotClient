using OxyPlot.WindowsForms;

namespace Teleoperation
{
    partial class TeleoperationPlotsControl
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
            pnlButtons   = new Panel();
            btnClear     = new Button();
            btnExportLog = new Button();
            tabPlots     = new TabControl();
            tabPagePos   = new TabPage();
            tlpArticular = new TableLayoutPanel();
            chartQ1      = new PlotView();
            chartQ2      = new PlotView();
            chartQ3      = new PlotView();
            pnlButtons.SuspendLayout();
            tabPlots.SuspendLayout();
            tabPagePos.SuspendLayout();
            tlpArticular.SuspendLayout();
            SuspendLayout();
            //
            // pnlButtons
            //
            pnlButtons.Controls.Add(btnClear);
            pnlButtons.Controls.Add(btnExportLog);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Size = new Size(883, 34);
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
            // btnExportLog
            //
            btnExportLog.Location = new Point(106, 5);
            btnExportLog.Name = "btnExportLog";
            btnExportLog.Size = new Size(150, 25);
            btnExportLog.TabIndex = 1;
            btnExportLog.Text = "Exportar registro";
            btnExportLog.UseVisualStyleBackColor = true;
            btnExportLog.Click += btnExportLog_Click;
            //
            // tabPlots
            //
            tabPlots.Controls.Add(tabPagePos);
            tabPlots.Dock = DockStyle.Fill;
            tabPlots.Location = new Point(0, 0);
            tabPlots.Name = "tabPlots";
            tabPlots.SelectedIndex = 0;
            tabPlots.Size = new Size(883, 618);
            tabPlots.TabIndex = 0;
            //
            // tabPagePos — Posición Articular
            //
            tabPagePos.Controls.Add(tlpArticular);
            tabPagePos.Location = new Point(4, 24);
            tabPagePos.Name = "tabPagePos";
            tabPagePos.Padding = new Padding(3);
            tabPagePos.Size = new Size(875, 590);
            tabPagePos.TabIndex = 0;
            tabPagePos.Text = "Posición Articular";
            tabPagePos.UseVisualStyleBackColor = true;
            //
            // tlpArticular
            //
            tlpArticular.ColumnCount = 1;
            tlpArticular.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpArticular.Controls.Add(chartQ1, 0, 0);
            tlpArticular.Controls.Add(chartQ2, 0, 1);
            tlpArticular.Controls.Add(chartQ3, 0, 2);
            tlpArticular.Dock = DockStyle.Fill;
            tlpArticular.Location = new Point(3, 3);
            tlpArticular.Name = "tlpArticular";
            tlpArticular.RowCount = 3;
            tlpArticular.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpArticular.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpArticular.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tlpArticular.Size = new Size(869, 584);
            tlpArticular.TabIndex = 0;
            //
            // chartQ1
            //
            chartQ1.Dock = DockStyle.Fill;
            chartQ1.Name = "chartQ1";
            chartQ1.PanCursor = Cursors.Hand;
            chartQ1.TabIndex = 0;
            chartQ1.ZoomHorizontalCursor = Cursors.SizeWE;
            chartQ1.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartQ1.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartQ2
            //
            chartQ2.Dock = DockStyle.Fill;
            chartQ2.Name = "chartQ2";
            chartQ2.PanCursor = Cursors.Hand;
            chartQ2.TabIndex = 1;
            chartQ2.ZoomHorizontalCursor = Cursors.SizeWE;
            chartQ2.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartQ2.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartQ3
            //
            chartQ3.Dock = DockStyle.Fill;
            chartQ3.Name = "chartQ3";
            chartQ3.PanCursor = Cursors.Hand;
            chartQ3.TabIndex = 2;
            chartQ3.ZoomHorizontalCursor = Cursors.SizeWE;
            chartQ3.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartQ3.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // TeleoperationPlotsControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabPlots);
            Controls.Add(pnlButtons);
            Name = "TeleoperationPlotsControl";
            Size = new Size(883, 652);
            pnlButtons.ResumeLayout(false);
            tabPlots.ResumeLayout(false);
            tabPagePos.ResumeLayout(false);
            tlpArticular.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel            pnlButtons;
        private Button           btnClear;
        private Button           btnExportLog;
        private TabControl       tabPlots;
        private TabPage          tabPagePos;
        private TableLayoutPanel tlpArticular;
        private PlotView         chartQ1;
        private PlotView         chartQ2;
        private PlotView         chartQ3;
    }
}
