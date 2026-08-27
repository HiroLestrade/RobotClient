using System.Runtime.InteropServices;

namespace RobotClient
{
    /// <summary>
    /// Managed wrapper around DirectEstimation exported by GeomagicCore.dll.
    /// Estimates external joint torques using the robot dynamic model:
    ///   tau_e = H(q)*qpp + C(q,qp)*qp + tau_f(qp) + g(q) - tau
    /// </summary>
    internal sealed class GeomagicDirectEstimation : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicDirectEstimation_Create();

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicDirectEstimation_Destroy(IntPtr estimator);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicDirectEstimation_GetForce(IntPtr estimator, [Out] double[] F);

        private IntPtr _handle;
        private bool _disposed;

        /// <summary>Native handle passed to GeomagicController_SetEstimator.</summary>
        internal IntPtr NativeHandle => _handle;

        public GeomagicDirectEstimation()
        {
            _handle = GeomagicDirectEstimation_Create();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicDirectEstimation_Create returned null.");
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicDirectEstimation_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        /// <summary>
        /// Last Cartesian force estimate F_e = J^-T(q)*tau_e, in N, expressed in
        /// the robot base frame. All zeros before the first control tick.
        /// </summary>
        public double[] GetForce()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            double[] F = new double[3];
            GeomagicDirectEstimation_GetForce(_handle, F);
            return F;
        }
    }
}
