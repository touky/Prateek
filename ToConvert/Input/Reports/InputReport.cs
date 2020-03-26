namespace Assets.Prateek.ToConvert.Input.Reports
{
    using Assets.Prateek.ToConvert.Input.Enums;
    using UnityEngine;

    public class InputReport : IInputReport
    {
        #region Fields
        private TouchStatus status = TouchStatus.None;
        private Enums.TouchType previousTouchType;
        private Enums.TouchType currentTouchType;
        private InputRaycastHits inputRaycastHits;
        public InputHoldReport hold = new InputHoldReport();
        public InputSingleTouchReport singleTouch = new InputSingleTouchReport();
        public InputMultiTouchReport multiTouch = new InputMultiTouchReport();
        #endregion

        #region Properties
        public InputCursorReport Cursor
        {
            get
            {
                return currentTouchType == Enums.TouchType.SingleTouch
                    ? singleTouch.Cursor
                    : multiTouch.Cursor;
            }
        }

        public TouchStatus Status
        {
            get { return status; }
            internal set { status = value; }
        }

        public InputRaycastHits InputRaycastHits
        {
            get { return inputRaycastHits; }
            internal set { inputRaycastHits = value; }
        }

        private IInputReport SingleReport
        {
            get { return singleTouch; }
        }

        private IInputReport HoldReport
        {
            get { return hold; }
        }

        internal Enums.TouchType CurrentTouchType
        {
            get { return currentTouchType; }
            set
            {
                previousTouchType = currentTouchType;
                currentTouchType = value;
            }
        }
        #endregion

        #region Unity Methods
        public void Update(Vector2 position0, Vector2 position1)
        {
            if (status == TouchStatus.Begin || currentTouchType != previousTouchType)
            {
                SingleReport.Reset(position0);
                multiTouch.Reset(position0, position1);
            }

            if (currentTouchType == Enums.TouchType.SingleTouch)
            {
                singleTouch.Update(position0);
            }
            else
            {
                multiTouch.Update(position0, position1);
            }

            switch (status)
            {
                case TouchStatus.Begin:
                {
                    hold.Start(Cursor.Current);
                    break;
                }
                case TouchStatus.Active:
                {
                    hold.Update(Cursor.Current);
                    break;
                }
                case TouchStatus.End:
                {
                    hold.End(Cursor.Current);
                    break;
                }
            }
        }
        #endregion

        #region IInputReport Members
        public void Reset(Vector2 position)
        {
            status = TouchStatus.None;
            HoldReport.Reset(position);
            SingleReport.Reset(position);
            multiTouch.Reset(position, position);
        }
        #endregion
    }
}
