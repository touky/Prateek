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
using Prateek.ScriptTemplating;
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
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public abstract class FlagManager : GlobalManager
    {
        private Type m_enum = null;
        private Editors.Prefs.Mask256s m_mask;
        private bool m_deactivate_all = false;
        private int[] m_values = null;
        private string[] m_names = null;
        private int[] m_parents = null;

        //---------------------------------------------------------------------
        protected virtual Type GetFlagType()
        {
            return null;
        }

        //-----------------------------------------------------------------------------------------
        protected bool IsWrongType<T>() where T : struct, IConvertible
        {
            if (m_enum == null)
                return true;

            if (typeof(T) != m_enum && typeof(T) != typeof(int))
                return true;

            return false;
        }

        //---------------------------------------------------------------------
        protected bool IsActive(MaskFlag mask)
        {
            var parent = m_parents[mask.flag];
            if (parent >= 0 && !IsActive(parent))
            {
                return false;
            }
            return (m_mask.data & mask) != 0;
        }

        //-----------------------------------------------------------------------------------------
        protected MaskFlag ToMask<T>(T value) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return MaskFlag.zero;
            return value.GetHashCode();
        }

        //---------------------------------------------------------------------
        public override void OnInitialize()
        {
            base.OnInitialize();

            var type = GetFlagType();
            if (type == null ||!type.IsEnum)
            {
                Registry.instance.Unregister(GetType());
                return;
            }

            var values = (int[])Enum.GetValues(type);
            var default_mask = new Helpers.Mask256();
            if (values.Length < Helpers.Mask256.MAX_SIZE)
            {
                Registry.instance.Unregister(GetType());
                return;
            }

            m_enum = type;
            m_values = values;
            m_names = Enum.GetNames(type);
            m_mask = Editors.Prefs.Get(String.Format("{0}_{1}", GetType().Name, type.Name), default_mask);
            m_parents = new int[values.Length];
            for (int i = 0; i < m_parents.Length; i++)
            {
                m_parents[i] = -1;
            }
        }

        //---------------------------------------------------------------------
        public virtual bool HasParent<T>(T child) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return false;

            var mask = ToMask(child);
            if (m_parents == null || mask.flag >= m_parents.Length)
                return false;

            return m_parents[mask.flag] >= 0;
        }

        //---------------------------------------------------------------------
        public virtual int CountParents<T>(T child) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return 0;

            if (!HasParent(child))
                return 0;

            var mask = ToMask(child);
            var count = 1;
            var parent = m_parents[mask.flag];
            while (parent != 0 && m_parents[parent] >= 0)
            {
                count++;
                parent = m_parents[parent];
            }
            return count;
        }

        //---------------------------------------------------------------------
        public virtual void SetParent<T>(T child) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return;

            var mask = ToMask(child);
            if (m_parents == null || mask.flag >= m_parents.Length)
                return;

            m_parents[mask.flag] = -1;
        }

        //---------------------------------------------------------------------
        public virtual void SetParent<T>(T child, T parent) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return;

            var mask0 = ToMask(child);
            var mask1 = ToMask(parent);
            if (m_values == null
                || mask0.flag >= m_values.Length
                || mask1.flag >= m_values.Length)
                return;

            m_parents[mask0.flag] = mask1.flag;
        }

        //---------------------------------------------------------------------
        public virtual bool IsActive<T>(T value) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return false;

            if (m_deactivate_all)
                return false;

            return IsActive(ToMask(value));
        }

        //---------------------------------------------------------------------
        public virtual bool IsActiveAndSelected<T>(T value, MonoBehaviour behaviour) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return false;

            if (!IsActive(value))
                return false;

#if UNITY_EDITOR
            var gameObject = UnityEditor.Selection.activeGameObject;
            if (gameObject == null || !behaviour.gameObject.transform.IsChildOf(gameObject.transform))
            {
                return false;
            }
#endif // UNITY_EDITOR

            return true;
        }

        //---------------------------------------------------------------------
        public virtual void SetActive<T>(T value, bool active) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return;

            var mask = ToMask(value);
            if (active)
            {
                m_mask.data |= mask;
            }
            else
            {
                m_mask.data &= ~(new Helpers.Mask256(mask));
            }
        }

        //---------------------------------------------------------------------
        public virtual void SetActive(bool active)
        {
            m_deactivate_all = !active;
        }
    }
}
#endif //PRATEEK_DEBUG
