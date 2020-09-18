namespace Prateek.Runtime.KeynameFramework.Settings
{
    using Prateek.Runtime.Core.FrameworkSettings;

    /// <summary>
    ///     Static class to retrieve the default KeynameSettingsData
    /// </summary>
    public class KeynameSettings : FrameworkSettings<KeynameSettings, KeynameSettingsData, KeynameSettingsResource>
    {
        #region Properties
        protected override string DefaultPath
        {
            get { return KeynameSettingsResource.DEFAULT_PATH; }
        }
        #endregion
    }
}
