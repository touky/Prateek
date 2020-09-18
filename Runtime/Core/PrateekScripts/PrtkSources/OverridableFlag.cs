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
namespace Prateek.Runtime.Core.PrateekScripts.PrtkSources
{
    using System;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    [Serializable]
    public class Flag
    {
        ///---------------------------------------------------------------------
        public enum OverrideMode
        {
            None,
            On,
            Off
        }

        ///---------------------------------------------------------------------
        [Serializable]
        public struct Overridable
        {
            [SerializeField]
            private bool value;
            private OverrideMode mode;

            public bool CanUse { get { return mode == OverrideMode.None ? value : mode == OverrideMode.On; } }
            public OverrideMode Override { get { return mode; } set { mode = value; } }

            public Overridable(bool flag) : this (flag, OverrideMode.None) { }
            public Overridable(bool value, OverrideMode mode)
            {
                this.value = value;
                this.mode = mode;
            }

            public static implicit operator Overridable(bool flagValue)
            {
                return new Overridable(flagValue);
            }
        }

        ///---------------------------------------------------------------------
        protected bool CanUse(bool flagValue, OverrideMode overrideValue)
        {
            return (new Overridable(flagValue, overrideValue)).CanUse;
        }
    }
}
#endif //PRATEEK_DEBUG
