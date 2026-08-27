using System.ComponentModel;
using System.IO.Ports;

namespace RobotClient
{
    public partial class GeomagicConfigControl : UserControl
    {
        // GCode protocol:
        //   Handshake  → M115\n        expects response containing "ok"
        //   Activate   → M42 P1 S255\n expects "ok"
        //   Deactivate → M42 P1 S0\n   expects "ok"
        private SerialPort? _serialPort;

        public GeomagicConfigControl() => InitializeComponent();

        // ── Trajectory ───────────────────────────────────────────────────────────

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TrajectoryTimeSecs
        {
            get
            {
                if (double.TryParse(tbxTf.Text, out double v) && v > 0) return v;
                return 2.0;
            }
        }

        // ── Control ──────────────────────────────────────────────────────────────

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedController => cmbController.SelectedItem?.ToString() ?? "PID";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedTrajectory => cmbTrajectory.SelectedItem?.ToString() ?? "Polinomio de 5° grado";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedEndEffector => cmbEndEffector.SelectedItem?.ToString() ?? "Electroimán";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SampleTimeMs => pidGainsControl.SampleTimeMs;

        // ── Force sensor ─────────────────────────────────────────────────────────

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ForceSensorControl ForceSensor => forceSensorControl;

        public double[] GetKp() => pidGainsControl.GetKp();
        public double[] GetKi() => pidGainsControl.GetKi();
        public double[] GetKd() => pidGainsControl.GetKd();

        private void cmbController_SelectedIndexChanged(object sender, EventArgs e) =>
            pidGainsControl.Visible = SelectedController == "PID";

        // ── Serial port — connect / disconnect ───────────────────────────────────

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (_serialPort?.IsOpen == true)
            {
                _serialPort.Close();
                SetConnectionStatus(false);
                return;
            }

            btnConnect.Enabled = false;
            try
            {
                int baud = int.Parse(cmbBaudRate.SelectedItem!.ToString()!);
                _serialPort = new SerialPort(tbxComPort.Text.Trim(), baud,
                    Parity.None, 8, StopBits.One)
                {
                    ReadTimeout  = 3000,
                    WriteTimeout = 1000,
                    NewLine      = "\n",
                };
                _serialPort.Open();

                _serialPort.WriteLine("M115");
                string response = await Task.Run(() => _serialPort.ReadLine());

                if (response.Contains("ok", StringComparison.OrdinalIgnoreCase))
                {
                    SetConnectionStatus(true);
                }
                else
                {
                    _serialPort.Close();
                    SetConnectionStatus(false);
                    MessageBox.Show($"Respuesta inesperada: {response}",
                        "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                _serialPort?.Close();
                SetConnectionStatus(false);
                MessageBox.Show($"Error al conectar: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConnect.Enabled = true;
            }
        }

        private void SetConnectionStatus(bool connected)
        {
            lblStatus.Text      = connected ? "Estado: Conectado"    : "Estado: Desconectado";
            lblStatus.ForeColor = connected ? Color.Green             : Color.Red;
            btnConnect.Text     = connected ? "Desconectar"          : "Conectar";
            chkElectromagnet.Enabled = connected;
            if (!connected)
            {
                chkElectromagnet.Checked = false;
                chkElectromagnet.Text    = "Electroimán: OFF";
            }
        }

        // ── G-code interpreter ───────────────────────────────────────────────────

        public event EventHandler<string>? ExecuteGCodeRequested;

        internal void SetEndEffectorState(bool on) => chkElectromagnet.Checked = on;

        private void btnExecuteGCode_Click(object sender, EventArgs e)
        {
            string code = txtGCode.Text;
            if (!string.IsNullOrWhiteSpace(code))
                ExecuteGCodeRequested?.Invoke(this, code);
        }

        // ── Serial port — electromagnet toggle ───────────────────────────────────

        private void chkElectromagnet_CheckedChanged(object sender, EventArgs e)
        {
            bool on = chkElectromagnet.Checked;
            chkElectromagnet.Text = on ? "Electroimán: ON" : "Electroimán: OFF";

            if (_serialPort?.IsOpen != true) return;
            try
            {
                _serialPort.WriteLine(on ? "M42 P1 S255" : "M42 P1 S0");
            }
            catch { /* best-effort; next tick will reveal a broken connection */ }
        }
    }
}
