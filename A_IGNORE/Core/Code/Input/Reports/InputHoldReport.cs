namespace Mayfair.Core.Code.Input.Reports
{
    using System;
    using UnityEngine;

    public class InputHoldReport : IInputReport
    {
        #region Fields
        private HoldState holdState;
        internal Vector2 startPosition;
        internal float startTime;
        #endregion

        #region Properties
        public bool IsHolding
        {
            get { return holdState.HasFlag(HoldState.Holding); }
        }

        public float Progression
        {
            get { return IsHolding ? Mathf.Clamp01((Time.time - startTime) / InputDaemon.HOLD_LONG_TAP) : 0; }
        }

        public bool HasShortTapped
        {
            get { return holdState.HasFlag(HoldState.ShortTap); }
        }

        public bool HasLongTapped
        {
            get { return holdState.HasFlag(HoldState.LongTap); }
        }
        #endregion

        #region Unity Methods
        internal void Start(Vector2 position)
        {
            holdState |= HoldState.Holding;
            startPosition = position;
            startTime = Time.time;
        }

        internal void Update(Vector2 position)
        {
            // Check how long finger has been pressed down for
            if (Vector2.Distance(position, startPosition) > InputDaemon.HOLD_THRESHOLD)
            {
                holdState &= ~HoldState.Holding;
            }

            if (IsHolding && Time.time >= startTime + InputDaemon.HOLD_LONG_TAP)
            {
                holdState |= HoldState.LongTap;
            }
        }
        #endregion

        #region Class Methods
        internal void End(Vector2 position)
        {
            Update(position);

            if (IsHolding && Time.time <= startTime + InputDaemon.HOLD_SHORT_TAP)
            {
                holdState |= HoldState.ShortTap;
            }
        }
        #endregion

        #region IInputReport Members
        void IInputReport.Reset(Vector2 position)
        {
            holdState = HoldState.None;
            startPosition = position;
            startTime = 0;
        }
        #endregion

        #region HoldState enum
        [Flags]
        private enum HoldState
        {
            None,

            Holding = 1 << 0,
            ShortTap = 1 << 1,
            LongTap = 1 << 2
        }
        #endregion
    }
}
