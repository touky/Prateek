namespace Mayfair.CoreContent.Code.Debug
{
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;

    public class GameDebugMenuDaemonBranch : DebugMenuDaemonBranch
    {
        #region Class Methods
        public override void AddDebugContent(DebugMenuNotebook notebook, DebugMenuPage rootPage)
        {
            DebugSettingsPage newPage = new DebugSettingsPage("Debug Settings");
            notebook.AddPagesWithParent(rootPage, newPage);
            newPage.Build(notebook, GameDebugOptions.Instance);
        }
        #endregion
    }
}
