using System.Runtime.InteropServices;

namespace Teleoperation
{
    /// <summary>
    /// Managed wrapper around BilateralLoop in GeomagicCore.dll — the single
    /// control loop that drives both robots, mirroring the structure of the
    /// reference implementation used for the thesis experiments.
    ///
    /// One 2 ms timer computes both control laws from one position snapshot and
    /// then runs both estimators, so the torque exchange of (3.9) is broken by
    /// exactly one sample on every tick.
    ///
    /// The loop owns both controllers and both estimators. The handles exposed
    /// here point into it and must not be disposed separately; they stop being
    /// valid when the loop is disposed.
    /// </summary>
    internal sealed class GeomagicBilateralLoop : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicBilateralLoop_Create(
            IntPtr localDevice, IntPtr remoteDevice, double sampleTime);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_Destroy(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicBilateralLoop_GetLocalController(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicBilateralLoop_GetRemoteController(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicBilateralLoop_GetLocalEstimator(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicBilateralLoop_GetRemoteEstimator(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_Start(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_Stop(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicBilateralLoop_IsRunning(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_SetLocalDifferentiatorBounds(
            IntPtr loop, double L, double M);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_SetRemoteDifferentiatorBounds(
            IntPtr loop, double L, double M);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_SetVelocityFilter(
            IntPtr loop, double lambdaOwn, double lambdaPeer);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_SetVelocitySource(
            IntPtr loop, int source);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralLoop_SetDirtyLambda(
            IntPtr loop, double lambda);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicBilateralLoop_GetLogStride();

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicBilateralLoop_GetLogRowCount(IntPtr loop);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicBilateralLoop_CopyLog(IntPtr loop,
            [Out] double[] dest, int maxRows);

        private IntPtr _handle;
        private bool _disposed;

        /// <summary>Sample time of the control loop, in seconds (T = 2 ms).</summary>
        public const double SampleTime = 0.002;

        /// <summary>Robot i = l of (3.1).</summary>
        public GeomagicBilateralView Local { get; }

        /// <summary>Robot i = r of (3.2).</summary>
        public GeomagicBilateralView Remote { get; }

        public GeomagicBilateralLoop(IntPtr localDevice, IntPtr remoteDevice,
                                     double sampleTime = SampleTime)
        {
            _handle = GeomagicBilateralLoop_Create(localDevice, remoteDevice, sampleTime);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicBilateralLoop_Create returned null.");

            Local = new GeomagicBilateralView(
                GeomagicBilateralLoop_GetLocalController(_handle),
                GeomagicBilateralLoop_GetLocalEstimator(_handle),
                (l, m) => GeomagicBilateralLoop_SetLocalDifferentiatorBounds(_handle, l, m));
            Remote = new GeomagicBilateralView(
                GeomagicBilateralLoop_GetRemoteController(_handle),
                GeomagicBilateralLoop_GetRemoteEstimator(_handle),
                (l, m) => GeomagicBilateralLoop_SetRemoteDifferentiatorBounds(_handle, l, m));
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicBilateralLoop_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        /// <summary>
        /// First-order low-pass on the differentiator outputs, in rad/s, before
        /// they reach the control law and the estimator. Applies to both robots.
        /// Pass 0 to disable.
        /// </summary>
        public void SetVelocityFilter(double lambdaOwn, double lambdaPeer)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralLoop_SetVelocityFilter(_handle, lambdaOwn, lambdaPeer);
        }

        /// <summary>
        /// Resets both controllers and both estimators, then starts ticking.
        /// </summary>
        public void Start()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralLoop_Start(_handle);
        }

        /// <summary>Stops the loop and zeroes the torques on both devices.</summary>
        public void Stop()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralLoop_Stop(_handle);
        }

        /// <summary>Where qhat' comes from.</summary>
        public enum VelocitySource { Levant = 0, Dirty = 1 }

        /// <summary>
        /// Selects the velocity estimator. Under <see cref="VelocitySource.Dirty"/>
        /// the low-pass of <see cref="SetVelocityFilter"/> is bypassed.
        /// </summary>
        public void SetVelocitySource(VelocitySource source)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralLoop_SetVelocitySource(_handle, (int)source);
        }

        /// <summary>Corner of the dirty derivative's filter, rad/s.</summary>
        public void SetDirtyLambda(double lambda)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralLoop_SetDirtyLambda(_handle, lambda);
        }

        /// <summary>
        /// Column headings of the tick log, in order. One row per control tick.
        /// </summary>
        public static readonly string[] LogColumns =
        [
            "t",
            "qL1", "qL2", "qL3",
            "qR1", "qR2", "qR3",
            "vRawL1", "vRawL2", "vRawL3",
            "vRawR1", "vRawR2", "vRawR3",
            "vFiltL1", "vFiltL2", "vFiltL3",
            "vFiltR1", "vFiltR2", "vFiltR3",
            "tauL1", "tauL2", "tauL3",
            "tauR1", "tauR2", "tauR3",
        ];

        /// <summary>
        /// Writes the tick log captured since the last <see cref="Start"/> to a
        /// CSV file. Angles and velocities are converted to degrees; torques stay
        /// in N·m. Returns the number of rows written.
        /// </summary>
        public int ExportLog(string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            int stride = GeomagicBilateralLoop_GetLogStride();
            int rows   = GeomagicBilateralLoop_GetLogRowCount(_handle);
            if (rows <= 0) return 0;

            double[] buf = new double[(long)rows * stride <= int.MaxValue
                ? rows * stride : 0];
            rows = GeomagicBilateralLoop_CopyLog(_handle, buf, rows);

            const double Deg = 180.0 / Math.PI;
            using var w = new StreamWriter(path);
            w.WriteLine(string.Join(",", LogColumns));
            for (int r = 0; r < rows; r++)
            {
                int b = r * stride;
                var cells = new string[stride];
                for (int c = 0; c < stride; c++)
                {
                    // Column 0 is time in seconds; 1..18 are angles and angular
                    // velocities, shown in degrees; 19..24 are torques in N·m.
                    double v = buf[b + c];
                    if (c >= 1 && c <= 18) v *= Deg;
                    cells[c] = v.ToString("G9",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                w.WriteLine(string.Join(",", cells));
            }
            return rows;
        }

        public bool IsRunning
        {
            get
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return GeomagicBilateralLoop_IsRunning(_handle) != 0;
            }
        }
    }

    /// <summary>
    /// One robot's side of a <see cref="GeomagicBilateralLoop"/>: its controller,
    /// its estimator, and the bounds of the differentiators the loop runs for it.
    /// Holds no ownership — the loop owns all of them.
    /// </summary>
    internal sealed class GeomagicBilateralView
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralController_SetGains(IntPtr controller,
            [In] double[] ka, [In] double[] kp, [In] double[] kf,
            [In] double[] lambda, [In] double[] kbeta, [In] double[] kgamma);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralController_SetForceChannelGain(
            IntPtr controller, double gain);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicMomentumEstimator_SetGains(IntPtr estimator,
            [In] double[] k, [In] double[] ku);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicMomentumEstimator_GetEstimate(IntPtr estimator,
            [Out] double[] tau);

        private readonly IntPtr _controller;
        private readonly IntPtr _estimator;
        private readonly Action<double, double> _setBounds;

        internal GeomagicBilateralView(IntPtr controller, IntPtr estimator,
                                       Action<double, double> setBounds)
        {
            _controller = controller;
            _estimator  = estimator;
            _setBounds  = setBounds;
        }

        /// <summary>Ka, Kp, Kf, Lambda_x, Kbeta and Kgamma, one value per joint.</summary>
        public void SetControlGains(double[] ka, double[] kp, double[] kf,
                                    double[] lambda, double[] kbeta, double[] kgamma) =>
            GeomagicBilateralController_SetGains(_controller, ka, kp, kf, lambda, kbeta, kgamma);

        /// <summary>
        /// Scales tau_di where it enters (3.1)/(3.2). 1.0 is the law as
        /// written; 0.0 leaves position-only teleoperation. Diagnostic only —
        /// the law has no such factor.
        /// </summary>
        public void SetForceChannelGain(double gain) =>
            GeomagicBilateralController_SetForceChannelGain(_controller, gain);

        /// <summary>Kh / Ke and Kuh / Kue of the torque estimator.</summary>
        public void SetEstimatorGains(double[] k, double[] ku) =>
            GeomagicMomentumEstimator_SetGains(_estimator, k, ku);

        /// <summary>
        /// Bounds L and M of Assumption 3.1 for this robot's two differentiators:
        /// the order-3 one on its own position — shared by the control law and
        /// the torque estimator — and the order-2 one on the peer's position.
        /// M = 0 for a constant L.
        /// </summary>
        public void SetDifferentiatorBounds(double l, double m) => _setBounds(l, m);

        /// <summary>
        /// Last external-torque estimate, in N·m. For display: inside the loop
        /// the estimate reaches the other controller without passing here.
        /// </summary>
        public double[] GetEstimate()
        {
            double[] tau = new double[3];
            GeomagicMomentumEstimator_GetEstimate(_estimator, tau);
            return tau;
        }
    }
}
