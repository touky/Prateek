// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

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
    public static class Prefs
    {
        public const float UPDATE_TIME = 5f;

        //---------------------------------------------------------------------
        public abstract class ValueStorage
        {
            #region Fields
            protected float m_last_update = 0;
            protected string m_name = string.Empty;
            #endregion Fields

            //-----------------------------------------------------------------
            #region Properties
            public string name { get { return m_name; } }
            protected bool should_update
            {
                get
                {
#if UNITY_EDITOR
                    if (m_last_update < Time.realtimeSinceStartup)
                    {
                        m_last_update = Time.realtimeSinceStartup + UPDATE_TIME;
                        return true;
                    }
#endif //UNITY_EDITOR
                    return false;
                }
            }
            #endregion Properties

            //-----------------------------------------------------------------
            #region Properties
            protected ValueStorage(string name)
            {
                m_name = name;
            }

            //-----------------------------------------------------------------
            protected void TryGetting()
            {
#if UNITY_EDITOR
                if (should_update)
                {
                    GetFromPrefs();
                }
#endif //UNITY_EDITOR
            }

            //-----------------------------------------------------------------
            protected void TrySetting(bool should_update)
            {
#if UNITY_EDITOR
                if (should_update)
                {
                    SetToPrefs();
                }
#endif //UNITY_EDITOR
            }

            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected virtual void GetFromPrefs() { }
            protected virtual void SetToPrefs() { }
#endif //UNITY_EDITOR
            #endregion ctor
        }

        //---------------------------------------------------------------------
        #region Bool
        public static Bools Get(string name, bool default_value)
        {
            return new Bools(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Bools : ValueStorage
        {
            #region Fields
            protected bool m_value;
            protected bool m_default_value;
            #endregion Fields

            public bool data
            {
                get
                {
                    TryGetting();
                    return m_value;
                }
                set
                {
                    var do_update = m_value != value;
                    m_value = value;
                    TrySetting(do_update);
                }
            }

            public Bools(string name, bool default_value)
                : base(name)
            {
                m_value = default_value;
                m_default_value = default_value;
            }

            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                m_value = UnityEditor.EditorPrefs.GetBool(m_name, m_default_value);
            }

            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetBool(m_name, m_value);
            }
#endif //UNITY_EDITOR
        }
        #endregion Bool

        //---------------------------------------------------------------------
        #region Int
        public static Ints Get(string name, int default_value)
        {
            return new Ints(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Ints : ValueStorage
        {
            #region Fields
            protected int m_value;
            protected int m_default_value;
            #endregion Fields

            public int data
            {
                get
                {
                    TryGetting();
                    return m_value;
                }
                set
                {
                    var do_update = m_value != value;
                    m_value = value;
                    TrySetting(do_update);
                }
            }

            public Ints(string name, int default_value)
                : base(name)
            {
                m_value = default_value;
                m_default_value = default_value;
            }

            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                m_value = UnityEditor.EditorPrefs.GetInt(m_name, m_default_value);
            }

            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetInt(m_name, m_value);
            }
#endif //UNITY_EDITOR
        }
        #endregion Int

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

            public ulong data
            {
                get
                {
                    return ((ulong)m_f0.data << 32) | (uint)m_0f.data;
                }
                set
                {
                    m_0f.data = (int)((value << 32) >> 32);
                    m_f0.data = (int)(value >> 32);
                }
            }

            public ULongs(string name, ulong default_value)
                : base(name)
            {
                m_0f = new Ints(name + ".0f", (int)((default_value << 32) >> 32));
                m_f0 = new Ints(name + ".f0", (int)(default_value >> 32));
            }
        }
        #endregion ULong

        //---------------------------------------------------------------------
        #region Float
        public static Floats Get(string name, float default_value)
        {
            return new Floats(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Floats : ValueStorage
        {
            #region Fields
            protected float m_value;
            protected float m_default_value;
            #endregion Fields

            public float data
            {
                get
                {
                    TryGetting();
                    return m_value;
                }
                set
                {
                    var do_update = m_value != value;
                    m_value = value;
                    TrySetting(do_update);
                }
            }

            public Floats(string name, float default_value)
                : base(name)
            {
                m_value = default_value;
                m_default_value = default_value;
            }

            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                m_value = UnityEditor.EditorPrefs.GetFloat(m_name, m_default_value);
            }

            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetFloat(m_name, m_value);
            }
#endif //UNITY_EDITOR
        }
        #endregion Float

        //---------------------------------------------------------------------
        #region String
        public static Strings Get(string name, string default_value)
        {
            return new Strings(name, default_value);
        }

        //---------------------------------------------------------------------
        public class Strings : ValueStorage
        {
            #region Fields
            protected string m_value;
            protected string m_default_value;
            #endregion Fields

            public string Value
            {
                get
                {
                    TryGetting();
                    return m_value;
                }
                set
                {
                    var do_update = m_value != value;
                    m_value = value;
                    TrySetting(do_update);
                }
            }

            public Strings(string name, string default_value)
                : base(name)
            {
                m_value = default_value;
                m_default_value = default_value;
            }

            //-----------------------------------------------------------------
#if UNITY_EDITOR
            protected override void GetFromPrefs()
            {
                m_value = UnityEditor.EditorPrefs.GetString(m_name, m_default_value);
            }

            //-----------------------------------------------------------------
            protected override void SetToPrefs()
            {
                UnityEditor.EditorPrefs.SetString(m_name, m_value);
            }
#endif //UNITY_EDITOR
        }
        #endregion String

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
                    return new Vector2(m_x.data, m_y.data);
                }
                set
                {
                    m_x.data = value.x;
                    m_y.data = value.y;
                }
            }

            public Vector2s(string name, Vector2 default_value)
                : base(name)
            {
                m_x = new Floats(name + ".x", default_value.x);
                m_y = new Floats(name + ".y", default_value.y);
            }
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
                    return new Vector3(m_x.data, m_y.data, m_z.data);
                }
                set
                {
                    m_x.data = value.x;
                    m_y.data = value.y;
                    m_z.data = value.z;
                }
            }

            public Vector3s(string name, Vector3 default_value)
                : base(name)
            {
                m_x = new Floats(name + ".x", default_value.x);
                m_y = new Floats(name + ".y", default_value.y);
                m_z = new Floats(name + ".z", default_value.z);
            }
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
                    return new Vector4(m_x.data, m_y.data, m_z.data, m_w.data);
                }
                set
                {
                    m_x.data = value.x;
                    m_y.data = value.y;
                    m_z.data = value.z;
                    m_w.data = value.w;
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
                    return new Rect(m_x.data, m_y.data, m_width.data, m_height.data);
                }
                set
                {
                    m_x.data = value.x;
                    m_y.data = value.y;
                    m_width.data = value.width;
                    m_height.data = value.height;
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
        }
        #endregion Rect

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
                        m_values[i] = new ULongs(String.Format("{0}.{1}", name, GetPostfix(i)), 0);
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
                        mask.Set(i, m_values[i].data);
                    }
                    return mask;
                }
                set
                {
                    Resize(Helpers.Mask128.MAX_SIZE);
                    for (int i = 0; i < Helpers.Mask128.MAX_SIZE; i++)
                    {
                        m_values[i].data = value.Get(i);
                    }
                }
            }

            //-----------------------------------------------------------------
            public Mask128s(string name, Helpers.Mask128 default_value)
                : base(name)
            {
                Resize(Helpers.Mask128.MAX_SIZE);
            }
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
                        mask.Set(i, m_values[i].data);
                    }
                    return mask;
                }
                set
                {
                    Resize(Helpers.Mask256.MAX_SIZE);
                    for (int i = 0; i < Helpers.Mask256.MAX_SIZE; i++)
                    {
                        m_values[i].data = value.Get(i);
                    }
                }
            }

            //-----------------------------------------------------------------
            public Mask256s(string name, Helpers.Mask256 default_value)
                : base(name)
            {
                Resize(Helpers.Mask256.MAX_SIZE);
            }
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
                        mask.Set(i, m_values[i].data);
                    }
                    return mask;
                }
                set
                {
                    Resize(Helpers.Mask512.MAX_SIZE);
                    for (int i = 0; i < Helpers.Mask512.MAX_SIZE; i++)
                    {
                        m_values[i].data = value.Get(i);
                    }
                }
            }

            //-----------------------------------------------------------------
            public Mask512s(string name, Helpers.Mask512 default_value)
                : base(name)
            {
                Resize(Helpers.Mask512.MAX_SIZE);
            }
        }
        #endregion Mask
    }
}
