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
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUG
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

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
