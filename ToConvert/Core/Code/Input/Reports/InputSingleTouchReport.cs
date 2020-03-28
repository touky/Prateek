namespace Mayfair.Core.Code.Input.Reports
{
    using UnityEngine;

    public class InputSingleTouchReport : IInputReport
    {
        #region Fields
        private InputCursorReport cursor = new InputCursorReport();
        #endregion

        #region Properties
        internal InputCursorReport Cursor
        {
            get { return cursor; }
        }
        #endregion

        #region Unity Methods
        public void Update(Vector2 position)
        {
            cursor.Update(position);
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
