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
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
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
    #region NullDebugDisplay
    public sealed class NullDebugDisplay : DebugDisplayManager
    {
        //---------------------------------------------------------------------
        public override void OnCreate() { }

        //---------------------------------------------------------------------
        public override void OnRegister() { Registry.Instance.Register(typeof(DebugDisplayManager), this); }
        public override void OnUnregister() { Registry.Instance.Unregister(typeof(DebugDisplayManager)); }

        //-- Object Lifetime Messages------------------------------------------
        public override void OnInitialize() { }
        public override void OnStart() { }
        public override void OnUpdate(Registry.TickEvent tickEvent, float seconds) { }
        public override void OnUpdateUnscaled(Registry.TickEvent tickEvent, float seconds) { }
        public override void OnLateUpdate(Registry.TickEvent tickEvent, float seconds) { }
        public override void OnFixedUpdate(Registry.TickEvent tickEvent, float seconds) { }
        public override void OnDispose() { }

        //-- Application Messages----------------------------------------------
        public override void OnApplicationFocus(bool focusStatus) { }
        public override void OnApplicationPause(bool pauseStatus) { }
        public override void OnApplicationQuit() { }

#if UNITY_EDITOR
        //-- Ui Messages-------------------------------------------------------
        public override void OnGUI() { }
#endif //UNITY_EDITOR
    }
    #endregion NullDebugDisplay

    //-------------------------------------------------------------------------
    public abstract class DebugDisplayManager : FlagManager, FrameRecorderManager.IRecorderBase
    {
        //---------------------------------------------------------------------
        #region Declarations
        public struct DebugRecording : FrameRecorderManager.Frame.IData
        {
            //-----------------------------------------------------------------
            private DebugDisplayManager owner;
            private List<DebugDraw.PrimitiveSetup> framePrimitives;

            //-----------------------------------------------------------------
            public FrameRecorderManager.IRecorderBase Owner { get { return owner; } }
            public List<DebugDraw.PrimitiveSetup> FramePrimitives
            {
                get
                {
                    if (framePrimitives == null)
                        framePrimitives = new List<DebugDraw.PrimitiveSetup>();
                    return framePrimitives;
                }
            }

            //-----------------------------------------------------------------
            public DebugRecording(DebugDisplayManager owner)
            {
                this.owner = owner;
                framePrimitives = new List<DebugDraw.PrimitiveSetup>();
        }
    }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        private static FlagHierarchy flagHierarchy;
        private DebugRecording recordings;
        private DebugLineDisplayer lineDisplay;
        private List<DebugDraw.PrimitiveSetup> timedPrimitives;
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public List<DebugDraw.PrimitiveSetup> TimedPrimitives
        {
            get
            {
                if (timedPrimitives == null)
                    timedPrimitives = new List<DebugDraw.PrimitiveSetup>();
                return timedPrimitives;
            }
        }

        //---------------------------------------------------------------------
        public static FlagHierarchy DebugFlags { get { return flagHierarchy; } set { flagHierarchy = value; } }
        #endregion Properties

        //---------------------------------------------------------------------
        #region IGlobalManager integration
        public static BuilderBase GetBuilder()
        {
            return new Builder<DebugDisplayManager, NullDebugDisplay>();
        }

        //---------------------------------------------------------------------
        public override void OnRegister()
        {
            base.OnRegister();

            var go = new GameObject("DebugDisplayLine");
            go.transform.SetParent(Registry.Instance.TickerObject.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            lineDisplay = go.AddComponent<DebugLineDisplayer>();

            Registry.Instance.Register(typeof(DebugDisplayManager), this);
            FrameRecorder.Register(this);
        }

        //---------------------------------------------------------------------
        public override void OnInitialize()
        {
            flagDatas = flagHierarchy;

            base.OnInitialize();
        }

        //---------------------------------------------------------------------
        public override void OnLateUpdate(Registry.TickEvent tickEvent, float seconds)
        {
            if (tickEvent != Registry.TickEvent.FrameBeginning)
                return;

            for (int p = 0; p < timedPrimitives.Count; p++)
            {
                var prim = timedPrimitives[p];
                prim.setup.Duration -= seconds;
                if (prim.setup.Duration < 0)
                {
                    timedPrimitives.RemoveAt(p--);
                }
                else
                {
                    timedPrimitives[p] = prim;
                }
            }
        }

        //---------------------------------------------------------------------
        public override void OnUnregister()
        {
            Destroy(lineDisplay.gameObject);

            FrameRecorder.Unregister(this);
            Registry.Instance.Unregister(typeof(DebugDisplayManager));

            base.OnUnregister();
        }
        #endregion IGlobalManager integration

        //---------------------------------------------------------------------
        #region Flags setups
        protected static void SetupFlags(FlagHierarchy hierarchy)
        {
            DebugDisplayManager.flagHierarchy = hierarchy;
        }
        #endregion Flags setups

        //---------------------------------------------------------------------
        #region FrameRecorder.IRecorderBase
        public void BeginFrame() { }

        //---------------------------------------------------------------------
        public FrameRecorderManager.Frame.IData EndFrame()
        {
            for (int p = 0; p < timedPrimitives.Count; p++)
            {
                recordings.FramePrimitives.Add(timedPrimitives[p]);
            }
            var old = recordings;
            recordings = new DebugRecording(this);
            return old;
        }

        //---------------------------------------------------------------------
        public void PlayFrame(FrameRecorderManager.Frame.IData data)
        {
            var recordings = (DebugRecording)data;
            for (int r = 0; r < recordings.FramePrimitives.Count; r++)
            {
                DebugDraw.Render(lineDisplay, recordings.FramePrimitives[r]);
            }
        }
        #endregion FrameRecorder.IRecorderBase

        //---------------------------------------------------------------------
        #region Recording datas
        public static void Add(DebugDraw.PrimitiveSetup primitive)
        {
            var instance = Registry.GetManager<DebugDisplayManager>();
            if (instance == null)
                return;

            if (instance.IsAppPaused)
                return;

            if (primitive.setup.Duration < 0)
                instance.recordings.FramePrimitives.Add(primitive);
            else
                instance.TimedPrimitives.Add(primitive);
        }
        #endregion Recording datas
    }
}
#endif //PRATEEK_DEBUG
