namespace Mayfair.Core.Code.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug;
    using Prateek.Runtime.DaemonFramework.Servants;
    using UnityEngine;

    public class LocalizationServant : Servant<LocalizationDaemon, LocalizationServant>
    {
        #region Settings
        [SerializeField]
        protected List<LanguageJSON> minimalJSON;

        [SerializeField]
        protected List<LanguageJSON> fullJSON;
        #endregion

        #region Fields
        protected Dictionary<string, string> translationLookup;
        #endregion

        #region Properties
        public SystemLanguage Language { get; protected set; }

        public override int Priority
        {
            get { return Consts.FIRST_ITEM; }
        }
        #endregion

        #region Class Methods
        public bool SetLanguage(SystemLanguage language, bool useMinimalSet)
        {
            var languageJsonList = useMinimalSet ? minimalJSON : fullJSON;

            var languageJson = languageJsonList.Find(l => l.language == language);
            if (languageJson == null)
            {
                return false;
            }

            var localizationData = JsonUtility.FromJson<LocalizationJSON>(languageJson.json.text);
            if (localizationData == null)
            {
                return false;
            }

            Language = language;
            translationLookup = new Dictionary<string, string>();
            foreach (var kvp in localizationData.kvps)
            {
                translationLookup.Add(kvp.k, kvp.v);
            }

            return true;
        }

        public string GetStaticValue(string key)
        {
            return GetInternal(key, MISSING_OR_EXTRA_FORMAT_ITEMS_ERROR_FORMAT, NO_FORMAT_ITEMS);
        }

        public string GetDynamicValue(string key, params string[] formatItems)
        {
            return GetInternal(key, MISSING_OR_EXTRA_FORMAT_ITEMS_ERROR_FORMAT, formatItems);
        }

        public bool TryGetStaticValue(string key, out string value)
        {
            value = key;

            if (IsKeyNullOrEmpty(key))
            {
                return false;
            }

            if (translationLookup.TryGetValue(key, out var foundValue))
            {
                value = foundValue;

                // Matches {0}, {1}, {2}, etc.
                // Matches {0:#}, {0:##}, etc.
                return Regex.Matches(foundValue, @"\{\d+[^\{\}]*\}").Count <= 0;
            }

            return false;
        }

        private string GetInternal(string key, string formatItemError, params string[] formatItems)
        {
            if (IsKeyNullOrEmpty(key))
            {
                return key;
            }

            if (translationLookup.TryGetValue(key, out var value))
            {
                try
                {
                    return string.Format(value, formatItems);
                }
                catch (FormatException)
                {
                    DebugTools.LogError(string.Format(formatItemError), key, Language.ToString());
                    return key;
                }
            }

            DebugTools.LogError(string.Format(KEY_NOT_FOUND_FORMAT, key, Language.ToString()));
            return key;
        }

        private bool IsKeyNullOrEmpty(string key)
        {
            if (key == null)
            {
                DebugTools.LogError(NULL_KEY);
                return true;
            }

            if (key == string.Empty)
            {
                DebugTools.LogError(EMPTY_KEY);
                return true;
            }

            return false;
        }
        #endregion

        #region Nested type: LanguageJSON
        [Serializable]
        public class LanguageJSON
        {
            #region Fields
            public SystemLanguage language;
            public TextAsset json;
            #endregion
        }
        #endregion

        #region Statics and Consts
        protected readonly string[] NO_FORMAT_ITEMS = new string[0];
        public const string NULL_KEY = "Localization key is null";
        public const string EMPTY_KEY = "Localization key is empty";
        public const string KEY_NOT_FOUND_FORMAT = "Localization key \"{0}\" in \"{1}\" not found";
        public const string NO_FORMAT_ITEMS_ERROR_FORMAT = "Localization value for \"{0}\" in \"{1}\" has format items, but none were provided";
        public const string MISSING_OR_EXTRA_FORMAT_ITEMS_ERROR_FORMAT = "Localization value for \"{0}\" in \"{1}\" has missing or extra format items";
        #endregion
    }
}
