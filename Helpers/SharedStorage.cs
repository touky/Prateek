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
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public abstract class SharedStorage : ISerializationCallbackReceiver
    {
        [SerializeField]
        public List<string> _keys = new List<string>();
        [SerializeField]
        public List<object> _values = new List<object>();

        private Dictionary<string, object> storage = new Dictionary<string, object>();

        //---------------------------------------------------------------------
        protected object GetInstance(string key)
        {
            object instance = null;
            if (storage.TryGetValue(key, out instance))
            {
                if (instance != null)
                    return instance;
            }

            if ((instance = CreateInstance(key)) != null)
            {
                storage.Add(key, instance);
            }
            return instance;
        }

        //---------------------------------------------------------------------
        protected abstract object CreateInstance(string key);

        //---------------------------------------------------------------------
        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in storage)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        //---------------------------------------------------------------------
        public void OnAfterDeserialize()
        {
            storage = new Dictionary<string, object>();

            for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
                storage.Add(_keys[i], _values[i]);
        }
    }
}

