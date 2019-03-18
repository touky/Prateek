// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

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
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public sealed class StaticManager
    {
        private static StaticManager m_manager = null;
        private Dictionary<Type, object> m_instances = null;

        //---------------------------------------------------------------------
        private static StaticManager manager
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = new StaticManager();
                }
                return m_manager;
            }
        }

        //---------------------------------------------------------------------
        private StaticManager()
        {
            m_instances = new Dictionary<Type, object>();
        }

        //---------------------------------------------------------------------
        public static T Get<T>()
        {
            object instance = Get(typeof(T));
            if (instance != null)
                return (T)instance;
            return default(T);
        }

        //---------------------------------------------------------------------
        public static object Get(Type key)
        {
            object instance = null;
            if (manager.m_instances.TryGetValue(key, out instance))
            {
                return instance;
            }
            return null;
        }

        //---------------------------------------------------------------------
        public static void Set<T>(T value)
        {
            Set(typeof(T), value);
        }

        //---------------------------------------------------------------------
        public static void Set(Type key, object value)
        {
            if (manager.m_instances.ContainsKey(key))
            {
                manager.m_instances[key] = value;
            }
            else
            {
                manager.m_instances.Add(key, value);
            }
        }

        //---------------------------------------------------------------------
        public static void Remove<T>()
        {
            Remove(typeof(T));
        }

        //---------------------------------------------------------------------
        public static void Remove(Type key)
        {
            if (manager.m_instances.ContainsKey(key))
            {
                manager.m_instances.Remove(key);
            }
        }
    }
}