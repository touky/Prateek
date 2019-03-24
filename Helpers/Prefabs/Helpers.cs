// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

#region Unity
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Helpers
{
    //-------------------------------------------------------------------------
    public static class Prefabs
    {
        //---------------------------------------------------------------------
        #region Behaviour
        public static T Instantiate<T>(T original) where T : MonoBehaviour
        {
            return Instantiate<T>(original, null, Vector3.zero, Quaternion.identity);
        }
        //-----------------------------------------------------------------------------------------
        public static T Instantiate<T>(T original, Transform parent) where T : MonoBehaviour
        {
            return Instantiate<T>(original, parent, Vector3.zero, Quaternion.identity);
        }
        //-----------------------------------------------------------------------------------------
        public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            return Instantiate<T>(original, null, position, rotation);
        }
        //-----------------------------------------------------------------------------------------
        public static T Instantiate<T>(T original, Transform parent, Vector3 position, Quaternion rotation) where T : MonoBehaviour
        {
            var instance = Instantiate(original.gameObject, parent, position, rotation);
            return instance.GetComponent<T>();
        }

        //-----------------------------------------------------------------------------------------
        public static GameObject Instantiate(GameObject original)
        {
            return Instantiate(original, null, Vector3.zero, Quaternion.identity);
        }
        //-----------------------------------------------------------------------------------------
        public static GameObject Instantiate(GameObject original, Transform parent)
        {
            return Instantiate(original, parent, parent.position, parent.rotation);
        }
        //-----------------------------------------------------------------------------------------
        public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
            return Instantiate(original, null, position, rotation);
        }
        //-----------------------------------------------------------------------------------------
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

        //-----------------------------------------------------------------------------------------
        public static void AddPrefabInstance(GameObject gameObject, GameObject prefab)
        {
            var prefabInstance = gameObject.GetComponent<PrefabReference>();
            if (prefabInstance == null)
            {
                prefabInstance = gameObject.AddComponent<PrefabReference>();
            }
            prefabInstance.Prefab = prefab;
        }

        //-----------------------------------------------------------------------------------------
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