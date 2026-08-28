namespace ForceEstimation
{
    /// <summary>
    /// Conditions a raw ATI force-sensor reading and expresses it in the robot's
    /// base frame: F0 = R30 · Rs · Fs.
    ///   Fs  : raw reading, in the sensor's own frame (ATIForceSensor_Read).
    ///   Rs  : fixed rotation from the sensor frame to the end-effector frame
    ///         (mounting orientation; see ATIForceSensor's README "Rotación de
    ///         Rs a Rf"). Folded into the closed form below rather than applied
    ///         as an explicit matrix product.
    ///   R30 : rotation from the base frame to the end-effector frame, which
    ///         varies with the current joint angles (GeomagicModel.GetRotation).
    /// </summary>
    internal static class ForceSensorFrame
    {
        private static readonly double Sin45 = Math.Sin(Math.PI / 4.0);

        /// <summary>Fs (sensor frame) → F03 (end-effector frame), Rs folded in.</summary>
        private static double[] ToEndEffectorFrame(double[] fs)
        {
            double fx = fs[0], fy = fs[1], fz = fs[2];
            return
            [
                fz,
                Sin45 * (fy - fx),
                -Sin45 * (fx + fy),
            ];
        }

        /// <summary>Fs (sensor frame) → F0 (base frame) at joint angles q.</summary>
        public static double[] ToBaseFrame(double[] q, double[] fs)
        {
            double[] f03 = ToEndEffectorFrame(fs);
            double[] r30 = GeomagicModel.GetRotation(q); // row-major 3×3

            return
            [
                r30[0] * f03[0] + r30[1] * f03[1] + r30[2] * f03[2],
                r30[3] * f03[0] + r30[4] * f03[1] + r30[5] * f03[2],
                r30[6] * f03[0] + r30[7] * f03[1] + r30[8] * f03[2],
            ];
        }
    }
}
