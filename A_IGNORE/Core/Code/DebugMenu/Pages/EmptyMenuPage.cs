namespace Mayfair.Core.Code.DebugMenu.Pages
{
    public class EmptyMenuPage : DebugMenuPage
    {
        #region Properties
        public override bool IgnoreIndent
        {
            get { return true; }
        }
        #endregion

        #region Constructors
        public EmptyMenuPage(string title) : base(title)
        {
            this.title.ShowContent = true;
        }
        #endregion
    }
}
