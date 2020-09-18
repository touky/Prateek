// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.Editor.Core.EditorPrefs
{
    using System;
    using Prateek.Runtime.Core.Helpers;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static partial class Prefs
    {
        ///---------------------------------------------------------------------
        public const float UPDATE_TIME = 5f;

        ///---------------------------------------------------------------------
        public abstract class ValueStorage
        {
            #region Fields
            protected float lastUpdate = 0;
            protected string name = string.Empty;
            #endregion Fields

            ///-----------------------------------------------------------------
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

            ///-----------------------------------------------------------------
            #region CTor
            protected ValueStorage(string name)
            {
                this.name = name;
            }
            #endregion CTor

            ///-----------------------------------------------------------------
            #region Get/Set
            protected void TryGetting(bool forceUpdate)
            {
#if UNITY_EDITOR
                if (ShouldUpdate || forceUpdate)
                {
                    GetFromPrefs();
                }
#endif //UNITY_EDITOR
            }

            ///-----------------------------------------------------------------
            protected void TrySetting(bool forceUpdate)
            {
#if UNITY_EDITOR
                if (ShouldUpdate || forceUpdate)
                {
                    SetToPrefs();
                }
#endif //UNITY_EDITOR
            }
            #endregion Get/Set

            ///-----------------------------------------------------------------
            #region Prefs
#if UNITY_EDITOR
            public void ClearFromPrefs()
            {
                UnityEditor.EditorPrefs.DeleteKey(name);
            }

            ///-----------------------------------------------------------------
            protected abstract void GetFromPrefs();
            protected abstract void SetToPrefs();
#endif //UNITY_EDITOR
            #endregion Prefs
        }

        ///---------------------------------------------------------------------
        public abstract class TypedStorage<T> : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected T value;
            protected T defaultValue;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public T Value
            {
                get
                {
                    TryGetting(false);
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

            ///-----------------------------------------------------------------
            #region Behaviour
            public TypedStorage(string name, T defaultValue) : base(name)
            {
                value = defaultValue;
                this.defaultValue = defaultValue;

                TryGetting(true);
            }

            ///-----------------------------------------------------------------
            public abstract bool ShouldSetNewValue(T newValue);
            #endregion Behaviour
        }

        ///---------------------------------------------------------------------
        #region ULong
        public static ULongs Get(string name, ulong default_value)
        {
            return new ULongs(name, default_value);
        }

        ///---------------------------------------------------------------------
        public class ULongs : ValueStorage
        {
            #region Fields
            protected Prefs.Ints m_0f;
            protected Prefs.Ints m_f0;
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
                m_0f = new Prefs.Ints(name + ".0f", (int)((default_value << 32) >> 32));
                m_f0 = new Prefs.Ints(name + ".f0", (int)(default_value >> 32));
            }

            ///-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion ULong

        ///---------------------------------------------------------------------
        #region Mask
        public static Mask128s Get(string name, Mask128 default_value)
        {
            return new Mask128s(name, default_value);
        }

        ///---------------------------------------------------------------------
        public static Mask256s Get(string name, Mask256 default_value)
        {
            return new Mask256s(name, default_value);
        }

        ///---------------------------------------------------------------------
        public static Mask512s Get(string name, Mask512 default_value)
        {
            return new Mask512s(name, default_value);
        }

        ///---------------------------------------------------------------------
        public abstract class Masks : ValueStorage
        {
            #region Fields
            protected ULongs[] m_values;
            #endregion Fields

            ///-----------------------------------------------------------------
            protected string GetPostfix(int max)
            {
                var postfix = "f";
                for (int s = 0; s < max; s++)
                {
                    postfix = String.Format("0{0}", postfix);
                }
                return postfix;
            }

            ///-----------------------------------------------------------------
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

            ///-----------------------------------------------------------------
            protected Masks(string name)
                : base(name)
            { }
        }

        ///---------------------------------------------------------------------
        public class Mask128s : Masks
        {
            ///-----------------------------------------------------------------
            public Mask128 data
            {
                get
                {
                    var mask = new Mask128();
                    for (int i = 0; i < m_values.Length; i++)
                    {
                        mask.Set(i, m_values[i].Value);
                    }
                    return mask;
                }
                set
                {
                    Resize(Mask128.MAX_SIZE);
                    for (int i = 0; i < Mask128.MAX_SIZE; i++)
                    {
                        m_values[i].Value = value.Get(i);
                    }
                }
            }

            ///-----------------------------------------------------------------
            public Mask128s(string name, Mask128 default_value)
                : base(name)
            {
                Resize(Mask128.MAX_SIZE);
            }

            ///-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }

        ///---------------------------------------------------------------------
        public class Mask256s : Masks
        {
            ///-----------------------------------------------------------------
            public Mask256 data
            {
                get
                {
                    var mask = new Mask256();
                    for (int i = 0; i < m_values.Length; i++)
                    {
                        mask.Set(i, m_values[i].Value);
                    }
                    return mask;
                }
                set
                {
                    Resize(Mask256.MAX_SIZE);
                    for (int i = 0; i < Mask256.MAX_SIZE; i++)
                    {
                        m_values[i].Value = value.Get(i);
                    }
                }
            }

            ///-----------------------------------------------------------------
            public Mask256s(string name, Mask256 default_value)
                : base(name)
            {
                Resize(Mask256.MAX_SIZE);
            }

            ///-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }

        ///---------------------------------------------------------------------
        public class Mask512s : Masks
        {
            ///-----------------------------------------------------------------
            public Mask512 data
            {
                get
                {
                    var mask = new Mask512();
                    for (int i = 0; i < m_values.Length; i++)
                    {
                        mask.Set(i, m_values[i].Value);
                    }
                    return mask;
                }
                set
                {
                    Resize(Mask512.MAX_SIZE);
                    for (int i = 0; i < Mask512.MAX_SIZE; i++)
                    {
                        m_values[i].Value = value.Get(i);
                    }
                }
            }

            ///-----------------------------------------------------------------
            public Mask512s(string name, Mask512 default_value)
                : base(name)
            {
                Resize(Mask512.MAX_SIZE);
            }

            ///-----------------------------------------------------------------
            protected override void GetFromPrefs() { }
            protected override void SetToPrefs() { }
        }
        #endregion Mask
    }
}
