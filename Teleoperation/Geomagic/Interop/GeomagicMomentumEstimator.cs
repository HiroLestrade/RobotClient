using System.Runtime.InteropServices;

namespace Teleoperation
{
    /// <summary>
    /// Managed wrapper around MomentumEstimator in GeomagicCore.dll — the
    /// residual-based external-torque estimator of Section 3.1.2 in Guajardo,
    /// Eqs. (3.35)-(3.43). The local instance estimates the human torque
    /// tau_h (3.38); the remote one, the environment torque tau_e (3.39).
    /// </summary>
    internal sealed class GeomagicMomentumEstimator : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicMomentumEstimator_Create(int isLocal);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicMomentumEstimator_Destroy(IntPtr estimator);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicMomentumEstimator_SetGains(IntPtr estimator,
            [In] double[] k, [In] double[] ku);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicMomentumEstimator_GetEstimate(IntPtr estimator,
            [Out] double[] tau);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicMomentumEstimator_SetDifferentiatorBounds(
            IntPtr estimator, double L, double M);

        private IntPtr _handle;
        private bool _disposed;

        /// <summary>Native handle passed to GeomagicController_SetEstimator.</summary>
        internal IntPtr NativeHandle => _handle;

        public GeomagicMomentumEstimator(bool isLocal)
        {
            _handle = GeomagicMomentumEstimator_Create(isLocal ? 1 : 0);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicMomentumEstimator_Create returned null.");
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicMomentumEstimator_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        /// <summary>
        /// Kh / Ke (residual filter bandwidth) and Kuh / Kue (relay term).
        /// </summary>
        public void SetGains(double[] k, double[] ku)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicMomentumEstimator_SetGains(_handle, k, ku);
        }

        /// <summary>
        /// Bounds L and M of Assumption 3.1 for the internal differentiator.
        /// </summary>
        public void SetDifferentiatorBounds(double l, double m)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicMomentumEstimator_SetDifferentiatorBounds(_handle, l, m);
        }

        /// <summary>
        /// Last external-torque estimate, in N·m. All zeros before the first
        /// control tick with this estimator attached.
        /// </summary>
        public double[] GetEstimate()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            double[] tau = new double[3];
            GeomagicMomentumEstimator_GetEstimate(_handle, tau);
            return tau;
        }
    }
}
