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
namespace Prateek.Attributes
{
    //-------------------------------------------------------------------------
    public abstract class EnumBaseAttribute : PropertyAttribute
    { }

    //-------------------------------------------------------------------------
    //Use this on enums to take into account Categories&Names
    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class EnumAllowCategoriesAttribute : EnumBaseAttribute
    {
    }

    //-------------------------------------------------------------------------
    //Use this to treat enum as a mask or to apply an enum mask to an int/ulong/Mask{***}
    //-------------------------------------------------------------------------
    public class EnumMaskAttribute : EnumBaseAttribute
    {
        protected Type m_value = null;

        public Type value { get { return m_value; } }

        public EnumMaskAttribute() { }
        public EnumMaskAttribute(Type enum_type)
        {
            m_value = enum_type;
        }
    }

    //-------------------------------------------------------------------------
    //Use this to treat enum as a mask or to apply an enum mask to an int/ulong/Mask{***}
    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EnumMaskMethodAttribute : BaseNameAttribute
    {
        public EnumMaskMethodAttribute(string method) : base(method) { }
    }
}
