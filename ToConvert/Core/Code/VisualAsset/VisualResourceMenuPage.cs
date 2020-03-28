namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Mayfair.Core.Code.VisualAsset.Providers;

    internal class VisualResourceMenuPage : DebugMenuPage<VisualResourceServiceProvider>
    {
        #region Fields
        protected ReflectedField<Dictionary<string, Dictionary<Keyname, IAbstractResourceReference>>> resources = "resources";
        #endregion

        #region Constructors
        public VisualResourceMenuPage(VisualResourceServiceProvider owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(VisualResourceServiceProvider owner, DebugMenuContext context)
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
                                Dictionary<Keyname, IAbstractResourceReference> storedResources = resources.Value[groupKey];
                                foreach (Keyname resourceKey in storedResources.Keys)
                                {
                                    NewLabel.Draw(context, resourceKey);
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
