namespace ForceEstimation
{
    // Sliding-window median filter — removes impulse spikes without phase distortion.
    internal sealed class MedianFilter
    {
        private readonly double[] _buf;
        private readonly double[] _tmp;
        private int _head, _count;

        public MedianFilter(int window)
        {
            _buf = new double[window];
            _tmp = new double[window];
        }

        public double Filter(double value)
        {
            _buf[_head % _buf.Length] = value;
            _head++;
            if (_count < _buf.Length) _count++;

            for (int i = 0; i < _count; i++)
                _tmp[i] = _buf[(_head - 1 - i + _buf.Length * 2) % _buf.Length];
            Array.Sort(_tmp, 0, _count);
            return _tmp[_count / 2];
        }

        public void Reset() { _head = 0; _count = 0; }
    }

    public partial class GeomagicPlotsControl : UserControl
    {
        private bool     _isRecording;
        private DateTime _recordingStart;
        private double   _desiredTimeOffset;
        private double[] _prevQ   = new double[3];
        private double[] _prevDQ  = new double[3];
        private double   _prevT   = double.NaN;
        private bool     _hasPrevDQ;
        private const double MinDt = 0.005;

        private readonly MedianFilter[] _dqFilter  =
            [new MedianFilter(5), new MedianFilter(5), new MedianFilter(5)];
        private readonly MedianFilter[] _ddqFilter =
            [new MedianFilter(15), new MedianFilter(15), new MedianFilter(15)];

        // Provides controller and trajectory names for the CSV filename.
        // Set by GeomagicControl after construction.
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Func<(string controller, string trajectory)>? GetExportMetadata { get; set; }

        public GeomagicPlotsControl()
        {
            InitializeComponent();

            ConfigureChart(chartQ1,   "q₁ [°]");
            ConfigureChart(chartQ2,   "q₂ [°]");
            ConfigureChart(chartQ3,   "q₃ [°]");
            ConfigureChart(chartDQ1,  "q̇₁ [°/s]");
            ConfigureChart(chartDQ2,  "q̇₂ [°/s]");
            ConfigureChart(chartDQ3,  "q̇₃ [°/s]");
            ConfigureChart(chartDDQ1, "q̈₁ [°/s²]");
            ConfigureChart(chartDDQ2, "q̈₂ [°/s²]");
            ConfigureChart(chartDDQ3, "q̈₃ [°/s²]");
            ConfigureChart(chartX,    "x [cm]");
            ConfigureChart(chartY,    "y [cm]");
            ConfigureChart(chartZ,    "z [cm]");
            ConfigureChart(chartFx,   "F0x [N]", -3, 3);
            ConfigureChart(chartFy,   "F0y [N]", -3, 3);
            ConfigureChart(chartFz,   "F0z [N]", -3, 3);
            ConfigureChart(chartErrFx, "Error Fx [N]", -3, 3);
            ConfigureChart(chartErrFy, "Error Fy [N]", -3, 3);
            ConfigureChart(chartErrFz, "Error Fz [N]", -3, 3);

            foreach (var ch in AllCharts())
            {
                ch.Model.Series.Add(new OxyPlot.Series.LineSeries
                {
                    Color           = OxyPlot.OxyColors.OrangeRed,
                    StrokeThickness = 1.5,
                    LineStyle       = OxyPlot.LineStyle.Dash,
                });
            }
        }

        // ── Public recording API ─────────────────────────────────────────────────

        public void BeginRecording(double sampleTimeSeconds = 0.001)
        {
            _isRecording = false;
            _desiredTimeOffset = 0;
            ClearAllChartData();
            _prevT      = double.NaN;
            _hasPrevDQ  = false;
            foreach (var f in _dqFilter)  f.Reset();
            foreach (var f in _ddqFilter) f.Reset();
            _recordingStart = DateTime.UtcNow;
            _isRecording    = true;
        }

        public void StopRecording() => _isRecording = false;

        public void SetDesiredTimeOffset(double offset) => _desiredTimeOffset = offset;

        // ── Measured samples ─────────────────────────────────────────────────────

        public void RecordSample(double[] q, double[] p, double[]? force = null,
                                 double[]? estimatedForce = null)
        {
            if (!_isRecording) return;

            double q1deg = q[0] * 180.0 / Math.PI;
            double q2deg = q[1] * 180.0 / Math.PI;
            double q3deg = q[2] * 180.0 / Math.PI;
            double xcm   = p[0] * 100.0;
            double ycm   = p[1] * 100.0;
            double zcm   = p[2] * 100.0;

            double t  = (DateTime.UtcNow - _recordingStart).TotalSeconds;
            double dt = double.IsNaN(_prevT) ? 0.0 : t - _prevT;

            Add(chartQ1, 0, t, q1deg);
            Add(chartQ2, 0, t, q2deg);
            Add(chartQ3, 0, t, q3deg);
            Add(chartX,  0, t, xcm);
            Add(chartY,  0, t, ycm);
            Add(chartZ,  0, t, zcm);
            Invalidate(chartQ1, chartQ2, chartQ3, chartX, chartY, chartZ);

            if (force != null)
            {
                Add(chartFx, 0, t, force[0]);
                Add(chartFy, 0, t, force[1]);
                Add(chartFz, 0, t, force[2]);
                Invalidate(chartFx, chartFy, chartFz);
            }

            if (estimatedForce != null)
            {
                Add(chartFx, 1, t, estimatedForce[0]);
                Add(chartFy, 1, t, estimatedForce[1]);
                Add(chartFz, 1, t, estimatedForce[2]);
                Invalidate(chartFx, chartFy, chartFz);
            }

            // Estimation error: Fe (estimated) - Fs (sensor), in N.
            if (force != null && estimatedForce != null)
            {
                Add(chartErrFx, 0, t, estimatedForce[0] - force[0]);
                Add(chartErrFy, 0, t, estimatedForce[1] - force[1]);
                Add(chartErrFz, 0, t, estimatedForce[2] - force[2]);
                Invalidate(chartErrFx, chartErrFy, chartErrFz);
            }

            if (!double.IsNaN(_prevT) && dt >= MinDt)
            {
                double dq1 = _dqFilter[0].Filter((q1deg - _prevQ[0]) / dt);
                double dq2 = _dqFilter[1].Filter((q2deg - _prevQ[1]) / dt);
                double dq3 = _dqFilter[2].Filter((q3deg - _prevQ[2]) / dt);
                Add(chartDQ1, 0, t, dq1);
                Add(chartDQ2, 0, t, dq2);
                Add(chartDQ3, 0, t, dq3);
                Invalidate(chartDQ1, chartDQ2, chartDQ3);

                if (_hasPrevDQ)
                {
                    Add(chartDDQ1, 0, t, _ddqFilter[0].Filter((dq1 - _prevDQ[0]) / dt));
                    Add(chartDDQ2, 0, t, _ddqFilter[1].Filter((dq2 - _prevDQ[1]) / dt));
                    Add(chartDDQ3, 0, t, _ddqFilter[2].Filter((dq3 - _prevDQ[2]) / dt));
                    Invalidate(chartDDQ1, chartDDQ2, chartDDQ3);
                }

                _prevDQ[0] = dq1; _prevDQ[1] = dq2; _prevDQ[2] = dq3;
                _hasPrevDQ = true;
                _prevQ[0]  = q1deg; _prevQ[1] = q2deg; _prevQ[2] = q3deg;
                _prevT     = t;
            }
            else if (double.IsNaN(_prevT))
            {
                _prevQ[0] = q1deg; _prevQ[1] = q2deg; _prevQ[2] = q3deg;
                _prevT    = t;
            }
        }

        // ── Desired samples ──────────────────────────────────────────────────────

        public void RecordDesiredPoint(double t, double[] qd, double[] qpd, double[] qppd)
        {
            if (!_isRecording) return;

            t += _desiredTimeOffset;
            Add(chartQ1,   1, t, qd[0]   * 180.0 / Math.PI);
            Add(chartQ2,   1, t, qd[1]   * 180.0 / Math.PI);
            Add(chartQ3,   1, t, qd[2]   * 180.0 / Math.PI);
            Add(chartDQ1,  1, t, qpd[0]  * 180.0 / Math.PI);
            Add(chartDQ2,  1, t, qpd[1]  * 180.0 / Math.PI);
            Add(chartDQ3,  1, t, qpd[2]  * 180.0 / Math.PI);
            Add(chartDDQ1, 1, t, qppd[0] * 180.0 / Math.PI);
            Add(chartDDQ2, 1, t, qppd[1] * 180.0 / Math.PI);
            Add(chartDDQ3, 1, t, qppd[2] * 180.0 / Math.PI);

            double[] pd = GeomagicModel.ForwardKinematics(qd);
            Add(chartX, 1, t, pd[0] * 100.0);
            Add(chartY, 1, t, pd[1] * 100.0);
            Add(chartZ, 1, t, pd[2] * 100.0);

            Invalidate(chartQ1, chartQ2, chartQ3,
                       chartDQ1, chartDQ2, chartDQ3,
                       chartDDQ1, chartDDQ2, chartDDQ3,
                       chartX, chartY, chartZ);
        }

        // ── Button handlers ──────────────────────────────────────────────────────

        private void btnClear_Click(object sender, EventArgs e)
        {
            _isRecording = false;
            ClearAllChartData();
            _prevT     = double.NaN;
            _hasPrevDQ = false;
            foreach (var f in _dqFilter)  f.Reset();
            foreach (var f in _ddqFilter) f.Reset();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var (ctrl, traj) = GetExportMetadata?.Invoke() ?? ("control", "trayectoria");

            string safeName = $"{Sanitize(ctrl)}_{Sanitize(traj)}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";

            using var dlg = new SaveFileDialog
            {
                Title            = "Exportar datos de gráficas",
                Filter           = "CSV (*.csv)|*.csv",
                FileName         = safeName,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            ExportToCsv(dlg.FileName);
        }

        // ── CSV export ───────────────────────────────────────────────────────────

        private void ExportToCsv(string filePath)
        {
            // One time column: all signals share the same encoder tick.
            // Velocity (N-1 rows) and acceleration (N-2 rows) are padded with NaN
            // at the start because they need 1-2 previous samples to initialise.
            var t     = YList(chartQ1,   0, x: true);
            var q1    = YList(chartQ1,   0); var q2  = YList(chartQ2,  0); var q3  = YList(chartQ3,  0);
            var xm    = YList(chartX,    0); var ym  = YList(chartY,   0); var zm  = YList(chartZ,   0);
            var dq1   = YList(chartDQ1,  0); var dq2 = YList(chartDQ2, 0); var dq3 = YList(chartDQ3, 0);
            var ddq1  = YList(chartDDQ1, 0); var ddq2= YList(chartDDQ2,0); var ddq3= YList(chartDDQ3,0);
            var q1d   = YList(chartQ1,   1); var q2d = YList(chartQ2,  1); var q3d = YList(chartQ3,  1);
            var xd    = YList(chartX,    1); var yd  = YList(chartY,   1); var zd  = YList(chartZ,   1);
            var dq1d  = YList(chartDQ1,  1); var dq2d= YList(chartDQ2, 1); var dq3d= YList(chartDQ3, 1);
            var ddq1d = YList(chartDDQ1, 1); var ddq2d=YList(chartDDQ2,1); var ddq3d=YList(chartDDQ3,1);

            int n     = t.Count;
            int nVel  = dq1.Count;   // N-1 (first sample has no previous)
            int nAcc  = ddq1.Count;  // N-2
            int nDes  = q1d.Count;

            // Offset so velocity row j aligns with position row (n - nVel + j).
            int velOff = n - nVel;
            int accOff = n - nAcc;
            int desOff = n - nDes;

            using var w = new System.IO.StreamWriter(filePath, append: false,
                encoding: System.Text.Encoding.UTF8);

            w.WriteLine(
                "t," +
                "q1,q2,q3,x,y,z," +
                "dq1,dq2,dq3,ddq1,ddq2,ddq3," +
                "q1d,q2d,q3d,xd,yd,zd,dq1d,dq2d,dq3d,ddq1d,ddq2d,ddq3d");

            for (int i = 0; i < n; i++)
            {
                int vi = i - velOff; int ai = i - accOff; int di = i - desOff;

                string vel = vi >= 0
                    ? $"{dq1[vi]:F6},{dq2[vi]:F6},{dq3[vi]:F6},{ddq1[ai >= 0 ? ai : 0]:F6}," +
                      (ai >= 0 ? $"{ddq2[ai]:F6},{ddq3[ai]:F6}" : "NaN,NaN")
                    : "NaN,NaN,NaN,NaN,NaN,NaN";

                // Fix: build acceleration separately to avoid index reuse
                string acc = ai >= 0
                    ? $"{ddq1[ai]:F6},{ddq2[ai]:F6},{ddq3[ai]:F6}"
                    : "NaN,NaN,NaN";

                string velOnly = vi >= 0
                    ? $"{dq1[vi]:F6},{dq2[vi]:F6},{dq3[vi]:F6}"
                    : "NaN,NaN,NaN";

                string des = di >= 0
                    ? $"{q1d[di]:F6},{q2d[di]:F6},{q3d[di]:F6},{xd[di]:F6},{yd[di]:F6},{zd[di]:F6}," +
                      $"{dq1d[di]:F6},{dq2d[di]:F6},{dq3d[di]:F6},{ddq1d[di]:F6},{ddq2d[di]:F6},{ddq3d[di]:F6}"
                    : "NaN,NaN,NaN,NaN,NaN,NaN,NaN,NaN,NaN,NaN,NaN,NaN";

                w.WriteLine(
                    $"{t[i]:F6}," +
                    $"{q1[i]:F6},{q2[i]:F6},{q3[i]:F6},{xm[i]:F6},{ym[i]:F6},{zm[i]:F6}," +
                    $"{velOnly},{acc}," +
                    $"{des}");
            }
        }

        private static List<double> YList(
            OxyPlot.WindowsForms.PlotView chart, int series, bool x = false) =>
            Pts(chart, series).Select(p => x ? p.X : p.Y).ToList();

        // ── Helpers ──────────────────────────────────────────────────────────────

        private OxyPlot.WindowsForms.PlotView[] AllCharts() =>
            [chartQ1, chartQ2, chartQ3,
             chartDQ1, chartDQ2, chartDQ3,
             chartDDQ1, chartDDQ2, chartDDQ3,
             chartX, chartY, chartZ,
             chartFx, chartFy, chartFz,
             chartErrFx, chartErrFy, chartErrFz];

        private void ClearAllChartData()
        {
            foreach (var ch in AllCharts())
            {
                foreach (var s in ch.Model.Series)
                    ((OxyPlot.Series.LineSeries)s).Points.Clear();
                ch.Model.ResetAllAxes();
                ch.InvalidatePlot(true);
                ch.Refresh();
            }
        }

        private static System.Collections.Generic.List<OxyPlot.DataPoint> Pts(
            OxyPlot.WindowsForms.PlotView chart, int series) =>
            ((OxyPlot.Series.LineSeries)chart.Model.Series[series]).Points;

        private static void Add(OxyPlot.WindowsForms.PlotView chart, int series, double x, double y) =>
            ((OxyPlot.Series.LineSeries)chart.Model.Series[series]).Points.Add(
                new OxyPlot.DataPoint(x, y));

        private static void Invalidate(params OxyPlot.WindowsForms.PlotView[] charts)
        {
            foreach (var ch in charts)
                ch.InvalidatePlot(true);
        }

        private static string Sanitize(string name) =>
            new string(name.Select(c => char.IsLetterOrDigit(c) ? c : '_').ToArray())
                .Trim('_');

        private static void ConfigureChart(OxyPlot.WindowsForms.PlotView chart, string yLabel,
                                           double? yMin = null, double? yMax = null)
        {
            var model = new OxyPlot.PlotModel();
            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position           = OxyPlot.Axes.AxisPosition.Bottom,
                Title              = "t [s]",
                MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                MajorGridlineColor = OxyPlot.OxyColors.LightGray,
            });
            model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position           = OxyPlot.Axes.AxisPosition.Left,
                Title              = yLabel,
                MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                MajorGridlineColor = OxyPlot.OxyColors.LightGray,
                Minimum            = yMin ?? double.NaN,
                Maximum            = yMax ?? double.NaN,
            });
            model.Series.Add(new OxyPlot.Series.LineSeries
            {
                Color           = OxyPlot.OxyColors.SteelBlue,
                StrokeThickness = 2,
            });
            chart.Model = model;
        }
    }
}
