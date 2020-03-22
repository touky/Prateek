// -BEGIN_PRATEEK_COPYRIGHT-// -BEGIN_PRATEEK_COPYRIGHT-
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
// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
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
// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Helpers
{
    using Prateek.Base;
    using UnityEditor;
    using UnityEngine;

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