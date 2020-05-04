namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     A simple title field, used by Notebooks as a category for their style
    /// </summary>
    public class TitleField : CategoryField
    {
        #region Fields
        private bool isTitle = false;
        private string shortTitle = string.Empty;
        #endregion

        #region Properties
        public bool IsTitle
        {
            get { return isTitle; }
        }
        #endregion

        #region Constructors
        public TitleField() : base() { }

        public TitleField(string text) : base(text) { }
        public TitleField(bool isTitle) : this(string.Empty, isTitle) { }

        public TitleField(string longTitle, bool isTitle) : base(longTitle)
        {
            this.isTitle = isTitle;
        }

        public TitleField(string shortTitle, string longTitle, bool isTitle) : base(longTitle)
        {
            this.isTitle = isTitle;
            this.shortTitle = shortTitle;
        }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            string guiText = context.UseShortNames && !string.IsNullOrEmpty(shortTitle) ? shortTitle : text;
            if (isTitle && ShowContent && !context.UseShortNames)
            {
                guiText = $"> {guiText}";
            }

            if (GUILayout.Button(guiText, GetStyle(context)))
            {
                ShowContent = !ShowContent;
            }
        }
        #endregion

        #region Class Methods
        public float GetSize(DebugMenuContext context)
        {
            string guiText = context.UseShortNames && !string.IsNullOrEmpty(shortTitle) ? shortTitle : text;
            GUIStyle style = GetStyle(context);
            Vector2 size = style.CalcSize(new GUIContent("0"));
            return size.x * guiText.Length;
        }

        private GUIStyle GetStyle(DebugMenuContext context)
        {
            return isTitle ? context.Title(ShowContent) : context.Page;
        }
        #endregion
    }
}
