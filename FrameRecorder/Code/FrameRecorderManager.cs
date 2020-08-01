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
namespace Prateek.FrameRecorder.Code
{
    using System.Collections.Generic;
    using Prateek.Core.Code.Extensions;
    using static Prateek.Core.Code.Extensions.CSharp;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public sealed class FrameRecorderManager
    {
        ///---------------------------------------------------------------------
        #region Declarations
        public enum StateType
        {
            Inactive,
            Recording,
            Playback,
        }

        ///---------------------------------------------------------------------
        public interface IRecorderBase
        {
            ///-----------------------------------------------------------------
            void BeginFrame();
            Frame.IData EndFrame();
            void PlayFrame(Frame.IData data);
        }

        ///---------------------------------------------------------------------
        public class Frame
        {
            ///-----------------------------------------------------------------
            public interface IData
            {
                IRecorderBase Owner { get; }
            }

            ///-----------------------------------------------------------------
            public List<IData> datas = new List<IData>();
        }
        #endregion Declarations

        ///---------------------------------------------------------------------
        #region Fields
        private StateType state = StateType.Inactive;
        private int frameCapacity = 50;
        private Vector2Int frameRange = Vector2Int.zero;
        private Frame lastActiveFrame = null;
        private List<Frame> history = new List<Frame>();
        private List<IRecorderBase> recorders = new List<IRecorderBase>();
        #endregion Fields

        ///---------------------------------------------------------------------
        #region Properties
        //public override TickType TickType { get { return TickType.ALL; } } //todo TickType.BeginFrame | TickType.EndFrame; } }
        public StateType State { get { return state; } set { state = value; } }
        public bool PlaybackActive { get { return state == StateType.Playback; } }
        public int FrameCount { get { return history.Count; } }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public Vector2Int CurrentFrameRange
        {
            get { return frameRange; }
            set { frameRange = clamp(value, 0, frameCapacity - 1); }
        }
        #endregion Properties

        ///---------------------------------------------------------------------
        #region IGlobalManager integration
        //public override void OnInitialize()
        //{
        //    priority = int.MaxValue;
        //    lastActiveFrame = new Frame();
        //}

        ///---------------------------------------------------------------------
        //public override void OnUpdate(TickType tickType, float seconds)
        //{
        //    //if (tickType != TickType.BeginFrame)
        //        return;

        //    BeginFrame();
        //}

        ///---------------------------------------------------------------------
#if UNITY_EDITOR
        public void OnFakeUpdate()
        {
            //OnUpdate(TickType.BeginFrame, 0);
            //OnLateUpdate(TickType.EndFrame, 0);
        }
#endif //UNITY_EDITOR

        ///---------------------------------------------------------------------
        //public override void OnLateUpdate(TickType tickType, float seconds)
        //{
        //    //if (tickType != TickType.EndFrame)
        //        return;

        //    var lastFrame = EndFrame();
        //    {
        //        if (!IsAppPaused && state == StateType.Recording)
        //        {
        //            if (history.Count == frameCapacity)
        //            {
        //                history.RemoveAt(0);
        //            }

        //            history.Add(lastFrame);
        //        }
        //    }

        //    if (state != StateType.Playback)
        //    {
        //        if (!IsAppPaused)
        //            lastActiveFrame = lastFrame;
        //        PlayFrame(lastFrame);
        //    }
        //    else
        //    {
        //        DoPlayback();
        //    }
        //}
        #endregion IGlobalManager integration

        ///---------------------------------------------------------------------
        #region External Access
        public void Register(IRecorderBase recorder)
        {
            recorders.Add(recorder);
        }

        ///---------------------------------------------------------------------
        public void Unregister(IRecorderBase recorder)
        {
            recorders.Remove(recorder);
        }
        #endregion External Access

        ///---------------------------------------------------------------------
        #region Instance Methods
        public void InternalClearHistory()
        {
            history.Clear();
            frameRange = Vector2Int.zero;
        }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        private Frame EndFrame()
        {
            var lastFrame = new Frame();
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

                lastFrame.datas.Add(data);
            }
            return lastFrame;
        }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        private void DoPlayback()
        {
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
}
