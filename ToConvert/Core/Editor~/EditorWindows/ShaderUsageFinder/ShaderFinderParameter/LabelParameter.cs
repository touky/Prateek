namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEditor;
    using UnityEngine;

    public class LabelParameter : ShaderFinderParameter
    {
        #region Class Methods
        public override EditorAction OnContentGUI(Rect rect, SearchResult result)
        {
            EditorGUI.LabelField(rect, GetLabel(result));
            return EditorAction.Nothing;
        }

        public virtual string GetLabel(SearchResult result)
        {
            return "####";
        }
        #endregion
    }
}
