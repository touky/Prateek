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
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public class FrameRecorder
    {
        //---------------------------------------------------------------------
        #region Declarations
        public enum State
        {
            Inactive,
            Recording,
            Playback,
            PlaybackPaused,
        }

        //---------------------------------------------------------------------
        public class Frame
        {
            public List<Draw.LineSetup> m_lines = new List<Draw.LineSetup>();
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        private State m_status = State.Inactive;
        private int m_frame_capacity = 50;
        private int m_frame_index = 0;
        private List<Frame> m_history = new List<Frame>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public static State status
        {
            get
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return State.Inactive;
                return instance.m_status;
            }
            set
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return;
                instance.m_status = value;
            }
        }

        //---------------------------------------------------------------------
        public static bool playback_active
        {
            get
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return false;
                return instance.m_status >= State.Playback;
            }
        }

        //---------------------------------------------------------------------
        public static int frame_count
        {
            get
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return 0;
                return instance.m_history.Count;
            }
        }

        //---------------------------------------------------------------------
        public static int MaxFrameRecorded
        {
            get
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return 0;
                return instance.m_frame_capacity;
            }
            set
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return;

                instance.m_frame_capacity = value;
                if (instance.m_history.Count > instance.m_frame_capacity)
                {
                    var history = instance.m_history.GetRange(instance.m_history.Count - instance.m_frame_capacity, instance.m_frame_capacity);
                    FrameRecorder.ClearHistory();
                    instance.m_history = history;
                }
            }
        }

        //---------------------------------------------------------------------
        public static int CurrentFrameIndex
        {
            get
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return 0;
                return instance.m_frame_index;
            }
            set
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return;

                var max = Mathf.Max(1, Mathf.Min(instance.m_history.Count, instance.m_frame_capacity));
                instance.m_frame_index = (value + max) % max;
            }
        }

        //---------------------------------------------------------------------
        public static Frame CurrentFrame
        {
            get
            {
                var instance = Helpers.StaticManager.Get<FrameRecorder>();
                if (instance == null)
                    return new Frame();
                return instance.m_history[Mathf.Min(instance.m_frame_index, instance.m_history.Count - 1)];
            }
        }
        #endregion Properties

        //---------------------------------------------------------------------
        #region Static Methods
        public static void AddLine(Draw.LineSetup line)
        {
            var instance = Helpers.StaticManager.Get<FrameRecorder>();
            if (instance == null)
                return;

            if (instance.m_status != State.Recording)
                return;

            instance.InternalAddLine(ref line);
        }

        //---------------------------------------------------------------------
        public static void EndFrame()
        {
            var instance = Helpers.StaticManager.Get<FrameRecorder>();
            if (instance == null)
                return;

            if (instance.m_status != State.Recording)
                return;

            instance.InternalEndFrame();
        }
        #endregion Static Methods

        //---------------------------------------------------------------------
        #region Instance Methods
        public static void ClearHistory()
        {
            var instance = Helpers.StaticManager.Get<FrameRecorder>();
            if (instance == null)
                return;

            instance.m_history.Clear();
            instance.m_history.Capacity = instance.m_frame_capacity;
        }

        //---------------------------------------------------------------------
        private void InternalEndFrame()
        {
            if (m_history.Count == m_frame_capacity)
            {
                m_history.RemoveAt(0);
            }
            m_history.Add(new Frame());
            m_frame_index = m_history.Count - 1;
        }

        //---------------------------------------------------------------------
        private void InternalAddLine(ref Draw.LineSetup line)
        {
            CurrentFrameIndex = m_frame_index;
            if (m_frame_index >= m_history.Count)
                return;

            m_history[m_frame_index].m_lines.Add(line);
        }
        #endregion Instance Methods
    }
}
#endif //PRATEEK_DEBUG
