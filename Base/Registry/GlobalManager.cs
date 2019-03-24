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
namespace Prateek.Base
{
    //-------------------------------------------------------------------------
    public abstract class GlobalManager : ScriptableObject, IGlobalManager, IUpdatable
    {
        public const string CREATE_METHOD = "GetBuilder";

        //---------------------------------------------------------------------
        public abstract class BuilderBase
        {
            public Type m_real_type = null;
            public Type m_empty_type = null;

            public BuilderBase(Type real_type, Type empty_type)
            {
                m_real_type = real_type;
                m_empty_type = empty_type;
            }
        }

        public class Builder<REAL, EMPTY> : BuilderBase where REAL : class where EMPTY : class
        {
            public Builder() : base(typeof(REAL), typeof(EMPTY)) { }
        }

        [SerializeField]
        private int m_priority;
        public int priority { get { return m_priority; } }

        //---------------------------------------------------------------------
        public virtual void OnCreate() { }

        // Object Lifetime Messages
        public virtual void OnInitialize() { }
        public virtual void OnStart() { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnLateUpdate(float deltaTime) { }
        public virtual void OnFixedUpdate(float deltaTime) { }
        public virtual void OnTimescaleIndependantUpdate(float deltaTime) { }
        public virtual void OnDispose() { }
        public virtual void OnRegister() { }
        public virtual void OnUnregister() { }

        // Application Messages
        public virtual void OnApplicationFocus(bool focusStatus) { }
        public virtual void OnApplicationPause(bool pauseStatus) { }
        public virtual void OnApplicationQuit() { }

        // Ui Messages
        public virtual void OnGUI() { }
    }
}
