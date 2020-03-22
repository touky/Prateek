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
namespace Prateek.Base
{
    using UnityEngine;

    //-------------------------------------------------------------------------
    public sealed class RegistryTickerFrameEnd : RegistryTicker
    {
        //---------------------------------------------------------------------
        public delegate void TickableEvent(Registry.TickEvent tickEvent, float seconds);

        //---------------------------------------------------------------------
        private TickableEvent onUpdate;
        private TickableEvent onUpdateUnscaled;
        private TickableEvent onLateUpdate;
        private TickableEvent onFixedUpdate;

        //---------------------------------------------------------------------
        public void Register(TickableEvent onUpdate, TickableEvent onUpdateUnscaled, TickableEvent onLateUpdate, TickableEvent onFixedUpdate)
        {
            this.onUpdate = onUpdate;
            this.onUpdateUnscaled = onUpdateUnscaled;
            this.onLateUpdate = onLateUpdate;
            this.onFixedUpdate = onFixedUpdate;
        }

        //---------------------------------------------------------------------
        private void Update()
        {
            onUpdate(Registry.TickEvent.FrameEnding, Time.deltaTime);
            onUpdateUnscaled(Registry.TickEvent.FrameEnding, Time.unscaledDeltaTime);
        }
        private void LateUpdate() { onLateUpdate(Registry.TickEvent.FrameEnding, Time.deltaTime); }
        private void FixedUpdate() { onFixedUpdate(Registry.TickEvent.FrameEnding, Time.fixedDeltaTime); }
    }
}
