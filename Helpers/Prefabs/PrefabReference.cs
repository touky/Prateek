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
    public class PrefabReference : BaseBehaviour
    {
        //---------------------------------------------------------------------
        #region Settings
        [SerializeField] //TODO:, Readonly]
        private GameObject prefab;
        #endregion Settings

        //---------------------------------------------------------------------
        #region Properties
        public GameObject Prefab { get { return prefab; } set { prefab = value; } }
        #endregion Properties

        //---------------------------------------------------------------------
#if UNITY_EDITOR
        #region Unity Defaults
        protected void Reset()
        {
            Verify(true);
        }

        //---------------------------------------------------------------------
        protected void OnEnable()
        {
            Verify(false);
        }

        //---------------------------------------------------------------------
        protected void OnValidate()
        {
            Verify(true);
        }
        #endregion Unity Defaults

        //---------------------------------------------------------------------
        #region Behaviour
        void Verify(bool force)
        {
            if (force || prefab == null)
            {
                var root = PrefabUtility.FindPrefabRoot(gameObject);
                var parent = PrefabUtility.GetCorrespondingObjectFromSource(root) as GameObject;

                //we have a parent for the found root, use it
                if (parent)
                {
                    prefab = parent;
                }
                //We're not the highest gameobject in the prefab, reference our "local parent"
                else if (root)
                {
                    prefab = root;
                }
                //We're the prefab in the editor AND the highest GameObject in it, so reference ourselves
                else
                {
                    prefab = gameObject;
                }
            }
        }
        #endregion Behaviour
#endif //UNITY_EDITOR
    }
}