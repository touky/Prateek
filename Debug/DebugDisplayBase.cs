// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
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

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR
#endregion Editor

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
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public abstract class DebugDisplayBase : FlagManager
    {
        //---------------------------------------------------------------------
        public static BuilderBase GetBuilder()
        {
            return new Builder<DebugDisplayBase, NullDebugDisplay>();
        }

        //---------------------------------------------------------------------
        public override void OnRegister()
        {
            base.OnRegister();

            Registry.instance.Register(typeof(DebugDisplayBase), this);
        }

        //---------------------------------------------------------------------
        public override void OnUnregister()
        {
            Registry.instance.Unregister(typeof(DebugDisplayBase));

            base.OnUnregister();
        }
    }
    
    //-------------------------------------------------------------------------
    public sealed class NullDebugDisplay : DebugDisplayBase
    {
        //---------------------------------------------------------------------
        public override void OnCreate() { }

        //---------------------------------------------------------------------
        public override void OnRegister() { Registry.instance.Register(typeof(DebugDisplayBase), this); }
        public override void OnUnregister() { Registry.instance.Unregister(typeof(DebugDisplayBase)); }

        // Object Lifetime Messages
        public override void OnInitialize() { }
        public override void OnStart() { }
        public override void OnUpdate(float deltaTime) { }
        public override void OnLateUpdate(float deltaTime) { }
        public override void OnFixedUpdate(float deltaTime) { }
        public override void OnTimescaleIndependantUpdate(float deltaTime) { }
        public override void OnDispose() { }

        // Application Messages
        public override void OnApplicationFocus(bool focusStatus) { }
        public override void OnApplicationPause(bool pauseStatus) { }
        public override void OnApplicationQuit() { }

        // Ui Messages
        public override void OnGUI() { }

        //---------------------------------------------------------------------
        public override bool HasParent<T>(T child) { return false; }
        public override int CountParents<T>(T child) { return 0; }
        public override void SetParent<T>(T child) { }
        public override void SetParent<T>(T child, T parent) { }
        public override bool IsActive<T>(T value) { return false; }
        public override bool IsActiveAndSelected<T>(T value, MonoBehaviour behaviour) { return false; }
        public override void SetActive<T>(T value, bool active) { }
        public override void SetActive(bool active) { }
    }
}
#endif //PRATEEK_DEBUG
