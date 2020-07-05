namespace Mayfair.Core.Editor.EditorService
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;
    using UnityEditor;
    using UnityEngine;

    public abstract class EditorService<TService, TProvider> : ScriptableObject
        where TService : ScriptableObject, IEditorService
        where TProvider : ScriptableObject, IEditorServiceProvider
    {
        #region Static and Constants
        private static TService instance;
        #endregion

        #region Fields
        protected List<TProvider> providers = new List<TProvider>();

        private double lastRefreshTime = 0;
        #endregion

        #region Properties
        protected virtual double RefreshTimer
        {
            get { return -1; }
        }
        #endregion

        #region Class Methods
        protected static TService GetValidInstance(string rootPath)
        {
            if (instance == null)
            {
                string path = $"{ConstsFolders.ASSET}/{rootPath}{typeof(TService).Name}.asset";
                instance = AssetDatabase.LoadAssetAtPath<TService>(path);
                if (instance == null)
                {
                    Debug.Assert(instance != null, $"Couldn't find Resources at path {path}");
                    return null;
                }

                instance.LoadContent();
            }
            else
            {
                instance.TryReloadContent();
            }

            return instance;
        }

        public virtual void LoadContent()
        {
            lastRefreshTime = EditorApplication.timeSinceStartup;
        }

        public void TryReloadContent()
        {
            if (RefreshTimer < 0)
            {
                return;
            }

            if (EditorApplication.timeSinceStartup - lastRefreshTime <= RefreshTimer)
            {
                return;
            }

            LoadContent();
        }

        protected void ReloadProviders(string assetPath)
        {
            EditorServiceHelpers.LoadAllAt(assetPath, ref providers);

            foreach (TProvider provider in providers)
            {
                provider.LoadContent();
            }
        }
        #endregion
    }
}
