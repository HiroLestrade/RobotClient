namespace Teleoperation
{
    /// <summary>
    /// Gain set of the bilateral law, one value per joint.
    /// </summary>
    internal sealed record BilateralGainSet(
        double[] Ka,
        double[] Kp,
        double[] Kf,
        double[] Lambda,
        double[] Kbeta,
        double[] Kgamma);

    /// <summary>
    /// Tunings for the law of Guajardo, Section 3.1.
    ///
    /// <para><b>Paper*</b> are the experimental gains reported in Section 4.1.
    /// They were obtained on a test-bed whose torque limit is roughly two
    /// orders of magnitude above this one: <c>GeomagicDevice::appliedTorque</c>
    /// saturates the Geomagic Touch at ±1 N·m, so Ka = 750 already saturates at
    /// 1.3 mrad/s of joint speed. They are kept here as the reference target,
    /// not as something to command the hardware with as-is.</para>
    ///
    /// <para><b>Start*</b> is the set the form actually loads. It follows the
    /// thesis' own tuning procedure (Section 4.1): the pole positions Lambda
    /// are the paper's, Ka and Kp are scaled to the ±1 N·m limit, and the sigma
    /// loop (Kbeta, Kgamma) and the force integral (Kf) start at zero — rules 2
    /// and 6 of that procedure, "set Kbeta as small as possible" and "Kfi
    /// should be set small and increased gradually".</para>
    ///
    /// <para>Note that Kf = 0 does <b>not</b> switch off force reflection: the
    /// other robot's estimated torque tau_di is injected by (3.1)/(3.2)
    /// regardless. Kf only weights the integral correction Kf·dp.</para>
    /// </summary>
    internal static class BilateralGains
    {
        // ── Section 4.1, as published ────────────────────────────────────────
        // Kal = 135I, Kar = 750I, Kpl = 375I, Kpr = diag(750,1050,750),
        // Kfl = Kfr = 3000I, Lxl = 5I, Lxr = 2I, Kbl = 0.005I, Kbr = 0.001I,
        // Kgl = Kgr = 0.1I.

        public static readonly BilateralGainSet PaperLocal = new(
            Ka:     [135.0, 135.0, 135.0],
            Kp:     [375.0, 375.0, 375.0],
            Kf:     [3000.0, 3000.0, 3000.0],
            Lambda: [5.0, 5.0, 5.0],
            Kbeta:  [0.005, 0.005, 0.005],
            Kgamma: [0.1, 0.1, 0.1]);

        public static readonly BilateralGainSet PaperRemote = new(
            Ka:     [750.0, 750.0, 750.0],
            Kp:     [750.0, 1050.0, 750.0],
            Kf:     [3000.0, 3000.0, 3000.0],
            Lambda: [2.0, 2.0, 2.0],
            Kbeta:  [0.001, 0.001, 0.001],
            Kgamma: [0.1, 0.1, 0.1]);

        // ── Starting set for this hardware ───────────────────────────────────
        // Ka and Kp are the paper's divided by 500, which puts them inside the
        // two binding constraints of the Touch:
        //   Ka · qp_max      <= 1 N·m   -> at 0.5 rad/s, Ka <= 2
        //   Kp · Lambda · dq <= 1 N·m   -> at dq = 0.1 rad, Kp·Lambda <= 10
        // Lambda keeps the paper's values: it places the error pole and carries
        // no torque constraint of its own.

        public static readonly BilateralGainSet StartLocal = new(
            Ka:     [0.27, 0.27, 0.27],
            Kp:     [0.75, 0.75, 0.75],
            Kf:     [0.0, 0.0, 0.0],
            Lambda: [5.0, 5.0, 5.0],
            Kbeta:  [0.0, 0.0, 0.0],
            Kgamma: [0.0, 0.0, 0.0]);

        public static readonly BilateralGainSet StartRemote = new(
            Ka:     [1.5, 1.5, 1.5],
            Kp:     [1.5, 2.1, 1.5],
            Kf:     [0.0, 0.0, 0.0],
            Lambda: [2.0, 2.0, 2.0],
            Kbeta:  [0.0, 0.0, 0.0],
            Kgamma: [0.0, 0.0, 0.0]);

        // ── Torque estimator, Section 4.1: Kh = Ke = 20I, Kuh = Kue = 0.01I ──
        public static readonly double[] EstimatorK  = [20.0, 20.0, 20.0];
        public static readonly double[] EstimatorKu = [0.01, 0.01, 0.01];

        // ── Differentiator bounds, Assumption 3.1 ────────────────────────────
        // L bounds the highest derivative reconstructed; M bounds |L'|/L.
        //
        // M MUST stay at 0 while L is a constant, and not only because that is
        // what the assumption gives: the mu_k·M terms turn the differentiator
        // into a resonator of damping 0.21 at 2.93·M rad/s, and qhat' feeds the
        // torque command through -(Ka + Kp). The first run used M = 50 — a
        // 23 Hz mode with a loop gain of 1.5 to 5.5 through -(Ka + Kp)·qhat'
        // against the joint inertia H·s². That is the buzz.
        //
        // DirectEstimation runs M = 50 safely because in experiment 1 its
        // output is only plotted: the PID there uses in.qpf, the lambda = 10
        // dirty derivative of JointController, never the Levant estimate.
        public const double DifferentiatorL = 500.0;
        public const double DifferentiatorM = 0.0;
    }
}
