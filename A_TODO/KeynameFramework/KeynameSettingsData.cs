namespace Prateek.KeynameFramework
{
    using System.Text.RegularExpressions;
    using Prateek.Core.Code.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    /// Defines actual content of the keyname settings
    /// </summary>
    public class KeynameSettingsData : FrameworkSettingsData
    {
        #region Settings
        [SerializeField]
        internal string separator = string.Empty;

        [SerializeField]
        internal Regex keywordRegex = new Regex("([A-Z][a-z]+)");

        [SerializeField]
        internal Regex numberRegex = new Regex("([0-9]+)+");
        #endregion
    }
}
