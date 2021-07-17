namespace Prateek.Runtime.AppContentFramework.ContentAccess
{
    using System;
    using Prateek.Runtime.AppContentFramework.ContentAccess.Interfaces;
    using Prateek.Runtime.Core.HierarchicalTree;
    using UnityEngine;

    [Serializable]
    public class ContentAccessSettings
        : IContentAccessSettings
    {
        #region Settings
        [SerializeField]
        private HierarchicalTreeSettingsData settings = null;

        [SerializeField]
        private string[] contentPaths = new string[0];

        [SerializeField]
        private string[] contentExtensions = new string[0];
        #endregion

        #region Properties
        public string[] ContentPaths { get { return contentPaths; } }

        public string[] ContentExtensions { get { return contentExtensions; } }

        public HierarchicalTreeSettingsData Settings { get { return settings; } }
        #endregion

        #region Class Methods
        public void AddContentPath(params string[] contentPaths)
        {
            Concatenate(ref this.contentPaths, contentPaths);
        }

        public void AddContentExtensions(params string[] contentExtensions)
        {
            Concatenate(ref this.contentExtensions, contentExtensions);
        }

        private static void Concatenate(ref string[] destination, string[] source)
        {
            var newPaths = new string[destination.Length + source.Length];
            for (var i = 0; i < newPaths.Length; i++)
            {
                newPaths[i] = i < destination.Length
                    ? destination[i]
                    : source[i - destination.Length];
            }

            destination = newPaths;
        }
        #endregion
    }
}
