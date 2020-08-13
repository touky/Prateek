namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Drawers;
    using Prateek.Runtime.DebugFramework.Reflection;

    internal class ContentRegistrySection
        : DebugMenuSection<ContentRegistryDaemon>
    {
        #region Fields
        private DebugField<HierarchicalTree<ContentLoader>> hierarchicalTree = "hierarchicalTree";
        #endregion

        #region Constructors
        public ContentRegistrySection(string title) : base(title) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
        {
            if (!hierarchicalTree.AssertDrawable())
            {
                return;
            }

            hierarchicalTree.Value.Draw();
        }
        #endregion
    }
}
