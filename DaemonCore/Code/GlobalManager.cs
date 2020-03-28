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
    using System;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public abstract class GlobalManager : ScriptableObject, IGlobalManager, IUpdatable
    {
        #region Static and Constants
        //---------------------------------------------------------------------
        public const string CREATE_METHOD = "GetBuilder";
        #endregion

        #region Settings
        [SerializeField]
        protected int priority;
        #endregion

        #region Fields
        private bool isAppFocused = false;
        private bool isAppPaused = false;
        #endregion

        #region Properties
        public int Priority
        {
            get { return priority; }
        }

        protected bool IsAppFocused
        {
            get { return isAppFocused; }
        }

        protected bool IsAppPaused
        {
            get { return isAppPaused; }
        }

        public virtual Registry.TickEvent TickEvent
        {
            get { return Registry.TickEvent.FrameBeginning; }
        }
        #endregion

        //---------------------------------------------------------------------

        #region Declarations
        public abstract class BuilderBase
        {
            #region Fields
            public Type realType = null;
            public Type emptyType = null;
            #endregion

            #region Constructors
            public BuilderBase(Type realType, Type emptyType)
            {
                this.realType = realType;
                this.emptyType = emptyType;
            }
            #endregion
        }

        //---------------------------------------------------------------------
        public class Builder<REAL, FAKE> : BuilderBase where REAL : class where FAKE : class
        {
            #region Constructors
            public Builder() : base(typeof(REAL), typeof(FAKE)) { }
            #endregion
        }
        #endregion Declarations

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------

        //---------------------------------------------------------------------

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
        public virtual void OnApplicationFocus(bool focusStatus)
        {
            isAppFocused = focusStatus;
        }

        public virtual void OnApplicationPause(bool pauseStatus)
        {
            isAppPaused = pauseStatus;
        }

        public virtual void OnApplicationQuit() { }

        //-- Ui Messages ------------------------------------------------------
        public virtual void OnGUI() { }
        #endregion IGlobalManager integration
    }
}
