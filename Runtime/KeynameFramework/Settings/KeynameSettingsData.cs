namespace Prateek.Runtime.KeynameFramework.Settings
{
    using System;
    using System.Text.RegularExpressions;
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    ///     Defines actual content of the keyname settings
    /// </summary>
    [Serializable]
    public class KeynameSettingsData : FrameworkSettingsData
    {
        #region Settings
        [SerializeField]
        internal string separatorForRebuild = string.Empty;

        [SerializeField]
        internal string keywordMatch = "((^[a-z]+)|([A-Z][a-z]+))";

        [SerializeField]
        internal string numberMatch = "([0-9]+)+";
        #endregion

        #region Fields
        private Regex keywordRegex = null;
        private Regex numberRegex = null;
        #endregion

        #region Properties
        internal Regex KeywordRegex
        {
            get
            {
                if (keywordRegex == null)
                {
                    keywordRegex = new Regex(keywordMatch);
                }

                return keywordRegex;
            }
        }

        internal Regex NumberRegex
        {
            get
            {
                if (numberRegex == null)
                {
                    numberRegex = new Regex(numberMatch);
                }

                return numberRegex;
            }
        }
        #endregion
    }
}
