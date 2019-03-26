// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 26/03/2019
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

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
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
namespace Prateek.Editors
{
    //-------------------------------------------------------------------------
    public static partial class Prefs
    {
        //---------------------------------------------------------------------
        #region bool
        public static Bools Get(string name, bool defaultValue)
        {
            return new Bools(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Bools : TypedStorage<bool>
        {
            //-----------------------------------------------------------------
            public Bools(string name, bool defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(bool newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
            #if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetBool(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetBool(name, value);
            }
            #endif //UNITY_EDITOR
        }
        #endregion bool
        
        //---------------------------------------------------------------------
        #region int
        public static Ints Get(string name, int defaultValue)
        {
            return new Ints(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Ints : TypedStorage<int>
        {
            //-----------------------------------------------------------------
            public Ints(string name, int defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(int newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
            #if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetInt(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetInt(name, value);
            }
            #endif //UNITY_EDITOR
        }
        #endregion int
        
        //---------------------------------------------------------------------
        #region float
        public static Floats Get(string name, float defaultValue)
        {
            return new Floats(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Floats : TypedStorage<float>
        {
            //-----------------------------------------------------------------
            public Floats(string name, float defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(float newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
            #if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetFloat(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetFloat(name, value);
            }
            #endif //UNITY_EDITOR
        }
        #endregion float
        
        //---------------------------------------------------------------------
        #region string
        public static Strings Get(string name, string defaultValue)
        {
            return new Strings(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Strings : TypedStorage<string>
        {
            //-----------------------------------------------------------------
            public Strings(string name, string defaultValue) : base(name, defaultValue) { }
        
            //-----------------------------------------------------------------
            public override bool ShouldSetNewValue(string newValue)
            {
                return this.value != newValue;
            }
             
            //-----------------------------------------------------------------
            #if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                value = UnityEditor.EditorPrefs.GetString(name, defaultValue);
            }
        
            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetString(name, value);
            }
            #endif //UNITY_EDITOR
        }
        #endregion string
        
        
    }
}
