namespace Mayfair.Core.Code.GUIExt
{
    using UnityEngine;

    public class ColorScope : GUI.Scope
    {
        #region Fields
        private Color oldColor;
        #endregion

        #region Properties
        public Color NewColor { get; }
        #endregion

        #region Constructors
        public ColorScope(Color color)
        {
            this.oldColor = GUI.color;
            NewColor = color;
            GUI.color = color;
        }
        #endregion

        #region Methods
        protected override void CloseScope()
        {
            GUI.color = this.oldColor;
        }
        #endregion
    }
}