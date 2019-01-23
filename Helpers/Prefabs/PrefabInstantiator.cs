//
//  Prateek, a library that is "bien pratique"
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUG
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

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

        //-----------------------------------------------------------------------------------------
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

        //-----------------------------------------------------------------------------------------
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