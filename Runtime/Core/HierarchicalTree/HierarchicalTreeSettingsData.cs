namespace Prateek.Runtime.Core.HierarchicalTree
{
    using System;
    using System.Text.RegularExpressions;
    using Prateek.Runtime.Core.FrameworkSettings;
    using UnityEngine;

    /// <summary>
    ///     Defines the settings used by a hierarchical tree to sort datas
    /// </summary>
    [Serializable]
    public class HierarchicalTreeSettingsData : FrameworkSettingsData
    {
        #region Settings
        [SerializeField]
        internal bool useAsOverride = false;

        [SerializeField]
        internal string folderMatch = "([^\\\\/]+)(?:[\\\\/]*)";

        [SerializeField]
        internal string extensionMatch = "([^\\\\/]+)(\\.[^\\\\/]+)$";
        #endregion

        #region Fields
        private Regex folderRegex = null;
        private Regex extensionRegex = null;
        #endregion

        #region Properties
        internal Regex FolderRegex
        {
            get
            {
                if (folderRegex == null)
                {
                    folderRegex = new Regex(folderMatch);
                }

                return folderRegex;
            }
        }

        internal Regex ExtensionRegex
        {
            get
            {
                if (extensionRegex == null)
                {
                    extensionRegex = new Regex(extensionMatch);
                }

                return extensionRegex;
            }
        }

        public bool UseAsOverride { get { return useAsOverride; } }
        #endregion
    }
}
