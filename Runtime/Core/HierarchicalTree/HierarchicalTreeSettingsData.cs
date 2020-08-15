namespace Prateek.Runtime.Core.HierarchicalTree
{
    using System.Text.RegularExpressions;
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    ///     Defines the settings used by a hierarchical tree to sort datas
    /// </summary>
    public class HierarchicalTreeSettingsData : FrameworkSettingsData
    {
        #region Settings
        [SerializeField]
        internal bool useAsOverride = false;

        [SerializeField]
        internal Regex folderRegex = new Regex("([^\\\\/]+)(?:[\\\\/]*)");

        [SerializeField]
        internal Regex extensionRegex = new Regex("([^\\\\/]+)(\\.[^\\\\/]+)$");
        #endregion

        #region Properties
        public bool UseAsOverride
        {
            get { return useAsOverride; }
        }
        #endregion
    }
}
