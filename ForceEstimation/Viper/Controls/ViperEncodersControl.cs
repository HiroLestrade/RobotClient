using System.ComponentModel;

namespace ForceEstimation
{
    /// <summary>
    /// Read-only joint and Cartesian readout for the Viper X-300S.
    ///
    /// Six joints, not three. Joints 2 and 3 are each driven by two motors
    /// (IDs 2+3 and 4+5), but the second of each pair shadows the first through
    /// its Secondary ID — mechanically and logically it is one joint, so it gets
    /// one box, not two.
    ///
    /// Same row geometry as <see cref="ViperHomeControl"/> and
    /// <see cref="ViperDestinationControl"/>, so the three columns line up.
    ///
    /// <para>Both sets of boxes are fed by <see cref="ViperControls"/>, which
    /// converts out of the motors' convention first: the joints show real
    /// degrees and the Cartesian boxes centimetres from the forward
    /// kinematics. A dash means there is no reading, never a zero that would
    /// read as a measurement.</para>
    /// </summary>
    public partial class ViperEncodersControl : UserControl
    {
        /// <summary>Shown wherever there is no reading to show.</summary>
        private const string NoData = "—";

        public event EventHandler? ReadClicked;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ReadButtonText
        {
            get => bttnReadEncoders.Text;
            set => bttnReadEncoders.Text = value;
        }

        public ViperEncodersControl() => InitializeComponent();

        private Label[] JointBoxes       => [tbxQ1, tbxQ2, tbxQ3, tbxQ4, tbxQ5, tbxQ6];
        private Label[] CartesianBoxes   => [tbxX, tbxY, tbxZ];
        private Label[] OrientationBoxes => [tbxVx, tbxVy, tbxVz, tbxPsi];

        /// <summary>
        /// Joint angles in <b>real degrees</b>, 0 at centre. Cartesian position in
        /// cm. Orientation as the unit tool direction followed by the spin ψ in
        /// degrees — four numbers, the same four the Home and Destination boxes
        /// take, so a pose read here can be typed straight into them. Any of them
        /// <c>null</c> when there is nothing to show.
        /// </summary>
        public void UpdateDisplay(double[] qDeg, double[]? pCm, double[]? dirPsi = null)
        {
            Label[] joints = JointBoxes;
            for (int i = 0; i < joints.Length && i < qDeg.Length; i++)
                joints[i].Text = $"{qDeg[i]:F2}";

            Fill(CartesianBoxes,   pCm);
            Fill(OrientationBoxes, dirPsi);
        }

        /// <summary>
        /// Writes three values, or dashes when there are none. Guarded against
        /// repainting a label that already says the same thing, because this runs
        /// at 50 Hz.
        /// </summary>
        private static void Fill(Label[] boxes, double[]? values)
        {
            if (values == null || values.Length == 0)
            {
                foreach (Label l in boxes)
                    if (l.Text != NoData) l.Text = NoData;
                return;
            }

            for (int i = 0; i < boxes.Length && i < values.Length; i++)
            {
                string text = $"{values[i]:F2}";
                if (boxes[i].Text != text) boxes[i].Text = text;
            }
        }

        /// <summary>
        /// Blanks every box. Used when the arm is not connected — zeros would
        /// read as "the arm is at the origin", which is exactly the confusion
        /// ViperCore avoids by invalidating a failed read instead of returning
        /// zeros.
        /// </summary>
        public void ClearDisplay()
        {
            foreach (Label l in JointBoxes)       l.Text = NoData;
            foreach (Label l in CartesianBoxes)   l.Text = NoData;
            foreach (Label l in OrientationBoxes) l.Text = NoData;
        }

        private void bttnReadEncoders_Click(object sender, EventArgs e) =>
            ReadClicked?.Invoke(this, EventArgs.Empty);
    }
}
