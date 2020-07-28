namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using Mayfair.Core.Code.FrameworkSettings;

    public class KeynameSettings : FrameworkSettings<KeynameSettings, KeynameSettingsData, KeynameSettingsResource>
    {
        protected override string DataPath
        {
            get { return KeynameSettingsResource.DEFAULT_PATH; }
        }
    }
}
