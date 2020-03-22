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
namespace Prateek.Base
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Base class for a singleton NamedBehaviour.
    /// Creates an empty game object in your scene and adds itself as a component on it the first time you call the Instance accessor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BehaviourSingleton<T> : NamedBehaviour, ISingleton where T : BehaviourSingleton<T>
    {
        //---------------------------------------------------------------------
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    if ((instance = FindObjectOfType<T>()) == null)
                    {
                        var gameObject = new GameObject(typeof(T).Name + " Instance");
                        instance = gameObject.AddComponent<T>();
                    }

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(instance.gameObject);
                    }
                }
                return instance;
            }
        }

        //---------------------------------------------------------------------
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (transform.parent == null && Application.isPlaying)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            else if (this != instance)
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
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
