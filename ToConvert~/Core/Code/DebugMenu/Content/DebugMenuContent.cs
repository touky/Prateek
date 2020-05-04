namespace Mayfair.Core.Code.DebugMenu.Content
{
    using Mayfair.Core.Code.DebugMenu.Fields;

    public abstract class DebugMenuContent
    {
        #region Fields
        protected TitleField title;
        #endregion

        #region Properties
        public TitleField TitleField
        {
            get { return title; }
        }

        public bool ShowContent
        {
            get { return title.ShowContent; }
        }
        #endregion

        #region Constructors
        protected DebugMenuContent(string title = "Empty Content")
        {
            this.title = new TitleField(title);
        }
        #endregion

        #region Class Methods
        public abstract void Draw(DebugMenuContext context);
        public virtual void OnGUI() { }
        #endregion
    }
}
