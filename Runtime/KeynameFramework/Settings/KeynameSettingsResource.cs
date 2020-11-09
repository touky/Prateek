namespace Prateek.Runtime.KeynameFramework.Settings
{
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    ///     ScriptableObject holder of the KeynameSettingsData
    /// </summary>
    [CreateAssetMenu(fileName = nameof(KeynameSettings), menuName = ConstFolder.SETTINGS + "/Create " + nameof(KeynameSettings))]
    public class KeynameSettingsResource : FrameworkSettingsResource<KeynameSettingsData>
    {
        #region Static and Constants
        public static readonly string DEFAULT_PATH = $"{ConstFolder.PRATEEK_SETTINGS}/{nameof(KeynameSettings)}";
        #endregion

    }
}
