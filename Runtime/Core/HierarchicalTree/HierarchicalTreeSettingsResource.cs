namespace Prateek.Runtime.Core.HierarchicalTree
{
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    ///     ScriptableObject holder of the HierarchicalTreeSettingsData
    /// </summary>
    [CreateAssetMenu(fileName = nameof(HierarchicalTreeSettings), menuName = ConstFolder.SETTINGS + "/Create " + nameof(HierarchicalTreeSettings))]
    public class HierarchicalTreeSettingsResource : FrameworkSettingsResource<HierarchicalTreeSettingsData>
    {
        #region Static and Constants
        public static readonly string DEFAULT_PATH = $"{ConstFolder.PRATEEK_SETTINGS}/{nameof(HierarchicalTreeSettings)}";
        #endregion
    }
}
