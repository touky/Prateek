namespace Assets.Prateek.ToConvert.Localization
{
    using Assets.Prateek.ToConvert.Service;
    using UnityEngine;

    public class LocalizationService : ServiceSingletonBehaviour<LocalizationService, LocalizationServiceProvider>
    {
        #region Methods
        #region Unity Methods
        protected override void OnAwake() { }
        #endregion Unity Methods

        public static bool SetLanguage(SystemLanguage language, bool useMinimalSet)
        {
            return Instance.GetFirstValidProvider().SetLanguage(language, useMinimalSet);
        }

        public static bool TryGetStaticValue(string key, out string value)
        {
            return Instance.GetFirstValidProvider().TryGetStaticValue(key, out value);
        }

        public static string GetDynamicValue(string key, params string[] formatItems)
        {
            return Instance.GetFirstValidProvider().GetDynamicValue(key, formatItems);
        }
        #endregion
    }
}
