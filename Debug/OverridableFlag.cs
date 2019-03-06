// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 05/03/19
//
//  Copyright © 2017—2019 Benjamin "Touky" Huet <huet.benjamin@gmail.com>
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
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
    //---------------------------------------------------------------------
    [Serializable]
    public class Flag
    {
        //---------------------------------------------------------------------
        public enum OverrideType
        {
            None,
            On,
            Off
        }

        //---------------------------------------------------------------------
        [Serializable]
        public struct Overridable
        {
            [SerializeField]
            private bool m_value;
            private OverrideType m_override;

            public bool CanUse { get { return m_override == OverrideType.None ? m_value : m_override == OverrideType.On; } }
            public OverrideType Override { get { return m_override; } set { m_override = value; } }

            public Overridable(bool flag) : this (flag, OverrideType.None) { }
            public Overridable(bool flag, OverrideType @override)
            {
                m_value = flag;
                m_override = @override;
            }

            public static implicit operator Overridable(bool flagValue)
            {
                return new Overridable(flagValue);
            }
        }

        protected bool CanUse(bool flagValue, OverrideType overrideValue)
        {
            return (new Overridable(flagValue, overrideValue)).CanUse;
        }
    }
}
#endif //PRATEEK_DEBUG
