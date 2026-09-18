using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace ForceEstimation
{
    /// <summary>
    /// Tabbed plot panel for the Viper X-300S: joint position, velocity and
    /// acceleration, plus Cartesian position.
    ///
    /// <para>Each joint gets its own sub-tab with exactly two traces: the
    /// measured angle solid and the one the trajectory asked for dashed in the
    /// same colour. Reading a controller means reading the gap between those
    /// two, and six joints stacked on one pair of axes hid it behind the other
    /// five.</para>
    ///
    /// <para>The Cartesian tab plots the tool tip through the forward
    /// kinematics. Force plots still come after the sensor.</para>
    ///
    /// <para><b>All three quantities are measured or analytic — none is
    /// differentiated here.</b> The motors report velocity directly, the loop
    /// derives acceleration once from that velocity, and the desired traces come
    /// from the quintic's own derivatives. Nothing in this panel differentiates
    /// a position twice, which is what makes the acceleration plot readable at
    /// all.</para>
    /// </summary>
    public partial class ViperPlotsControl : UserControl
    {
        private const int JointCount = 6;

        // Measured, one series per joint on each of the three joint tabs.
        private readonly LineSeries[] _position     = new LineSeries[JointCount];
        private readonly LineSeries[] _velocity     = new LineSeries[JointCount];
        private readonly LineSeries[] _acceleration = new LineSeries[JointCount];

        // Desired, same joints, dashed.
        private readonly LineSeries[] _positionD     = new LineSeries[JointCount];
        private readonly LineSeries[] _velocityD     = new LineSeries[JointCount];
        private readonly LineSeries[] _accelerationD = new LineSeries[JointCount];

        /// <summary>Position error, qd − q, one series per joint.</summary>
        private readonly LineSeries[] _error = new LineSeries[JointCount];

        private readonly LineSeries[] _cartesian = new LineSeries[3];

        private bool _isRecording;

        private static readonly OxyColor[] JointColors =
        [
            OxyColors.SteelBlue, OxyColors.OrangeRed, OxyColors.SeaGreen,
            OxyColors.MediumPurple, OxyColors.Goldenrod, OxyColors.Teal,
        ];

        public ViperPlotsControl()
        {
            InitializeComponent();

            // Each joint quantity gets one chart per joint, on its own nested
            // tabs: two traces on a pair of axes instead of twelve.
            for (int i = 0; i < JointCount; i++)
            {
                chartPositionJoint[i].Model =
                    BuildOneJointModel(i, $"q{i + 1} [°]",    _position,     _positionD);
                chartVelocityJoint[i].Model =
                    BuildOneJointModel(i, $"q̇{i + 1} [°/s]",  _velocity,     _velocityD);
                chartAccelJoint[i].Model =
                    BuildOneJointModel(i, $"q̈{i + 1} [°/s²]", _acceleration, _accelerationD);
                chartErrorJoint[i].Model =
                    BuildErrorModel(i, $"e{i + 1} = q{i + 1}d − q{i + 1} [°]");
            }

            chartCartesian.Model = BuildCartesianModel();
        }

        // ── Recording ────────────────────────────────────────────────────────

        /// <summary>
        /// Whether samples handed to <see cref="AddSample"/> are being kept.
        /// </summary>
        public bool IsRecording => _isRecording;

        /// <summary>Clears every trace and starts accepting samples.</summary>
        public void BeginRecording()
        {
            _isRecording = false;
            Clear();
            _isRecording = true;
        }

        public void StopRecording() => _isRecording = false;

        /// <summary>
        /// Appends one sample. Cheap on purpose — it only grows the point lists.
        /// Repainting is <see cref="RefreshPlots"/>, called once per batch, so a
        /// loop running well above the screen's refresh rate does not force one
        /// repaint per tick.
        /// </summary>
        public void AddSample(double t,
                              double[] q,  double[] qp,  double[] qpp,
                              double[] qd, double[] qpd, double[] qppd,
                              double[]? pCm = null)
        {
            if (!_isRecording) return;

            // Cartesian shares one pair of axes on purpose: x, y and z are three
            // components of one point, not three joints to be compared.
            if (pCm != null)
                for (int i = 0; i < _cartesian.Length && i < pCm.Length; i++)
                    _cartesian[i].Points.Add(new DataPoint(t, pCm[i]));

            for (int i = 0; i < JointCount; i++)
            {
                if (i < q.Length)    _position[i].Points.Add(new DataPoint(t, q[i]));
                if (i < qp.Length)   _velocity[i].Points.Add(new DataPoint(t, qp[i]));
                if (i < qpp.Length)  _acceleration[i].Points.Add(new DataPoint(t, qpp[i]));

                if (i < qd.Length)   _positionD[i].Points.Add(new DataPoint(t, qd[i]));
                if (i < qpd.Length)  _velocityD[i].Points.Add(new DataPoint(t, qpd[i]));
                if (i < qppd.Length) _accelerationD[i].Points.Add(new DataPoint(t, qppd[i]));

                // Both terms come from the same tick, so the error is the gap
                // the position tab shows — just read off an axis.
                if (i < q.Length && i < qd.Length)
                    _error[i].Points.Add(new DataPoint(t, qd[i] - q[i]));
            }
        }

        /// <summary>
        /// Repaints the joint tabs. Call once after a batch.
        ///
        /// <para>Every per-joint chart is invalidated, not just the visible one:
        /// invalidating is what rescales the axes, and a chart that only did it
        /// while on screen would show the previous motion's ranges for the first
        /// frame after switching tabs. Six models of a few hundred points each
        /// are cheap enough that the distinction is not worth the bug.</para>
        /// </summary>
        public void RefreshPlots()
        {
            foreach (PlotView view in JointCharts())
                view.Model.InvalidatePlot(true);

            chartCartesian.Model.InvalidatePlot(true);
        }

        /// <summary>Every per-joint chart, across all three quantities.</summary>
        private IEnumerable<PlotView> JointCharts() =>
            chartPositionJoint
                .Concat(chartErrorJoint)
                .Concat(chartVelocityJoint)
                .Concat(chartAccelJoint);

        // ── Building ─────────────────────────────────────────────────────────

        /// <summary>
        /// One joint on its own axes: its measured trace and the one the
        /// trajectory asked for, and nothing else competing with them.
        /// </summary>
        private static PlotModel BuildOneJointModel(int joint, string yLabel,
                                                    LineSeries[] measured,
                                                    LineSeries[] desired)
        {
            var model = NewModel(yLabel);

            measured[joint] = new LineSeries
            {
                Title           = "medida",
                Color           = JointColors[joint],
                StrokeThickness = 1.5,
            };
            desired[joint] = new LineSeries
            {
                Title           = "deseada",
                Color           = JointColors[joint],
                StrokeThickness = 1.0,
                LineStyle       = LineStyle.Dash,
            };

            model.Series.Add(measured[joint]);
            model.Series.Add(desired[joint]);
            return model;
        }

        /// <summary>
        /// One joint's tracking error on its own axes, with a line at zero.
        ///
        /// <para>The zero line is what makes the plot readable: the error's
        /// <b>sign</b> says which side of the reference the joint is on, and
        /// whether the curve is symmetric about zero separates a pure lag — which
        /// flips sign with the direction of travel — from a bias that gravity or
        /// friction holds to one side.</para>
        /// </summary>
        private PlotModel BuildErrorModel(int joint, string yLabel)
        {
            var model = NewModel(yLabel);

            model.Annotations.Add(new LineAnnotation
            {
                Type            = LineAnnotationType.Horizontal,
                Y               = 0.0,
                Color           = OxyColors.Gray,
                StrokeThickness = 1.0,
                LineStyle       = LineStyle.Solid,
            });

            _error[joint] = new LineSeries
            {
                Title           = "error",
                Color           = JointColors[joint],
                StrokeThickness = 1.5,
            };
            model.Series.Add(_error[joint]);
            return model;
        }

        private PlotModel BuildCartesianModel()
        {
            var model = NewModel("posición [cm]");
            string[] names = ["x", "y", "z"];
            for (int i = 0; i < 3; i++)
            {
                _cartesian[i] = new LineSeries
                {
                    Title           = names[i],
                    Color           = JointColors[i],
                    StrokeThickness = 1.5,
                };
                model.Series.Add(_cartesian[i]);
            }
            return model;
        }

        private static PlotModel NewModel(string yLabel)
        {
            var model = new PlotModel();
            model.Axes.Add(new LinearAxis
            {
                Position           = AxisPosition.Bottom,
                Title              = "t [s]",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.LightGray,
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
            return model;
        }

        /// <summary>Empties every trace.</summary>
        public void Clear()
        {
            foreach (var set in new[] { _position, _velocity, _acceleration,
                                        _positionD, _velocityD, _accelerationD,
                                        _error, _cartesian })
                foreach (var s in set)
                    s.Points.Clear();

            foreach (PlotView view in JointCharts())
                view.Model.InvalidatePlot(true);

            chartCartesian.Model.InvalidatePlot(true);
        }

        private void btnClear_Click(object sender, EventArgs e) => Clear();
    }
}
