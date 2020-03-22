// -BEGIN_PRATEEK_COPYRIGHT-// -BEGIN_PRATEEK_COPYRIGHT-
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
// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    using System;
    using System.Collections.Generic;
    using Prateek.Base;
    using Prateek.Helpers;

    //-------------------------------------------------------------------------
    public abstract class FlagManager : GlobalManager
    {
        //---------------------------------------------------------------------
        #region Declarations
        public struct FlagHierarchy
        {
            //-----------------------------------------------------------------
            public struct Data
            {
                public bool active;
                public int parent;
                public List<int> children;
            }

            //-----------------------------------------------------------------
            public Type maskType;
            public List<Data> datas;

            //-----------------------------------------------------------------
            public void Build(ref Mask256 mask)
            {
                mask.Reset();
                for (int h = 0; h < datas.Count; h++)
                {
                    if (!IsActive(datas[h].parent))
                        continue;
                    mask += datas[h].parent;
                }
            }

            //-----------------------------------------------------------------
            public void SetStatus(int value, bool enable)
            {
                for (int h = 0; h < datas.Count; h++)
                {
                    var block = datas[h];
                    if (block.parent == value)
                    {
                        block.active = enable;
                        datas[h] = block;
                        break;
                    }
                }
            }

            //-----------------------------------------------------------------
            public bool IsActive(int value)
            {
                for (int h = 0; h < datas.Count; h++)
                {
                    if (datas[h].parent == value)
                    {
                        if (!datas[h].active)
                            return false;

                        int parent = 0;
                        if (GetParent(value, ref parent))
                        {
                            return IsActive(parent);
                        }
                        return true;
                    }
                }
                return false;
            }

            //-----------------------------------------------------------------
            public void Add(int parent, params int[] children)
            {
                if (datas == null)
                    datas = new List<Data>();

                int i = -1;
                for (int h = 0; h < datas.Count; h++)
                {
                    if (datas[h].parent == parent)
                    {
                        i = h;
                        break;
                    }
                }

                if (i < 0)
                {
                    i = datas.Count;
                    datas.Add(new Data() { parent = parent, children = new List<int>() } );
                }

                if (children == null)
                    return;

                var info = datas[i];
                {
                    info.children.AddRange(children);
                }
                datas[i] = info;

                for (int c = 0; c < children.Length; c++)
                {
                    Add(children[c], null);
                }
            }

            //-----------------------------------------------------------------
            public bool GetParent(int child, ref int parent)
            {
                for (int h = 0; h < datas.Count; h++)
                {
                    for (int c = 0; c < datas[h].children.Count; c++)
                    {
                        if (datas[h].children[c] == child)
                        {
                            parent = datas[h].parent;
                            return true;
                        }
                    }
                }
                return false;
            }

            //-----------------------------------------------------------------
            public int CountParent(int child)
            {
                for (int h = 0; h < datas.Count; h++)
                {
                    for (int c = 0; c < datas[h].children.Count; c++)
                    {
                        if (datas[h].children[c] == child)
                        {
                            return CountParent(datas[h].parent) + 1;
                        }
                    }
                }
                return 0;
            }

            //-----------------------------------------------------------------
            public bool HasChildren(int parent)
            {
                var i = -1;
                for (int h = 0; h < datas.Count; h++)
                {
                    if (datas[h].parent == parent)
                    {
                        return datas[h].children.Count > 0;
                    }
                }
                return false;
            }
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        protected FlagHierarchy flagDatas;
        private Mask256 mask;
        private bool deactivateAll = false;
        #endregion Fields

        //---------------------------------------------------------------------
        #region IGlobalManager integration
        public override void OnInitialize()
        {
            base.OnInitialize();

            var type = flagDatas.maskType;
            if (type == null || !type.IsEnum)
            {
                Registry.Instance.Unregister(GetType());
                return;
            }

            //var values = (ulong[])Enum.GetValues(type);
            //var default_mask = new Helpers.Mask256();
            //if (values.Length < Helpers.Mask256.MAX_SIZE)
            //{
            //    Registry.Instance.Unregister(GetType());
            //    return;
            //}

            //this.values = values;
            //names = Enum.GetNames(type);
            //mask = Editors.Prefs.Get(String.Format("{0}_{1}", GetType().Name, type.Name), default_mask);
            //parents = new int[values.Length];
            //for (int i = 0; i < parents.Length; i++)
            //{
            //    parents[i] = -1;
            //}
        }
        #endregion IGlobalManager integration

        //---------------------------------------------------------------------
        #region Flag manageement
        public void Build()
        {
            flagDatas.Build(ref mask);
        }

        //---------------------------------------------------------------------
        protected bool IsWrongType<T>() where T : struct, IConvertible
        {
            if (flagDatas.maskType == null)
                return true;

            if (typeof(T) != flagDatas.maskType && typeof(T) != typeof(int))
                return true;

            return false;
        }

        //---------------------------------------------------------------------
        public bool IsActive(MaskFlag flag)
        {
            return mask == flag;
        }

        //---------------------------------------------------------------------
        protected MaskFlag ToMask<T>(T value) where T : struct, IConvertible
        {
            if (IsWrongType<T>())
                return MaskFlag.zero;
            return value.GetHashCode();
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
        #endregion Flag manageement
    }
}
#endif //PRATEEK_DEBUG
