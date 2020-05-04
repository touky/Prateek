namespace Mayfair.Core.Code.Utils.Extensions
{
    using System;
    using UnityEngine;

    public static class GameObjectExtensions
    {
        #region Class Methods
        public static void ClearComponents<T>(this GameObject go, bool clearChildren = false) where T : Component
        {
            ClearComponents(go, typeof(T), clearChildren);
        }

        public static void ClearComponents(this GameObject go, Type type, bool clearChildren = false)
        {
            Component[] components = clearChildren ? go.GetComponentsInChildren(type) : go.GetComponents(type);
            foreach (Component component in components)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    UnityEngine.Object.Destroy(component);
                }
                else
                {
                    UnityEngine.Object.DestroyImmediate(component);
                }
#else
                UnityEngine.Object.Destroy(component);
#endif
            }
        }

        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T result = go.GetComponent<T>();
            if (result == null)
            {
                result = go.AddComponent<T>();
            }

            return result;
        }
        #endregion
    }
}
