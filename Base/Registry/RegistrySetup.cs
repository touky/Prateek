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
using System.Reflection;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Base
{
    using System;
    using System.Collections.Generic;
    using Prateek.Attributes;
    using UnityEngine;

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
