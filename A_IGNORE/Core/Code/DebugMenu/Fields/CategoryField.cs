namespace Mayfair.Core.Code.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     A toggleable category field, can be queried for show/hide behaviour
    /// </summary>
    public class CategoryField : StringField
    {
        private static readonly string CATEGORY_PREFS = $".showContent.";

        #region Fields
        internal DebugToggleStatus showContent = DebugToggleStatus.None;
        #endregion

        #region Properties
        public bool ShowContent
        {
            get { return DebugMenuDaemon.Get(showContent, $"{GetType().Name}{CATEGORY_PREFS}{text}", false); }
            set { DebugMenuDaemon.Set(ref showContent, $"{GetType().Name}{CATEGORY_PREFS}{text}", ShowContent, value); }
        }
        #endregion

        #region Constructors
        public CategoryField() : base() { }
        public CategoryField(string text) : base(text) { }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(DebugMenuContext context)
        {
            if (GUILayout.Button(text, context.Category(Indent)))
            {
                ShowContent = !ShowContent;
            }
        }
        #endregion

        #region Class Methods
        public new bool Draw(DebugMenuContext context)
        {
            base.Draw(context);
            return ShowContent;
        }

        public new bool Draw(DebugMenuContext context, string value, bool defaultShowContent = false)
        {
            text = value;
            return Draw(context);
        }
        #endregion
    }
}
