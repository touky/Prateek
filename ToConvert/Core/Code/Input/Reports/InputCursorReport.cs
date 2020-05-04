namespace Mayfair.Core.Code.Input.Reports
{
    using UnityEngine;

    public class InputCursorReport : IInputReport
    {
        #region Fields
        private Vector2 previous;
        private Vector2 current;
        #endregion

        #region Properties
        public Vector2 Previous
        {
            get { return previous; }
        }

        public Vector2 Current
        {
            get { return current; }
        }

        public Vector2 Delta
        {
            get { return current - previous; }
        }
        #endregion

        #region Unity Methods
        internal void Update(Vector2 position)
        {
            previous = current;
            current = position;
        }
        #endregion

        #region IInputReport Members
        void IInputReport.Reset(Vector2 position)
        {
            current = position;
            previous = current;
        }
        #endregion
    }
}
