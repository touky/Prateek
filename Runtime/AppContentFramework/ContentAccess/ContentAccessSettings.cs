namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using System;
    using Prateek.Runtime.Core.HierarchicalTree;
    using UnityEngine;

    [Serializable]
    public class ContentAccessSettings
    {
        #region Settings
        [SerializeField]
        private HierarchicalTreeSettingsData settings = null;

        [SerializeField]
        private string[] contentPaths;

        [SerializeField]
        private string[] contentExtensions;
        #endregion

        #region Properties
        public string[] ContentPaths
        {
            get { return contentPaths; }
        }

        public string[] ContentExtensions
        {
            get { return contentExtensions; }
        }

        public HierarchicalTreeSettingsData Settings
        {
            get { return settings; }
        }
        #endregion
    }
}
