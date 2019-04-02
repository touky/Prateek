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

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.Draw.Style.QuickCTor;
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
        //---------------------------------------------------------------------
        #region Declarations
        public struct FlagData
        {
            //-----------------------------------------------------------------
            public struct HierarchyInfos
            {
                public ulong parent;
                public List<ulong> children;
            }

            //-----------------------------------------------------------------
            public Type maskType;
            public List<HierarchyInfos> hierarchy;

            //-----------------------------------------------------------------
            public void Add(ulong parent, params ulong[] children)
            {
                if (hierarchy == null)
                    hierarchy = new List<HierarchyInfos>();

                int i = -1;
                for (int h = 0; h < hierarchy.Count; h++)
                {
                    if (hierarchy[h].parent == parent)
                    {
                        i = h;
                        break;
                    }
                }

                if (i < 0)
                {
                    i = hierarchy.Count;
                    hierarchy.Add(new HierarchyInfos() { parent = parent, children = new List<ulong>() } );
                }

                if (children == null)
                    return;

                var info = hierarchy[i];
                {
                    info.children.AddRange(children);
                }
                hierarchy[i] = info;

                for (int c = 0; c < children.Length; c++)
                {
                    Add(children[c], null);
                }
            }

            //-----------------------------------------------------------------
            public bool GetParent(ulong value, ref ulong parent)
            {
                for (int h = 0; h < hierarchy.Count; h++)
                {
                    for (int c = 0; c < hierarchy[h].children.Count; c++)
                    {
                        if (hierarchy[h].children[c] == value)
                        {
                            parent = hierarchy[h].parent;
                            return true;
                        }
                    }
                }
                return false;
            }

            //-----------------------------------------------------------------
            public int CountParent(ulong value)
            {
                var i = -1;
                for (int h = 0; h < hierarchy.Count; h++)
                {
                    for (int c = 0; c < hierarchy[h].children.Count; c++)
                    {
                        if (hierarchy[h].children[c] == value)
                        {
                            return CountParent(hierarchy[h].parent) + 1;
                        }
                    }
                }
                return 0;
            }

            //-----------------------------------------------------------------
            public bool HasChildren(ulong value)
            {
                var i = -1;
                for (int h = 0; h < hierarchy.Count; h++)
                {
                    if (hierarchy[h].parent == value)
                    {
                        return hierarchy[h].children.Count > 0;
                    }
                }
                return false;
            }
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        protected static FlagData flagDatas;

        private Editors.Prefs.Mask256s mask;
        private bool deactivateAll = false;
        private ulong[] values = null;
        private string[] names = null;
        private int[] parents = null;
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public static FlagData FlagDatas { get { return flagDatas; } }
        #endregion Properties

        //---------------------------------------------------------------------
        #region FrameRecorder.IRecorderBase
        public override void OnInitialize()
        {
            base.OnInitialize();

            var type = flagDatas.maskType;
            if (type == null || !type.IsEnum)
            {
                Registry.Instance.Unregister(GetType());
                return;
            }

            var values = (ulong[])Enum.GetValues(type);
            var default_mask = new Helpers.Mask256();
            if (values.Length < Helpers.Mask256.MAX_SIZE)
            {
                Registry.Instance.Unregister(GetType());
                return;
            }

            this.values = values;
            names = Enum.GetNames(type);
            mask = Editors.Prefs.Get(String.Format("{0}_{1}", GetType().Name, type.Name), default_mask);
            parents = new int[values.Length];
            for (int i = 0; i < parents.Length; i++)
            {
                parents[i] = -1;
            }
        }
        #endregion FrameRecorder.IRecorderBase

        //---------------------------------------------------------------------
        #region Flag manageement
        protected bool IsWrongType<T>() where T : struct, IConvertible
        {
            if (flagDatas.maskType == null)
                return true;

            if (typeof(T) != flagDatas.maskType && typeof(T) != typeof(int))
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
        public bool HasParent<T>(T child) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return false;

            var mask = ToMask(child);
            if (parents == null || mask.flag >= parents.Length)
                return false;

            return parents[mask.flag] >= 0;
        }

        //---------------------------------------------------------------------
        public int CountParents<T>(T child) where T : struct, IConvertible
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
        public void SetParent<T>(T child) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return;

            var mask = ToMask(child);
            if (parents == null || mask.flag >= parents.Length)
                return;

            parents[mask.flag] = -1;
        }

        //---------------------------------------------------------------------
        public void SetParent<T>(T child, T parent) where T : struct, IConvertible
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
        public bool IsActive<T>(T value) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return false;

            if (deactivateAll)
                return false;

            return IsActive(ToMask(value));
        }

        //---------------------------------------------------------------------
        public bool IsActiveAndSelected<T>(T value, MonoBehaviour behaviour) where T : struct, IConvertible
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
        public void SetActive<T>(T value, bool active) where T : struct, IConvertible
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
        public void SetActive(bool active)
        {
            deactivateAll = !active;
        }
        #endregion Flag manageement
    }
}
#endif //PRATEEK_DEBUG
