namespace Prateek.Runtime.KeynameFramework
{
    using Prateek.Runtime.Core.FrameworkSettings;
    using Prateek.Runtime.KeynameFramework.Serializables;

    public class KeynameSettings : FrameworkSettings<KeynameSettings, KeynameSettingsData, KeynameSettingsResource>
    {
        protected override string DataPath
        {
            get { return KeynameSettingsResource.DEFAULT_PATH; }
        }
    }
}
