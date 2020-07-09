namespace Mayfair.Core.Code.TagSystem
{
    public class KeynameSettings : FrameworkSettings<KeynameSettings>
    {
        #region Fields
        private KeynameSettingsData settingsData = null;
        #endregion

        #region Properties
        public override bool IsAvailable
        {
            get { return settingsData != null; }
        }
        #endregion

        #region Class Methods
        protected override void Init()
        {
            settingsData = LoadResource<KeynameSettingsData>(KeynameSettingsData.DEFAULT_PATH);
        }
        #endregion
    }
}
