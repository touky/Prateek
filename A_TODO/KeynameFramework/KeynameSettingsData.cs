namespace Mayfair.Core.Code.TagSystem
{
    using System.Text.RegularExpressions;
    using UnityEngine;

    [CreateAssetMenu(fileName = nameof(KeynameSettings), menuName = FrameworkSettings.DEFAULT_PATH + "Create " + nameof(KeynameSettings))]
    public class KeynameSettingsData : ScriptableObject
    {
        #region Static and Constants
        public static readonly string DEFAULT_PATH = $"{FrameworkSettings.DEFAULT_PATH}{nameof(KeynameSettings)}";
        #endregion

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
