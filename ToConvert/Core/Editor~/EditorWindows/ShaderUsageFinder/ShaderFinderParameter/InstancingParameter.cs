namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEditor;
    using UnityEngine;

    public class InstancingParameter : ToggleParameter
    {
        #region Constructors
        public InstancingParameter(string title, GUIStyle[] buttonStyle)
            : base(title, buttonStyle) { }
        #endregion

        #region Class Methods
        public override EditorAction OnContentGUI(Rect rect, SearchResult result)
        {
            EditorAction action = EditorAction.Nothing;

            string content = "##";
            GUIStyle style = buttonStyle[2];
            if (result.instance != null)
            {
                content = result.instance.enableInstancing ? "ON" : "OFF";
                style = result.instance.enableInstancing ? buttonStyle[1] : buttonStyle[0];
            }

            using (new EditorGUI.DisabledGroupScope(result.instance == null))
            {
                if (GUI.Button(rect, content, style))
                {
                    action |= EditorAction.DoAction | EditorAction.Select | EditorAction.Checkout | EditorAction.RecordUndo | EditorAction.SetDirty;
                }
            }

            return action;
        }

        protected override void InternalDoAction(SearchResult result)
        {
            Material material = result.instance;
            material.enableInstancing = !material.enableInstancing;
        }
        #endregion
    }
}
