using System.Runtime.InteropServices;

namespace ForceEstimation
{
    /// <summary>
    /// Managed wrapper around the GeomagicDevice C exports in GeomagicCore.dll.
    /// Follows the same handle + free-function pattern used in the Cooperative project.
    /// </summary>
    internal sealed class GeomagicDevice : IDisposable
    {
        // ── P/Invoke declarations ────────────────────────────────────────────────

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicDevice_Create();

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicDevice_Destroy(IntPtr device);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl,
                   CharSet = CharSet.Ansi)]
        private static extern int GeomagicDevice_Initialize(IntPtr device, string? deviceName);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicDevice_Calibrate(IntPtr device);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicDevice_GetJointAngles(IntPtr device,
            [Out] double[] q);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicDevice_SetTorques(IntPtr device,
            [In] double[] taus);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicDevice_IsInitialized(IntPtr device);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicDevice_StartScheduler();

        /// <summary>
        /// Starts the shared HD scheduler. Call once after ALL devices have been
        /// initialized.
        /// </summary>
        public static void StartScheduler() => GeomagicDevice_StartScheduler();

        // ── State ────────────────────────────────────────────────────────────────

        private IntPtr _handle;
        private bool _disposed;

        /// <summary>Opaque native handle passed to GeomagicController_Create.</summary>
        internal IntPtr NativeHandle => _handle;

        // ── Construction / disposal ──────────────────────────────────────────────

        public GeomagicDevice()
        {
            _handle = GeomagicDevice_Create();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicDevice_Create returned null.");
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicDevice_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        // ── Public API ───────────────────────────────────────────────────────────

        /// <summary>
        /// Initializes the haptic device and starts the OpenHaptics servo loop.
        /// Pass <c>null</c> to use HD_DEFAULT_DEVICE.
        /// </summary>
        public bool Initialize(string? deviceName = null)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return GeomagicDevice_Initialize(_handle, deviceName) != 0;
        }

        /// <summary>
        /// Runs the inkwell / encoder-reset calibration synchronously.
        /// </summary>
        public bool Calibrate()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            return GeomagicDevice_Calibrate(_handle) != 0;
        }

        /// <summary>
        /// Returns the three calibrated joint angles in radians.
        /// </summary>
        public double[] GetJointAngles()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            double[] q = new double[3];
            GeomagicDevice_GetJointAngles(_handle, q);
            return q;
        }

        /// <summary>
        /// Sets the three joint torques in N·m.
        /// </summary>
        public void SetTorques(double[] taus)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (taus.Length < 3) throw new ArgumentException("taus must have at least 3 elements.");
            GeomagicDevice_SetTorques(_handle, taus);
        }

        public bool IsInitialized
        {
            get
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return GeomagicDevice_IsInitialized(_handle) != 0;
            }
        }
    }
}
