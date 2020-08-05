namespace Prateek.Runtime.KeynameFramework.Settings
{
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    ///     ScriptableObject holder of the KeynameSettingsData
    /// </summary>
    [CreateAssetMenu(fileName = nameof(KeynameSettings), menuName = FrameworkSettings.DEFAULT_PATH + "Create " + nameof(KeynameSettings))]
    public class KeynameSettingsResource : FrameworkSettingsResource<KeynameSettingsData>
    {
        #region Static and Constants
        public static readonly string DEFAULT_PATH = $"{FrameworkSettings.DEFAULT_PATH}{nameof(KeynameSettings)}";
        #endregion

    }
}
