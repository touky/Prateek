namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using UnityEditor;
    using UnityEditor.VersionControl;
    using UnityEngine;

    public class ShaderFinderParameter
    {
        #region Fields
        protected string title;
        protected SortBy sort;
        protected string undoTag;
        #endregion

        #region Properties
        protected virtual string Title
        {
            get { return title; }
        }

        public virtual SortBy Sort
        {
            get { return sort; }
        }

        public string UndoTag
        {
            get { return undoTag; }
        }
        #endregion

        #region Constructors
        public ShaderFinderParameter(string title = "####", SortBy sortBy = SortBy.Nothing)
        {
            this.title = title;
            sort = sortBy;
            undoTag = GetType().ToString();
        }
        #endregion

        #region Class Methods
        public virtual float GetTitleWidth(GUIStyle titleStyle)
        {
            return titleStyle.CalcSize(new GUIContent(Title)).x;
        }

        public virtual SortBy OnTitleGUI(Rect rect, GUIStyle titleBoxStyle, GUIStyle titleTextStyle)
        {
            bool sendSort = false;
            if (Sort != SortBy.Nothing)
            {
                if (GUI.Button(rect, GUIContent.none, titleBoxStyle))
                {
                    sendSort = true;
                }
            }
            else
            {
                GUI.Box(rect, GUIContent.none, titleBoxStyle);
            }

            EditorGUI.LabelField(rect, Title, titleTextStyle);

            return sendSort ? Sort : SortBy.Nothing;
        }

        public virtual EditorAction OnContentGUI(Rect rect, SearchResult result)
        {
            return EditorAction.Nothing;
        }

        public void DoAction(EditorAction action, SearchResult result)
        {
            if (action.Has(EditorAction.RecordUndo))
            {
                Undo.RecordObject(result.instance, result.material + UndoTag);
            }

            if (action.Has(EditorAction.DoAction))
            {
                InternalDoAction(result);
            }

            if (action.Has(EditorAction.Checkout))
            {
                Provider.Checkout(result.instance, CheckoutMode.Asset);
            }

            if (action.Has(EditorAction.SetDirty))
            {
                EditorUtility.SetDirty(result.instance);
            }

            if (action.Has(EditorAction.Select))
            {
                Selection.activeObject = result.instance;
                EditorGUIUtility.PingObject(result.instance);
            }
        }

        protected virtual void InternalDoAction(SearchResult result) { }
        #endregion
    }
}
