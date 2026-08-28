
using OxyPlot.WindowsForms;

namespace ForceEstimation
{
    partial class GeomagicPlotsControl
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
            pnlButtons     = new TableLayoutPanel();
            btnClear       = new Button();
            btnExport      = new Button();
            tabPlots       = new TabControl();
            tabPage2       = new TabPage();
            tlpArticular   = new TableLayoutPanel();
            chartQ1        = new PlotView();
            chartQ2        = new PlotView();
            chartQ3        = new PlotView();
            tabPageVel     = new TabPage();
            tlpVelocidad   = new TableLayoutPanel();
            chartDQ1       = new PlotView();
            chartDQ2       = new PlotView();
            chartDQ3       = new PlotView();
            tabPageAcc     = new TabPage();
            tlpAceleracion = new TableLayoutPanel();
            chartDDQ1      = new PlotView();
            chartDDQ2      = new PlotView();
            chartDDQ3      = new PlotView();
            tabPage3       = new TabPage();
            tlpCartesiano  = new TableLayoutPanel();
            chartX         = new PlotView();
            chartY         = new PlotView();
            chartZ         = new PlotView();
            tabPageForce   = new TabPage();
            tlpFuerza      = new TableLayoutPanel();
            chartFx        = new PlotView();
            chartFy        = new PlotView();
            chartFz        = new PlotView();
            tabPageError   = new TabPage();
            tlpError       = new TableLayoutPanel();
            chartErrFx     = new PlotView();
            chartErrFy     = new PlotView();
            chartErrFz     = new PlotView();
            pnlButtons.SuspendLayout();
            tabPlots.SuspendLayout();
            tabPage2.SuspendLayout();
            tlpArticular.SuspendLayout();
            tabPageVel.SuspendLayout();
            tlpVelocidad.SuspendLayout();
            tabPageAcc.SuspendLayout();
            tlpAceleracion.SuspendLayout();
            tabPage3.SuspendLayout();
            tlpCartesiano.SuspendLayout();
            tabPageForce.SuspendLayout();
            tlpFuerza.SuspendLayout();
            tabPageError.SuspendLayout();
            tlpError.SuspendLayout();
            SuspendLayout();
            //
            // tabPlots
            //
            tabPlots.Controls.Add(tabPage2);
            tabPlots.Controls.Add(tabPageVel);
            tabPlots.Controls.Add(tabPageAcc);
            tabPlots.Controls.Add(tabPage3);
            tabPlots.Controls.Add(tabPageForce);
            tabPlots.Controls.Add(tabPageError);
            tabPlots.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            tabPlots.Location = new Point(0, 0);
            tabPlots.Name = "tabPlots";
            tabPlots.SelectedIndex = 0;
            tabPlots.Size = new Size(785, 644);
            tabPlots.TabIndex = 0;
            //
            // tabPage2 — Posición Articular
            //
            tabPage2.Controls.Add(tlpArticular);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(777, 650);
            tabPage2.TabIndex = 0;
            tabPage2.Text = "Posición Articular";
            tabPage2.UseVisualStyleBackColor = true;
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
            tlpArticular.Size = new Size(771, 644);
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
            // tabPageVel — Velocidad Articular
            //
            tabPageVel.Controls.Add(tlpVelocidad);
            tabPageVel.Location = new Point(4, 24);
            tabPageVel.Name = "tabPageVel";
            tabPageVel.Padding = new Padding(3);
            tabPageVel.Size = new Size(777, 650);
            tabPageVel.TabIndex = 2;
            tabPageVel.Text = "Velocidad Articular";
            tabPageVel.UseVisualStyleBackColor = true;
            //
            // tlpVelocidad
            //
            tlpVelocidad.ColumnCount = 1;
            tlpVelocidad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpVelocidad.Controls.Add(chartDQ1, 0, 0);
            tlpVelocidad.Controls.Add(chartDQ2, 0, 1);
            tlpVelocidad.Controls.Add(chartDQ3, 0, 2);
            tlpVelocidad.Dock = DockStyle.Fill;
            tlpVelocidad.Location = new Point(3, 3);
            tlpVelocidad.Name = "tlpVelocidad";
            tlpVelocidad.RowCount = 3;
            tlpVelocidad.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpVelocidad.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpVelocidad.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tlpVelocidad.Size = new Size(771, 644);
            tlpVelocidad.TabIndex = 0;
            //
            // chartDQ1
            //
            chartDQ1.Dock = DockStyle.Fill;
            chartDQ1.Name = "chartDQ1";
            chartDQ1.PanCursor = Cursors.Hand;
            chartDQ1.TabIndex = 0;
            chartDQ1.ZoomHorizontalCursor = Cursors.SizeWE;
            chartDQ1.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartDQ1.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartDQ2
            //
            chartDQ2.Dock = DockStyle.Fill;
            chartDQ2.Name = "chartDQ2";
            chartDQ2.PanCursor = Cursors.Hand;
            chartDQ2.TabIndex = 1;
            chartDQ2.ZoomHorizontalCursor = Cursors.SizeWE;
            chartDQ2.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartDQ2.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartDQ3
            //
            chartDQ3.Dock = DockStyle.Fill;
            chartDQ3.Name = "chartDQ3";
            chartDQ3.PanCursor = Cursors.Hand;
            chartDQ3.TabIndex = 2;
            chartDQ3.ZoomHorizontalCursor = Cursors.SizeWE;
            chartDQ3.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartDQ3.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // tabPageAcc — Aceleración Articular
            //
            tabPageAcc.Controls.Add(tlpAceleracion);
            tabPageAcc.Location = new Point(4, 24);
            tabPageAcc.Name = "tabPageAcc";
            tabPageAcc.Padding = new Padding(3);
            tabPageAcc.Size = new Size(777, 650);
            tabPageAcc.TabIndex = 3;
            tabPageAcc.Text = "Aceleración Articular";
            tabPageAcc.UseVisualStyleBackColor = true;
            //
            // tlpAceleracion
            //
            tlpAceleracion.ColumnCount = 1;
            tlpAceleracion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpAceleracion.Controls.Add(chartDDQ1, 0, 0);
            tlpAceleracion.Controls.Add(chartDDQ2, 0, 1);
            tlpAceleracion.Controls.Add(chartDDQ3, 0, 2);
            tlpAceleracion.Dock = DockStyle.Fill;
            tlpAceleracion.Location = new Point(3, 3);
            tlpAceleracion.Name = "tlpAceleracion";
            tlpAceleracion.RowCount = 3;
            tlpAceleracion.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpAceleracion.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpAceleracion.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tlpAceleracion.Size = new Size(771, 644);
            tlpAceleracion.TabIndex = 0;
            //
            // chartDDQ1
            //
            chartDDQ1.Dock = DockStyle.Fill;
            chartDDQ1.Name = "chartDDQ1";
            chartDDQ1.PanCursor = Cursors.Hand;
            chartDDQ1.TabIndex = 0;
            chartDDQ1.ZoomHorizontalCursor = Cursors.SizeWE;
            chartDDQ1.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartDDQ1.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartDDQ2
            //
            chartDDQ2.Dock = DockStyle.Fill;
            chartDDQ2.Name = "chartDDQ2";
            chartDDQ2.PanCursor = Cursors.Hand;
            chartDDQ2.TabIndex = 1;
            chartDDQ2.ZoomHorizontalCursor = Cursors.SizeWE;
            chartDDQ2.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartDDQ2.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartDDQ3
            //
            chartDDQ3.Dock = DockStyle.Fill;
            chartDDQ3.Name = "chartDDQ3";
            chartDDQ3.PanCursor = Cursors.Hand;
            chartDDQ3.TabIndex = 2;
            chartDDQ3.ZoomHorizontalCursor = Cursors.SizeWE;
            chartDDQ3.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartDDQ3.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // tabPage3 — Posición Cartesiana
            //
            tabPage3.Controls.Add(tlpCartesiano);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(777, 650);
            tabPage3.TabIndex = 1;
            tabPage3.Text = "Posición Cartesiana";
            tabPage3.UseVisualStyleBackColor = true;
            //
            // tlpCartesiano
            //
            tlpCartesiano.ColumnCount = 1;
            tlpCartesiano.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCartesiano.Controls.Add(chartX, 0, 0);
            tlpCartesiano.Controls.Add(chartY, 0, 1);
            tlpCartesiano.Controls.Add(chartZ, 0, 2);
            tlpCartesiano.Dock = DockStyle.Fill;
            tlpCartesiano.Location = new Point(3, 3);
            tlpCartesiano.Name = "tlpCartesiano";
            tlpCartesiano.RowCount = 3;
            tlpCartesiano.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpCartesiano.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpCartesiano.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tlpCartesiano.Size = new Size(771, 644);
            tlpCartesiano.TabIndex = 0;
            //
            // chartX
            //
            chartX.Dock = DockStyle.Fill;
            chartX.Name = "chartX";
            chartX.PanCursor = Cursors.Hand;
            chartX.TabIndex = 0;
            chartX.ZoomHorizontalCursor = Cursors.SizeWE;
            chartX.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartX.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartY
            //
            chartY.Dock = DockStyle.Fill;
            chartY.Name = "chartY";
            chartY.PanCursor = Cursors.Hand;
            chartY.TabIndex = 1;
            chartY.ZoomHorizontalCursor = Cursors.SizeWE;
            chartY.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartY.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartZ
            //
            chartZ.Dock = DockStyle.Fill;
            chartZ.Name = "chartZ";
            chartZ.PanCursor = Cursors.Hand;
            chartZ.TabIndex = 2;
            chartZ.ZoomHorizontalCursor = Cursors.SizeWE;
            chartZ.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartZ.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // tabPageForce — Fuerza
            //
            tabPageForce.Controls.Add(tlpFuerza);
            tabPageForce.Location = new Point(4, 24);
            tabPageForce.Name = "tabPageForce";
            tabPageForce.Padding = new Padding(3);
            tabPageForce.Size = new Size(777, 650);
            tabPageForce.TabIndex = 4;
            tabPageForce.Text = "Fuerza";
            tabPageForce.UseVisualStyleBackColor = true;
            //
            // tlpFuerza
            //
            tlpFuerza.ColumnCount = 1;
            tlpFuerza.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpFuerza.Controls.Add(chartFx, 0, 0);
            tlpFuerza.Controls.Add(chartFy, 0, 1);
            tlpFuerza.Controls.Add(chartFz, 0, 2);
            tlpFuerza.Dock = DockStyle.Fill;
            tlpFuerza.Location = new Point(3, 3);
            tlpFuerza.Name = "tlpFuerza";
            tlpFuerza.RowCount = 3;
            tlpFuerza.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpFuerza.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpFuerza.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tlpFuerza.Size = new Size(771, 644);
            tlpFuerza.TabIndex = 0;
            //
            // chartFx
            //
            chartFx.Dock = DockStyle.Fill;
            chartFx.Name = "chartFx";
            chartFx.PanCursor = Cursors.Hand;
            chartFx.TabIndex = 0;
            chartFx.ZoomHorizontalCursor = Cursors.SizeWE;
            chartFx.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartFx.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartFy
            //
            chartFy.Dock = DockStyle.Fill;
            chartFy.Name = "chartFy";
            chartFy.PanCursor = Cursors.Hand;
            chartFy.TabIndex = 1;
            chartFy.ZoomHorizontalCursor = Cursors.SizeWE;
            chartFy.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartFy.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartFz
            //
            chartFz.Dock = DockStyle.Fill;
            chartFz.Name = "chartFz";
            chartFz.PanCursor = Cursors.Hand;
            chartFz.TabIndex = 2;
            chartFz.ZoomHorizontalCursor = Cursors.SizeWE;
            chartFz.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartFz.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // tabPageError — Error de estimación
            //
            tabPageError.Controls.Add(tlpError);
            tabPageError.Location = new Point(4, 24);
            tabPageError.Name = "tabPageError";
            tabPageError.Padding = new Padding(3);
            tabPageError.Size = new Size(777, 650);
            tabPageError.TabIndex = 5;
            tabPageError.Text = "Error de estimación";
            tabPageError.UseVisualStyleBackColor = true;
            //
            // tlpError
            //
            tlpError.ColumnCount = 1;
            tlpError.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpError.Controls.Add(chartErrFx, 0, 0);
            tlpError.Controls.Add(chartErrFy, 0, 1);
            tlpError.Controls.Add(chartErrFz, 0, 2);
            tlpError.Dock = DockStyle.Fill;
            tlpError.Location = new Point(3, 3);
            tlpError.Name = "tlpError";
            tlpError.RowCount = 3;
            tlpError.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpError.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpError.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tlpError.Size = new Size(771, 644);
            tlpError.TabIndex = 0;
            //
            // chartErrFx
            //
            chartErrFx.Dock = DockStyle.Fill;
            chartErrFx.Name = "chartErrFx";
            chartErrFx.PanCursor = Cursors.Hand;
            chartErrFx.TabIndex = 0;
            chartErrFx.ZoomHorizontalCursor = Cursors.SizeWE;
            chartErrFx.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartErrFx.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartErrFy
            //
            chartErrFy.Dock = DockStyle.Fill;
            chartErrFy.Name = "chartErrFy";
            chartErrFy.PanCursor = Cursors.Hand;
            chartErrFy.TabIndex = 1;
            chartErrFy.ZoomHorizontalCursor = Cursors.SizeWE;
            chartErrFy.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartErrFy.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // chartErrFz
            //
            chartErrFz.Dock = DockStyle.Fill;
            chartErrFz.Name = "chartErrFz";
            chartErrFz.PanCursor = Cursors.Hand;
            chartErrFz.TabIndex = 2;
            chartErrFz.ZoomHorizontalCursor = Cursors.SizeWE;
            chartErrFz.ZoomRectangleCursor = Cursors.SizeNWSE;
            chartErrFz.ZoomVerticalCursor = Cursors.SizeNS;
            //
            // pnlButtons  (TableLayoutPanel — 2 columnas iguales)
            //
            pnlButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlButtons.ColumnCount = 2;
            pnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlButtons.Controls.Add(btnClear,  0, 0);
            pnlButtons.Controls.Add(btnExport, 1, 0);
            pnlButtons.Location = new Point(0, 648);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.RowCount = 1;
            pnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlButtons.Size = new Size(785, 30);
            pnlButtons.TabIndex = 1;
            //
            // btnClear
            //
            btnClear.Dock = DockStyle.Fill;
            btnClear.Name = "btnClear";
            btnClear.TabIndex = 0;
            btnClear.Text = "Limpiar gráficas";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            //
            // btnExport
            //
            btnExport.Dock = DockStyle.Fill;
            btnExport.Name = "btnExport";
            btnExport.TabIndex = 1;
            btnExport.Text = "Descargar CSV";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            //
            // GeomagicPlotsControl
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabPlots);
            Controls.Add(pnlButtons);
            Name = "GeomagicPlotsControl";
            Size = new Size(785, 678);
            pnlButtons.ResumeLayout(false);
            tabPlots.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tlpArticular.ResumeLayout(false);
            tabPageVel.ResumeLayout(false);
            tlpVelocidad.ResumeLayout(false);
            tabPageAcc.ResumeLayout(false);
            tlpAceleracion.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            tlpCartesiano.ResumeLayout(false);
            tabPageForce.ResumeLayout(false);
            tlpFuerza.ResumeLayout(false);
            tabPageError.ResumeLayout(false);
            tlpError.ResumeLayout(false);
            ResumeLayout(false);
        }

        private TableLayoutPanel pnlButtons;
        private Button  btnClear;
        private Button  btnExport;
        private TabControl tabPlots;
        private TabPage tabPage2;
        private TabPage tabPageVel;
        private TabPage tabPageAcc;
        private TabPage tabPage3;
        private TabPage tabPageForce;
        private TabPage tabPageError;
        private TableLayoutPanel tlpArticular;
        private TableLayoutPanel tlpVelocidad;
        private TableLayoutPanel tlpAceleracion;
        private TableLayoutPanel tlpCartesiano;
        private TableLayoutPanel tlpFuerza;
        private TableLayoutPanel tlpError;
        private PlotView chartQ1;
        private PlotView chartQ2;
        private PlotView chartQ3;
        private PlotView chartDQ1;
        private PlotView chartDQ2;
        private PlotView chartDQ3;
        private PlotView chartDDQ1;
        private PlotView chartDDQ2;
        private PlotView chartDDQ3;
        private PlotView chartX;
        private PlotView chartY;
        private PlotView chartZ;
        private PlotView chartFx;
        private PlotView chartFy;
        private PlotView chartFz;
        private PlotView chartErrFx;
        private PlotView chartErrFy;
        private PlotView chartErrFz;
    }
}
