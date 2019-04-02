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
using static Prateek.Debug.Draw.Style.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Extensions
{
    //-------------------------------------------------------------------------
    public static partial class RectExt
    {
        //---------------------------------------------------------------------
        #region Declarations
        public static Rect Inflate(this Rect rect, float value)
        {
            return Inflate(rect, Vector2.one * value);
        }

        //---------------------------------------------------------------------
        public static Rect Inflate(this Rect rect, Vector2 value)
        {
            rect.position -= value * sign(rect.size);
            rect.size += value * sign(rect.size) * 2;
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect TruncateX(this Rect rect, float size)
        {
            if (size > 0)
            {
                rect.x += size;
            }
            rect.width -= abs(size);
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect TruncateY(this Rect rect, float size)
        {
            if (size > 0)
            {
                rect.y += size;
            }
            rect.height -= abs(size);
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect NextLine(this Rect rect)
        {
            rect.y += rect.height;
            return rect;
        }

        //---------------------------------------------------------------------
        public static Rect NextColumn(this Rect rect)
        {
            rect.x += rect.width;
            return rect;
        }
        #endregion Declarations
    }
}