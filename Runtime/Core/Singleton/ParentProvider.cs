namespace Prateek.Runtime.Core.Singleton
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Provides a 'DontDestroyOnLoad' storage system to set children to a specific parent.
    /// Use AddChildToParent() to add a transform to the named parent.
    /// </summary>
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void DomainReload()
        {
            instance = null;
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

        internal static void AddChildToParent(Transform transform, string parentName)
        {
            var parent = GetParent(parentName);
            transform.SetParent(parent);
        }
        #endregion
    }
}