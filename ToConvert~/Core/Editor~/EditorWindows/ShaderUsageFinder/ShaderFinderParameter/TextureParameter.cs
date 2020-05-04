namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEditor;
    using UnityEngine;

    public class TextureParameter : PropertyParameter
    {
        #region Constructors
        public TextureParameter(string title, GUIStyle[] buttonStyle, string[] properties)
            : base(title, buttonStyle, properties, null) { }
        #endregion

        #region Class Methods
        public override EditorAction OnContentGUI(Rect rect, SearchResult result)
        {
            bool hasProperty = false;
            string foundProperty = string.Empty;
            foreach (string property in properties)
            {
                if (result.instance.HasProperty(property))
                {
                    hasProperty = true;
                    foundProperty = property;
                    break;
                }
            }

            EditorAction action = EditorAction.Nothing;
            if (hasProperty)
            {
                Texture tex = result.instance.GetTexture(foundProperty);
                if (tex != null)
                {
                    Rect texRect = RectHelper.TruncateX(ref rect, rect.height);
                    if (GUI.Button(texRect, GUIContent.none))
                    {
                        action = EditorAction.DoAction;
                    }

                    EditorGUI.DrawPreviewTexture(texRect, tex);
                    EditorGUI.LabelField(rect, tex.name);
                }
            }

            return action;
        }

        protected override void InternalDoAction(SearchResult result)
        {
            bool hasProperty = false;
            string foundProperty = string.Empty;
            foreach (string property in properties)
            {
                if (result.instance.HasProperty(property))
                {
                    hasProperty = true;
                    foundProperty = property;
                    break;
                }
            }

            if (hasProperty)
            {
                Texture tex = result.instance.GetTexture(foundProperty);
                if (tex != null)
                {
                    Selection.activeObject = tex;
                    EditorGUIUtility.PingObject(tex);
                }
            }
        }
        #endregion
    }
}
