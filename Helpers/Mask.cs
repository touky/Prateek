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
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public struct MaskFlag
    {
        private int m_flag;
        private ulong m_mask;
        private int m_offset;

        public int flag { get { return m_flag; } }
        public ulong mask { get { return m_mask; } }
        public int offset { get { return m_offset; } }
        public static MaskFlag zero { get { return new MaskFlag(); } }

        //---------------------------------------------------------------------
        public static implicit operator MaskFlag(int flag_index)
        {
            return new MaskFlag(flag_index);
        }

        //---------------------------------------------------------------------
        public static implicit operator MaskFlag(ulong mask)
        {
            return new MaskFlag(mask);
        }

        //---------------------------------------------------------------------
        private MaskFlag(int flag_index)
        {
            m_flag = flag_index;
            m_offset = flag_index / 64;
            m_mask = ((ulong)1) << (flag_index % 64);
        }

        //---------------------------------------------------------------------
        private MaskFlag(ulong mask)
        {
            m_offset = 0;
            m_mask = mask;
            m_flag = 0;
            for (int i = 0; i < 64; i++)
            {
                if (((((ulong)1) << i) & mask) != 0)
                {
                    m_flag = i;
                    break;
                }
            }
        }
    }

    //-------------------------------------------------------------------------
    public partial struct Mask128
    {
        //---------------------------------------------------------------------
        #region Override this for bigger Mask
        public const int MAX_SIZE = 2;
        private ulong m_mask_0f;
        private ulong m_mask_f0;

        //---------------------------------------------------------------------
        private ulong this[long index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m_mask_0f;
                    case 1: return m_mask_f0;
                }
                return 0;
            }
            set
            {
                switch (index)
                {
                    case 0: m_mask_0f = value; break;
                    case 1: m_mask_f0 = value; break;
                }
            }
        }

        //---------------------------------------------------------------------
        private Mask128(bool none)
        {
            m_mask_0f = 0;
            m_mask_f0 = 0;
        }
        #endregion Override this for bigger Mask
    }

    //-------------------------------------------------------------------------
    public partial struct Mask256
    {
        //---------------------------------------------------------------------
        #region Override this for bigger Mask
        public const int MAX_SIZE = 4;
        private ulong m_mask_000f;
        private ulong m_mask_00f0;
        private ulong m_mask_0f00;
        private ulong m_mask_f000;

        //---------------------------------------------------------------------
        private ulong this[long index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m_mask_000f;
                    case 1: return m_mask_00f0;
                    case 2: return m_mask_0f00;
                    case 3: return m_mask_f000;
                }
                return 0;
            }
            set
            {
                switch (index)
                {
                    case 0: m_mask_000f = value; break;
                    case 1: m_mask_00f0 = value; break;
                    case 2: m_mask_0f00 = value; break;
                    case 3: m_mask_f000 = value; break;
                }
            }
        }

        //---------------------------------------------------------------------
        private Mask256(bool none)
        {
            m_mask_000f = 0;
            m_mask_00f0 = 0;
            m_mask_0f00 = 0;
            m_mask_f000 = 0;
        }

        //---------------------------------------------------------------------
        private void Set(int offset, Mask128 mask)
        {
            for (int i = offset; i < offset + Mask128.MAX_SIZE; i++)
            {
                this[i] = mask.Get(i - Mask128.MAX_SIZE);
            }
        }

        //---------------------------------------------------------------------
        public Mask256(Mask128 mask_0f)
            : this(false)
        {
            Set(0, mask_0f);
        }

        //---------------------------------------------------------------------
        public Mask256(Mask128 mask_0f, Mask128 mask_f0)
            : this(mask_0f)
        {
            Set(Mask128.MAX_SIZE, mask_f0);
        }
        #endregion Override this for bigger Mask
    }

    //-------------------------------------------------------------------------
    public partial struct Mask512
    {
        //---------------------------------------------------------------------
        #region Override this for bigger Mask
        public const int MAX_SIZE = 8;
        private ulong m_mask_0000000f;
        private ulong m_mask_000000f0;
        private ulong m_mask_00000f00;
        private ulong m_mask_0000f000;
        private ulong m_mask_000f0000;
        private ulong m_mask_00f00000;
        private ulong m_mask_0f000000;
        private ulong m_mask_f0000000;

        //---------------------------------------------------------------------
        private ulong this[long index]
        {
            get
            {
                switch (index)
                {
                    case 0: return m_mask_0000000f;
                    case 1: return m_mask_000000f0;
                    case 2: return m_mask_00000f00;
                    case 3: return m_mask_0000f000;
                    case 4: return m_mask_000f0000;
                    case 5: return m_mask_00f00000;
                    case 6: return m_mask_0f000000;
                    case 7: return m_mask_f0000000;
                }
                return 0;
            }
            set
            {
                switch (index)
                {
                    case 0: m_mask_0000000f = value; break;
                    case 1: m_mask_000000f0 = value; break;
                    case 2: m_mask_00000f00 = value; break;
                    case 3: m_mask_0000f000 = value; break;
                    case 4: m_mask_000f0000 = value; break;
                    case 5: m_mask_00f00000 = value; break;
                    case 6: m_mask_0f000000 = value; break;
                    case 7: m_mask_f0000000 = value; break;
                }
            }
        }

        //---------------------------------------------------------------------
        private Mask512(bool none)
        {
            m_mask_0000000f = 0;
            m_mask_000000f0 = 0;
            m_mask_00000f00 = 0;
            m_mask_0000f000 = 0;
            m_mask_000f0000 = 0;
            m_mask_00f00000 = 0;
            m_mask_0f000000 = 0;
            m_mask_f0000000 = 0;
        }

        //---------------------------------------------------------------------
        private void Set(int offset, Mask128 mask)
        {
            for (int i = offset; i < offset + Mask128.MAX_SIZE; i++)
            {
                this[i] = mask.Get(i - Mask128.MAX_SIZE);
            }
        }

        //---------------------------------------------------------------------
        public Mask512(Mask128 mask_000f)
            : this(false)
        {
            Set(0, mask_000f);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask128 mask_000f, Mask128 mask_00f0)
            : this(mask_000f)
        {
            Set(Mask128.MAX_SIZE, mask_00f0);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask128 mask_000f, Mask128 mask_00f0, Mask128 mask_0f00)
            : this(mask_000f, mask_00f0)
        {
            Set(Mask128.MAX_SIZE * 2, mask_0f00);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask128 mask_000f, Mask128 mask_00f0, Mask128 mask_0f00, Mask128 mask_f000)
            : this(mask_000f, mask_00f0, mask_0f00)
        {
            Set(Mask128.MAX_SIZE * 3, mask_f000);
        }

        //---------------------------------------------------------------------
        private void Set(int offset, Mask256 mask)
        {
            for (int i = offset; i < offset + Mask256.MAX_SIZE; i++)
            {
                this[i] = mask.Get(i - Mask256.MAX_SIZE);
            }
        }

        //---------------------------------------------------------------------
        public Mask512(Mask256 mask_0f)
            : this(false)
        {
            Set(0, mask_0f);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask256 mask_0f, Mask256 mask_f0)
            : this(mask_0f)
        {
            Set(Mask256.MAX_SIZE, mask_f0);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask256 mask_00ff, Mask128 mask_0f00)
            : this(mask_00ff)
        {
            Set(Mask128.MAX_SIZE * 2, mask_0f00);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask256 mask_00ff, Mask128 mask_0f00, Mask128 mask_f000)
            : this(mask_00ff)
        {
            Set(Mask128.MAX_SIZE * 2, mask_0f00);
            Set(Mask128.MAX_SIZE * 3, mask_f000);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask128 mask_000f, Mask256 mask_0ff0)
            : this(mask_000f)
        {
            Set(Mask128.MAX_SIZE, mask_0ff0);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask128 mask_000f, Mask256 mask_0ff0, Mask128 mask_f000)
            : this(mask_000f)
        {
            Set(Mask128.MAX_SIZE, mask_0ff0);
            Set(Mask128.MAX_SIZE + Mask256.MAX_SIZE, mask_f000);
        }

        //---------------------------------------------------------------------
        public Mask512(Mask128 mask_000f, Mask128 mask_00f0, Mask256 mask_ff00)
            : this(mask_000f, mask_00f0)
        {
            Set(Mask128.MAX_SIZE * 2, mask_ff00);
        }
        #endregion Override this for bigger Mask
    }

    //-------------------------------------------------------------------------
    public partial struct Mask128
    {
        //---------------------------------------------------------------------
        #region Size management
        public void Reset()
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                this[i] = 0;
            }
        }
        #endregion Size management

        //---------------------------------------------------------------------
        #region Add/Remove/Invert
        public static Mask128 operator +(Mask128 mask, MaskFlag flag)
        {
            mask[flag.offset] |= flag.mask;
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask128 operator -(Mask128 mask, MaskFlag flag)
        {
            mask[flag.offset] &= ~flag.mask;
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask128 operator ~(Mask128 mask)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask[i] = ~mask[i];
            }
            return mask;
        }
        #endregion Add/Remove/Invert

        //---------------------------------------------------------------------
        #region Unary
        public static Mask128 operator &(Mask128 mask0, Mask128 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] &= mask1[i];
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask128 operator &(Mask128 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] &= mask1.mask;
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask128 operator |(Mask128 mask0, Mask128 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] |= mask1[i];
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask128 operator |(Mask128 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] |= mask1.mask;
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask128 operator ^(Mask128 mask0, Mask128 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] ^= mask1[i];
            }
            return mask0;
        }
        //---------------------------------------------------------------------
        public static Mask128 operator ^(Mask128 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] ^= mask1.mask;
            }
            return mask0;
        }

        #endregion Unary

        //---------------------------------------------------------------------
        #region Shift
        private void Shift(int shift)
        {
            var start = shift < 0 ? MAX_SIZE - 1 : 0;
            var end = shift < 0 ? 0 : MAX_SIZE - 1;
            var direction = shift < 0 ? -1 : 1;
            for (int i = start; 0 <= i && i < MAX_SIZE; i += direction)
            {
                if (shift < 0)
                {
                    this[i] <<= shift;
                    if (0 < i)
                    {
                        this[i] |= (this[i - 1] >> (64 - shift));
                    }
                }
                else
                {
                    this[i] >>= shift;
                    if (i + 1 < MAX_SIZE)
                    {
                        this[i] |= (this[i + 1] << (64 - shift));
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        public static Mask128 operator <<(Mask128 mask, int shift)
        {
            shift = Mathf.Abs(shift);
            if (shift >= 64 * MAX_SIZE)
            {
                mask.Reset();
                return mask;
            }

            for (; shift > 0; shift -= 64)
            {
                mask.Shift(-Mathf.Min(shift, 64));
            }
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask128 operator >>(Mask128 mask, int shift)
        {
            shift = Mathf.Abs(shift);
            if (shift >= 64 * MAX_SIZE)
            {
                mask.Reset();
                return mask;
            }

            for (; shift > 0; shift -= 64)
            {
                mask.Shift(Mathf.Min(shift, 64));
            }
            return mask;
        }
        #endregion Shift

        //---------------------------------------------------------------------
        #region Bool/Equalities
        public static bool operator !=(Mask128 mask0, Mask128 mask1) { return !(mask0 == mask1); }
        public static bool operator ==(Mask128 mask0, Mask128 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                var m0 = (ulong)0;
                var m1 = (ulong)0;
                if (i < MAX_SIZE)
                {
                    m0 = mask0[i];
                    m1 = mask1[i];
                }

                if (m0 != m1)
                    return false;
            }
            return true;
        }

        //---------------------------------------------------------------------
        public static bool operator !=(Mask128 mask, MaskFlag flag) { return !(mask == flag); }
        public static bool operator ==(Mask128 mask, MaskFlag flag)
        {
            if (flag.offset >= MAX_SIZE)
                return false;
            return (mask[flag.offset] & flag.mask) != 0;
        }

        //---------------------------------------------------------------------
        public static bool operator !(Mask128 mask) { return !(bool)mask; }
        public static bool operator true(Mask128 mask) { return (bool)mask == true; }
        public static bool operator false(Mask128 mask) { return (bool)mask == false; }
        public static explicit operator bool(Mask128 mask)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                if (mask[i] != 0)
                    return true;
            }
            return false;
        }

        //---------------------------------------------------------------------
        public override bool Equals(object obj) { return (obj is Mask128) ? this == ((Mask128)obj) : false; }
        public override int GetHashCode()
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                if (this[i] != 0)
                    return this[i].GetHashCode();
            }
            return 0;
        }
        #endregion Bool/Equalities

        //---------------------------------------------------------------------
        #region Ctor
        public Mask128(params MaskFlag[] flags)
            : this(false)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                this += flags[i];
            }
        }

        //---------------------------------------------------------------------
        public Mask128(Mask128 other)
            : this(false)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                this[i] = other[i];
            }
        }

        //---------------------------------------------------------------------
        public void Set(int index, ulong value)
        {
            this[index] = value;
        }

        //---------------------------------------------------------------------
        public ulong Get(int index)
        {
            return this[index];
        }

        //---------------------------------------------------------------------
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < MAX_SIZE; i++)
            {
                for (int s = 0; s < 64; s++)
                {
                    result = String.Format("{1}{0}", result, (this[i] >> s) & 1);
                }
            }
            return result;
        }
        #endregion Ctor
    }

    //-------------------------------------------------------------------------
    // ALL CODE AFTER THAT IS COPIED FROM Mask128
    //-------------------------------------------------------------------------
    public partial struct Mask256
    {
        //---------------------------------------------------------------------
        #region Size management
        public void Reset()
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                this[i] = 0;
            }
        }
        #endregion Size management

        //---------------------------------------------------------------------
        #region Add/Remove/Invert
        public static Mask256 operator +(Mask256 mask, MaskFlag flag)
        {
            mask[flag.offset] |= flag.mask;
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask256 operator -(Mask256 mask, MaskFlag flag)
        {
            mask[flag.offset] &= ~flag.mask;
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask256 operator ~(Mask256 mask)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask[i] = ~mask[i];
            }
            return mask;
        }
        #endregion Add/Remove/Invert

        //---------------------------------------------------------------------
        #region Unary
        public static Mask256 operator &(Mask256 mask0, Mask256 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] &= mask1[i];
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask256 operator &(Mask256 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] &= mask1.mask;
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask256 operator |(Mask256 mask0, Mask256 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] |= mask1[i];
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask256 operator |(Mask256 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] |= mask1.mask;
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask256 operator ^(Mask256 mask0, Mask256 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] ^= mask1[i];
            }
            return mask0;
        }
        //---------------------------------------------------------------------
        public static Mask256 operator ^(Mask256 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] ^= mask1.mask;
            }
            return mask0;
        }

        #endregion Unary

        //---------------------------------------------------------------------
        #region Shift
        private void Shift(int shift)
        {
            var start = shift < 0 ? MAX_SIZE - 1 : 0;
            var end = shift < 0 ? 0 : MAX_SIZE - 1;
            var direction = shift < 0 ? -1 : 1;
            for (int i = start; 0 <= i && i < MAX_SIZE; i += direction)
            {
                if (shift < 0)
                {
                    this[i] <<= shift;
                    if (0 < i)
                    {
                        this[i] |= (this[i - 1] >> (64 - shift));
                    }
                }
                else
                {
                    this[i] >>= shift;
                    if (i + 1 < MAX_SIZE)
                    {
                        this[i] |= (this[i + 1] << (64 - shift));
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        public static Mask256 operator <<(Mask256 mask, int shift)
        {
            shift = Mathf.Abs(shift);
            if (shift >= 64 * MAX_SIZE)
            {
                mask.Reset();
                return mask;
            }

            for (; shift > 0; shift -= 64)
            {
                mask.Shift(-Mathf.Min(shift, 64));
            }
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask256 operator >>(Mask256 mask, int shift)
        {
            shift = Mathf.Abs(shift);
            if (shift >= 64 * MAX_SIZE)
            {
                mask.Reset();
                return mask;
            }

            for (; shift > 0; shift -= 64)
            {
                mask.Shift(Mathf.Min(shift, 64));
            }
            return mask;
        }
        #endregion Shift

        //---------------------------------------------------------------------
        #region Bool/Equalities
        public static bool operator !=(Mask256 mask0, Mask256 mask1) { return !(mask0 == mask1); }
        public static bool operator ==(Mask256 mask0, Mask256 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                var m0 = (ulong)0;
                var m1 = (ulong)0;
                if (i < MAX_SIZE)
                {
                    m0 = mask0[i];
                    m1 = mask1[i];
                }

                if (m0 != m1)
                    return false;
            }
            return true;
        }

        //---------------------------------------------------------------------
        public static bool operator !=(Mask256 mask, MaskFlag flag) { return !(mask == flag); }
        public static bool operator ==(Mask256 mask, MaskFlag flag)
        {
            if (flag.offset >= MAX_SIZE)
                return false;
            return (mask[flag.offset] & flag.mask) != 0;
        }

        //---------------------------------------------------------------------
        public static bool operator !(Mask256 mask) { return !(bool)mask; }
        public static bool operator true(Mask256 mask) { return (bool)mask == true; }
        public static bool operator false(Mask256 mask) { return (bool)mask == false; }
        public static explicit operator bool(Mask256 mask)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                if (mask[i] != 0)
                    return true;
            }
            return false;
        }

        //---------------------------------------------------------------------
        public override bool Equals(object obj) { return (obj is Mask256) ? this == ((Mask256)obj) : false; }
        public override int GetHashCode()
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                if (this[i] != 0)
                    return this[i].GetHashCode();
            }
            return 0;
        }
        #endregion Bool/Equalities

        //---------------------------------------------------------------------
        #region Ctor
        public Mask256(params MaskFlag[] flags)
            : this(false)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                this += flags[i];
            }
        }

        //---------------------------------------------------------------------
        public Mask256(Mask256 other)
            : this(false)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                this[i] = other[i];
            }
        }

        //---------------------------------------------------------------------
        public void Set(int index, ulong value)
        {
            this[index] = value;
        }

        //---------------------------------------------------------------------
        public ulong Get(int index)
        {
            return this[index];
        }

        //---------------------------------------------------------------------
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < MAX_SIZE; i++)
            {
                for (int s = 0; s < 64; s++)
                {
                    result = String.Format("{1}{0}", result, (this[i] >> s) & 1);
                }
            }
            return result;
        }
        #endregion Ctor
    }

    //-------------------------------------------------------------------------
    public partial struct Mask512
    {
        //---------------------------------------------------------------------
        #region Size management
        public void Reset()
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                this[i] = 0;
            }
        }
        #endregion Size management

        //---------------------------------------------------------------------
        #region Add/Remove/Invert
        public static Mask512 operator +(Mask512 mask, MaskFlag flag)
        {
            mask[flag.offset] |= flag.mask;
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask512 operator -(Mask512 mask, MaskFlag flag)
        {
            mask[flag.offset] &= ~flag.mask;
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask512 operator ~(Mask512 mask)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask[i] = ~mask[i];
            }
            return mask;
        }
        #endregion Add/Remove/Invert

        //---------------------------------------------------------------------
        #region Unary
        public static Mask512 operator &(Mask512 mask0, Mask512 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] &= mask1[i];
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask512 operator &(Mask512 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] &= mask1.mask;
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask512 operator |(Mask512 mask0, Mask512 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] |= mask1[i];
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask512 operator |(Mask512 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] |= mask1.mask;
            }
            return mask0;
        }

        //---------------------------------------------------------------------
        public static Mask512 operator ^(Mask512 mask0, Mask512 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                mask0[i] ^= mask1[i];
            }
            return mask0;
        }
        //---------------------------------------------------------------------
        public static Mask512 operator ^(Mask512 mask0, MaskFlag mask1)
        {
            if (mask1.offset < MAX_SIZE)
            {
                mask0[mask1.offset] ^= mask1.mask;
            }
            return mask0;
        }

        #endregion Unary

        //---------------------------------------------------------------------
        #region Shift
        private void Shift(int shift)
        {
            var start = shift < 0 ? MAX_SIZE - 1 : 0;
            var end = shift < 0 ? 0 : MAX_SIZE - 1;
            var direction = shift < 0 ? -1 : 1;
            for (int i = start; 0 <= i && i < MAX_SIZE; i += direction)
            {
                if (shift < 0)
                {
                    this[i] <<= shift;
                    if (0 < i)
                    {
                        this[i] |= (this[i - 1] >> (64 - shift));
                    }
                }
                else
                {
                    this[i] >>= shift;
                    if (i + 1 < MAX_SIZE)
                    {
                        this[i] |= (this[i + 1] << (64 - shift));
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        public static Mask512 operator <<(Mask512 mask, int shift)
        {
            shift = Mathf.Abs(shift);
            if (shift >= 64 * MAX_SIZE)
            {
                mask.Reset();
                return mask;
            }

            for (; shift > 0; shift -= 64)
            {
                mask.Shift(-Mathf.Min(shift, 64));
            }
            return mask;
        }

        //---------------------------------------------------------------------
        public static Mask512 operator >>(Mask512 mask, int shift)
        {
            shift = Mathf.Abs(shift);
            if (shift >= 64 * MAX_SIZE)
            {
                mask.Reset();
                return mask;
            }

            for (; shift > 0; shift -= 64)
            {
                mask.Shift(Mathf.Min(shift, 64));
            }
            return mask;
        }
        #endregion Shift

        //---------------------------------------------------------------------
        #region Bool/Equalities
        public static bool operator !=(Mask512 mask0, Mask512 mask1) { return !(mask0 == mask1); }
        public static bool operator ==(Mask512 mask0, Mask512 mask1)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                var m0 = (ulong)0;
                var m1 = (ulong)0;
                if (i < MAX_SIZE)
                {
                    m0 = mask0[i];
                    m1 = mask1[i];
                }

                if (m0 != m1)
                    return false;
            }
            return true;
        }

        //---------------------------------------------------------------------
        public static bool operator !=(Mask512 mask, MaskFlag flag) { return !(mask == flag); }
        public static bool operator ==(Mask512 mask, MaskFlag flag)
        {
            if (flag.offset >= MAX_SIZE)
                return false;
            return (mask[flag.offset] & flag.mask) != 0;
        }

        //---------------------------------------------------------------------
        public static bool operator !(Mask512 mask) { return !(bool)mask; }
        public static bool operator true(Mask512 mask) { return (bool)mask == true; }
        public static bool operator false(Mask512 mask) { return (bool)mask == false; }
        public static explicit operator bool(Mask512 mask)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                if (mask[i] != 0)
                    return true;
            }
            return false;
        }

        //---------------------------------------------------------------------
        public override bool Equals(object obj) { return (obj is Mask512) ? this == ((Mask512)obj) : false; }
        public override int GetHashCode()
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                if (this[i] != 0)
                    return this[i].GetHashCode();
            }
            return 0;
        }
        #endregion Bool/Equalities

        //---------------------------------------------------------------------
        #region Ctor
        public Mask512(params MaskFlag[] flags)
            : this(false)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                this += flags[i];
            }
        }

        //---------------------------------------------------------------------
        public Mask512(Mask512 other)
            : this(false)
        {
            for (int i = 0; i < MAX_SIZE; i++)
            {
                this[i] = other[i];
            }
        }

        //---------------------------------------------------------------------
        public void Set(int index, ulong value)
        {
            this[index] = value;
        }

        //---------------------------------------------------------------------
        public ulong Get(int index)
        {
            return this[index];
        }

        //---------------------------------------------------------------------
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < MAX_SIZE; i++)
            {
                for (int s = 0; s < 64; s++)
                {
                    result = String.Format("{1}{0}", result, (this[i] >> s) & 1);
                }
            }
            return result;
        }
        #endregion Ctor
    }
}