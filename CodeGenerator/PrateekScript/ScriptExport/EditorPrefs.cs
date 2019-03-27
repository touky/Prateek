// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 27/03/2019
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

//-----------------------------------------------------------------------------
namespace Prateek.Editors
{
    //-------------------------------------------------------------------------
    public static partial class Prefs
    {
        
        //---------------------------------------------------------------------
        #region Vector2
        public static Vector2s Get(string name, Vector2 defaultValue)
        {
            return new Vector2s(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector2s : ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
        
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector2 data
            {
                get
                {
                    return new Vector2(x.Value, y.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
        
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector2s(string name, Vector2 defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
        
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector2
        
        //---------------------------------------------------------------------
        #region Vector3
        public static Vector3s Get(string name, Vector3 defaultValue)
        {
            return new Vector3s(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector3s : ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
            protected Floats z;
        
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector3 data
            {
                get
                {
                    return new Vector3(x.Value, y.Value, z.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    z.Value = value.z;
        
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector3s(string name, Vector3 defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
                z = new Floats(name + ".z", defaultValue.z);
        
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector3
        
        //---------------------------------------------------------------------
        #region Vector4
        public static Vector4s Get(string name, Vector4 defaultValue)
        {
            return new Vector4s(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Vector4s : ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
            protected Floats z;
            protected Floats w;
        
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Vector4 data
            {
                get
                {
                    return new Vector4(x.Value, y.Value, z.Value, w.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    z.Value = value.z;
                    w.Value = value.w;
        
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Vector4s(string name, Vector4 defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
                z = new Floats(name + ".z", defaultValue.z);
                w = new Floats(name + ".w", defaultValue.w);
        
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Vector4
        
        //---------------------------------------------------------------------
        #region Rect
        public static Rects Get(string name, Rect defaultValue)
        {
            return new Rects(name, defaultValue);
        }
        
        //---------------------------------------------------------------------
        public class Rects : ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected Floats x;
            protected Floats y;
            protected Floats width;
            protected Floats height;
        
            #endregion Fields
        
            //-----------------------------------------------------------------
            #region Properties
            public Rect data
            {
                get
                {
                    return new Rect(x.Value, y.Value, width.Value, height.Value);
                }
                set
                {
                    x.Value = value.x;
                    y.Value = value.y;
                    width.Value = value.width;
                    height.Value = value.height;
        
                }
            }
            #endregion Properties
        
            //-----------------------------------------------------------------
            #region Behaviour
            public Rects(string name, Rect defaultValue) : base(name)
            {
                x = new Floats(name + ".x", defaultValue.x);
                y = new Floats(name + ".y", defaultValue.y);
                width = new Floats(name + ".width", defaultValue.width);
                height = new Floats(name + ".height", defaultValue.height);
        
            }
        
            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
            #endregion Behaviour
        }
        #endregion Rect
        
    }
}
