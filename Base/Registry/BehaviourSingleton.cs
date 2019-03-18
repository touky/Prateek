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
namespace Prateek.Base
{
    /// <summary>
    /// Base class for a singleton NamedBehaviour.
    /// Creates an empty game object in your scene and adds itself as a component on it the first time you call the Instance accessor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BehaviourSingleton<T> : NamedBehaviour, ISingleton where T : BehaviourSingleton<T>
    {
        //---------------------------------------------------------------------
        private static T m_instance;
        public static T instance
        {
            get
            {
                if (m_instance == null)
                {
                    if ((m_instance = FindObjectOfType<T>()) == null)
                    {
                        GameObject game_object = new GameObject(typeof(T).Name);
                        m_instance = game_object.AddComponent<T>();
                    }

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(m_instance.gameObject);
                    }
                }
                return m_instance;
            }
        }

        //---------------------------------------------------------------------
        protected virtual void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this as T;
                if (transform.parent == null && Application.isPlaying)
                {
                    DontDestroyOnLoad(m_instance.gameObject);
                }
            }
            else if (this != m_instance)
            {
                UnityEngine.Debug.LogError(String.Format("Singleton for {0} already exists. Destroying {1}.", typeof(T).ToString(), name));
                Destroy(gameObject);
            }
        }

        //---------------------------------------------------------------------
        protected virtual void OnApplicationQuit()
        {
            Destroy(gameObject);
        }

        //---------------------------------------------------------------------
        protected virtual void OnDestroy()
        {
            if (m_instance == this)
            {
                m_instance = null;
            }
        }
    }
}
