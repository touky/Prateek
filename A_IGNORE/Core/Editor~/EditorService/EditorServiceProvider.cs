namespace Mayfair.Core.Editor.EditorService
{
    using UnityEditor;
    using UnityEngine;

    public class EditorServiceProvider : ScriptableObject, IEditorServiceProvider
    {
        #region Fields
        private double lastRefreshTime = 0;
        #endregion

        #region Properties
        protected virtual double RefreshTimer
        {
            get { return -1; }
        }
        #endregion

        #region IEditorServiceProvider Members
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
        #endregion
    }
}
