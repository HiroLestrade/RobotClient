using System.Runtime.InteropServices;

namespace Teleoperation
{
    /// <summary>
    /// Managed wrapper around JointController in GeomagicCore.dll. Only the
    /// free-running form is used here: Start() runs the loop with no trajectory
    /// until Stop(), which is what the bilateral law needs.
    /// </summary>
    internal sealed class GeomagicJointController : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicController_Create(IntPtr device, double sampleTime);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_Destroy(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_SetController(IntPtr controller, IntPtr icontroller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_SetEstimator(IntPtr controller, IntPtr estimator);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_Start(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_Stop(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicController_IsRunning(IntPtr controller);

        private IntPtr _handle;
        private bool _disposed;

        /// <summary>Sample time of the control loop, in seconds (T = 2 ms).</summary>
        public const double SampleTime = 0.002;

        public GeomagicJointController(IntPtr deviceHandle, double sampleTime = SampleTime)
        {
            _handle = GeomagicController_Create(deviceHandle, sampleTime);
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicController_Create returned null.");
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicController_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        public void SetController(IntPtr icontrollerHandle)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_SetController(_handle, icontrollerHandle);
        }

        public void SetEstimator(IntPtr estimatorHandle)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_SetEstimator(_handle, estimatorHandle);
        }

        /// <summary>
        /// Starts the control loop with no trajectory. Resets the controller and
        /// the estimator, then ticks until <see cref="Stop"/>.
        /// </summary>
        public void Start()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_Start(_handle);
        }

        /// <summary>Stops the loop and zeroes the torques.</summary>
        public void Stop()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_Stop(_handle);
        }

        public bool IsRunning
        {
            get
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return GeomagicController_IsRunning(_handle) != 0;
            }
        }
    }
}
