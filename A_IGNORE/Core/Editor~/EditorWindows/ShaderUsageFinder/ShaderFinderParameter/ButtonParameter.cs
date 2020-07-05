namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEngine;

    public class ButtonParameter : ShaderFinderParameter
    {
        #region Fields
        protected GUIContent content;
        #endregion

        #region Constructors
        public ButtonParameter(GUIContent content)
        {
            this.content = content;
        }
        #endregion

        #region Class Methods
        public override EditorAction OnContentGUI(Rect rect, SearchResult result)
        {
            return GUI.Button(rect, content) ? EditorAction.DoAction : EditorAction.Nothing;
        }
        #endregion
    }
}
