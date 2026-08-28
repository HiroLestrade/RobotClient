using System.Runtime.InteropServices;

namespace ForceEstimation
{
    /// <summary>
    /// Managed wrapper around the global (handle-less) C exports in ATIForceSensor.dll.
    /// The native functions are declared __stdcall without a .def file, so MSVC
    /// exports them name-decorated (e.g. "_ATIForceSensor_Connect@4"); the entry
    /// points below spell out those decorated names explicitly.
    /// </summary>
    internal sealed class ForceSensorDevice : IDisposable
    {
        // ── P/Invoke declarations ────────────────────────────────────────────────

        [DllImport("ATIForceSensor.dll", EntryPoint = "_ATIForceSensor_Connect@4",
                   CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern int ATIForceSensor_Connect(string deviceName);

        [DllImport("ATIForceSensor.dll", EntryPoint = "_ATIForceSensor_Disconnect@0",
                   CallingConvention = CallingConvention.StdCall)]
        private static extern int ATIForceSensor_Disconnect();

        [DllImport("ATIForceSensor.dll", EntryPoint = "_ATIForceSensor_Tare@8",
                   CallingConvention = CallingConvention.StdCall)]
        private static extern int ATIForceSensor_Tare(int numSamples, IntPtr outOffsets);

        [DllImport("ATIForceSensor.dll", EntryPoint = "_ATIForceSensor_Read@4",
                   CallingConvention = CallingConvention.StdCall)]
        private static extern int ATIForceSensor_Read([Out] double[] force);

        [DllImport("ATIForceSensor.dll", EntryPoint = "_ATIForceSensor_GetLastError@0",
                   CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern string ATIForceSensor_GetLastError();

        // ── State ────────────────────────────────────────────────────────────────

        private bool _connected;
        private bool _disposed;

        public bool IsConnected => _connected;

        public static string LastError => ATIForceSensor_GetLastError();

        // ── Public API ───────────────────────────────────────────────────────────

        public bool Connect(string deviceName)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            _connected = ATIForceSensor_Connect(deviceName) == 0;
            return _connected;
        }

        public void Disconnect()
        {
            if (_disposed || !_connected) return;
            ATIForceSensor_Disconnect();
            _connected = false;
        }

        /// <summary>Averages <paramref name="numSamples"/> raw readings as the zero offset.</summary>
        public bool Tare(int numSamples = 100)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (!_connected) return false;
            return ATIForceSensor_Tare(numSamples, IntPtr.Zero) == 0;
        }

        /// <summary>Reads calibrated {Fx, Fy, Fz} in the sensor's own frame. False on failure.</summary>
        public bool TryRead(out double[] force)
        {
            force = new double[3];
            ObjectDisposedException.ThrowIf(_disposed, this);
            if (!_connected) return false;
            return ATIForceSensor_Read(force) == 0;
        }

        public void Dispose()
        {
            if (_disposed) return;
            Disconnect();
            _disposed = true;
        }
    }
}
