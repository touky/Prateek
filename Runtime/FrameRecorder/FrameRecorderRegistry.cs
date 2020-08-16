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
namespace Prateek.Runtime.FrameRecorder
{
    using System.Collections.Generic;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.Core.Singleton;
    using Prateek.Runtime.TickableFramework.Interfaces;
    using UnityEngine;
    using static Core.Extensions.Statics;

    ///-------------------------------------------------------------------------
    public sealed class FrameRecorderRegistry
        : SingletonBehaviour<FrameRecorderRegistry>
        , IEarlyUpdateTickable
        , IPostLateUpdateTickable
    {
        #region Fields
        ///---------------------------------------------------------------------
        private RecorderState currentState = RecorderState.Inactive;

        private RecorderState nextState = RecorderState.Inactive;
        private List<FrameRecorder> recorders = new List<FrameRecorder>();
        private List<RegistryFrame> history = new List<RegistryFrame>();

        private RegistryFrame recordingFrame = null;
        private int frameCapacity = 50;

        private Vector2Int playbackRange = Vector2Int.zero;
        #endregion

        #region Properties
        ///---------------------------------------------------------------------
#if UNITY_EDITOR
        public static FrameRecorderRegistry EditorInstance { get { return Instance; } }
#endif
        internal RecorderState CurrentState { get { return currentState; } }

        internal RecorderState NextState { get { return nextState; } set { nextState = value; } }

        internal bool PlaybackActive { get { return currentState == RecorderState.Playback; } }


        ///---------------------------------------------------------------------
        internal int FrameCount { get { return history.Count; } }

        internal int FrameCapacity
        {
            get
            {
                return frameCapacity;
            }
            set
            {
                frameCapacity = value;
                if (frameCapacity < history.Count)
                {
                    history.RemoveRange(frameCapacity, history.Count - frameCapacity);
                }
            }
        }

        ///---------------------------------------------------------------------
        internal Vector2Int PlaybackRange { get { return playbackRange; } set { playbackRange = clamp(value, 0, frameCapacity - 1); } }
        #endregion

        #region Register/Unregister
        protected override void OnAwake()
        {
            this.AutoRegister();
        }

        private void OnDestroy()
        {
            this.AutoUnregister();
        }

        ///---------------------------------------------------------------------
        internal static void Register(FrameRecorder recorder)
        {
            var instance = Instance;
            if (instance == null)
            {
                return;
            }

            instance.recorders.Add(recorder);
        }

        ///---------------------------------------------------------------------
        internal static void Unregister(FrameRecorder recorder)
        {
            var instance = Instance;
            if (instance == null)
            {
                return;
            }

            instance.recorders.Remove(recorder);
        }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        private void OpenFrame()
        {
            if (history.Count >= frameCapacity)
            {
                recordingFrame = history.Pop();
            }
            else
            {
                recordingFrame = new RegistryFrame();
            }

            foreach (var recorder in recorders)
            {
                recordingFrame.Open(recorder);
            }
        }

        ///---------------------------------------------------------------------
        private void CloseFrame()
        {
            foreach (var recorder in recorders)
            {
                recordingFrame.Close(recorder);
            }

            history.Insert(0, recordingFrame);
        }

        ///---------------------------------------------------------------------
        internal void ClearHistory()
        {
            history.Clear();
            playbackRange = Vector2Int.zero;
        }

        ///---------------------------------------------------------------------
        private void Playback()
        {
            if (history.Count == 0)
            {
                return;
            }

            var isPlayback = currentState == RecorderState.Playback;
            playbackRange = !isPlayback ? vec2i(0) : clamp(playbackRange, 0, frameCapacity - 1);
            for (var h = playbackRange.x; h < min(playbackRange.y + 1, history.Count); h++)
            {
                var frame = history[history.Count - (1 + h)];
                if (frame == null)
                {
                    continue;
                }

                frame.Play(isPlayback);
            }
        }

        private void UpdateState()
        {
            currentState = nextState;
        }
        #endregion

        #region IEarlyUpdateTickable Members
        public void EarlyUpdate()
        {
            UpdateState();

            switch (currentState)
            {
                case RecorderState.Recording:
                {
                    OpenFrame();
                    break;
                }
                case RecorderState.Playback:
                {
                    Playback();
                    break;
                }
            }
        }

        public int Priority(IPriority<IEarlyUpdateTickable> type)
        {
            return Const.VERY_LATE;
        }

        public int DefaultPriority { get { return 0; } }
        #endregion

        #region IPostLateUpdateTickable Members
        public void PostLateUpdate()
        {
            switch (currentState)
            {
                case RecorderState.Recording:
                {
                    CloseFrame();
                    Playback();
                    break;
                }
            }
        }

        public int Priority(IPriority<IPostLateUpdateTickable> type)
        {
            return Const.VERY_EARLY;
        }
        #endregion

        #region Nested type: IRecorderBase
        ///---------------------------------------------------------------------
        public interface IRecorderBase
        {
            #region Class Methods
            ///-----------------------------------------------------------------
            void BeginFrame();

            IRecordedFrame EndFrame();
            void PlayFrame(IRecordedFrame recordedFrame);
            #endregion
        }
        #endregion
    }
}
