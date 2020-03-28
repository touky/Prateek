namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using System.Collections.Generic;
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEditor;
    using UnityEngine;

    public class PropertyParameter : ToggleParameter
    {
        #region Fields
        protected List<string> properties = new List<string>();
        protected List<string> keywords = new List<string>();
        #endregion

        #region Constructors
        public PropertyParameter(string title, GUIStyle[] buttonStyle, string[] properties, string[] keywords)
            : base(title, buttonStyle)
        {
            if (properties != null)
            {
                this.properties.AddRange(properties);
            }

            if (keywords != null)
            {
                this.keywords.AddRange(keywords);
            }
        }
        #endregion

        #region Class Methods
        public override EditorAction OnContentGUI(Rect rect, SearchResult result)
        {
            EditorAction action = EditorAction.Nothing;

            bool hasProperty = false;
            foreach (string property in properties)
            {
                if (result.instance.HasProperty(property))
                {
                    hasProperty = true;
                    break;
                }
            }

            bool hasKeywords = true;
            foreach (string keyword in keywords)
            {
                if (!result.instance.IsKeywordEnabled(keyword))
                {
                    hasKeywords = false;
                    break;
                }
            }

            using (new EditorGUI.DisabledGroupScope(!hasProperty))
            {
                if (GUI.Button(rect, !hasProperty ? "OFF" : !hasKeywords ? "OFF" : "ON", !hasProperty ? buttonStyle[2] : hasKeywords ? buttonStyle[1] : buttonStyle[0]))
                {
                    action |= EditorAction.DoAction | EditorAction.Select | EditorAction.Checkout | EditorAction.RecordUndo | EditorAction.SetDirty;
                }
            }

            return action;
        }

        protected override void InternalDoAction(SearchResult result)
        {
            Material material = result.instance;
            bool hasKeywords = true;
            foreach (string keyword in keywords)
            {
                if (!material.IsKeywordEnabled(keyword))
                {
                    hasKeywords = false;
                    break;
                }
            }

            foreach (string keyword in keywords)
            {
                if (hasKeywords)
                {
                    material.DisableKeyword(keyword);
                }
                else
                {
                    material.EnableKeyword(keyword);
                }
            }
        }
        #endregion
    }
}
