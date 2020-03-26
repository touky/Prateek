namespace Assets.Prateek.ToConvert.Input.Reports
{
    using UnityEngine;

    public class InputMultiTouchReport : IInputReport
    {
        #region Fields
        private bool active = false;
        private bool tiltingBlocked = false;
        private float pinchRatio = 0;
        private float twistAngle = 0;

        private InputCursorReport cursorA = new InputCursorReport();
        private InputCursorReport cursorB = new InputCursorReport();

        private InputCursorReport cursor = new InputCursorReport();
        #endregion

        #region Properties
        public bool Active
        {
            get { return active; }
        }

        public bool TiltingBlocked
        {
            get { return tiltingBlocked; }
        }

        public float PinchRatio
        {
            get { return pinchRatio; }
        }

        public float TwistAngle
        {
            get { return twistAngle; }
        }

        internal InputCursorReport Cursor
        {
            get { return cursor; }
        }

        public InputCursorReport CursorA
        {
            get { return cursorA; }
        }

        public InputCursorReport CursorB
        {
            get { return cursorB; }
        }
        #endregion

        #region Unity Methods
        internal void Update(Vector2 positionA, Vector2 positionB)
        {
            active = true;

            cursorA.Update(positionA);
            cursorB.Update(positionB);

            var cA = cursorA;
            var cB = cursorB;

            var oldToNewA = cA.Current - cA.Previous;
            var oldToNewB = cB.Current - cB.Previous;
            var oldAtoB   = cB.Previous - cA.Previous;
            var newAtoB   = cB.Current - cA.Current;
            var distBtoD  = newAtoB.magnitude;
            var distAtoC  = oldAtoB.magnitude;

            Vector2 oldAvg;
            Vector2 newAvg;
            GetAverage(out oldAvg, out newAvg);

            //Calculate movement delta between two fingers
            var distA = oldToNewA.magnitude;
            var distB = oldToNewB.magnitude;

            //calculate movement center of mass
            var centerOfMass = (oldAvg - newAvg).magnitude;

            //calculate pinch
            var pinchDifference = distBtoD - distAtoC;
            var pinchRatio      = pinchDifference / Screen.height;

            //calculate twist
            var vectorDelta = Vector3.Cross(oldAtoB, newAtoB).normalized;
            var twistAngle  = Vector2.Angle(newAtoB, oldAtoB) * vectorDelta.z;

            //Checking if pinch ratio is below a certain threshold
            var isRotating = Mathf.Abs(twistAngle) > 0.5f && (centerOfMass < distA * 0.9f || centerOfMass < distB * 0.9f);

            //Add this && !isRotating; below to make actions happen one at a time
            var isPinching = Mathf.Abs(pinchDifference) > Screen.height * InputService.PINCH_MIN_THRESHOLD;

            //Disable tilting motion if one finger isn't moving
            tiltingBlocked = oldToNewA.magnitude <= 0.1f || oldToNewB.magnitude <= 0.1f;

            //Differentiate between pinch, twist and two finger dragging
            this.pinchRatio = isPinching ? pinchRatio : 0;

            if (isRotating)
            {
                this.twistAngle = twistAngle;

                IInputReport report = cursor;
                report.Reset(newAvg);
            }
            else
            {
                this.twistAngle = 0;

                cursor.Update(newAvg);
            }
        }
        #endregion

        #region Class Methods
        private void GetAverage(out Vector2 oldAvg, out Vector2 newAvg)
        {
            oldAvg = (cursorA.Previous + cursorB.Previous) * 0.5f;
            newAvg = (cursorA.Current + cursorB.Current) * 0.5f;
        }

        internal void Reset(Vector2 position0, Vector2 position1)
        {
            IInputReport cA = cursorA;
            IInputReport cB = cursorB;

            cA.Reset(position0);
            cB.Reset(position1);

            Vector2 oldAvg;
            Vector2 newAvg;
            GetAverage(out oldAvg, out newAvg);

            IInputReport report = this;
            report.Reset(newAvg);

            active = false;

            pinchRatio = 0;
            twistAngle = 0;
        }
        #endregion

        #region IInputReport Members
        void IInputReport.Reset(Vector2 position)
        {
            IInputReport report = cursor;
            report.Reset(position);
        }
        #endregion
    }
}
