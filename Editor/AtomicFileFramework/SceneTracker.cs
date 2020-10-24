namespace Prateek.Editor.AtomicFileFramework
{
    using System;
    using System.Collections.Generic;
    using UnityEditor.SceneManagement;
    using UnityEngine;

    /// <summary>
    ///     Tracks the currently open scenes in the editor
    /// </summary>
    [Serializable]
    internal class SceneTracker
    {
        #region Settings
        [SerializeField]
        private List<string> openedPaths = new List<string>();
        #endregion

        #region Class Methods
        public void AddScene(string path, OpenSceneMode mode, Action<string, bool> importAction)
        {
            if (mode == OpenSceneMode.Single)
            {
                Clear(importAction);
            }

            openedPaths.Add(path);
            importAction(path, false);

            Log("AddScene");
        }

        public void RemoveScene(string path, Action<string, bool> importAction)
        {
            openedPaths.Remove(path);
            importAction(path, true);

            Log($"RemoveScene: {path}");
        }

        public void Clear(Action<string, bool> importAction)
        {
            for (var p = 0; p < openedPaths.Count; p++)
            {
                RemoveScene(openedPaths[p--], importAction);
            }
        }

        private void Log(string header)
        {
            var content = $"{header}\r\n";
            foreach (var openedPath in openedPaths)
            {
                content += $"> {openedPath}\r\n";
            }

            Debug.Log(content);
        }
        #endregion
    }
}
