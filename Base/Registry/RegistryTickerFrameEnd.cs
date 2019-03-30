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
using System.Reflection;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Base
{
    //-------------------------------------------------------------------------
    public sealed class RegistryTickerFrameEnd : RegistryTicker
    {
        //---------------------------------------------------------------------
        public delegate void TickableEvent(Registry.TickEvent tickEvent, float seconds);

        //---------------------------------------------------------------------
        private TickableEvent onUpdate;
        private TickableEvent onUpdateUnscaled;
        private TickableEvent onLateUpdate;
        private TickableEvent onFixedUpdate;

        //---------------------------------------------------------------------
        public void Register(TickableEvent onUpdate, TickableEvent onUpdateUnscaled, TickableEvent onLateUpdate, TickableEvent onFixedUpdate)
        {
            this.onUpdate = onUpdate;
            this.onUpdateUnscaled = onUpdateUnscaled;
            this.onLateUpdate = onLateUpdate;
            this.onFixedUpdate = onFixedUpdate;
        }

        //---------------------------------------------------------------------
        private void Update()
        {
            onUpdate(Registry.TickEvent.FrameEnding, Time.deltaTime);
            onUpdateUnscaled(Registry.TickEvent.FrameEnding, Time.unscaledDeltaTime);
        }
        private void LateUpdate() { onLateUpdate(Registry.TickEvent.FrameEnding, Time.deltaTime); }
        private void FixedUpdate() { onFixedUpdate(Registry.TickEvent.FrameEnding, Time.fixedDeltaTime); }
    }
}