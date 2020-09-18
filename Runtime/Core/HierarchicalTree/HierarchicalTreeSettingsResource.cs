namespace Prateek.Runtime.Core.HierarchicalTree
{
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    ///     ScriptableObject holder of the HierarchicalTreeSettingsData
    /// </summary>
    [CreateAssetMenu(fileName = nameof(HierarchicalTreeSettings), menuName = FrameworkSettings.DEFAULT_PATH + "Create " + nameof(HierarchicalTreeSettings))]
    public class HierarchicalTreeSettingsResource : FrameworkSettingsResource<HierarchicalTreeSettingsData>
    {
        #region Static and Constants
        public static readonly string DEFAULT_PATH = $"{FrameworkSettings.DEFAULT_PATH}{nameof(HierarchicalTreeSettings)}";
        #endregion
    }
}
