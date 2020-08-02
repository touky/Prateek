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
namespace Prateek.Runtime.TickableFramework
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    [CreateAssetMenu(fileName = "RegistrySetup", menuName = "Prateek/Create registry setup")]
    public sealed class RegistrySetup : ScriptableObject
    {
        #region Settings
        [SerializeField]
        private List<string> loadOnInit = new List<string>();

        //[SerializeField]
        //[TypeRef(typeof(GlobalManager))]
        //private List<string> createOnInit = new List<string>();
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        public void Initialize()
        {
            //todo //Load stored resources
            //todo for (var i = 0; i < loadOnInit.Count; i++)
            //todo {
            //todo     var resource = Resources.Load(loadOnInit[i]);
            //todo     if (resource == null)
            //todo     {
            //todo         continue; //TODO: ERROR OUT
            //todo     }

            //todo     var instance = Instantiate(resource) as GlobalManager;
            //todo     if (instance == null)
            //todo     {
            //todo         continue; //TODO: ERROR OUT
            //todo     }

            //todo     //DaemonRegistry.Instance.Register(instance.GetType(), instance);
            //todo }

            //todo //Create listed classes
            //todo for (var i = 0; i < createOnInit.Count; i++)
            //todo {
            //todo     TryCreating(Core.Code.Helpers.Types.GetType(createOnInit[i]));
            //todo }
        }

        ///---------------------------------------------------------------------
        public static void TryCreating(Type type, bool force_null = false)
        {
            //var builder = TickableRegistry.GetBuilder(type);
            //if (builder == null)
            //{
            //    var method_info = type.GetMethod(GlobalManager.CREATE_METHOD, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //    if (method_info != null)
            //    {
            //        builder = method_info.Invoke(null, null) as GlobalManager.BuilderBase;
            //    }
            //}

            //if (builder != null)
            //{
            //    //DaemonRegistry.Instance.Register(type, builder);

            //    if (!force_null
            //     && builder.realType != null
            //     && !builder.realType.IsAbstract
            //     && !builder.realType.IsInterface)
            //    {
            //        DoCreate(builder.realType);
            //    }
            //    else if (builder.emptyType != null
            //          && !builder.emptyType.IsAbstract
            //          && !builder.emptyType.IsInterface)
            //    {
            //        DoCreate(builder.emptyType);
            //    }
            //    else
            //    {
            //        // ERROR !
            //    }
            //}
        }

        ///---------------------------------------------------------------------
        private static void DoCreate(Type type)
        {
            //todo var instance = CreateInstance(type) as GlobalManager;
            //todo instance.name = type.Name + "(Instance)";
            //todo //DaemonRegistry.Instance.Register(type, instance);
        }
        #endregion

        ///---------------------------------------------------------------------
    }
}
