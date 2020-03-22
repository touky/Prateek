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
namespace Prateek.Helpers
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

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

