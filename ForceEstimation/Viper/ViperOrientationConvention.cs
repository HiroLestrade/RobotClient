namespace ForceEstimation
{
    /// <summary>
    /// The one place that says what the panel's three orientation boxes mean.
    ///
    /// <para><b>Proper Euler ZYZ</b>, in degrees:
    /// <c>R = Rz(φ) · Ry(θ) · Rz(ψ)</c> — rotate about z, then about the
    /// <i>new</i> y, then about the <i>new</i> z. Intrinsic, in that order.</para>
    ///
    /// <para><b>Why ZYZ and not roll-pitch-yaw.</b> The Viper's wrist is itself a
    /// proper Euler set. Working the D-H table's last three rows out gives
    /// <c>R³₆ = Ry(q4)·Rz(q5)·Ry(q6)·Rx(−π/2)</c> — a YZY chain times a constant
    /// twist. Proper Euler means the first and third axes repeat, which is what a
    /// spherical wrist does mechanically; roll-pitch-yaw (ZYX) is a Tait-Bryan
    /// set, a different family, and using it would mean converting between two
    /// unrelated parameterisations on every target.</para>
    ///
    /// <para><b>What the three numbers mean physically.</b> The tool axis in the
    /// base frame is the third column of R:
    /// <code>
    ///   ẑ6 = ( cos φ · sin θ ,  sin φ · sin θ ,  cos θ )
    /// </code>
    /// so <b>θ is the polar angle of the tool from vertical</b> and <b>φ its
    /// azimuth</b>, with <b>ψ the spin about the tool's own axis</b>:
    /// <list type="bullet">
    /// <item>θ = 0° — tool points straight up</item>
    /// <item>θ = 180° — tool points straight down, the common case</item>
    /// <item>θ = 90° — horizontal, aimed at azimuth φ</item>
    /// </list></para>
    ///
    /// <para><b>The degenerate set.</b> When sin θ = 0 — θ at 0° or 180° — only
    /// <c>φ ∓ ψ</c> survives in R: the two angles turn about the same line and
    /// infinitely many pairs describe the same orientation. That is a property of
    /// every three-number parameterisation of rotations, not of ZYZ; the
    /// degenerate set can be moved, never removed.
    ///
    /// It costs nothing <b>going in</b> — <see cref="FromZyzDeg"/> is perfectly
    /// defined there — and only bites coming back out, so
    /// <see cref="ToZyzDeg"/> picks a representative and
    /// <see cref="IsDegenerate"/> reports when it had to.</para>
    ///
    /// <para>Note this degeneracy is <b>not</b> the wrist singularity at q5 = 0.
    /// This one is in the base frame, that one is in frame 3, and R⁰₃ sits
    /// between them. Same kind of degeneracy, different configurations.</para>
    /// </summary>
    internal static class ViperOrientationConvention
    {
        private const double DegToRad = Math.PI / 180.0;
        private const double RadToDeg = 180.0 / Math.PI;

        /// <summary>
        /// Below this |sin θ| the ZYZ decomposition is degenerate. 1e-9 keeps the
        /// tie-break for genuinely singular poses rather than merely near ones,
        /// where φ and ψ are still separately meaningful.
        /// </summary>
        private const double SinThetaFloor = 1e-9;

        /// <summary>
        /// The three panel angles, in degrees, to a rotation matrix — row-major,
        /// <c>R[i, j]</c>. Total: every (φ, θ, ψ) names exactly one orientation,
        /// the degenerate set included.
        /// </summary>
        public static double[,] FromZyzDeg(double phiDeg, double thetaDeg, double psiDeg)
        {
            double cf = Math.Cos(phiDeg   * DegToRad), sf = Math.Sin(phiDeg   * DegToRad);
            double ct = Math.Cos(thetaDeg * DegToRad), st = Math.Sin(thetaDeg * DegToRad);
            double cp = Math.Cos(psiDeg   * DegToRad), sp = Math.Sin(psiDeg   * DegToRad);

            return new double[,]
            {
                { cf * ct * cp - sf * sp, -cf * ct * sp - sf * cp,  cf * st },
                { sf * ct * cp + cf * sp, -sf * ct * sp + cf * cp,  sf * st },
                {          -st * cp,                st * sp,             ct },
            };
        }

        /// <summary>
        /// Tool axis ẑ6 in the base frame for a given φ and θ — the third column
        /// of <see cref="FromZyzDeg"/>, which ψ does not affect because ψ turns
        /// about that very axis.
        /// </summary>
        public static double[] ToolAxis(double phiDeg, double thetaDeg)
        {
            double st = Math.Sin(thetaDeg * DegToRad);
            return
            [
                Math.Cos(phiDeg * DegToRad) * st,
                Math.Sin(phiDeg * DegToRad) * st,
                Math.Cos(thetaDeg * DegToRad),
            ];
        }

        /// <summary>
        /// A rotation matrix back to the three panel angles, in degrees, with
        /// θ in [0°, 180°].
        ///
        /// <para>On the degenerate set this cannot recover the pair that produced
        /// R, because infinitely many produce it. The representative chosen is
        /// <b>ψ = 0</b>, putting the whole turn into φ — so a readout of a
        /// tool-down pose shows the spin in φ and a clean zero in ψ, which is the
        /// same pair a user would have typed. <paramref name="degenerate"/> says
        /// when that choice was made.</para>
        /// </summary>
        public static (double PhiDeg, double ThetaDeg, double PsiDeg) ToZyzDeg(
            double[,] r, out bool degenerate)
        {
            // R[2,2] is cos θ; clamped because a matrix that has been through a
            // few products can land a hair outside [-1, 1] and Acos returns NaN
            // there — a NaN on the readout for a pose that is merely vertical.
            double ct    = Math.Clamp(r[2, 2], -1.0, 1.0);
            double theta = Math.Acos(ct);
            double st    = Math.Sin(theta);

            degenerate = Math.Abs(st) < SinThetaFloor;

            if (degenerate)
            {
                // At θ = 0 the matrix reduces to Rz(φ + ψ), so R[0,0] = cos(φ+ψ).
                // At θ = 180 it reduces to Rz(φ − ψ) about a flipped frame, and
                // the same entry comes back negated — hence the two branches.
                // Either way one angle carries the whole turn, and ψ = 0 puts it
                // in φ.
                double phiOnly = ct > 0.0
                    ? Math.Atan2(-r[0, 1],  r[0, 0])
                    : Math.Atan2(-r[0, 1], -r[0, 0]);

                return (phiOnly * RadToDeg, theta * RadToDeg, 0.0);
            }

            // Off the degenerate set the two are separable: φ from the tool axis
            // in the last column, ψ from the last row.
            double phi = Math.Atan2(r[1, 2],  r[0, 2]);
            double psi = Math.Atan2(r[2, 1], -r[2, 0]);

            return (phi * RadToDeg, theta * RadToDeg, psi * RadToDeg);
        }

        /// <summary>
        /// Whether θ puts the orientation on the degenerate set, where φ and ψ
        /// turn about the same line and only their combination is meaningful.
        /// </summary>
        public static bool IsDegenerate(double thetaDeg) =>
            Math.Abs(Math.Sin(thetaDeg * DegToRad)) < SinThetaFloor;

        // ── Direction and spin: what the panel actually asks for ─────────────
        //
        // The three ZYZ angles above are the convention; they are not a good
        // thing to type. What a person means by an orientation for this arm is
        // "point the tool that way, and spin it this much", and the ZYZ form
        // hands that over almost for free: the tool axis is the third column,
        //
        //     ẑ6 = (cos φ·sin θ, sin φ·sin θ, cos θ)
        //
        // so a direction determines θ and φ outright and leaves exactly ψ free.
        // That is the whole idea — the wrist being a proper Euler set is what
        // makes the split clean.

        /// <summary>
        /// A direction shorter than this is not a direction. Squared, so the
        /// check costs no square root of its own.
        /// </summary>
        private const double MinDirectionSq = 1e-18;

        /// <summary>
        /// Below this the direction is along ±z and its azimuth stops existing:
        /// every φ names the same axis. The convention pins φ = 0 there and lets
        /// ψ carry the whole spin, which is the one thing a user can still mean.
        /// </summary>
        private const double PoleFloor = 1e-12;

        /// <summary>
        /// Rotation matrix from a tool direction in the base frame and a spin
        /// about it, in degrees.
        ///
        /// <para><paramref name="v"/> need not be a unit vector — only its
        /// direction is read, and normalising is part of this step. Its length
        /// carries no meaning and a zero-length vector is rejected.</para>
        ///
        /// <para><b>Where ψ = 0 points.</b> With ψ = 0 the tool's own x axis lies
        /// in the vertical plane that contains the tool direction, tilted towards
        /// the base's up. That is the natural zero: it is the orientation with no
        /// roll about the tool. On the poles, where that plane stops being
        /// defined, φ = 0 fixes the reference instead and ψ measures from the
        /// base's x axis.</para>
        /// </summary>
        public static bool TryFromDirection(
            double[] v, double psiDeg, out double[,] r, out string reason)
        {
            r      = Identity3();
            reason = string.Empty;

            if (v.Length < 3)
            {
                reason = "La dirección necesita tres componentes.";
                return false;
            }

            double n2 = v[0] * v[0] + v[1] * v[1] + v[2] * v[2];
            if (n2 < MinDirectionSq)
            {
                reason = "El vector de dirección no puede ser cero: no apunta a ningún lado.";
                return false;
            }

            double n = Math.Sqrt(n2);
            double x = v[0] / n, y = v[1] / n, z = v[2] / n;

            double thetaDeg = Math.Acos(Math.Clamp(z, -1.0, 1.0)) * RadToDeg;
            double phiDeg   = Math.Abs(x) < PoleFloor && Math.Abs(y) < PoleFloor
                ? 0.0
                : Math.Atan2(y, x) * RadToDeg;

            r = FromZyzDeg(phiDeg, thetaDeg, psiDeg);
            return true;
        }

        /// <summary>
        /// The inverse of <see cref="TryFromDirection"/>: the tool direction, unit
        /// length, and the spin that reproduces this rotation.
        ///
        /// <para>The direction needs no decomposition — it <i>is</i> the third
        /// column. Only the spin does, and on the poles it has to be read out of
        /// φ instead of ψ, because that is where <see cref="ToZyzDeg"/> parks it
        /// and this convention parks it in the other one.</para>
        /// </summary>
        public static (double[] Direction, double PsiDeg) ToDirection(double[,] r)
        {
            double[] v = [r[0, 2], r[1, 2], r[2, 2]];

            (double phi, double theta, double psi) = ToZyzDeg(r, out bool degenerate);

            if (!degenerate) return (v, psi);

            // θ = 0 leaves Rz(φ + ψ) and θ = 180 leaves Rz(φ − ψ), so the single
            // surviving angle relates to ψ with opposite signs at the two poles.
            double spin = theta < 90.0 ? phi : -phi;
            return (v, Wrap180(spin));
        }

        /// <summary>Folds degrees into (−180, 180].</summary>
        private static double Wrap180(double deg)
        {
            double w = deg % 360.0;
            if (w >   180.0) w -= 360.0;
            if (w <= -180.0) w += 360.0;
            return w;
        }

        private static double[,] Identity3() => new double[,]
        {
            { 1, 0, 0 },
            { 0, 1, 0 },
            { 0, 0, 1 },
        };
    }
}
