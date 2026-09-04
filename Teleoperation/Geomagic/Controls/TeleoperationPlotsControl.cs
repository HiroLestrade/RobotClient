using System.Diagnostics;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace Teleoperation
{
    /// <summary>
    /// Tabbed plot panel for the teleoperation experiment.
    ///
    /// Unlike the plots of experiment 1, these are not a recording of a run with
    /// a start and an end: teleoperation has no trajectory to finish. They
    /// behave like an oscilloscope instead. The horizontal axis is always the
    /// last <see cref="WindowSeconds"/> seconds, so the newest sample sits at
    /// the right edge, the trace grows leftwards while the window fills, and
    /// once it is full every new sample pushes the oldest one off the left.
    /// </summary>
    public partial class TeleoperationPlotsControl : UserControl
    {
        /// <summary>Width of the horizontal window, in seconds.</summary>
        public const double WindowSeconds = 20.0;

        // The encoder timer samples at ~60 Hz; redrawing three charts that often
        // costs more than it shows. Points are appended every sample, the charts
        // are repainted at this period.
        private const double RenderPeriod = 1.0 / 25.0;

        private readonly Stopwatch _clock = new();
        private double _lastRender = double.NegativeInfinity;

        private readonly LineSeries[] _local  = new LineSeries[3];
        private readonly LineSeries[] _remote = new LineSeries[3];

        public TeleoperationPlotsControl()
        {
            InitializeComponent();

            PlotView[] charts = [chartQ1, chartQ2, chartQ3];
            string[] titles   = ["q₁ [°]", "q₂ [°]", "q₃ [°]"];

            for (int i = 0; i < 3; i++)
            {
                _local[i]  = new LineSeries
                {
                    Title           = "Local",
                    Color           = OxyColors.SteelBlue,
                    StrokeThickness = 2,
                };
                _remote[i] = new LineSeries
                {
                    Title           = "Remoto",
                    Color           = OxyColors.OrangeRed,
                    StrokeThickness = 2,
                };
                charts[i].Model = BuildModel(titles[i], _local[i], _remote[i]);
            }
        }

        private static PlotModel BuildModel(string yLabel, params LineSeries[] series)
        {
            var model = new PlotModel();
            model.Axes.Add(new LinearAxis
            {
                Position           = AxisPosition.Bottom,
                Title              = "t [s]",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
                // Fixed span; the tick handler slides it.
                Minimum            = -WindowSeconds,
                Maximum            = 0.0,
                IsZoomEnabled      = false,
                IsPanEnabled       = false,
            });
            model.Axes.Add(new LinearAxis
            {
                Position           = AxisPosition.Left,
                Title              = yLabel,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
            });
            model.Legends.Add(new OxyPlot.Legends.Legend
            {
                LegendPosition    = OxyPlot.Legends.LegendPosition.TopRight,
                LegendPlacement   = OxyPlot.Legends.LegendPlacement.Inside,
                LegendOrientation = OxyPlot.Legends.LegendOrientation.Horizontal,
                LegendFontSize    = 11,
            });
            foreach (var s in series)
                model.Series.Add(s);
            return model;
        }

        /// <summary>
        /// Appends one sample of both robots' joint angles, in degrees. Call it
        /// from the same tick that reads both encoders, so the two traces share
        /// a time base.
        /// </summary>
        public void AddSample(double[] qLocalDeg, double[] qRemoteDeg)
        {
            if (!_clock.IsRunning) _clock.Restart();
            double t = _clock.Elapsed.TotalSeconds;

            for (int i = 0; i < 3; i++)
            {
                _local[i].Points.Add(new DataPoint(t, qLocalDeg[i]));
                _remote[i].Points.Add(new DataPoint(t, qRemoteDeg[i]));
            }

            // Drop everything that has scrolled off the left edge. A little
            // slack keeps the leftmost segment from ending mid-plot.
            double cutoff = t - WindowSeconds * 1.05;
            for (int i = 0; i < 3; i++)
            {
                TrimOlderThan(_local[i], cutoff);
                TrimOlderThan(_remote[i], cutoff);
            }

            if (t - _lastRender < RenderPeriod) return;
            _lastRender = t;

            PlotView[] charts = [chartQ1, chartQ2, chartQ3];
            foreach (var chart in charts)
            {
                var xAxis = chart.Model.Axes[0];
                xAxis.Minimum = t - WindowSeconds;
                xAxis.Maximum = t;
                chart.Model.InvalidatePlot(true);
            }
        }

        private static void TrimOlderThan(LineSeries s, double cutoff)
        {
            int drop = 0;
            while (drop < s.Points.Count && s.Points[drop].X < cutoff)
                drop++;
            if (drop > 0)
                s.Points.RemoveRange(0, drop);
        }

        /// <summary>Empties the traces and restarts the time base.</summary>
        public void Clear()
        {
            _clock.Reset();
            _lastRender = double.NegativeInfinity;

            for (int i = 0; i < 3; i++)
            {
                _local[i].Points.Clear();
                _remote[i].Points.Clear();
            }

            PlotView[] charts = [chartQ1, chartQ2, chartQ3];
            foreach (var chart in charts)
            {
                var xAxis = chart.Model.Axes[0];
                xAxis.Minimum = -WindowSeconds;
                xAxis.Maximum = 0.0;
                chart.Model.InvalidatePlot(true);
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => Clear();

        /// <summary>
        /// Supplied by the form: writes the control loop's tick log to the given
        /// path and returns the row count. Null while no loop exists.
        /// </summary>
        [System.ComponentModel.DesignerSerializationVisibility(
            System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Func<string, int>? ExportLog { get; set; }

        private void btnExportLog_Click(object sender, EventArgs e)
        {
            if (ExportLog == null)
            {
                MessageBox.Show(
                    "Conecte los robots antes de exportar el registro.",
                    "Sin registro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dlg = new SaveFileDialog
            {
                Filter   = "CSV (*.csv)|*.csv",
                FileName = $"teleop_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            try
            {
                int rows = ExportLog(dlg.FileName);
                MessageBox.Show(
                    rows > 0
                        ? $"{rows} muestras escritas ({rows * 0.002:F1} s a 500 Hz)."
                        : "El registro está vacío: inicie la teleoperación primero.",
                    "Exportar registro", MessageBoxButtons.OK,
                    rows > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo escribir el archivo:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
