using System.Runtime.InteropServices;

namespace Teleoperation
{
    internal static class GeomagicModel
    {
        [DllImport("GeomagicCore.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void GeomagicModel_ForwardKinematics(
            [In]  double[] q,
            [Out] double[] p);

        /// <summary>
        /// Forward kinematics: q[3] (rad) -> Cartesian position p[3] (m).
        /// </summary>
        public static double[] ForwardKinematics(double[] q)
        {
            double[] p = new double[3];
            GeomagicModel_ForwardKinematics(q, p);
            return p;
        }
    }
}
