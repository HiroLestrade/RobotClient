namespace Teleoperation
{
    public partial class ForceSensorControl : UserControl
    {
        private ForceSensorDevice? _device;

        /// <summary>Raised whenever the connection to the force sensor changes.</summary>
        public event EventHandler<bool>? ConnectionChanged;

        public bool IsConnected => _device?.IsConnected == true;

        public ForceSensorControl() => InitializeComponent();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _device?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Reads {Fx, Fy, Fz} once, in the sensor's own frame. Returns null if not
        /// connected or the read fails. Intended to be called from the same sample
        /// tick that records the robot's measured trajectory, so force and motion
        /// data share a common time base.
        /// </summary>
        public double[]? TryRead() =>
            _device != null && _device.TryRead(out double[] force) ? force : null;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (_device?.IsConnected == true)
            {
                _device.Disconnect();
                SetConnected(false);
                return;
            }

            string deviceName = string.IsNullOrWhiteSpace(tbxDeviceName.Text)
                ? "Dev1" : tbxDeviceName.Text.Trim();

            _device ??= new ForceSensorDevice();
            if (_device.Connect(deviceName))
            {
                SetConnected(true);
            }
            else
            {
                SetConnected(false);
                MessageBox.Show(
                    $"No se pudo conectar al sensor de fuerza:\n{ForceSensorDevice.LastError}",
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTare_Click(object sender, EventArgs e)
        {
            if (_device?.IsConnected != true) return;

            btnTare.Enabled = false;
            try
            {
                if (_device.Tare())
                    MessageBox.Show("Tara completada.", "Sensor de fuerza",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"No se pudo tarar el sensor:\n{ForceSensorDevice.LastError}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { btnTare.Enabled = true; }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            double[]? f = TryRead();
            if (f == null)
            {
                MessageBox.Show($"No se pudo leer el sensor:\n{ForceSensorDevice.LastError}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            tbxFx.Text = $"{f[0]:F4}";
            tbxFy.Text = $"{f[1]:F4}";
            tbxFz.Text = $"{f[2]:F4}";
        }

        private void SetConnected(bool connected)
        {
            lblStatus.Text      = connected ? "Estado: Conectado" : "Estado: Desconectado";
            lblStatus.ForeColor = connected ? Color.Green : Color.Red;
            btnConnect.Text     = connected ? "Desconectar" : "Conectar";
            tbxDeviceName.Enabled = !connected;
            btnTare.Enabled     = connected;
            btnRead.Enabled     = connected;
            if (!connected)
                tbxFx.Text = tbxFy.Text = tbxFz.Text = "0.0000";

            ConnectionChanged?.Invoke(this, connected);
        }
    }
}
