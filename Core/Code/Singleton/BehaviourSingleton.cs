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
namespace Prateek.DaemonCore.Code
{
    using Prateek.Core.Code.Behaviours;
    using UnityEngine;

    /// <summary>
    ///     Base class for a singleton NamedBehaviour.
    ///     Creates an empty game object in your scene and adds itself as a component on it the first time you call the
    ///     Instance accessor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BehaviourSingleton<T> : NamedBehaviour, ISingleton where T : BehaviourSingleton<T>
    {
        #region Static and Constants
        //---------------------------------------------------------------------
        private static T instance;
        #endregion

        #region Properties
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
        #endregion

        #region Unity Methods
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
                Debug.LogError(string.Format("Singleton for {0} already exists. Destroying {1}.", typeof(T).ToString(), name));
                Destroy(gameObject);
            }
        }

        //---------------------------------------------------------------------
        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        //---------------------------------------------------------------------
        protected virtual void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
        #endregion
    }
}
