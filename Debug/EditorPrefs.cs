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
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Editors
{
    //-------------------------------------------------------------------------
    public static partial class Prefs
    {
        //---------------------------------------------------------------------
        public const float UPDATE_TIME = 5f;

        //---------------------------------------------------------------------
        public abstract class ValueStorage
        {
            #region Fields
            protected float lastUpdate = 0;
            protected string name = string.Empty;
            #endregion Fields

            //-----------------------------------------------------------------
            #region Properties
            public string Name { get { return name; } }
            protected bool ShouldUpdate
            {
                get
                {
#if UNITY_EDITOR
                    if (lastUpdate < Time.realtimeSinceStartup)
                    {
                        lastUpdate = Time.realtimeSinceStartup + UPDATE_TIME;
                        return true;
                    }
#endif //UNITY_EDITOR
                    return false;
                }
            }
            #endregion Properties

            //-----------------------------------------------------------------
            #region CTor
            protected ValueStorage(string name)
            {
                this.name = name;
            }
            #endregion CTor

            //-----------------------------------------------------------------
            #region Get/Set
            protected void TryGetting()
            {
#if UNITY_EDITOR
                if (ShouldUpdate)
                {
                    GetFromPrefs();
                }
#endif //UNITY_EDITOR
            }

            //-----------------------------------------------------------------
            protected void TrySetting(bool shouldUpdate)
            {
#if UNITY_EDITOR
                if (shouldUpdate)
                {
                    SetToPrefs();
                }
#endif //UNITY_EDITOR
            }
            #endregion Get/Set

            //-----------------------------------------------------------------
            #region Prefs
#if UNITY_EDITOR
            public void ClearFromPrefs()
            {
                UnityEditor.EditorPrefs.DeleteKey(name);
            }

            //-----------------------------------------------------------------
            protected abstract void GetFromPrefs();
            protected abstract void SetToPrefs();
#endif //UNITY_EDITOR
            #endregion Prefs
        }

        //---------------------------------------------------------------------
        public abstract class TypedStorage<T> : ValueStorage
        {
            //-----------------------------------------------------------------
            #region Fields
            protected T value;
            protected T defaultValue;
            #endregion Fields

            //-----------------------------------------------------------------
            #region Properties
            public T Value
            {
                get
                {
                    TryGetting();
                    return value;
                }
                set
                {
                    var doUpdate = ShouldSetNewValue(value);
                    this.value = value;
                    TrySetting(doUpdate);
                }
            }
            #endregion Properties

            //-----------------------------------------------------------------
            #region Behaviour
            public TypedStorage(string name, T defaultValue) : base(name)
            {
                value = defaultValue;
                this.defaultValue = defaultValue;

                TryGetting();
            }

            //-----------------------------------------------------------------
            public abstract bool ShouldSetNewValue(T newValue);
            #endregion Behaviour
        }

        //---------------------------------------------------------------------
        #region List<string>
        public static ListStrings Get(string name, List<string> default_value)
        {
            return new ListStrings(name, default_value);
        }

        //---------------------------------------------------------------------
        public class ListStrings : ValueStorage
        {
            #region Fields
            protected Ints count;
            protected List<Strings> strings = new List<Strings>();
            protected List<string> values = new List<string>();
            #endregion Fields

            public List<string> data
            {
                get
                {
                    return values;
                }
                set
                {
                    var valueCount = value == null ? 0 : value.Count;
                    if (valueCount != strings.Count)
                    {
                        while (strings.Count > valueCount)
                        {
                            var last = strings.Last() as ValueStorage;
                            last.ClearFromPrefs();
                            strings.RemoveLast();
                        }

                        while (strings.Count < valueCount)
                        {
                            strings.Add(new Strings(name + "[" + strings.Count + "]", value[strings.Count]));
                        }
                    }

                    if (value != null)
                    {
                        for (int v = 0; v < value.Count; v++)
                        {
                            strings[v].Value = value[v];
                        }
                        values = value;
                    }
                    else
                    {
                        values.Clear();
                    }
                }
            }

            public ListStrings(string name, List<string> default_value)
                : base(name)
            {
                this.name = name;
                count = new Ints(base.name + ".Count", default_value == null ? 0 : default_value.Count);
                data = default_value;
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion List<string>

        //---------------------------------------------------------------------
        #region Vector2
        public static Vector2s Get(string name, Vector2 default_value)
        {
            return new Vector2s(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Vector2s : ValueStorage
        {
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            #endregion Fields

            public Vector2 data
            {
                get
                {
                    return new Vector2(m_x.Value, m_y.Value);
                }
                set
                {
                    m_x.Value = value.x;
                    m_y.Value = value.y;
                }
            }

            public Vector2s(string name, Vector2 default_value)
                : base(name)
            {
                m_x = new Floats(name + ".x", default_value.x);
                m_y = new Floats(name + ".y", default_value.y);
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion Vector2

        //---------------------------------------------------------------------
        #region Vector3
        public static Vector3s Get(string name, Vector3 default_value)
        {
            return new Vector3s(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Vector3s : ValueStorage
        {
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            protected Floats m_z;
            #endregion Fields

            public Vector3 data
            {
                get
                {
                    return new Vector3(m_x.Value, m_y.Value, m_z.Value);
                }
                set
                {
                    m_x.Value = value.x;
                    m_y.Value = value.y;
                    m_z.Value = value.z;
                }
            }

            public Vector3s(string name, Vector3 default_value)
                : base(name)
            {
                m_x = new Floats(name + ".x", default_value.x);
                m_y = new Floats(name + ".y", default_value.y);
                m_z = new Floats(name + ".z", default_value.z);
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion Vector3

        //---------------------------------------------------------------------
        #region Vector4
        public static Vector4s Get(string name, Vector4 default_value)
        {
            return new Vector4s(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Vector4s : ValueStorage
        {
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            protected Floats m_z;
            protected Floats m_w;
            #endregion Fields

            public Vector4 data
            {
                get
                {
                    return new Vector4(m_x.Value, m_y.Value, m_z.Value, m_w.Value);
                }
                set
                {
                    m_x.Value = value.x;
                    m_y.Value = value.y;
                    m_z.Value = value.z;
                    m_w.Value = value.w;
                }
            }

            public Vector4s(string name, Vector4 default_value)
                : base(name)
            {
                m_x = new Floats(name + ".x", default_value.x);
                m_y = new Floats(name + ".y", default_value.y);
                m_z = new Floats(name + ".z", default_value.z);
                m_w = new Floats(name + ".w", default_value.w);
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion Vector4

        //---------------------------------------------------------------------
        #region Rect
        public static Rects Get(string name, Rect default_value)
        {
            return new Rects(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Rects : ValueStorage
        {
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            protected Floats m_width;
            protected Floats m_height;
            #endregion Fields

            public Rect data
            {
                get
                {
                    return new Rect(m_x.Value, m_y.Value, m_width.Value, m_height.Value);
                }
                set
                {
                    m_x.Value = value.x;
                    m_y.Value = value.y;
                    m_width.Value = value.width;
                    m_height.Value = value.height;
                }
            }

            public Rects(string name, Rect default_value)
                : base(name)
            {
                m_x = new Floats(name + ".x", default_value.x);
                m_y = new Floats(name + ".y", default_value.y);
                m_width = new Floats(name + ".width", default_value.width);
                m_height = new Floats(name + ".heigth", default_value.height);
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion Rect

        //---------------------------------------------------------------------
        #region ULong
        public static ULongs Get(string name, ulong default_value)
        {
            return new ULongs(name, default_value);
        }

        //---------------------------------------------------------------------
        public class ULongs : ValueStorage
        {
            #region Fields
            protected Ints m_0f;
            protected Ints m_f0;
            #endregion Fields

            public ulong Value
            {
                get
                {
                    return ((ulong)m_f0.Value << 32) | (uint)m_0f.Value;
                }
                set
                {
                    m_0f.Value = (int)((value << 32) >> 32);
                    m_f0.Value = (int)(value >> 32);
                }
            }

            public ULongs(string name, ulong default_value)
                : base(name)
            {
                m_0f = new Ints(name + ".0f", (int)((default_value << 32) >> 32));
                m_f0 = new Ints(name + ".f0", (int)(default_value >> 32));
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion ULong

        //---------------------------------------------------------------------
        #region Mask
        public static Mask128s Get(string name, Helpers.Mask128 default_value)
        {
            return new Mask128s(name, default_value);
        }

        //---------------------------------------------------------------------
        public static Mask256s Get(string name, Helpers.Mask256 default_value)
        {
            return new Mask256s(name, default_value);
        }

        //---------------------------------------------------------------------
        public static Mask512s Get(string name, Helpers.Mask512 default_value)
        {
            return new Mask512s(name, default_value);
        }

        //---------------------------------------------------------------------
        public abstract class Masks : ValueStorage
        {
            #region Fields
            protected ULongs[] m_values;
            #endregion Fields

            //-----------------------------------------------------------------
            protected string GetPostfix(int max)
            {
                var postfix = "f";
                for (int s = 0; s < max; s++)
                {
                    postfix = String.Format("0{0}", postfix);
                }
                return postfix;
            }

            //-----------------------------------------------------------------
            protected void Resize(int size)
            {
                if (m_values == null)
                {
                    m_values = new ULongs[0];
                }

                if (size != m_values.Length)
                {
                    var old_values = m_values;
                    m_values = new ULongs[size];

                    for (int i = 0; i < old_values.Length && i < m_values.Length; i++)
                    {
                        m_values[i] = old_values[i];
                    }

                    for (int i = old_values.Length; i < m_values.Length; i++)
                    {
                        m_values[i] = new ULongs(String.Format("{0}.{1}", Name, GetPostfix(i)), 0);
                    }
                }
            }

            //-----------------------------------------------------------------
            protected Masks(string name)
                : base(name)
            { }
        }

        //---------------------------------------------------------------------
        public class Mask128s : Masks
        {
            //-----------------------------------------------------------------
            public Helpers.Mask128 data
            {
                get
                {
                    var mask = new Helpers.Mask128();
                    for (int i = 0; i < m_values.Length; i++)
                    {
                        mask.Set(i, m_values[i].Value);
                    }
                    return mask;
                }
                set
                {
                    Resize(Helpers.Mask128.MAX_SIZE);
                    for (int i = 0; i < Helpers.Mask128.MAX_SIZE; i++)
                    {
                        m_values[i].Value = value.Get(i);
                    }
                }
            }

            //-----------------------------------------------------------------
            public Mask128s(string name, Helpers.Mask128 default_value)
                : base(name)
            {
                Resize(Helpers.Mask128.MAX_SIZE);
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }

        //---------------------------------------------------------------------
        public class Mask256s : Masks
        {
            //-----------------------------------------------------------------
            public Helpers.Mask256 data
            {
                get
                {
                    var mask = new Helpers.Mask256();
                    for (int i = 0; i < m_values.Length; i++)
                    {
                        mask.Set(i, m_values[i].Value);
                    }
                    return mask;
                }
                set
                {
                    Resize(Helpers.Mask256.MAX_SIZE);
                    for (int i = 0; i < Helpers.Mask256.MAX_SIZE; i++)
                    {
                        m_values[i].Value = value.Get(i);
                    }
                }
            }

            //-----------------------------------------------------------------
            public Mask256s(string name, Helpers.Mask256 default_value)
                : base(name)
            {
                Resize(Helpers.Mask256.MAX_SIZE);
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }

        //---------------------------------------------------------------------
        public class Mask512s : Masks
        {
            //-----------------------------------------------------------------
            public Helpers.Mask512 data
            {
                get
                {
                    var mask = new Helpers.Mask512();
                    for (int i = 0; i < m_values.Length; i++)
                    {
                        mask.Set(i, m_values[i].Value);
                    }
                    return mask;
                }
                set
                {
                    Resize(Helpers.Mask512.MAX_SIZE);
                    for (int i = 0; i < Helpers.Mask512.MAX_SIZE; i++)
                    {
                        m_values[i].Value = value.Get(i);
                    }
                }
            }

            //-----------------------------------------------------------------
            public Mask512s(string name, Helpers.Mask512 default_value)
                : base(name)
            {
                Resize(Helpers.Mask512.MAX_SIZE);
            }

            //-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion Mask
    }
}
