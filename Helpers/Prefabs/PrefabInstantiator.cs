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
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Helpers
{
    using Prateek.Base;
    using UnityEngine;

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