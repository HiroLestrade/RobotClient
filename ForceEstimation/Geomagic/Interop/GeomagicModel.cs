using System.Runtime.InteropServices;

namespace ForceEstimation
{
    internal static class GeomagicModel
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicModel_ForwardKinematics(
            [In]  double[] q,
            [Out] double[] p);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GeomagicModel_InverseKinematics(
            [In]  double[] p,
            [Out] double[] q);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicModel_GetJacobian(
            [In]  double[] q,
            [Out] double[] Jm);

        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicModel_GetRotation(
            [In]  double[] q,
            [Out] double[] R);

        /// <summary>
        /// Forward kinematics: q[3] (rad) -> Cartesian position p[3] (m).
        /// </summary>
        public static double[] ForwardKinematics(double[] q)
        {
            double[] p = new double[3];
            GeomagicModel_ForwardKinematics(q, p);
            return p;
        }

        /// <summary>
        /// Inverse kinematics (elbow up): Cartesian position p[3] (m) -> q[3] (rad).
        /// Returns null if the point is outside the workspace.
        /// </summary>
        public static double[]? InverseKinematics(double[] p)
        {
            double[] q = new double[3];
            return GeomagicModel_InverseKinematics(p, q) != 0 ? q : null;
        }

        /// <summary>
        /// Returns the 3×3 geometric Jacobian at q (rad), stored row-major in a flat
        /// double[9] array: element [i,j] = Jm[i*3+j].
        /// </summary>
        public static double[] GetJacobian(double[] q)
        {
            double[] Jm = new double[9];
            GeomagicModel_GetJacobian(q, Jm);
            return Jm;
        }

        /// <summary>
        /// Returns the 3×3 rotation matrix R30(q) from the base frame (0) to the
        /// end-effector frame, stored row-major in a flat double[9]: element
        /// [i,j] = R[i*3+j].
        /// </summary>
        public static double[] GetRotation(double[] q)
        {
            double[] R = new double[9];
            GeomagicModel_GetRotation(q, R);
            return R;
        }
    }
}
