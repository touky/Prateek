// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.Core.Code.Prefabs
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static class Prefabs
    {
        ///---------------------------------------------------------------------
        #region Behaviour
        public static T Instantiate<T>(T original) where T : MonoBehaviour
        {
            return Instantiate<T>(original, null, Vector3.zero, Quaternion.identity);
        }
        ///---------------------------------------------------------------------
        public static T Instantiate<T>(T original, Transform parent) where T : MonoBehaviour
        {
            return Instantiate<T>(original, parent, Vector3.zero, Quaternion.identity);
        }
        ///---------------------------------------------------------------------
        public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            return Instantiate<T>(original, null, position, rotation);
        }
        ///---------------------------------------------------------------------
        public static T Instantiate<T>(T original, Transform parent, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            var instance = Instantiate(original.gameObject, parent, position, rotation);
            return instance.GetComponent<T>();
        }

        ///---------------------------------------------------------------------
        public static GameObject Instantiate(GameObject original)
        {
            return Instantiate(original, null, Vector3.zero, Quaternion.identity);
        }
        ///---------------------------------------------------------------------
        public static GameObject Instantiate(GameObject original, Transform parent)
        {
            return Instantiate(original, parent, parent.position, parent.rotation);
        }
        ///---------------------------------------------------------------------
        public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
            return Instantiate(original, null, position, rotation);
        }
        ///---------------------------------------------------------------------
        public static GameObject Instantiate(GameObject original, Transform parent, Vector3 position, Quaternion rotation)
        {
            var instance = UnityEngine.Object.Instantiate(original, position, rotation) as GameObject;

            var instantiator = instance.GetComponent<PrefabInstantiator>();
            if (instantiator != null)
            {
                // Override the instance with the one of the Instantiator. 
                instance = instantiator.CreateInstance();
            }

            AddPrefabInstance(instance, original);

            if (parent != null)
            {
                instance.transform.SetParent(parent, true);
            }
            //TODO
            //else if (MainManager.Instance)
            //{
            //    instance.transform.SetParent(MainManager.Instance.SpawnedObjects.transform, true);
            //}

            // Our instance might not be an instantiator, but it might have children that are.
            PrefabInstantiator.CreateChildrenInstances(instance);

            return instance;
        }

        ///---------------------------------------------------------------------
        public static void AddPrefabInstance(GameObject gameObject, GameObject prefab)
        {
            var prefabInstance = gameObject.GetComponent<PrefabReference>();
            if (prefabInstance == null)
            {
                prefabInstance = gameObject.AddComponent<PrefabReference>();
            }
            prefabInstance.Prefab = prefab;
        }

        ///---------------------------------------------------------------------
        public static int GetPrefabInstanceID(GameObject gameObject)
        {
            var prefabInstance = gameObject.GetComponent<PrefabReference>();
            if (prefabInstance == null)
            {
                Debug.LogWarning("Invalid PrefabInstance");
                return 0;
            }

            if (prefabInstance.Prefab == null)
            {
                Debug.LogWarning("Invalid PrefabInstance");
                return 0;
            }

            return prefabInstance.Prefab.GetInstanceID();
        }
        #endregion Behaviour
    }
}