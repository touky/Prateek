namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEngine;

    public class SelectParameter : ButtonParameter
    {
        #region Fields
        private readonly float maxWidth = -60;
        #endregion

        #region Constructors
        public SelectParameter(GUIContent content, float maxWidth = -60) : base(content)
        {
            this.maxWidth = maxWidth;
        }
        #endregion

        #region Class Methods
        public override float GetTitleWidth(GUIStyle titleStyle)
        {
            return maxWidth;
        }

        public override EditorAction OnContentGUI(Rect rect, SearchResult result)
        {
            EditorAction action = base.OnContentGUI(rect, result);
            if (action.Has(EditorAction.DoAction))
            {
                return EditorAction.Select;
            }

            return EditorAction.Nothing;
        }
        #endregion
    }
}
