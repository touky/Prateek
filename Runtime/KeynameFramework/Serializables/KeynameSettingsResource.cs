namespace Prateek.Runtime.KeynameFramework.Serializables
{
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(KeynameSettings), menuName = FrameworkSettings.DEFAULT_PATH + "Create " + nameof(KeynameSettings))]
    public class KeynameSettingsResource : FrameworkSettingsResource<KeynameSettingsData>
    {
        #region Static and Constants
        public static readonly string DEFAULT_PATH = $"{FrameworkSettings.DEFAULT_PATH}{nameof(KeynameSettings)}";
        #endregion

    }
}
