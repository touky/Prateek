namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using UnityEngine;

    public class ToggleParameter : ShaderFinderParameter
    {
        #region Fields
        protected GUIStyle[] buttonStyle;
        #endregion

        #region Constructors
        public ToggleParameter(string title, GUIStyle[] buttonStyle)
            : base(title)
        {
            this.buttonStyle = buttonStyle;
        }
        #endregion
    }
}
