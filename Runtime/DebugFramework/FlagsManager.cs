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
#if PRATEEK_DEBUG
namespace Prateek.Runtime.DebugFramework
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.AssemblyForager;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Interfaces;
    using UnityEngine.Assertions;

    ///-------------------------------------------------------------------------
    /// Debug flag manager to keep track of the debug flags
    public sealed class FlagManager<TRootKeyword, TRootMetaword>
        where TRootKeyword : MasterKeyword
        where TRootMetaword : IMasterMetaword
    {
        ///---------------------------------------------------------------------
        private class FlagStatus
        {
            public Type type;
            public HashSet<Type> parents;
            public List<FlagStatus> children;
            public bool enable = true;

            public FlagStatus(Type type, bool enable)
            {
                this.type = type;
                this.enable = enable;

                var baseType = type.BaseType;
                do
                {
                    parents.Add(baseType);
                    baseType = baseType.BaseType;
                } while (baseType != typeof(TRootKeyword) && baseType != typeof(TRootMetaword));
            }

            public void Build(Dictionary<Type, FlagStatus> flagStatuses)
            {
                foreach (var flagStatus in flagStatuses.Values)
                {
                    if (flagStatus.parents.Contains(type)
                        || type.IsAssignableFrom(flagStatus.type))
                    {
                        children.Add(flagStatus);
                    }
                }
            }
        }

        ///---------------------------------------------------------------------
        #region Fields
        private Dictionary<Type, FlagStatus> flagStatuses = new Dictionary<Type, FlagStatus>();
        private HashSet<Type> activeFlags = new HashSet<Type>();
        private FlagsForagerWorker worker = null;

        private Mask256 mask;
        private bool deactivateAll = false;
        #endregion Fields

        ///---------------------------------------------------------------------
        #region IGlobalManager integration
        //public override void OnInitialize()
        //{
        //    base.OnInitialize();

        //    var type = flagDatas.maskType;
        //    if (type == null || !type.IsEnum)
        //    {
        //        //todo DaemonRegistry.Instance.Unregister(GetType());
        //        return;
        //    }

        //    //var values = (ulong[])Enum.GetValues(type);
        //    //var default_mask = new Helpers.Mask256();
        //    //if (values.Length < Helpers.Mask256.MAX_SIZE)
        //    //{
        //    //    Registry.Instance.Unregister(GetType());
        //    //    return;
        //    //}

        //    //this.values = values;
        //    //names = Enum.GetNames(type);
        //    //mask = Editors.Prefs.Get(String.Format("{0}_{1}", GetType().Name, type.Name), default_mask);
        //    //parents = new int[values.Length];
        //    //for (int i = 0; i < parents.Length; i++)
        //    //{
        //    //    parents[i] = -1;
        //    //}
        //}
        #endregion IGlobalManager integration

        ///---------------------------------------------------------------------
        private void Init()
        {
            if (worker == null)
            {
                return;
            }

            worker = FlagsForagerWorker.Instance;

            foreach (var flag in worker.flags)
            {
                flagStatuses.Add(flag, new FlagStatus(flag, true));
                activeFlags.Add(flag);
            }

            foreach (var overlay in worker.overlays)
            {
                flagStatuses.Add(overlay, new FlagStatus(overlay, false));
            }

            foreach (var flagStatus in flagStatuses.Values)
            {
                flagStatus.Build(flagStatuses);
            }
        }

        ///---------------------------------------------------------------------
        private void ValidateType<TFlagKeyword>()
        {
            Assert.IsTrue(typeof(TFlagKeyword).IsSubclassOf(typeof(TRootKeyword))
                       || typeof(TRootMetaword).IsAssignableFrom(typeof(TFlagKeyword)));
        }

        ///---------------------------------------------------------------------
        public bool IsActive<TFlagKeyword>()
            where TFlagKeyword : IMasterMetaword
        {
            ValidateType<TFlagKeyword>();

            Init();

            if (!activeFlags.Contains(typeof(TFlagKeyword)))
            {
                return false;
            }

            return true;
        }

        ///---------------------------------------------------------------------
        public void SetStatus<TFlagKeyword>(bool enable)
            where TFlagKeyword : IMasterMetaword
        {
            ValidateType<TFlagKeyword>();

            Init();

            var type = typeof(TFlagKeyword);
            var status = flagStatuses[type];
            status.enable = enable;
            flagStatuses[type] = status;
        }

        private abstract class FlagsForagerWorker : AssemblyForagerWorker
        {
            private static FlagsForagerWorker instance;
            internal List<Type> flags = new List<Type>(50);
            internal List<Type> overlays = new List<Type>(50);

            public static FlagsForagerWorker Instance
            {
                get { return instance; }
            }

            #region Class Methods
            public override void PrepareSearch()
            {
                instance = this;

                Search(typeof(TRootKeyword));
                Search(typeof(TRootMetaword));
            }

            public override void WorkDone()
            {
                base.WorkDone();

                foreach (var foundType in FoundTypes)
                {
                    if (foundType.IsSubclassOf(typeof(TRootKeyword)))
                    {
                        flags.Add(foundType);
                    }
                    else
                    {
                        overlays.Add(foundType);
                    }
                }
            }
            #endregion
        }
    }
}
#endif //PRATEEK_DEBUG
