using System.Globalization;

namespace ForceEstimation
{
    /// <summary>
    /// Runs a small G-code dialect against any <see cref="IGCodeRobot"/>.
    ///
    /// <code>
    ///   G0  &lt;axes&gt;        joint move; one letter per axis, in the panel's units
    ///   G1  X Y Z [...]   Cartesian move, cm; the robot reads the rest
    ///   G28               home
    ///   G4  P&lt;ms&gt;         dwell
    ///   M64 / M65         end effector on / off
    /// </code>
    ///
    /// <para>Comments run from <c>;</c> to end of line. One command per line.</para>
    ///
    /// <para><b>Moves are paced by time, not by completion.</b> A line is sent and
    /// then the interpreter waits <see cref="IGCodeRobot.MoveWaitSecs"/>. That is
    /// why the robot, not this class, answers how long a move takes: it is the one
    /// that knows what its controller does after the trajectory ends.</para>
    /// </summary>
    internal sealed class GCodeInterpreter
    {
        private readonly IGCodeRobot _robot;

        internal GCodeInterpreter(IGCodeRobot robot) => _robot = robot;

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
                case "M64": _robot.SetEndEffectorState(true);     break;
                case "M65": _robot.SetEndEffectorState(false);    break;

                default:
                    throw new InvalidOperationException($"Comando desconocido: {tokens[0]}");
            }
        }

        private async Task JointMoveAsync(string[] tokens, CancellationToken ct)
        {
            string letters = _robot.AxisLetters;
            var q = new double[letters.Length];

            for (int i = 0; i < letters.Length; i++)
            {
                q[i] = GetParam(tokens, letters[i]);
                if (double.IsNaN(q[i]))
                    throw new InvalidOperationException(
                        $"G0 requiere {Listed(letters)}. Ej: G0 {Example(letters)}");
            }

            if (!_robot.MoveToJoints(q))
                throw new InvalidOperationException("El movimiento no pudo iniciarse.");

            await Wait(_robot.MoveWaitSecs, ct);
        }

        private async Task CartesianMoveAsync(string[] tokens, CancellationToken ct)
        {
            Dictionary<char, double> words = Words(tokens);

            if (!words.ContainsKey('X') || !words.ContainsKey('Y') || !words.ContainsKey('Z'))
                throw new InvalidOperationException(
                    "G1 requiere X, Y y Z en cm. Ej: G1 X10 Y5 Z20");

            if (!_robot.TryCartesianToJoints(words, out double[] q, out string reason))
                throw new InvalidOperationException(reason);

            if (!_robot.MoveToJoints(q))
                throw new InvalidOperationException("El movimiento no pudo iniciarse.");

            await Wait(_robot.MoveWaitSecs, ct);
        }

        private async Task GoHomeAsync(CancellationToken ct)
        {
            if (!_robot.GoHome())
                throw new InvalidOperationException("El movimiento a Home no pudo iniciarse.");

            await Wait(_robot.HomeWaitSecs, ct);
        }

        private static async Task DwellAsync(string[] tokens, CancellationToken ct)
        {
            int ms = (int)GetParam(tokens, 'P', 0);
            if (ms > 0) await Task.Delay(ms, ct);
        }

        private static Task Wait(double secs, CancellationToken ct) =>
            Task.Delay((int)(secs * 1000.0), ct);

        /// <summary>Every <c>letter+number</c> token on the line, keyed by letter.</summary>
        private static Dictionary<char, double> Words(string[] tokens)
        {
            var words = new Dictionary<char, double>();

            // tokens[0] is the command itself.
            for (int i = 1; i < tokens.Length; i++)
            {
                string t = tokens[i];
                if (t.Length > 1 && char.IsLetter(t[0]) &&
                    double.TryParse(t[1..], NumberStyles.Float,
                                    CultureInfo.InvariantCulture, out double v))
                {
                    words[t[0]] = v;
                }
            }

            return words;
        }

        private static double GetParam(string[] tokens, char letter, double fallback = double.NaN)
        {
            foreach (string t in tokens)
            {
                if (t.Length > 1 && t[0] == letter &&
                    double.TryParse(t[1..], NumberStyles.Float,
                                    CultureInfo.InvariantCulture, out double v))
                    return v;
            }

            return fallback;
        }

        private static string Listed(string letters) =>
            letters.Length <= 1
                ? letters
                : string.Join(", ", letters[..^1].ToCharArray()) + " y " + letters[^1];

        private static string Example(string letters) =>
            string.Join(' ', letters.Select((c, i) => $"{c}{(i % 2 == 0 ? 10 : -20)}"));
    }
}
