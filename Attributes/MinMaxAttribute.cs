// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
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

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
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
    public abstract class MathAttribute : PropertyAttribute
    {
        public enum ValueType
        {
            Int,
            Float
        }

        #region Fields
        protected ValueType m_value_type = ValueType.Int;

        protected int m_min_int;
        protected int m_max_int;

        protected float m_min_float;
        protected float m_max_float;
        #endregion Fields

        #region Fields
        public ValueType value_type { get { return m_value_type; } }

        public int min_int { get { return m_min_int; } }
        public int max_int { get { return m_max_int; } }

        public float min_float { get { return m_min_float; } }
        public float max_float { get { return m_max_float; } }
        #endregion Fields

        //---------------------------------------------------------------------
        protected MathAttribute(int min_value, int max_value)
        {
            m_value_type = ValueType.Int;
            m_min_int = min_value;
            m_max_int = max_value;
        }

        //---------------------------------------------------------------------
        protected MathAttribute(float min_float, float max_float)
        {
            m_value_type = ValueType.Float;
            m_min_float = min_float;
            m_max_float = max_float;
        }
    }

    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MinAttribute : MathAttribute
    {
        public MinAttribute(float value) : base(value, float.MaxValue) { }
        public MinAttribute(double value) : base((float)value, float.MaxValue) { }
        public MinAttribute(int value) : base(value, int.MaxValue) { }
    }

    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class MaxAttribute : MathAttribute
    {
        public MaxAttribute(float value) : base(float.MinValue, value) { }
        public MaxAttribute(double value) : base(float.MinValue, (float)value) { }
        public MaxAttribute(int value) : base(int.MinValue, value) { }
    }

    //-------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ClampAttribute : MathAttribute
    {
        public ClampAttribute(float min, float max) : base(min, max) { }
        public ClampAttribute(double min, double max) : base((float)min, (float)max) { }
        public ClampAttribute(int min, int max) : base(min, max) { }
    }
}
