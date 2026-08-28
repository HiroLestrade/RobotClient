using System.Runtime.InteropServices;

namespace Teleoperation
{
    /// <summary>
    /// Managed wrapper around BilateralController in GeomagicCore.dll — the
    /// position/force law of Section 3.1 in Guajardo, Eqs. (3.1)-(3.11).
    /// One instance per robot: the local one applies (3.1), the remote (3.2).
    /// </summary>
    internal sealed class GeomagicBilateralController : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicBilateralController_Create(int isLocal);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralController_Destroy(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralController_SetGains(IntPtr controller,
            [In] double[] ka, [In] double[] kp, [In] double[] kf,
            [In] double[] lambda, [In] double[] kbeta, [In] double[] kgamma);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralController_SetEstimator(IntPtr controller,
            IntPtr estimator);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralController_SetPeer(IntPtr controller,
            IntPtr peerDevice, IntPtr peerEstimator);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicBilateralController_SetDifferentiatorBounds(
            IntPtr controller, double L, double M);

        private IntPtr _handle;
        private bool _disposed;

        /// <summary>Native handle passed to GeomagicController_SetController.</summary>
        internal IntPtr NativeHandle => _handle;

        public GeomagicBilateralController(bool isLocal)
        {
            _handle = GeomagicBilateralController_Create(isLocal ? 1 : 0);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicBilateralController_Create returned null.");
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicBilateralController_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        /// <summary>
        /// Ka, Kp, Kf, Lambda_x, Kbeta and Kgamma, one value per joint.
        /// </summary>
        public void SetGains(double[] ka, double[] kp, double[] kf,
                             double[] lambda, double[] kbeta, double[] kgamma)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralController_SetGains(_handle, ka, kp, kf, lambda, kbeta, kgamma);
        }

        /// <summary>
        /// Bounds L and M of Assumption 3.1 for the two differentiators. Their
        /// estimates go straight into the torque command, so M is what decides
        /// whether the loop buzzes: M = 0 for a constant L.
        /// </summary>
        public void SetDifferentiatorBounds(double l, double m)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralController_SetDifferentiatorBounds(_handle, l, m);
        }

        /// <summary>This robot's own estimator — tauhat_i of (3.10).</summary>
        public void SetEstimator(IntPtr estimatorHandle)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralController_SetEstimator(_handle, estimatorHandle);
        }

        /// <summary>
        /// The other end of the channel: q_di of (3.13) is read from
        /// <paramref name="peerDeviceHandle"/> and tau_di of (3.9) from
        /// <paramref name="peerEstimatorHandle"/>, both at the control rate.
        /// </summary>
        public void SetPeer(IntPtr peerDeviceHandle, IntPtr peerEstimatorHandle)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicBilateralController_SetPeer(_handle, peerDeviceHandle, peerEstimatorHandle);
        }
    }
}
