//
//  Prateek, a library that is "bien pratique"
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
#endregion Namespaces

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
