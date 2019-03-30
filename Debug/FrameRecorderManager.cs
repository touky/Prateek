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
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Manager
{
    //-------------------------------------------------------------------------
    #region NullFrameRecorderManager
    public sealed class NullFrameRecorderManager : GlobalManager
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
    #endregion NullFrameRecorderManager

    //-------------------------------------------------------------------------
    public sealed class FrameRecorderManager : GlobalManager
    {
        //---------------------------------------------------------------------
        #region Declarations
        public enum StateType
        {
            Inactive,
            Recording,
            Playback,
        }

        //---------------------------------------------------------------------
        public interface IRecorderBase
        {
            //-----------------------------------------------------------------
            void BeginFrame();
            Frame.IData EndFrame();
            void PlayFrame(Frame.IData data);
        }

        //---------------------------------------------------------------------
        public class Frame
        {
            //-----------------------------------------------------------------
            public interface IData
            {
                IRecorderBase Owner { get; }
            }

            //-----------------------------------------------------------------
            public List<IData> datas = new List<IData>();
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Settings
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        private StateType state = StateType.Inactive;
        private int frameCapacity = 50;
        private Vector2Int frameRange = Vector2Int.zero;
        private Frame activeFrame = null;
        private List<Frame> playback = new List<Frame>();
        private List<Frame> history = new List<Frame>();
        private List<IRecorderBase> recorders = new List<IRecorderBase>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public override Registry.TickEvent TickEvent { get { return Registry.TickEvent.FrameBeginning | Registry.TickEvent.FrameEnding; } }
        public StateType State { get { return state; } set { state = value; } }
        public bool PlaybackActive { get { return state == StateType.Playback; } }
        public int FrameCount { get { return history.Count; } }

        //---------------------------------------------------------------------
        public int MaxFrameRecorded
        {
            get { return frameCapacity; }
            set
            {
                frameCapacity = value;
                if (history.Count > frameCapacity)
                {
                    var newHistory = history.GetRange(history.Count - frameCapacity, frameCapacity);
                    InternalClearHistory();
                    history = newHistory;
                }
            }
        }

        //---------------------------------------------------------------------
        public Vector2Int CurrentFrameRange
        {
            get { return frameRange; }
            set { frameRange = clamp(value, 0, frameCapacity - 1); }
        }
        #endregion Properties

        //---------------------------------------------------------------------
        #region GlobalManager integration
        public static BuilderBase GetBuilder()
        {
            return new Builder<FrameRecorderManager, NullFrameRecorderManager>();
        }

        //---------------------------------------------------------------------
        public override void OnRegister()
        {
            base.OnRegister();
        }

        //---------------------------------------------------------------------
        public override void OnUnregister()
        {
            base.OnUnregister();
        }

        //---------------------------------------------------------------------
        public override void OnInitialize()
        {
            priority = int.MaxValue;
            activeFrame = new Frame();
        }

        //---------------------------------------------------------------------
        public override void OnUpdate(Registry.TickEvent tickEvent, float seconds)
        {
            if (tickEvent != Registry.TickEvent.FrameBeginning)
                return;

            BeginFrame();
        }

        //---------------------------------------------------------------------
        public override void OnLateUpdate(Registry.TickEvent tickEvent, float seconds)
        {
            if (tickEvent != Registry.TickEvent.FrameEnding)
                return;

            EndFrame();
            {
                if (state == StateType.Recording)
                {
                    if (history.Count == frameCapacity)
                    {
                        history.RemoveAt(0);
                    }

                    history.Add(activeFrame);
                }
            }

            if (state != StateType.Playback)
            {
                PlayFrame(activeFrame);
            }
            else
            {
                DoPlayback();
            }
        }
        #endregion GlobalManager integration

        //---------------------------------------------------------------------
        #region External Access
        public void Register(IRecorderBase recorder)
        {
            recorders.Add(recorder);
        }

        //---------------------------------------------------------------------
        public void Unregister(IRecorderBase recorder)
        {
            recorders.Remove(recorder);
        }

        //---------------------------------------------------------------------
        public static void ClearHistory()
        {
            var instance = Registry.GetManager<FrameRecorderManager>();
            if (instance == null)
                return;

            instance.InternalClearHistory();
        }
        #endregion External Access

        //---------------------------------------------------------------------
        #region Instance Methods
        private void InternalClearHistory()
        {
            history.Clear();
            frameRange = Vector2Int.zero;
        }

        //---------------------------------------------------------------------
        private void BeginFrame()
        {
            for (int r = 0; r < recorders.Count; r++)
            {
                var recorder = recorders[r];
                if (recorder == null)
                {
                    recorders.RemoveAt(r--);
                    continue;
                }

                recorder.BeginFrame();
            }
        }

        //---------------------------------------------------------------------
        private void EndFrame()
        {
            activeFrame = new Frame();
            for (int r = 0; r < recorders.Count; r++)
            {
                var recorder = recorders[r];
                if (recorder == null)
                {
                    recorders.RemoveAt(r--);
                    continue;
                }

                var data = recorder.EndFrame();
                if (data == null)
                    continue;

                activeFrame.datas.Add(data);
            }
        }

        //---------------------------------------------------------------------
        private void PlayFrame(Frame frame)
        {
            if (frame == null)
                return;

            for (int r = 0; r < frame.datas.Count; r++)
            {
                var data = frame.datas[r];
                if (data == null || data.Owner == null)
                    continue;

                data.Owner.PlayFrame(data);
            }
        }

        //---------------------------------------------------------------------
        private void DoPlayback()
        {
            playback.Clear();

            frameRange = clamp(frameRange, 0, frameCapacity - 1);
            for (int h = frameRange.x; h < min(frameRange.y + 1, history.Count); h++)
            {
                var frame = history[history.Count - (1 + h)];
                if (frame == null)
                    continue;

                PlayFrame(frame);
            }
        }
        #endregion Instance Methods
    }

    //-------------------------------------------------------------------------
    public static class FrameRecorder
    {
        //---------------------------------------------------------------------
        #region Properties
        public static FrameRecorderManager.StateType State
        {
            get
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return FrameRecorderManager.StateType.Inactive;
                return instance.State;
            }
            set
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return;
                instance.State = value;
            }
        }

        //---------------------------------------------------------------------
        public static bool PlaybackActive
        {
            get
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return false;
                return instance.PlaybackActive;
            }
        }

        //---------------------------------------------------------------------
        public static int FrameCount
        {
            get
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return 0;
                return instance.FrameCount;
            }
        }

        //---------------------------------------------------------------------
        public static int MaxFrameRecorded
        {
            get
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return 0;
                return instance.MaxFrameRecorded;
            }
            set
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return;
                instance.MaxFrameRecorded = value;
            }
        }

        //---------------------------------------------------------------------
        public static Vector2Int CurrentFrameRange
        {
            get
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return Vector2Int.zero;
                return instance.CurrentFrameRange;
            }
            set
            {
                var instance = Registry.GetManager<FrameRecorderManager>();
                if (instance == null)
                    return;
                instance.CurrentFrameRange = value;
            }
        }
        #endregion Properties

        //---------------------------------------------------------------------
        #region External Access
        public static void Register(FrameRecorderManager.IRecorderBase recorder)
        {
            var instance = Registry.GetManager<FrameRecorderManager>();
            if (instance == null)
                return;

            instance.Register(recorder);
        }

        //---------------------------------------------------------------------
        public static void Unregister(FrameRecorderManager.IRecorderBase recorder)
        {
            var instance = Registry.GetManager<FrameRecorderManager>();
            if (instance == null)
                return;

            instance.Unregister(recorder);
        }
        #endregion External Access
    }
}
