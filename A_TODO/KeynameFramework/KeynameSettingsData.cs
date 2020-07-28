namespace Mayfair.Core.Code.TagSystem
{
    using System.Text.RegularExpressions;
    using Mayfair.Core.Code.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    /// Defines actual content of the keyname settings
    /// </summary>
    public class KeynameSettingsData : FrameworkSettingsData
    {
        #region Settings
        [SerializeField]
        private string separator = string.Empty;

        [SerializeField]
        private Regex tagRegex = new Regex("([A-Z][a-z]+)");

        [SerializeField]
        private Regex numberRegex = new Regex("([0-9]+)+");
        #endregion
    }
}
