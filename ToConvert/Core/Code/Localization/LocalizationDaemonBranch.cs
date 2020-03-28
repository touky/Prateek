using System;
using Mayfair.Core.Code.Utils;
using Mayfair.Core.Code.Utils.Debug;

namespace Mayfair.Core.Code.Localization
{
    using Mayfair.Core.Code.Service;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Prateek.DaemonCore.Code.Branches;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class LocalizationDaemonBranch : DaemonBranchBehaviour<LocalizationDaemonCore, LocalizationDaemonBranch>
    {
        #region Classes

        [Serializable]
        public class LanguageJSON
        {
            public SystemLanguage language;
            public TextAsset json;
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

        #region Fields

        [SerializeField]
        protected List<LanguageJSON> minimalJSON;

        [SerializeField]
        protected List<LanguageJSON> fullJSON;

        public SystemLanguage Language { get; protected set; }
        protected override bool IsAliveInternal => true;
        public override int Priority => Consts.FIRST_ITEM;

        protected Dictionary<string, string> translationLookup;

        #endregion

        public bool SetLanguage(SystemLanguage language, bool useMinimalSet)
        {
            List<LanguageJSON> languageJsonList = useMinimalSet ? minimalJSON : fullJSON;

            LanguageJSON languageJson = languageJsonList.Find(l => l.language == language);
            if (languageJson == null)
            {
                return false;
            }

            LocalizationJSON localizationData = JsonUtility.FromJson<LocalizationJSON>(languageJson.json.text);
            if (localizationData == null)
            {
                return false;
            }

            Language = language;
            translationLookup = new Dictionary<string, string>();
            foreach (LocalizationJSONKeyValuePair kvp in localizationData.kvps)
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

            if (translationLookup.TryGetValue(key, out string foundValue))
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

            if (translationLookup.TryGetValue(key, out string value))
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
    }
}