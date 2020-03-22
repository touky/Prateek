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
namespace Prateek.Base.Registry
{
    using System;
    using System.Collections.Generic;
    using Prateek.Base.Behaviour;
    using Prateek.Extensions;
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    //todo: fix that [InitializeOnLoad]
    class RegistryLoader
    {
        static RegistryLoader()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode
                && !Application.isPlaying)
#endif //UNITY_EDITOR
            {
                //Registry.Instance.Initialize();
            }
        }
    }

    //-------------------------------------------------------------------------
    public sealed class Registry : Singleton<Registry>
    {
        //---------------------------------------------------------------------
        #region Declarations
        [Flags]
        public enum TickEvent
        {
            FrameBeginning = 1,
            FrameEnding = 2,

            MAX
        }
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Fields
        private bool sortManagers = true;
        private GameObject tickerObject;
        private RegistryTickerFrameBegin tickerBegin;
        private RegistryTickerFrameEnd tickerEnd;

        private Dictionary<Type, GlobalManager.BuilderBase> registryBuilders = new Dictionary<Type, GlobalManager.BuilderBase>();
        private Dictionary<Type, GlobalManager> registryManagers = new Dictionary<Type, GlobalManager>();
        private Dictionary<Type, List<RegistrableBehaviour>> registryBehaviours = new Dictionary<Type, List<RegistrableBehaviour>>();

        private List<GlobalManager> managersToStart = new List<GlobalManager>();
        private List<GlobalManager> managersToUpdate = new List<GlobalManager>();
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        public GameObject TickerObject { get { return tickerObject; } }
        #endregion Properties

        //---------------------------------------------------------------------
        public void Initialize()
        {
            if (tickerBegin != null || tickerEnd != null)
                return;

            tickerObject = new GameObject("Registry Tickers");
            tickerObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            tickerBegin = tickerObject.AddComponent<RegistryTickerFrameBegin>();
            tickerEnd = tickerObject.AddComponent<RegistryTickerFrameEnd>();

            tickerBegin.Register(OnUpdate, OnUpdateUnscaled, OnLateUpdate, OnFixedUpdate,
                                 OnApplicationFocus, OnApplicationPause, OnApplicationQuit, OnGUI);
            tickerEnd.Register(OnUpdate, OnUpdateUnscaled, OnLateUpdate, OnFixedUpdate);
            GameObject.DontDestroyOnLoad(tickerObject);

            var setup = Resources.Load<RegistrySetup>("RegistrySetup");
            if (setup != null)
            {
                setup.Initialize();
            }
        }

        //---------------------------------------------------------------------
        #region Builders
        public void Register(Type type, GlobalManager.BuilderBase builder)
        {
            registryBuilders[type] = builder;
        }
        #endregion Builders

        //---------------------------------------------------------------------
        #region Managers
        public void Register(Type type, GlobalManager manager)
        {
            if (type == null || manager == null)
                return;

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
        public void Unregister(GlobalManager manager) { Unregister(manager.GetType()); }
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
        #endregion Managers

        //---------------------------------------------------------------------
        #region RegistrableBehaviour
        public void Register(RegistrableBehaviour behaviour)
        {
            var type = behaviour.GetType();

            //Register all types of the behavior (including base type)
            List<RegistrableBehaviour> behaviours = null;
            while (type != null && type != typeof(RegistrableBehaviour))
            {
                if (registryBehaviours.TryGetValue(type, out behaviours) == false)
                {
                    registryBehaviours[type] = (behaviours = new List<RegistrableBehaviour>());
                }

                behaviours.AddUnique(behaviour);
                type = type.BaseType;
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
        #endregion RegistrableBehaviour

        //---------------------------------------------------------------------
        #region Getters
        public static T GetManager<T>() where T : class, IGlobalManager
        {
            if (Instance == null)
                return null;

            var type = typeof(T);

            if (Instance.registryManagers.ContainsKey(type))
                return Instance.registryManagers[type] as T;

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
                return null;

            if (Instance.registryBuilders.ContainsKey(type))
                return Instance.registryBuilders[type];
            return null;
        }

        //---------------------------------------------------------------------
        public static List<T> GetRegistrable<T>() where T : RegistrableBehaviour
        {
            if (Instance == null)
                return null;

            var behaviours = (List<T>)null;
            GetRegistrable<T>(ref behaviours);
            return behaviours;
        }

        //---------------------------------------------------------------------
        public static bool GetRegistrable<T>(ref List<T> behaviours) where T : RegistrableBehaviour
        {
            if (Instance == null)
                return false;

            var temp = (List<RegistrableBehaviour>)null;
            behaviours = null;
            if (Instance.registryBehaviours.TryGetValue(typeof(T), out temp))
            {
                if (behaviours.Count > 0)
                {
                    behaviours = new List<T>();
                    for (int i = 0; i < temp.Count; i++)
                    {
                        behaviours.Add(temp[i] as T);
                    }
                }
                return behaviours.Count > 0;
            }
            return false;
        }
        #endregion Getters

        //---------------------------------------------------------------------
        #region Tickable events
        private void OnUpdate(TickEvent tickEvent, float seconds)
        {
            //Only do those operations at the start of the frame
            if (tickEvent == TickEvent.FrameBeginning)
            {
                //Start the managers that need it
                if (managersToStart.Count > 0)
                {
                    managersToStart.Sort((a, b) => a.Priority - b.Priority);
                    for (int i = 0; i < managersToStart.Count; i++)
                    {
                        managersToStart[i].OnStart();
                    }
                    managersToStart.Clear();
                }

                //Sort managers before update
                if (sortManagers)
                {
                    sortManagers = false;
                    managersToUpdate.Sort((a, b) => a.Priority - b.Priority);
                }
            }

            UpdateManagers(tickEvent, seconds);
        }

        //---------------------------------------------------------------------
        private void OnUpdateUnscaled(TickEvent tickEvent, float seconds) { UpdateUnscaledManagers(tickEvent, seconds); }
        private void OnLateUpdate(TickEvent tickEvent, float seconds) { LateUpdateManagers(tickEvent, seconds); }
        private void OnFixedUpdate(TickEvent tickEvent, float seconds) { FixedUpdateManagers(tickEvent, seconds); }
        #endregion Tickable events

        //---------------------------------------------------------------------
        #region Unity events
        private void OnApplicationFocus(bool focusStatus) { OnApplicationFocusManagers(focusStatus); }
        private void OnApplicationPause(bool pauseStatus) { OnApplicationPauseManagers(pauseStatus); }
        private void OnApplicationQuit(bool status)
        {
            OnApplicationQuitManagers();

            GameObject.Destroy(tickerBegin.gameObject);

            tickerBegin = null;
            tickerEnd = null;
        }

        //---------------------------------------------------------------------
        private void OnGUI(bool status) { OnGUIManagers(); }
        #endregion Unity events

        //---------------------------------------------------------------------
        #region Manager Events
        private void UpdateManagers(TickEvent tickEvent, float seconds)
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnUpdate(tickEvent, seconds);
            }
        }

        //---------------------------------------------------------------------
        private void UpdateUnscaledManagers(TickEvent tickEvent, float seconds)
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnUpdateUnscaled(tickEvent, seconds);
            }
        }

        //---------------------------------------------------------------------
        private void LateUpdateManagers(TickEvent tickEvent, float seconds)
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnLateUpdate(tickEvent, seconds);
            }
        }

        //---------------------------------------------------------------------
        private void FixedUpdateManagers(TickEvent tickEvent, float seconds)
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnFixedUpdate(tickEvent, seconds);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationFocusManagers(bool focusStatus)
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnApplicationFocus(focusStatus);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationPauseManagers(bool pauseStatus)
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnApplicationPause(pauseStatus);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationQuitManagers()
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnApplicationQuit();
            }
        }

        //---------------------------------------------------------------------
        private void OnGUIManagers()
        {
            for (int i = 0; i < managersToUpdate.Count; ++i)
            {
                managersToUpdate[i].OnGUI();
            }
        }
        #endregion Manager Events
    }
}
