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

#if PRATEEK_DEBUGS
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
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public abstract class FlagManager : GlobalManager
    {
        private Type enumType = null;
        private Editors.Prefs.Mask256s mask;
        private bool deactivateAll = false;
        private int[] values = null;
        private string[] names = null;
        private int[] parents = null;

        //---------------------------------------------------------------------
        protected virtual Type GetFlagType()
        {
            return null;
        }

        //---------------------------------------------------------------------
        protected bool IsWrongType<T>() where T : struct, IConvertible
        {
            if (enumType == null)
                return true;

            if (typeof(T) != enumType && typeof(T) != typeof(int))
                return true;

            return false;
        }

        //---------------------------------------------------------------------
        protected bool IsActive(MaskFlag mask)
        {
            var parent = parents[mask.flag];
            if (parent >= 0 && !IsActive(parent))
            {
                return false;
            }
            return (this.mask.data & mask) != 0;
        }

        //---------------------------------------------------------------------
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
                Registry.Instance.Unregister(GetType());
                return;
            }

            var values = (int[])Enum.GetValues(type);
            var default_mask = new Helpers.Mask256();
            if (values.Length < Helpers.Mask256.MAX_SIZE)
            {
                Registry.Instance.Unregister(GetType());
                return;
            }

            enumType = type;
            this.values = values;
            names = Enum.GetNames(type);
            mask = Editors.Prefs.Get(String.Format("{0}_{1}", GetType().Name, type.Name), default_mask);
            parents = new int[values.Length];
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = -1;
            }
        }

        //---------------------------------------------------------------------
        public virtual bool HasParent<T>(T child) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return false;

            var mask = ToMask(child);
            if (parents == null || mask.flag >= parents.Length)
                return false;

            return parents[mask.flag] >= 0;
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
            var parent = parents[mask.flag];
            while (parent != 0 && parents[parent] >= 0)
            {
                count++;
                parent = parents[parent];
            }
            return count;
        }

        //---------------------------------------------------------------------
        public virtual void SetParent<T>(T child) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return;

            var mask = ToMask(child);
            if (parents == null || mask.flag >= parents.Length)
                return;

            parents[mask.flag] = -1;
        }

        //---------------------------------------------------------------------
        public virtual void SetParent<T>(T child, T parent) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return;

            var mask0 = ToMask(child);
            var mask1 = ToMask(parent);
            if (values == null
                || mask0.flag >= values.Length
                || mask1.flag >= values.Length)
                return;

            parents[mask0.flag] = mask1.flag;
        }

        //---------------------------------------------------------------------
        public virtual bool IsActive<T>(T value) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return false;

            if (deactivateAll)
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
                this.mask.data |= mask;
            }
            else
            {
                this.mask.data &= ~(new Helpers.Mask256(mask));
            }
        }

        //---------------------------------------------------------------------
        public virtual void SetActive(bool active)
        {
            deactivateAll = !active;
        }
    }
}
#endif //PRATEEK_DEBUG
