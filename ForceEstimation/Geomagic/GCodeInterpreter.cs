namespace ForceEstimation
{
    // Supported commands (one per line, comments after ';'):
    //   G0 A<deg> B<deg> C<deg>  → joint-space move (degrees) via GoFinal
    //   G1 X<cm>  Y<cm>  Z<cm>  → Cartesian move (cm) via IK + GoFinal
    //   G28                      → go home (all joints to 0°) via GoHome
    //   G4  P<ms>                → dwell / pause (milliseconds)
    //   M64                      → end effector ON
    //   M65                      → end effector OFF
    internal sealed class GCodeInterpreter
    {
        private readonly IRobotAdapter         _adapter;
        private readonly GeomagicConfigControl _config;

        internal GCodeInterpreter(IRobotAdapter adapter, GeomagicConfigControl config)
        {
            _adapter = adapter;
            _config  = config;
        }

        internal async Task RunAsync(string program, Action<string>? log, CancellationToken ct)
        {
            string[] lines = program.Split('\n', StringSplitOptions.None);
            int lineNum = 0;
            foreach (string rawLine in lines)
            {
                lineNum++;
                if (ct.IsCancellationRequested) break;

                string line = rawLine;
                int ci = line.IndexOf(';');
                if (ci >= 0) line = line[..ci];
                line = line.Trim();
                if (string.IsNullOrEmpty(line)) continue;

                log?.Invoke($"[{lineNum}] {rawLine.Trim()}");

                try
                {
                    await ExecuteLineAsync(line.ToUpperInvariant(), ct);
                }
                catch (OperationCanceledException)
                {
                    log?.Invoke("Ejecución cancelada.");
                    return;
                }
                catch (Exception ex)
                {
                    log?.Invoke($"Error en línea {lineNum}: {ex.Message}");
                    return;
                }
            }
            log?.Invoke("Programa completado.");
        }

        private async Task ExecuteLineAsync(string line, CancellationToken ct)
        {
            string[] tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return;

            switch (tokens[0])
            {
                case "G0":  await JointMoveAsync(tokens, ct);     break;
                case "G1":  await CartesianMoveAsync(tokens, ct); break;
                case "G28": await GoHomeAsync(ct);                break;
                case "G4":  await DwellAsync(tokens, ct);         break;
                case "M64": _config.SetEndEffectorState(true);    break;
                case "M65": _config.SetEndEffectorState(false);   break;
                default:
                    throw new InvalidOperationException($"Comando desconocido: {tokens[0]}");
            }
        }

        private async Task JointMoveAsync(string[] tokens, CancellationToken ct)
        {
            double a = GetParam(tokens, 'A');
            double b = GetParam(tokens, 'B');
            double c = GetParam(tokens, 'C');
            if (double.IsNaN(a) || double.IsNaN(b) || double.IsNaN(c))
                throw new InvalidOperationException(
                    "G0 requiere A, B y C en grados. Ej: G0 A10 B-20 C5");

            double tf = _config.TrajectoryTimeSecs;
            _adapter.GoFinal([a * Math.PI / 180.0, b * Math.PI / 180.0, c * Math.PI / 180.0]);
            await Task.Delay((int)(tf * 1000) + 150, ct);
        }

        private async Task CartesianMoveAsync(string[] tokens, CancellationToken ct)
        {
            double x = GetParam(tokens, 'X');
            double y = GetParam(tokens, 'Y');
            double z = GetParam(tokens, 'Z');
            if (double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z))
                throw new InvalidOperationException(
                    "G1 requiere X, Y y Z en cm. Ej: G1 X10 Y5 Z20");

            double[]? qf = GeomagicModel.InverseKinematics([x / 100.0, y / 100.0, z / 100.0]);
            if (qf == null)
                throw new InvalidOperationException(
                    $"Posición ({x}, {y}, {z}) cm fuera del espacio de trabajo.");

            double tf = _config.TrajectoryTimeSecs;
            _adapter.GoFinal(qf);
            await Task.Delay((int)(tf * 1000) + 150, ct);
        }

        private async Task GoHomeAsync(CancellationToken ct)
        {
            _adapter.GoHome([0.0, 0.0, 0.0]);
            await Task.Delay(2150, ct); // TfHome = 2.0 s + buffer
        }

        private static async Task DwellAsync(string[] tokens, CancellationToken ct)
        {
            int ms = (int)GetParam(tokens, 'P', 0);
            if (ms > 0) await Task.Delay(ms, ct);
        }

        private static double GetParam(string[] tokens, char letter, double fallback = double.NaN)
        {
            foreach (string t in tokens)
            {
                if (t.Length > 1 && t[0] == letter &&
                    double.TryParse(t[1..],
                        System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double v))
                    return v;
            }
            return fallback;
        }
    }
}
