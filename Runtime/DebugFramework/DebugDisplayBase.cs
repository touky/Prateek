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
#if PRATEEK_DEBUG
namespace Prateek.Runtime.DebugFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.FrameRecorder;

    ///-------------------------------------------------------------------------
    public abstract class DebugDisplayManager
        : FlagManager
        , FrameRecorderRegistry.IRecorderBase
    {
        ///---------------------------------------------------------------------
        #region Fields
        private static FlagHierarchy flagHierarchy;
        private DebugDisplayFrame displayFrames;
        private DebugLineDisplayer lineDisplay;
        private List<DebugDraw.PrimitiveSetup> timedPrimitives;
        #endregion Fields

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public static FlagHierarchy DebugFlags { get { return flagHierarchy; } set { flagHierarchy = value; } }
        #endregion Properties

        ///---------------------------------------------------------------------
        #region IGlobalManager integration
        //public override void OnRegister()
        //{
        //    base.OnRegister();

        //    //todo var go = new GameObject("DebugDisplayLine");
        //    //todo go.transform.SetParent(DaemonRegistry.Instance.TickerObject.transform);
        //    //todo go.transform.localPosition = Vector3.zero;
        //    //todo go.transform.localRotation = Quaternion.identity;
        //    //todo lineDisplay = go.AddComponent<DebugLineDisplayer>();

        //    //todo DaemonRegistry.Instance.Register(typeof(DebugDisplayManager), this);
        //    //todo FrameRecorder.Register(this);
        //}

        ///---------------------------------------------------------------------
        //public override void OnInitialize()
        //{
        //    flagDatas = flagHierarchy;

        //    base.OnInitialize();
        //}

        ///---------------------------------------------------------------------
        //public override void OnLateUpdate(TickType tickType, float seconds)
        //{
        //    //if (timedPrimitives == null || tickType != TickType.BeginFrame)
        //        return;

        //    for (int p = 0; p < timedPrimitives.Count; p++)
        //    {
        //        var prim = timedPrimitives[p];
        //        prim.setup.Duration -= seconds;
        //        if (prim.setup.Duration < 0)
        //        {
        //            timedPrimitives.RemoveAt(p--);
        //        }
        //        else
        //        {
        //            timedPrimitives[p] = prim;
        //        }
        //    }
        //}

        ///---------------------------------------------------------------------
        //public override void OnUnregister()
        //{
        //    Destroy(lineDisplay.gameObject);

        //    FrameRecorder.Unregister(this);
        //    //todo DaemonRegistry.Instance.Unregister(typeof(DebugDisplayManager));

        //    base.OnUnregister();
        //}
        #endregion IGlobalManager integration

        ///---------------------------------------------------------------------
        #region Flags setups
        protected static void SetupFlags(FlagHierarchy hierarchy)
        {
            DebugDisplayManager.flagHierarchy = hierarchy;
        }
        #endregion Flags setups

        ///---------------------------------------------------------------------
        #region FrameRecorder.IRecorderBase
        public void BeginFrame() { }

        ///---------------------------------------------------------------------
        public IRecordedFrame EndFrame()
        {
            if (timedPrimitives == null)
                return null;

            for (int p = 0; p < timedPrimitives.Count; p++)
            {
                displayFrames.FramePrimitives.Add(timedPrimitives[p]);
            }
            var old = displayFrames;
            displayFrames = new DebugDisplayFrame(this);
            return old;
        }

        ///---------------------------------------------------------------------
        public void PlayFrame(IRecordedFrame recordedFrame)
        {
            var recordings = (DebugDisplayFrame)recordedFrame;
            for (int r = 0; r < recordings.FramePrimitives.Count; r++)
            {
                DebugDraw.Render(lineDisplay, recordings.FramePrimitives[r]);
            }
        }
        #endregion FrameRecorder.IRecorderBase

        ///---------------------------------------------------------------------
        #region Recording datas
        public static void Add(DebugDraw.PrimitiveSetup primitive)
        {
            return;
            //var instance = TickableRegistry.GetManager<DebugDisplayManager>();
            //if (instance == null)
            //    return;

            //if (instance.IsAppPaused)
            //    return;

            //if (primitive.setup.Duration < 0)
            //    instance.recordings.FramePrimitives.Add(primitive);
            //else
            //    instance.TimedPrimitives.Add(primitive);
        }
        #endregion Recording datas
    }
    ///---------------------------------------------------------------------
    public struct DebugDisplayFrame : IRecordedFrame
    {
        ///-----------------------------------------------------------------
        private DebugDisplayManager owner;
        private List<DebugDraw.PrimitiveSetup> framePrimitives;

        ///-----------------------------------------------------------------
        public FrameRecorderRegistry.IRecorderBase Owner { get { return owner; } }
        public List<DebugDraw.PrimitiveSetup> FramePrimitives
        {
            get
            {
                if (framePrimitives == null)
                    framePrimitives = new List<DebugDraw.PrimitiveSetup>();
                return framePrimitives;
            }
        }

        public FrameRecorder SourceRecorder => throw new System.NotImplementedException();

        ///-----------------------------------------------------------------
        public DebugDisplayFrame(DebugDisplayManager owner)
        {
            this.owner = owner;
            framePrimitives = new List<DebugDraw.PrimitiveSetup>();
        }

        public void Open()
        {
            throw new System.NotImplementedException();
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void Recycle()
        {
            throw new System.NotImplementedException();
        }

        public IRecordedFrame CloneEmpty()
        {
            throw new System.NotImplementedException();
        }

        public void Play()
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif
