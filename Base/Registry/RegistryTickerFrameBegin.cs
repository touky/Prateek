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
namespace Prateek.Base.Registry
{
    using Prateek.Base.Behaviour;
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public class RegistryTicker : NamedBehaviour { }

    //-------------------------------------------------------------------------
    public sealed class RegistryTickerFrameBegin : RegistryTicker
    {
        //---------------------------------------------------------------------
        public delegate void TickableEvent(Registry.TickEvent tickEvent, float seconds);
        public delegate void ApplicationEvent(bool status);

        //---------------------------------------------------------------------
        private TickableEvent onUpdate;
        private TickableEvent onUpdateUnscaled;
        private TickableEvent onLateUpdate;
        private TickableEvent onFixedUpdate;
        private ApplicationEvent onFocus;
        private ApplicationEvent onPause;
        private ApplicationEvent onQuit;
        private ApplicationEvent onGUI;

        //---------------------------------------------------------------------
        public void Register(TickableEvent onUpdate, TickableEvent onUpdateUnscaled, TickableEvent onLateUpdate, TickableEvent onFixedUpdate,
                             ApplicationEvent onFocus, ApplicationEvent onPause, ApplicationEvent onQuit,
                             ApplicationEvent onGUI)
        {
            this.onUpdate = onUpdate;
            this.onUpdateUnscaled = onUpdateUnscaled;
            this.onLateUpdate = onLateUpdate;
            this.onFixedUpdate = onFixedUpdate;
            this.onFocus = onFocus;
            this.onPause = onPause;
            this.onQuit = onQuit;
            this.onGUI = onGUI;
#if UNITY_EDITOR
            EditorApplication.pauseStateChanged += PauseStateChanged;
#endif //UNITY_EDITOR
        }

        //---------------------------------------------------------------------
        #region Editor behaviour
#if UNITY_EDITOR
        private void PauseStateChanged(PauseState state)
        {
            OnApplicationPause(state == PauseState.Paused);
        }

        //---------------------------------------------------------------------
        private void OnDestroy()
        {
            EditorApplication.pauseStateChanged -= PauseStateChanged;
        }
#endif //UNITY_EDITOR
        #endregion Editor behaviour

        //---------------------------------------------------------------------
        private void Update()
        {
            onUpdate.Invoke(Registry.TickEvent.FrameBeginning, Time.deltaTime);
            onUpdateUnscaled(Registry.TickEvent.FrameBeginning, Time.unscaledDeltaTime);
        }
        private void LateUpdate() { onLateUpdate(Registry.TickEvent.FrameBeginning, Time.deltaTime); }
        private void FixedUpdate() { onFixedUpdate(Registry.TickEvent.FrameBeginning, Time.fixedDeltaTime); }

        //---------------------------------------------------------------------
        private void OnApplicationFocus(bool focusStatus) { onFocus(focusStatus); }
        private void OnApplicationPause(bool pauseStatus) { onPause(pauseStatus); }
        private void OnApplicationQuit() { onQuit(true); }

        //---------------------------------------------------------------------
        private void OnGUI() { onGUI(true); }
    }
}

