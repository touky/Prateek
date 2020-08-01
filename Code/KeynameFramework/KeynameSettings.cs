namespace Prateek.KeynameFramework
{
    using Prateek.Core.Code.FrameworkSettings;
    using Prateek.KeynameFramework.Serializables;

    public class KeynameSettings : FrameworkSettings<KeynameSettings, KeynameSettingsData, KeynameSettingsResource>
    {
        protected override string DataPath
        {
            get { return KeynameSettingsResource.DEFAULT_PATH; }
        }
    }
}
