// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 05/03/19
//
//  Copyright © 2017—2019 Benjamin "Touky" Huet <huet.benjamin@gmail.com>
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
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
    public struct CachedArray<T>
    {
        public const int MaxCount = 10;
        private T m_default;
        private T m_value0;
        private T m_value1;
        private T m_value2;
        private T m_value3;
        private T m_value4;
        private T m_value5;
        private T m_value6;
        private T m_value7;
        private T m_value8;
        private T m_value9;
        private int m_count;

        //---------------------------------------------------------------------
        public int Count { get { return m_count; } }

        //---------------------------------------------------------------------
        private T CurrentValue
        {
            get
            {
                switch (m_count)
                {
                    case 0: return m_value0;
                    case 1: return m_value1;
                    case 2: return m_value2;
                    case 3: return m_value3;
                    case 4: return m_value4;
                    case 5: return m_value5;
                    case 6: return m_value6;
                    case 7: return m_value7;
                    case 8: return m_value8;
                    case 9: return m_value9;
                }
                return m_default;
            }
            set
            {
                switch (m_count)
                {
                    case 0: m_value0 = value; break;
                    case 1: m_value1 = value; break;
                    case 2: m_value2 = value; break;
                    case 3: m_value3 = value; break;
                    case 4: m_value4 = value; break;
                    case 5: m_value5 = value; break;
                    case 6: m_value6 = value; break;
                    case 7: m_value7 = value; break;
                    case 8: m_value8 = value; break;
                    case 9: m_value9 = value; break;
                }
            }
        }

        //---------------------------------------------------------------------
        public CachedArray(T def)
        {
            m_default = def;
            m_value0 = def;
            m_value1 = def;
            m_value2 = def;
            m_value3 = def;
            m_value4 = def;
            m_value5 = def;
            m_value6 = def;
            m_value7 = def;
            m_value8 = def;
            m_value9 = def;
            m_count = 0;
        }

        //---------------------------------------------------------------------
        public void Resize(int newCount)
        {
            if (newCount <= MaxCount)
            {
                if (newCount != m_count)
                {
                    while (newCount < m_count)
                    {
                        Pop();
                    }
                    while (newCount > m_count)
                    {
                        Push(m_default);
                    }
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("Stack allocated array size is too small for you usage.");
            }
        }

        //---------------------------------------------------------------------
        public void Push(T value)
        {
            if (m_count < MaxCount)
            {
                CurrentValue = value;
                m_count++;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Stack allocated array size is too small for you usage.");
            }
        }

        //---------------------------------------------------------------------
        public void Pop()
        {
            if (m_count > 0)
            {
                CurrentValue = m_default;
                m_count--;
            }
        }

        //---------------------------------------------------------------------
        public T Peek(int index)
        {
            if (index >= 0 && index < m_count)
            {
                int oldCount = m_count;
                m_count = index;
                T result = CurrentValue;
                m_count = oldCount;
                return result;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Stack allocated array size is too small for you usage.");
            }
            return m_default;
        }

        //---------------------------------------------------------------------
        public T Peek<TE>(TE index) where TE : struct, IConvertible
        {
            if (!typeof(TE).IsEnum)
            {
                throw new ArgumentException("TE must be an enumerated type");
            }

            return Peek((int)((object)index));
        }

        //---------------------------------------------------------------------
        public void Set(int index, T value)
        {
            if (index >= 0 && index < m_count)
            {
                int oldCount = m_count;
                m_count = index;
                CurrentValue = value;
                m_count = oldCount;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Stack allocated array size is too small for you usage.");
            }
        }

        //---------------------------------------------------------------------
        public void Set<TE>(TE index, T value) where TE : struct, IConvertible
        {
            if (!typeof(TE).IsEnum)
            {
                throw new ArgumentException("TE must be an enumerated type");
            }

            Set((int)((object)index), value);
        }

        //---------------------------------------------------------------------
        public bool Has(Predicate<T> match)
        {
            for (int i = 0; i < m_count; ++i)
            {
                if (match.Invoke(Peek(i)))
                    return true;
            }
            return false;
        }

        //---------------------------------------------------------------------
        public int FindIndex(Predicate<T> match)
        {
            for (int i = 0; i < m_count; ++i)
            {
                if (match.Invoke(Peek(i)))
                    return i;
            }
            return -1;
        }

        //---------------------------------------------------------------------
        public void Clear()
        {
            m_count = 0;
        }

        //---------------------------------------------------------------------
        public T[] ToArray()
        {
            T[] result = new T[m_count];
            for (int i = 0; i < m_count; ++i)
            {
                result[i] = Peek(i);
            }
            return result;
        }
    }
}
