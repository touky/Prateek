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
using static Prateek.Debug.Draw.Setup.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    public static class Format
    {
        //---------------------------------------------------------------------
        #region Time
        public static string Time(float time, bool ignoreZeroHour = true)
        {
            return Time(time, "{3}:", "{2}:", "{1}:", "{0}", ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, bool litteral, bool ignoreZeroHour)
        {
            if (litteral)
                return Time(time, "{3}h", "{2}m", "{1}s", "{0}ms", ignoreZeroHour);
            return Time(time, ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, string MSFormat, bool ignoreZeroHour = true)
        {
            return Time(time, "{3}:", "{2}:", "{1}:", MSFormat, ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, string MSFormat, bool litteral, bool ignoreZeroHour)
        {
            if (litteral)
                return Time(time, "{3}h", "{2}m", "{1}s", MSFormat, ignoreZeroHour);
            return Time(time, MSFormat, ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, string SFormat, string MSFormat, bool ignoreZeroHour = true)
        {
            return Time(time, "{3}:", "{2}:", SFormat, MSFormat, ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, string SFormat, string MSFormat, bool litteral, bool ignoreZeroHour)
        {
            if (litteral)
                return Time(time, "{3}h", "{2}m", SFormat, MSFormat, ignoreZeroHour);
            return Time(time, SFormat, MSFormat, ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, string MFormat, string SFormat, string MSFormat, bool ignoreZeroHour = true)
        {
            return Time(time, "{3}:", MFormat, SFormat, MSFormat, ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, string MFormat, string SFormat, string MSFormat, bool litteral, bool ignoreZeroHour)
        {
            if (litteral)
                return Time(time, "{3}h", MFormat, SFormat, MSFormat, ignoreZeroHour);
            return Time(time, "{3}:", MFormat, SFormat, MSFormat, ignoreZeroHour);
        }

        //---------------------------------------------------------------------
        public static string Time(float time, string HFormat, string MFormat, string SFormat, string MSFormat, bool ignoreZeroHour = true)
        {
            int milliseconds = (int)((time - Mathf.Floor(time)) * 1000f);
            int seconds = ((int)time) % 60;
            int minutes = (((int)time) / 60) % 60;
            int hours = (((int)time) / 60) / 60;

            return string.Format((ignoreZeroHour && hours == 0 ? string.Empty : HFormat) + MFormat + SFormat + MSFormat, milliseconds, seconds, minutes, hours);
        }
        #endregion Time

        //---------------------------------------------------------------------
        #region Text coloring
        // return a color tag like "<color=#FFAA00FF">" to be rendered inside rich texts
        // Note: Color is implicitly converted to Color32, so it works for both types
        public static string ToRichText(this Color32 col)
        {
            return "<color=#" + col.r.ToString("X2") + col.g.ToString("X2") + col.b.ToString("X2") + col.a.ToString("X2") + ">";
        }

        //---------------------------------------------------------------------
        public static string Color(this string text, Color32 col)
        {
            return ToRichText(col) + text + "</color>";
        }
        #endregion Text coloring
    }
}