namespace Prateek.Runtime.Core.Singleton
{
    using System.Collections.Generic;
    using UnityEngine;

    internal class ParentProvider
        : MonoBehaviour
    {
        #region Static and Constants
        private static ParentProvider instance;
        #endregion

        #region Fields
        private Dictionary<string, Transform> parents = new Dictionary<string, Transform>();
        #endregion

        #region Properties
        private static ParentProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("ParentProvider").AddComponent<ParentProvider>();
                    DontDestroyOnLoad(instance);
                }

                return instance;
            }
        }
        #endregion

        #region Unity Methods
        private void OnApplicationQuit()
        {
            Destroy(gameObject);
        }

        private static Transform GetParent(string parentName)
        {
            if (!Instance.parents.TryGetValue(parentName, out var parent))
            {
                parent = new GameObject(parentName).transform;
                DontDestroyOnLoad(parent.gameObject);
                Instance.parents.Add(parentName, parent);
            }

            return parent;
        }

        internal static void SetParent(Transform transform, string parentName)
        {
            var parent = GetParent(parentName);
            parent.SetParent(transform);
        }
        #endregion
    }
}