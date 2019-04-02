// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
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
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

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
        protected Type value = null;

        public Type Value { get { return value; } }

        public EnumMaskAttribute() { }
        public EnumMaskAttribute(Type enumType)
        {
            value = enumType;
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
