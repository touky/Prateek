namespace Prateek.Runtime.Core.HierarchicalTree
{
    using Prateek.Runtime.Core.FrameworkSettings;

    /// <summary>
    ///     Static class to retrieve the default HierarchicalTreeSettingsData
    /// </summary>
    public class HierarchicalTreeSettings : FrameworkSettings<HierarchicalTreeSettings, HierarchicalTreeSettingsData, HierarchicalTreeSettingsResource>
    {
        #region Properties
        protected override string DefaultPath
        {
            get { return HierarchicalTreeSettingsResource.DEFAULT_PATH; }
        }
        #endregion
    }
}
