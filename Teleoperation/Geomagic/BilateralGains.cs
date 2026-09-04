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
    /// <para><b>Start*</b> is the set the form actually loads: the gains of the
    /// reference implementation, which is the one that ran the experiments.
    /// Only the sigma loop (Kbeta, Kgamma) is still at zero.</para>
    ///
    /// <para>Note that Kf does <b>not</b> switch force reflection on or off:
    /// the other robot's estimated torque tau_di is injected by (3.1)/(3.2)
    /// regardless of Kf. Kf weights the integral correction Kf·dp, and that
    /// integral is what keeps the two sides pinned to each other.</para>
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
        // Ka and Kp of the reference implementation. An earlier set, scaled from
        // the paper by 1/500, ran 3x higher on every one of them and rang.
        //
        // Why 3x mattered: with VelocityFilterOwn at the reference's 1.0 the
        // damping term -Ka·qhat' was nearly silent, so so_i collapsed towards
        // Lambda_x·dq_i and the law behaved as a spring of stiffness
        // Kp·Lambda_x damped only by the arm's friction. The filter now passes
        // real velocity again, so -Ka·qhat' does contribute; these gains have
        // not been re-checked against that and Ka may now be raisable.
        //
        //                     Kp·Lambda    wn         f
        //   3x set, local        3.75    19.8 rad/s  3.15 Hz
        //   3x set, remote j2    4.20    20.9 rad/s  3.33 Hz
        //   these, local         1.25    11.4 rad/s  1.82 Hz
        //   these, remote j2     1.40    12.1 rad/s  1.92 Hz
        //
        // Lambda keeps the paper's values: it places the error pole and carries
        // no torque constraint of its own. The sigma loop stays at zero.
        //
        // Kf, from the reference. An earlier comment here claimed Kf = 0 left a
        // divergent mode with static gain (Kp_l*Lambda_l)/(Kp_r*Lambda_r) = 1.25
        // on joints 1 and 3. That was wrong: solving the equilibrium properly,
        // with the local held and the remote at rest, gives
        //
        //   dq_r * (Kp_r*Lambda_r + Kp_l*Lambda_l) = g_l - g_r
        //
        // a SUM in the denominator, not a difference. The fixed point is stable
        // and vanishes when both arms sit in the same pose. Kf is kept because
        // the law calls for it, not because it rescues that mode.
        //
        // dp has no anti-windup, here or in the reference.

        // Ka is set per joint by a stability limit, not by a damping target.
        //
        // With the velocity estimate v = G(s)q, G = lambda^2 s/(s+lambda)^2, the
        // phase of G runs from +90 to -90 degrees. It passes through ZERO at
        // omega = lambda, and there the term -Ka*v is pure negative stiffness.
        // Against the 1/(H s^2) of the joint that closes a loop of gain
        // Ka/(2*H*lambda), so
        //
        //     (Ka + Kp) < 2 * H * lambda
        //
        // is what keeps the arm from limit-cycling on its own velocity feedback.
        // Note (Ka + Kp), not Ka: so_i carries qhat'_i as well. An earlier note
        // here had Ka alone, which understated the load by 4x. With lambda = 30
        // and Ka alone:
        //
        //     joint   H         limit    used (60% of it)
        //       1     0.0084    0.504    0.30
        //       2     0.0090    0.540    0.32
        //       3     0.0045    0.270    0.16
        //
        // Joint 3 carries half the inertia of the other two, so its limit is
        // half theirs -- which is why q3, and only q3, oscillated in every
        // recorded run while q1 and q2 stayed clean.
        //
        // Ka = 0.5 (the reference's) violates it on joint 3 by 1.9x. That was
        // measured: at 0.5 the remote sat in a 6.5 Hz limit cycle of 9 degrees
        // amplitude with its joint-3 torque saturated 77% of the time. The same
        // sequence with different filters put the limit cycle at 25 Hz and then
        // 43 Hz; the frequency moved with the filter, the cause did not.
        //
        // The reference's own Ka = 0.5 does not contradict this: it filters at
        // lambda = 1 through the Levant estimate, a different G with a different
        // limit. The criterion is tied to the estimator, not to the robot alone.
        //
        // Resulting zeta at 2 Hz: local [0.88, 0.91, 0.64], remote [0.98, 0.86,
        // 0.72] -- still well damped, so nothing is given up for this.
        public static readonly BilateralGainSet StartLocal = new(
            Ka:     [0.02, 0.02, 0.02],
            Kp:     [0.06, 0.06, 0.06],
            Kf:     [0.5, 0.5, 0.5],
            Lambda: [20.8, 20.8, 20.8],
            Kbeta:  [0.0, 0.0, 0.0],
            Kgamma: [0.0, 0.0, 0.0]);

        public static readonly BilateralGainSet StartRemote = new(
            Ka:     [0.02, 0.02, 0.02],
            Kp:     [0.06, 0.06, 0.06],
            Kf:     [0.5, 0.5, 0.5],
            Lambda: [16.7, 23.3, 16.7],
            Kbeta:  [0.0, 0.0, 0.0],
            Kgamma: [0.0, 0.0, 0.0]);

        // ── Why Kp is small and Lambda large ────────────────────────────────
        // Kp*Lambda_x is the position stiffness and is UNCHANGED from the
        // reference: 1.25 local, 1.00/1.40/1.00 remote. What changed is how it
        // is split, and that split is not cosmetic:
        //
        //     position stiffness  =  Kp * Lambda_x
        //     velocity feedback   =  Ka + Kp        (so_i carries qhat'_i)
        //
        // so moving weight from Kp into Lambda keeps the tracking and takes the
        // load off the velocity path, which is the loop that was ringing.
        //
        // The discrete closed loop -- ZOH plant, the two Euler poles of
        // DirtyDifferentiator, one tick of delay -- gives |z|max for the worst
        // joint (remote 3), sweeping the split at constant Kp*Lambda = 1.0:
        //
        //     Kp    Lambda   Ka+Kp | lam=50   75    100    150    200
        //     0.50    2.00    0.66 | 1.0219 1.0216 1.0197 1.0134 1.0055
        //     0.25    4.00    0.41 | 1.0098 1.0046 0.9982 0.9952 0.9951
        //     0.12    8.33    0.28 | 0.9988 0.9932 0.9930 0.9930 0.9930
        //
        // At the reference's Kp = 0.5 there is NO usable lambda: the rigid-body
        // loop needs lambda > 250 to settle, and above ~200 the loop starts
        // exciting a structural mode near 45-50 Hz that a rigid-body model does
        // not contain. That was measured twice -- 43 Hz at one pole/lambda 10,
        // 50 Hz at two poles/lambda 300, the latter with the remote joint-3
        // torque saturated 98% of the time and peaking at 18 N-m.
        //
        // At Kp = 0.12 a window opens between the two constraints, and
        // lambda = 100 sits in the middle of it. Predicted |z|max, all six:
        //     local  0.9941 0.9944 0.9914     remote 0.9953 0.9937 0.9931
        //
        // The model is not decoration: at lambda = 30 it predicts the dominant
        // pole at 6.7 Hz, and the two runs at that setting came out at 6.5 and
        // 7.3 Hz.

        // ── The 39-50 Hz mode is NOT in the rigid-body model ─────────────────
        // Every configuration tried so far ended in a limit cycle, and the
        // frequency tracked the estimator rather than the gains: 6.5-7.3 Hz at
        // lambda = 30, 39 Hz at lambda = 100, 43 Hz at one pole, 50 Hz at
        // lambda = 300. The discrete model here -- ZOH plant, the two Euler
        // poles, one tick of delay -- reproduces the first of those (it predicts
        // 6.7 Hz) and none of the others. What sits at 39-50 Hz is the arm
        // itself: structural or transmission flexibility a rigid-body model
        // cannot contain.
        //
        // So the gains answer to TWO criteria at once:
        //
        //   zeta  -- damping of the rigid-body mode, from the discrete model
        //   E39   -- gain of the velocity path at 39 Hz, (Ka+Kp)*|G(j*2pi*39)|,
        //            which is what feeds the mode the model cannot see
        //
        // The previous set (Ka 0.16, Kp 0.12, lambda 100) scored zeta = 0.13 and
        // E39 = 9.8. This one scores 0.35-0.48 and E39 = 1.9: better damping and
        // five times less excitation.
        //
        // How both improve at once: raising lambda cuts the estimate's phase lag,
        // so a MUCH smaller (Ka + Kp) still does real damping -- and it is
        // (Ka + Kp) that sets E39. The earlier sets had it backwards, a large
        // velocity gain fighting a large phase lag. Position stiffness Kp*Lambda
        // stays at the reference's 1.25 / 1.00-1.40-1.00 throughout.
        //
        // Predicted zeta: local 0.39/0.38/0.40, remote 0.46/0.35/0.48.

        // ── Torque estimator, Section 4.1: Kh = Ke = 20I, Kuh = Kue = 0.01I ──
        public static readonly double[] EstimatorK  = [20.0, 20.0, 20.0];
        public static readonly double[] EstimatorKu = [0.01, 0.01, 0.01];

        // ── Differentiator bounds, Assumption 3.1 ────────────────────────────
        // Values of the reference implementation, which is the one that ran the
        // experiments. Do not raise L without re-checking the relay amplitude
        // below.
        //
        // In the discretised Levant differentiator these two do very different
        // jobs, and it is the pairing that matters:
        //
        //   M sets the tracking bandwidth. The mu_k·M terms are the linear part
        //     of the differentiator; they are what follows the signal.
        //   L sets the relay amplitude. The third stage is
        //     qhat''' = -lambda_0·L·sign(s2), so the acceleration estimate
        //     moves by lambda_0·L·T per tick no matter what the input does:
        //         L = 1   -> 0.0022 rad/s^2 per tick
        //         L = 500 -> 1.1    rad/s^2 per tick
        //
        // A run with L = 500, M = 0 was tried on the reasoning that M = 0 is
        // what Assumption 3.1 gives for a constant L. That is true of the
        // assumption and wrong for this implementation: it removes the part
        // that tracks and multiplies the part that chatters by 500. Closed-loop
        // simulation of this exact code puts the acceleration estimate into a
        // ±1.1 rad/s^2 per tick sawtooth at ~25 Hz with the arm at rest, which
        // integrates to ±0.09 rad/s of velocity ripple and, through
        // -(Ka + Kp)·qhat', to ±0.27 N·m of torque against a ±1 N·m limit.
        // Every simulated configuration at L = 500 diverges; every one at
        // L = 1, M = 50 settles.
        //
        // These are also the values DirectEstimation already uses on the same
        // hardware in experiment 1.
        public const double DifferentiatorL = 1.0;
        public const double DifferentiatorM = 50.0;

        // ── Low-pass on the differentiator outputs, rad/s ────────────────────
        // The filter has to do two jobs at once and they pull in opposite
        // directions: kill the lightly damped mode the mu_k*M terms leave in
        // qhat' (23 Hz, measured at about 9x the true joint velocity), and
        // still pass enough real velocity for -Ka*qhat' to dissipate energy.
        //
        // The reference's 1.0 / 0.7 does the first job and abandons the second.
        // What matters for damping is not the magnitude that survives but the
        // part IN PHASE with the velocity, |G|*cos(phase), and at 1 rad/s the
        // filter lags a 2 Hz signal by 85 degrees:
        //
        //   lambda   |G| at 2 Hz   phase   dissipative   vs 1.0   23 Hz residue
        //      1        0.08        85       0.006         1x        0.06
        //      5        0.37        68       0.136        22x        0.30
        //     10        0.62        52       0.386        62x        0.60
        //     20        0.85        32       0.716       114x        1.20
        //
        // At 1.0 the effective damping is under 1% of nominal — nothing in the
        // loop dissipates, which is why the pair would sit in a background
        // oscillation and pump itself up as soon as it was moved: the force
        // channel injects energy with velocity and nothing took it out.
        //
        // 10 is picked so the 23 Hz residue (0.60) stays just under the useful
        // signal (0.62) while the dissipative part goes up 62-fold. Above 20
        // the residue overtakes the signal again. There is room for this choice
        // only because the resonance (23 Hz) and hand motion (1-3 Hz) are a
        // decade apart; the corner belongs between them, not below both.
        //
        // The residue that does get through sits at 23 Hz, where the arm's own
        // inertia H*w^2 is ~135x what it is at 2 Hz, so the same torque ripple
        // moves the arm that much less. It is mostly not mechanically visible.
        public const double VelocityFilterOwn  = 10.0;  // on qhat'_i and qhat''_i
        public const double VelocityFilterPeer = 7.0;   // on qhat'_di

        // ── Velocity estimator ───────────────────────────────────────────────
        // A 500 Hz recording settled what the arms were doing. On joint 3 the
        // Levant estimate sat ENTIRELY at 25 Hz -- the resonance of its own
        // mu_k*M terms -- at 2800 deg/s rms against a true joint speed of a few
        // tens. Through -(Ka + Kp) that put 2.1 N-m rms on a +-1 N-m actuator
        // and kept joint 3 saturated 73% of the time, which is the slamming.
        //
        //   estimator, joint 3        rms        torque it commands
        //   Levant, raw            2798 deg/s          --
        //   Levant + filter 10      183 deg/s      2.08 N-m   saturates
        //   filtered difference      30 deg/s      0.34 N-m   fits
        //
        // The last row is the same dirty derivative JointController computes and
        // the single-robot experiment's PID runs on, on this same hardware.
        //
        // This departs from Section 3.1.1, which specifies Levant. The reason it
        // is defensible: at T = 2 ms the exactness that justifies Levant is
        // already gone -- the relay term is swamped by the linear mu_k*M part,
        // and that part is a zeta = 0.21 resonator sitting inside the torque
        // loop. Set UseLevantVelocity = true to switch back and compare.
        // The estimator is TWO cascaded poles at DirtyLambda, not one. A run with
        // a single pole at 10 rad/s cut the joint-3 torque from 2.08 to 0.54 N-m
        // rms and its saturation from 73% to 2%, but the limit cycle moved from
        // 25 Hz to 43 Hz and felt worse. The reason is that a single pole leaves
        // |v/q| flat above its corner, so the velocity term keeps full authority
        // where it lags 90 degrees and acts as negative stiffness:
        //
        //                          dissipative 2 Hz   gain at 43 Hz
        //   1 pole,  lambda = 10        0.386             9.99
        //   2 poles, lambda = 30        0.595             3.26
        //
        // Two poles at 30 is better on both axes at once, which a single pole
        // cannot be at any lambda -- its best trade sits at lambda ~ 12.
        // DirtyLambda must sit WELL ABOVE the closed-loop bandwidth, not near it.
        // That is the mistake behind every limit cycle in this log: with
        // Kp*Lambda_x ~ 1.0-1.4 against H ~ 0.005-0.009 the loop runs at
        // wn = sqrt(Kp*Lambda/H) = 15-25 rad/s, and the filter was set to 30 --
        // essentially AT the bandwidth, where it contributes ~90 degrees of lag.
        //
        // The velocity estimate enters the law with coefficient (Ka + Kp), not
        // Ka alone, because so_i = qhat'_i - qhat'_di + Lambda_x*dq_i carries
        // qhat'_i too and Kp multiplies all of it. Kp dominates: 0.5 against
        // 0.16. The stability boundary is
        //
        //     (Ka + Kp) < 2 * H * lambda
        //
        // and at lambda = 30 the remote joint 3 violated it 2.4x. Solving the
        // closed-loop quartic per joint, the worst damping comes out:
        //
        //     lambda      local j1/j2/j3        remote j1/j2/j3
        //        30     -0.04 -0.03 -0.09     -0.09 -0.11 -0.15   all unstable
        //       100      0.27  0.28  0.17      0.17  0.12  0.06
        //       200      0.55  0.57  0.41      0.40  0.33  0.25
        //       300      0.76  0.78  0.59      0.57  0.49  0.39
        //
        // Nothing argues for keeping it low: measured on a quiet stretch of a
        // real recording, the estimator noise at lambda = 300 is 1.9 deg/s rms,
        // which through (Ka + Kp) is 0.022 N-m against a 1 N-m limit.
        //
        // lambda*T = 0.6 here, so the explicit Euler pole sits at 0.4 -- well
        // inside the unit circle. Do not go past lambda*T = 1.
        public const bool   UseLevantVelocity = false;
        public const double DirtyLambda       = 80.0;   // rad/s, both poles

        // ── Force-channel gain (diagnostic, not part of the law) ─────────────
        // Scales tau_di where it enters (3.1)/(3.2). The law says 1.0.
        //
        // Set to 0.0 for the current run on purpose: with the force channel
        // silent the pair is position-only teleoperation, which splits the
        // system in half. Everything that has been tuned so far — the
        // differentiator, the filter, Ka, Kp — lives in the position loop; the
        // reflected torque tau_di is the other half, injected at unity gain and
        // carrying the estimator's own lag (1/K = 50 ms, about 72 degrees at the
        // 4 Hz that shows up in the recordings). Delayed force feedback at unity
        // gain is the textbook way to put energy INTO a haptic loop, and energy
        // that grows with velocity is what the arms have been doing.
        //
        // If the oscillation survives at 0.0 it is entirely in the position loop
        // and the channel is exonerated. If it goes away, the channel is the
        // source and this factor becomes the transparency/stability knob:
        // raise it 0 -> 0.3 -> 0.6 -> 1.0 and stop where it starts to ring.
        public const double ForceChannelGain = 0.0;
    }
}
