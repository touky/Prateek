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
namespace Prateek.DaemonCore.Code
{
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code.Extensions;
    using Prateek.DaemonCore.Code.Interfaces;
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public sealed class DaemonRegistry : SingletonBehaviour<DaemonRegistry>
    {
        #region Fields
        private bool sortManagers = true;
        private GameObject tickerObject;
        private DaemonRegistryTickerFrameBegin tickerBegin;
        private DaemonRegistryTickerFrameEnd tickerEnd;

        private Dictionary<Type, GlobalManager.BuilderBase> registryBuilders = new Dictionary<Type, GlobalManager.BuilderBase>();
        private Dictionary<Type, GlobalManager> registryManagers = new Dictionary<Type, GlobalManager>();
        private Dictionary<Type, List<RegistrableBehaviour>> registryBehaviours = new Dictionary<Type, List<RegistrableBehaviour>>();

        private List<GlobalManager> managersToStart = new List<GlobalManager>();
        private List<GlobalManager> managersToUpdate = new List<GlobalManager>();

        private List<ITickable> tickablesToStart = new List<ITickable>();
        private Dictionary<TickType, List<ITickable>> tickables = new Dictionary<TickType, List<ITickable>>();
        #endregion

        #region Properties
        public GameObject TickerObject
        {
            get { return tickerObject; }
        }
        #endregion

        #region Unity Methods
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            Execute(TickType.OnApplicationQuit);

            tickerBegin = null;
            tickerEnd = null;
        }
        #endregion

        #region Unity EditorOnly Methods
        private void OnGUI()
        {
            Execute(TickType.OnGUI);
        }
        #endregion

        #region Service
        //---------------------------------------------------------------------
        protected override void OnAwake()
        {
            Initialize();
        }

        public void Register(Type type, GlobalManager.BuilderBase builder)
        {
            registryBuilders[type] = builder;
        }

        //---------------------------------------------------------------------
        public void Register(Type type, GlobalManager manager)
        {
            if (type == null || manager == null)
            {
                return;
            }

            if (registryManagers.ContainsKey(type))
            {
                if (manager != null)
                {
                    Unregister(type);
                    registryManagers[type] = manager;
                }
            }
            else if (manager != null)
            {
                registryManagers.Add(type, manager);
            }

            sortManagers = true;

            if (!managersToUpdate.Contains(manager))
            {
                managersToUpdate.Add(manager);

                manager.OnInitialize();
                manager.OnRegister();

                managersToStart.Add(manager);
            }
        }

        //---------------------------------------------------------------------

        public void Register(RegistrableBehaviour behaviour)
        {
            var type = behaviour.GetType();

            //Register all types of the behavior (including base type)
            List<RegistrableBehaviour> behaviours = null;
            while (type != null && type != typeof(RegistrableBehaviour))
            {
                if (registryBehaviours.TryGetValue(type, out behaviours) == false)
                {
                    registryBehaviours[type] = behaviours = new List<RegistrableBehaviour>();
                }

                behaviours.AddUnique(behaviour);
                type = type.BaseType;
            }
        }
        #endregion

        #region Class Methods
        //---------------------------------------------------------------------
        public void Initialize()
        {
            if (tickerBegin != null || tickerEnd != null)
            {
                return;
            }

            SetPauseFeedback(true);

            tickerObject = new GameObject("Registry Tickers");
            tickerObject.transform.SetParent(transform);
            tickerObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            tickerBegin = tickerObject.AddComponent<DaemonRegistryTickerFrameBegin>();
            tickerBegin.registry = this;
            tickerEnd = tickerObject.AddComponent<DaemonRegistryTickerFrameEnd>();
            tickerEnd.registry = this;

            var setup = Resources.Load<RegistrySetup>("RegistrySetup");
            if (setup != null)
            {
                setup.Initialize();
            }
        }

        internal void Execute(TickType tickType, bool status = false)
        {
            switch (tickType)
            {
                case TickType.BeginUpdate:
                case TickType.EndUpdate:
                {
                    Execute(tickType, Time.deltaTime, Time.unscaledDeltaTime);
                    break;
                }
                case TickType.BeginUpdateFixed:
                case TickType.EndUpdateFixed:
                {
                    Execute(tickType, Time.fixedDeltaTime);
                    break;
                }
                case TickType.BeginUpdateLate:
                case TickType.EndUpdateLate:
                {
                    Execute(tickType, Time.deltaTime);
                    break;
                }
                default:
                {
                    Execute(tickType, 0, 0, status);
                    break;
                }
            }
        }

        private void Execute(TickType tickType, float seconds, float unscaledSeconds = 0, bool status = false)
        {
            if (tickables.TryGetValue(tickType, out var list))
            {
                switch (tickType)
                {
                    case TickType.BeginUpdate:
                    case TickType.EndUpdate:
                    {
                        var frameEvent = tickType == TickType.BeginUpdate ? FrameEvent.BeginFrame : FrameEvent.EndFrame;
                        foreach (var tickable in list)
                        {
                            tickable.Update(frameEvent, seconds, unscaledSeconds);
                        }

                        break;
                    }
                    case TickType.BeginUpdateFixed:
                    case TickType.EndUpdateFixed:
                    {
                        var frameEvent = tickType == TickType.BeginUpdateFixed ? FrameEvent.BeginFrame : FrameEvent.EndFrame;
                        foreach (var tickable in list)
                        {
                            tickable.FixedUpdate(frameEvent, seconds);
                        }

                        break;
                    }
                    case TickType.BeginUpdateLate:
                    case TickType.EndUpdateLate:
                    {
                        var frameEvent = tickType == TickType.BeginUpdate ? FrameEvent.BeginFrame : FrameEvent.EndFrame;
                        foreach (var tickable in list)
                        {
                            tickable.LateUpdate(frameEvent, seconds);
                        }

                        break;
                    }
                    case TickType.OnApplicationFocus:
                    {
                        foreach (var tickable in list)
                        {
                            tickable.OnApplicationFocus(status);
                        }

                        break;
                    }
                    case TickType.OnApplicationPause:
                    {
                        foreach (var tickable in list)
                        {
                            tickable.OnApplicationPause(status);
                        }

                        break;
                    }
                    case TickType.OnApplicationQuit:
                    {
                        foreach (var tickable in list)
                        {
                            tickable.OnApplicationQuit();
                        }

                        break;
                    }
                    case TickType.OnGUI:
                    {
                        foreach (var tickable in list)
                        {
                            tickable.DrawGUI();
                        }

                        break;
                    }
                }
            }
            else
            {
                throw new Exception();
            }
        }

        //---------------------------------------------------------------------
        public void Unregister(GlobalManager manager)
        {
            Unregister(manager.GetType());
        }

        public void Unregister(Type type)
        {
            var manager = registryManagers[type];
            if (managersToUpdate.Contains(manager))
            {
                managersToUpdate.Remove(manager);

                manager.OnUnregister();
                manager.OnDispose();

                RegistrySetup.TryCreating(type, true);
            }
        }

        //---------------------------------------------------------------------
        public void Unregister(RegistrableBehaviour behaviour)
        {
            var type = behaviour.GetType();

            //Remove the behavior from all type lists (including base type)
            List<RegistrableBehaviour> behaviours = null;
            while (type != null && type != typeof(RegistrableBehaviour))
            {
                if (registryBehaviours.TryGetValue(type, out behaviours))
                {
                    behaviours.Remove(behaviour);
                }

                type = type.BaseType;
            }
        }

        //---------------------------------------------------------------------

        public static T GetManager<T>() where T : class, IGlobalManager
        {
            if (Instance == null)
            {
                return null;
            }

            var type = typeof(T);

            if (Instance.registryManagers.ContainsKey(type))
            {
                return Instance.registryManagers[type] as T;
            }

            //#if UNITY_EDITOR
            //            for (int i = 0; i < createOnAwake.Count; i++)
            //            {
            //                var testType = Helpers.Types.GetType(createOnAwake[i]);
            //                if (testType.IsSubclassOf(type))
            //                {
            //                    type = testType;
            //                }
            //            }
            //#endif // UNITY_EDITOR

            RegistrySetup.TryCreating(type);

            return Instance.registryManagers[type] as T;
        }

        //---------------------------------------------------------------------
        public static GlobalManager.BuilderBase GetBuilder(Type type)
        {
            if (Instance == null)
            {
                return null;
            }

            if (Instance.registryBuilders.ContainsKey(type))
            {
                return Instance.registryBuilders[type];
            }

            return null;
        }

        //---------------------------------------------------------------------
        private void OnUpdate(TickType tickType, float seconds, float unscaledSeconds)
        {
            //todo//Only do those operations at the start of the frame
            //todoif (tickType == TickType.BeginFrame)
            //todo{
            //todo    //Start the managers that need it
            //todo    if (managersToStart.Count > 0)
            //todo    {
            //todo        managersToStart.Sort((a, b) => a.Priority - b.Priority);
            //todo        for (var i = 0; i < managersToStart.Count; i++)
            //todo        {
            //todo            managersToStart[i].OnStart();
            //todo        }

            //todo        managersToStart.Clear();
            //todo    }

            //todo    //Sort managers before update
            //todo    if (sortManagers)
            //todo    {
            //todo        sortManagers = false;
            //todo        managersToUpdate.Sort((a, b) => a.Priority - b.Priority);
            //todo    }
            //todo}

            //UpdateManagers(tickType, seconds);
        }

        //---------------------------------------------------------------------
        private void OnApplicationFocus(bool focusStatus)
        {
            Execute(TickType.OnApplicationFocus, focusStatus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Execute(TickType.OnApplicationPause, pauseStatus);
        }
        #endregion
        
        //---------------------------------------------------------------------
        private void OnDestroy()
        {
            SetPauseFeedback(false);
        }
        
        //---------------------------------------------------------------------
        private void SetPauseFeedback(bool enable)
        {
#if UNITY_EDITOR
            if (enable)
            {
                EditorApplication.pauseStateChanged += PauseStateChanged;
            }
            else
            {
                EditorApplication.pauseStateChanged -= PauseStateChanged;
            }
#endif //UNITY_EDITOR
        }
        
        //---------------------------------------------------------------------
        private void PauseStateChanged(PauseState state)
        {
#if UNITY_EDITOR
            OnApplicationPause(state == PauseState.Paused);
#endif //UNITY_EDITOR
        }
    }
}
