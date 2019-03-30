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

#if PRATEEK_DEBUGS
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using System.Reflection;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Base
{
    //-------------------------------------------------------------------------
    [CreateAssetMenu(fileName = "RegistrySetup", menuName = "Prateek/Create registry setup")]
    public sealed class RegistrySetup : ScriptableObject
    {
        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        private List<string> loadOnInit = new List<string>();

        [SerializeField, TypeRef(typeof(GlobalManager))]
        private List<string> createOnInit = new List<string>();
        #endregion Settings

        //---------------------------------------------------------------------
        public void Initialize()
        {
            //Load stored resources
            for (int i = 0; i < loadOnInit.Count; i++)
            {
                var resource = Resources.Load(loadOnInit[i]);
                if (resource == null)
                    continue; //TODO: ERROR OUT

                var instance = GameObject.Instantiate(resource) as GlobalManager;
                if (instance == null)
                    continue; //TODO: ERROR OUT

                Registry.Instance.Register(instance.GetType(), instance);
            }

            //Create listed classes
            for (int i = 0; i < createOnInit.Count; i++)
            {
                TryCreating(Helpers.Types.GetType(createOnInit[i]));
            }
        }

        //---------------------------------------------------------------------
        public static void TryCreating(Type type, bool force_null = false)
        {
            var builder = Registry.GetBuilder(type);
            if (builder == null)
            {
                var method_info = type.GetMethod(GlobalManager.CREATE_METHOD, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (method_info != null)
                {
                    builder = method_info.Invoke(null, null) as GlobalManager.BuilderBase;
                }
            }

            if (builder != null)
            {
                Registry.Instance.Register(type, builder);

                if (!force_null
                && builder.realType != null
                && !builder.realType.IsAbstract
                && !builder.realType.IsInterface)
                {
                    DoCreate(builder.realType);
                }
                else if (builder.emptyType != null
                     && !builder.emptyType.IsAbstract
                     && !builder.emptyType.IsInterface)
                {
                    DoCreate(builder.emptyType);
                }
                else
                {
                    // ERROR !
                }
            }
        }

        //---------------------------------------------------------------------
        private static void DoCreate(Type type)
        {
            var instance = ScriptableObject.CreateInstance(type) as GlobalManager;
            instance.name = type.Name + "(Instance)";
            Registry.Instance.Register(type, instance);
        }
    }
}
