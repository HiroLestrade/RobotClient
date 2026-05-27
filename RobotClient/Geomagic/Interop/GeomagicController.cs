using System.Runtime.InteropServices;

namespace RobotClient
{
    internal sealed class GeomagicJointController : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicController_Create(IntPtr device, double sampleTime);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_Destroy(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_SetController(IntPtr controller, IntPtr icontroller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_SetTrajectory(IntPtr controller, IntPtr itrajectory);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_MoveTo(IntPtr controller,
            [In] double[] qf, double tf);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_Start(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_Stop(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicController_IsRunning(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicController_IsCompleted(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double GeomagicController_GetMotionStartTime(IntPtr controller);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_SetEstimator(IntPtr controller, IntPtr estimator);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicController_GetLastEstimate(IntPtr controller, [Out] double[] tau_e);

        private IntPtr _handle;
        private bool _disposed;

        public GeomagicJointController(IntPtr deviceHandle, double sampleTime = 0.001)
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

        public void SetTrajectory(IntPtr itrajectoryHandle)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_SetTrajectory(_handle, itrajectoryHandle);
        }

        public void MoveTo(double[] qf, double tf)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_MoveTo(_handle, qf, tf);
        }

        public void Start()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_Start(_handle);
        }

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

        public bool IsCompleted
        {
            get
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return GeomagicController_IsCompleted(_handle) != 0;
            }
        }

        public double MotionStartTime
        {
            get
            {
                ObjectDisposedException.ThrowIf(_disposed, this);
                return GeomagicController_GetMotionStartTime(_handle);
            }
        }

        public void SetEstimator(IntPtr estimatorHandle)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicController_SetEstimator(_handle, estimatorHandle);
        }

        public double[] GetLastEstimate()
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            double[] tau_e = new double[3];
            GeomagicController_GetLastEstimate(_handle, tau_e);
            return tau_e;
        }
    }

    internal sealed class GeomagicPIDController : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicPIDController_Create();

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicPIDController_Destroy(IntPtr pid);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicPIDController_SetGains(IntPtr pid,
            [In] double[] kp, [In] double[] ki, [In] double[] kd);

        private IntPtr _handle;
        private bool _disposed;

        internal IntPtr NativeHandle => _handle;

        public GeomagicPIDController()
        {
            _handle = GeomagicPIDController_Create();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicPIDController_Create returned null.");
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicPIDController_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        public void SetGains(double[] kp, double[] ki, double[] kd)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            GeomagicPIDController_SetGains(_handle, kp, ki, kd);
        }
    }

    internal sealed class GeomagicPolyTrajectory : IDisposable
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GeomagicPolyTrajectory_Create();

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicPolyTrajectory_Destroy(IntPtr traj);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicPolyTrajectory_Evaluate(IntPtr traj, double t,
            [Out] double[] qd, [Out] double[] qpd, [Out] double[] qppd);

        private IntPtr _handle;
        private bool _disposed;

        internal IntPtr NativeHandle => _handle;

        public GeomagicPolyTrajectory()
        {
            _handle = GeomagicPolyTrajectory_Create();
            if (_handle == IntPtr.Zero)
                throw new InvalidOperationException("GeomagicPolyTrajectory_Create returned null.");
        }

        public void Dispose()
        {
            if (!_disposed && _handle != IntPtr.Zero)
            {
                GeomagicPolyTrajectory_Destroy(_handle);
                _handle = IntPtr.Zero;
                _disposed = true;
            }
        }

        public void Evaluate(double t, out double[] qd, out double[] qpd, out double[] qppd)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            qd   = new double[3];
            qpd  = new double[3];
            qppd = new double[3];
            GeomagicPolyTrajectory_Evaluate(_handle, t, qd, qpd, qppd);
        }
    }
}
