// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
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
    public class PrefabInstantiator : BaseBehaviour
    {
        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        protected bool destroyOnSpawn = true;
        [SerializeField]
        protected GameObject prefab = null;
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        protected GameObject instance;
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public bool DestroyOnSpawn { get { return destroyOnSpawn; } }
        public GameObject Prefab { get { return prefab; } }
        public GameObject Instance { get { return instance; } }
        #endregion Properties

        //---------------------------------------------------------------------
        #region Unity Defaults
        private void Awake()
        {
            Spawn();
        }
        #endregion Unity Defaults

        //---------------------------------------------------------------------
        #region Instantiation
        protected void Spawn()
        {
            CreateInstance();
            if (instance != null)
            {
                CreateChildrenInstances(instance);
            }
        }

        //---------------------------------------------------------------------
        public static void CreateChildrenInstances(GameObject gameObject)
        {
            if (Application.isPlaying == false || gameObject == null)
                return;

            var instantiators = gameObject.GetComponentsInChildren<PrefabInstantiator>();
            foreach (var instantiator in instantiators)
            {
                instantiator.Spawn();
            }
        }

        //---------------------------------------------------------------------
        public virtual GameObject CreateInstance()
        {
            if (!Application.isPlaying || instance != null || prefab == null)
                return null;

            instance = Instantiate(prefab);
            Helpers.Prefabs.AddPrefabInstance(instance.gameObject, prefab);

            instance.transform.SetParent(transform.parent);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;

            Destroy(gameObject);

            return instance;
        }
        #endregion Instantiation
    }
}