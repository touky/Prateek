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
using static Prateek.Debug.Draw.Style.QuickCTor;
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
    public abstract class GlobalManager : ScriptableObject, IGlobalManager, IUpdatable
    {
        //---------------------------------------------------------------------
        public const string CREATE_METHOD = "GetBuilder";

        //---------------------------------------------------------------------
        #region Declarations
        public abstract class BuilderBase
        {
            public Type realType = null;
            public Type emptyType = null;

            public BuilderBase(Type realType, Type emptyType)
            {
                this.realType = realType;
                this.emptyType = emptyType;
            }
        }

        //---------------------------------------------------------------------
        public class Builder<REAL, FAKE> : BuilderBase where REAL : class where FAKE : class
        {
            public Builder() : base(typeof(REAL), typeof(FAKE)) { }
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        protected int priority;
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        private bool isAppFocused = false;
        private bool isAppPaused = false;
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public int Priority { get { return priority; } }
        protected bool IsAppFocused { get { return isAppFocused; } }
        protected bool IsAppPaused { get { return isAppPaused; } }
        public virtual Registry.TickEvent TickEvent { get { return Registry.TickEvent.FrameBeginning; } }
        #endregion Properties

        //---------------------------------------------------------------------
        #region IGlobalManager integration
        public virtual void OnCreate() { }

        //-- Object Lifetime Messages -----------------------------------------
        public virtual void OnInitialize() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate(Registry.TickEvent tickEvent, float seconds) { }
        public virtual void OnUpdateUnscaled(Registry.TickEvent tickEvent, float seconds) { }
        public virtual void OnLateUpdate(Registry.TickEvent tickEvent, float seconds) { }
        public virtual void OnFixedUpdate(Registry.TickEvent tickEvent, float seconds) { }
        public virtual void OnDispose() { }
        public virtual void OnRegister() { }
        public virtual void OnUnregister() { }

        //-- Application Messages ---------------------------------------------
        public virtual void OnApplicationFocus(bool focusStatus) { isAppFocused = focusStatus; }
        public virtual void OnApplicationPause(bool pauseStatus) { isAppPaused = pauseStatus; }
        public virtual void OnApplicationQuit() { }

        //-- Ui Messages ------------------------------------------------------
        public virtual void OnGUI() { }
        #endregion IGlobalManager integration
    }
}
