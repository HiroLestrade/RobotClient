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
    }
}
