namespace Assets.Prateek.ToConvert.DebugMenu.Fields
{
    using UnityEngine;

    /// <summary>
    ///     A toggleable category field, can be queried for show/hide behaviour
    /// </summary>
    public class CategoryField : StringField
    {
        #region Static and Constants
        private static readonly string CATEGORY_PREFS = ".showContent.";
        #endregion

        #region Fields
        internal DebugToggleStatus showContent = DebugToggleStatus.None;
        #endregion

        #region Properties
        public bool ShowContent
        {
            get { return DebugMenuService.Get(showContent, $"{GetType().Name}{CATEGORY_PREFS}{text}", false); }
            set { DebugMenuService.Set(ref showContent, $"{GetType().Name}{CATEGORY_PREFS}{text}", ShowContent, value); }
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

        public bool Draw(DebugMenuContext context, string value, bool defaultShowContent = false)
        {
            text = value;
            return Draw(context);
        }
        #endregion
    }
}
