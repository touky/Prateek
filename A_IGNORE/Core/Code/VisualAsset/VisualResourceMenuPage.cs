namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.VisualAsset.Providers;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces;
    using Prateek.Runtime.KeynameFramework;

    internal class VisualResourceMenuPage : DebugMenuPage<VisualResourceServant>
    {
        #region Fields
        protected ReflectedField<Dictionary<string, Dictionary<Keyname, IContentHandle>>> resources = "resources";
        #endregion

        #region Constructors
        public VisualResourceMenuPage(VisualResourceServant owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(VisualResourceServant owner, DebugMenuContext context)
        {
            if (NewCategory.Draw(context, "Available resources", true))
            {
                using (new ContextIndentScope(context))
                {
                    foreach (string groupKey in resources.Value.Keys)
                    {
                        if (NewCategory.Draw(context, groupKey))
                        {
                            using (new ContextIndentScope(context))
                            {
                                Dictionary<Keyname, IContentHandle> storedResources = resources.Value[groupKey];
                                foreach (Keyname resourceKey in storedResources.Keys)
                                {
                                    NewLabel.Draw(context, resourceKey.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}
