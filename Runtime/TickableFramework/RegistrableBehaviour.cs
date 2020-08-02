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
namespace Prateek.Runtime.TickableFramework
{
    using System;
    using Prateek.Runtime.Core.Behaviours;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    [Serializable]
    public abstract class RegistrableBehaviour : NamedBehaviour
    {
        #region Unity Methods
        ///---------------------------------------------------------------------
        protected virtual void OnEnable()
        {
            if (Application.isPlaying)
            {
                //todo DaemonRegistry.Instance.Register(this);
            }
        }

        ///---------------------------------------------------------------------
        protected virtual void OnDisable()
        {
            if (Application.isPlaying)
            {
                //todo DaemonRegistry.Instance.Unregister(this);
            }
        }
        #endregion
    }
}
