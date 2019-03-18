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
    //-------------------------------------------------------------------------
    public interface IGlobalManager
    {
    }

    //-------------------------------------------------------------------------
    public interface IUpdatable
    {
        // Object Lifetime Messages
        /// <summary>
        /// OnInitialize is called when the object is created. OnInitialize is not called during deserialization.
        /// </summary>
        void OnInitialize();
        /// <summary>
        /// On Start is called at the start of the next frame after the object has been created. OnStart is not called during deserialization.
        /// </summary>
        void OnStart();
        /// <summary>
        /// OnUpdate is called every frame.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnUpdate(float deltaTime);
        /// <summary>
        /// OnLateUpdate is called every frame after the OnUpdate for every object has been called.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnLateUpdate(float deltaTime);
        /// <summary>
        /// OnFixedUpdate is called every fixed physics engine update.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnFixedUpdate(float deltaTime);
        /// <summary>
        /// OnTimescaleIndependantUpdate is called every frame, after every OnUpdate has been called but before any OnLateUpdate has been called. It's deltaTime is timscale independant.
        /// </summary>
        /// <param name="deltaTime"></param>
        void OnTimescaleIndependantUpdate(float deltaTime);
        /// <summary>
        /// OnDispose is called just before the object is destroyed.
        /// </summary>
        void OnDispose();
        /// <summary>
        /// Called when registering the object for updates. Unlike OnInitialize and OnStart, OnRegister is called during serialization. (I couldn't call it OnEnable because it is a Unity message that ScriptableObjects receive)
        /// </summary>
        void OnRegister();
        /// <summary>
        /// Called when unregistering the object for updates. (I couldn't call it OnDisable because it is a Unity message that ScriptableObjects receive)
        /// </summary>
        void OnUnregister();

        // Application Messages
        void OnApplicationFocus(bool focusStatus);
        void OnApplicationPause(bool pauseStatus);
        void OnApplicationQuit();

        // Ui Messages
        void OnGUI();
    }

    //-------------------------------------------------------------------------
    public interface ISingleton
    {
        // Don't force to implement any function since we can't force static functions.
    }
}
