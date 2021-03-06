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
namespace Prateek.Editor.Debug
{
    using Prateek.Runtime.Core.Helpers;
    using UnityEditor;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static class AutoCheck
    {
        public const float UPDATE_TIME = 5f;

        ///---------------------------------------------------------------------
        public abstract class ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected bool hasChanged = false;
            protected bool isChecking = false;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public bool HasChanged
            {
                get
                {
                    var result = hasChanged;
                    hasChanged = false;
                    return result;
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            protected ValueStorage()
            {
            }

            ///-----------------------------------------------------------------
            protected void Begin()
            {
                hasChanged = false;
                isChecking = true;
                EditorGUI.BeginChangeCheck();
            }

            ///-----------------------------------------------------------------
            protected void End()
            {
                hasChanged = isChecking ? EditorGUI.EndChangeCheck() : hasChanged;
                isChecking = false;
            }
            #endregion ctor
        }

        ///---------------------------------------------------------------------
        #region Bool
        public class Bools : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected bool m_value;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public bool data
            {
                get
                {
                    Begin();
                    return m_value;
                }
                set
                {
                    m_value = value;
                    End();
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Bools(bool value) { return new Bools(value); }
            public Bools(bool value) { m_value = value; }
            #endregion ctor
        }
        #endregion Bool

        ///---------------------------------------------------------------------
        #region Int
        public class Ints : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected int m_value;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public int data
            {
                get
                {
                    Begin();
                    return m_value;
                }
                set
                {
                    m_value = value;
                    End();
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Ints(int value) { return new Ints(value); }
            public Ints(int value) { m_value = value; }
            #endregion ctor
        }
        #endregion Int

        ///---------------------------------------------------------------------
        #region ULong
        public class ULongs : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected ulong m_value;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public ulong data
            {
                get
                {
                    Begin();
                    return m_value;
                }
                set
                {
                    m_value = value;
                    End();
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator ULongs(ulong value) { return new ULongs(value); }
            public ULongs(ulong value) { m_value = value; }
            #endregion ctor
        }
        #endregion ULong

        ///---------------------------------------------------------------------
        #region Float
        public class Floats : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected float m_value;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public float data
            {
                get
                {
                    Begin();
                    return m_value;
                }
                set
                {
                    m_value = value;
                    End();
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Floats(float value) { return new Floats(value); }
            public Floats(float value) { m_value = value; }
            #endregion ctor
        }
        #endregion Float

        ///---------------------------------------------------------------------
        #region String
        public class Strings : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected string m_value;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public string data
            {
                get
                {
                    Begin();
                    return m_value;
                }
                set
                {
                    m_value = value;
                    End();
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Strings(string value) { return new Strings(value); }
            public Strings(string value) { m_value = value; }
            #endregion ctor
        }
        #endregion String

        ///---------------------------------------------------------------------
        #region Vector2
        public class Vector2s : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public float x
            {
                get
                {
                    return m_x.data;
                }
                set
                {
                    m_x.data = value;
                    hasChanged = m_x.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float y
            {
                get
                {
                    return m_y.data;
                }
                set
                {
                    m_y.data = value;
                    hasChanged = m_y.HasChanged;
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Vector2s(Vector2 value) { return new Vector2s(value); }
            public Vector2s(Vector2 value) { m_x = value.x; m_y = value.y; }
            #endregion ctor
        }
        #endregion Vector2

        ///---------------------------------------------------------------------
        #region Vector3
        public class Vector3s : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            protected Floats m_z;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public float x
            {
                get
                {
                    return m_x.data;
                }
                set
                {
                    m_x.data = value;
                    hasChanged = m_x.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float y
            {
                get
                {
                    return m_y.data;
                }
                set
                {
                    m_y.data = value;
                    hasChanged = m_y.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float z
            {
                get
                {
                    return m_z.data;
                }
                set
                {
                    m_z.data = value;
                    hasChanged = m_z.HasChanged;
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Vector3s(Vector3 value) { return new Vector3s(value); }
            public Vector3s(Vector3 value) { m_x = value.x; m_y = value.y; m_z = value.z; }
            #endregion ctor
        }
        #endregion Vector3

        ///---------------------------------------------------------------------
        #region Vector4
        public class Vector4s : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            protected Floats m_z;
            protected Floats m_w;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public float x
            {
                get
                {
                    return m_x.data;
                }
                set
                {
                    m_x.data = value;
                    hasChanged = m_x.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float y
            {
                get
                {
                    return m_y.data;
                }
                set
                {
                    m_y.data = value;
                    hasChanged = m_y.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float z
            {
                get
                {
                    return m_z.data;
                }
                set
                {
                    m_z.data = value;
                    hasChanged = m_z.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float w
            {
                get
                {
                    return m_w.data;
                }
                set
                {
                    m_w.data = value;
                    hasChanged = m_w.HasChanged;
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Vector4s(Vector4 value) { return new Vector4s(value); }
            public Vector4s(Vector4 value) { m_x = value.x; m_y = value.y; m_z = value.z; m_w = value.w; }
            #endregion ctor
        }
        #endregion Vector4

        ///---------------------------------------------------------------------
        #region Rect
        public class Rects : ValueStorage
        {
            ///-----------------------------------------------------------------
            #region Fields
            protected Floats m_x;
            protected Floats m_y;
            protected Floats m_width;
            protected Floats m_height;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public float x
            {
                get
                {
                    return m_x.data;
                }
                set
                {
                    m_x.data = value;
                    hasChanged = m_x.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float y
            {
                get
                {
                    return m_y.data;
                }
                set
                {
                    m_y.data = value;
                    hasChanged = m_y.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float width
            {
                get
                {
                    return m_width.data;
                }
                set
                {
                    m_width.data = value;
                    hasChanged = m_width.HasChanged;
                }
            }

            ///-----------------------------------------------------------------
            public float height
            {
                get
                {
                    return m_height.data;
                }
                set
                {
                    m_height.data = value;
                    hasChanged = m_height.HasChanged;
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Rects(Rect value) { return new Rects(value); }
            public Rects(Rect value) { m_x = value.x; m_y = value.y; m_width = value.width; m_height = value.height; }
            #endregion ctor
        }
        #endregion Rect

        ///---------------------------------------------------------------------
        #region Mask
        public abstract class Masks : ValueStorage
        {
            #region Fields
            protected ULongs[] m_values;
            #endregion Fields

            ///-----------------------------------------------------------------
            #region Properties
            public ulong this[int index]
            {
                get
                {
                    return m_values[index].data;
                }
                set
                {
                    m_values[index].data = value;
                    hasChanged = m_values[index].HasChanged;
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            protected Masks(params ulong[] values)
            {
                Resize(values.Length);

                for (int i = 0; i < values.Length; i++)
                {
                    this[i] = values[i];
                }
            }

            ///-----------------------------------------------------------------
            private void Resize(int size)
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
                        m_values[i] = new ULongs(0);
                    }
                }
            }
            #endregion ctor
        }

        ///---------------------------------------------------------------------
        public class Mask128s : Masks
        {
            ///-----------------------------------------------------------------
            #region Properties
            public Mask128 data
            {
                get
                {
                    return new Mask128(this[0], this[1]);
                }
                set
                {
                    for (int i = 0; i < 2; i++)
                    {
                        this[i] = value.Get(i);
                    }
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Mask128s(Mask128 value) { return new Mask128s(value); }
            public Mask128s(Mask128 value) : base(value.Get(0), value.Get(1)) { }
            #endregion ctor
        }

        ///---------------------------------------------------------------------
        public class Mask256s : Masks
        {
            ///-----------------------------------------------------------------
            #region Properties
            public Mask128 data
            {
                get
                {
                    return new Mask128(this[0], this[1], this[2], this[3]);
                }
                set
                {
                    for (int i = 0; i < 4; i++)
                    {
                        this[i] = value.Get(i);
                    }
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Mask256s(Mask256 value) { return new Mask256s(value); }
            public Mask256s(Mask256 value) : base(value.Get(0), value.Get(1), value.Get(2), value.Get(3)) { }
            #endregion ctor
        }

        ///---------------------------------------------------------------------
        public class Mask512s : Masks
        {
            ///-----------------------------------------------------------------
            #region Properties
            public Mask512 data
            {
                get
                {
                    return new Mask512(this[0], this[1], this[2], this[3],
                                               this[4], this[5], this[6], this[7]);
                }
                set
                {
                    for (int i = 0; i < 8; i++)
                    {
                        this[i] = value.Get(i);
                    }
                }
            }
            #endregion Properties

            ///-----------------------------------------------------------------
            #region ctor
            public static implicit operator Mask512s(Mask512 value) { return new Mask512s(value); }
            public Mask512s(Mask512 value) : base(value.Get(0), value.Get(1), value.Get(2), value.Get(3),
                                                          value.Get(4), value.Get(5), value.Get(6), value.Get(7))
            { }
            #endregion ctor
        }
        #endregion Mask
    }
}