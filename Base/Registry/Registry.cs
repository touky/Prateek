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
using Prateek.CodeGeneration;
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
using System.Reflection;
#endregion File namespaces

//-----------------------------------------------------------------------------
//Disabling obsolete warning for BaseGlobalManager
#pragma warning disable 618
namespace Prateek.Base
{
    //-------------------------------------------------------------------------
    public class Registry : BehaviourSingleton<Registry>
    {
        //-------------------------------------------------------------------------
        #region Settings
        [SerializeField]
        private List<string> loadOnAwake = new List<string>();

        [SerializeField, TypeRef(typeof(GlobalManager))]
        private List<string> createOnAwake = new List<string>();
        #endregion Settings

        //-------------------------------------------------------------------------
        #region Fields
        private bool sortManagers = false;
        private Dictionary<Type, List<RegistrableBehaviour>> registeredBehaviours = new Dictionary<Type, List<RegistrableBehaviour>>();
        private List<RegistrableBehaviour> emptyBehaviours = new List<RegistrableBehaviour>();

        private List<GlobalManager> registeredManagers = new List<GlobalManager>();
        private List<GlobalManager> startManagers = new List<GlobalManager>();
        private Dictionary<Type, GlobalManager> storedManagers = new Dictionary<Type, GlobalManager>();
        private Dictionary<Type, GlobalManager.BuilderBase> storedBuilder = new Dictionary<Type, GlobalManager.BuilderBase>();
        #endregion Fields

        //---------------------------------------------------------------------
        protected override void Awake()
        {
            sortManagers = true;

            //Load stored resources
            for (int i = 0; i < loadOnAwake.Count; i++)
            {
                var instance = GameObject.Instantiate(Resources.Load(loadOnAwake[i])) as GlobalManager;
                Registry.Instance.Register(instance.GetType(), instance);
            }

            //Create listed classes
            for (int i = 0; i < createOnAwake.Count; i++)
            {
                TryCreating(Helpers.Types.GetType(createOnAwake[i]));
            }

            base.Awake();
        }

        //---------------------------------------------------------------------
        protected void TryCreating(Type type, bool force_null = false)
        {
            GlobalManager.BuilderBase builder = null;
            if (!storedBuilder.ContainsKey(type))
            {
                var method_info = type.GetMethod(GlobalManager.CREATE_METHOD, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (method_info != null)
                {
                    builder = method_info.Invoke(null, null) as GlobalManager.BuilderBase;
                }
            }
            else
            {
                builder = storedBuilder[type];
            }

            if (builder != null)
            {
                storedBuilder[type] = builder;

                if (!force_null
                &&  builder.realType != null
                && !builder.realType.IsAbstract
                && !builder.realType.IsInterface)
                {
                    DoCreate(builder.realType);
                }
                else if (builder.emptyType != null
                     && !builder.emptyType.IsAbstract
                     && !builder.emptyType.IsInterface)
                {
                    DoCreate(builder.emptyType);
                }
                else
                {
                    // ERROR !
                }
            }
        }

        //---------------------------------------------------------------------
        protected void DoCreate(Type type)
        {
            var instance = ScriptableObject.CreateInstance(type) as GlobalManager;
            instance.name = type.Name + "(Instance)";
            Registry.Instance.Register(type, instance);
        }

        //---------------------------------------------------------------------
        #region Managers
        public T GetManager<T>() where T : class, IGlobalManager
        {
            var type = typeof(T);

            if (storedManagers.ContainsKey(type))
                return storedManagers[type] as T;

#if UNITY_EDITOR
            for (int i = 0; i < createOnAwake.Count; i++)
            {
                var testType = Helpers.Types.GetType(createOnAwake[i]);
                if (testType.IsSubclassOf(type))
                {
                    type = testType;
                }
            }
#endif // UNITY_EDITOR

            TryCreating(type);

            return storedManagers[type] as T;
        }

        //---------------------------------------------------------------------
        public void Register(Type type, GlobalManager manager)
        {
            if (type == null || manager == null)
                return;

            if (storedManagers.ContainsKey(type))
            {
                if (manager != null)
                {
                    Unregister(type);
                    storedManagers[type] = manager;
                }
            }
            else if (manager != null)
            {
                storedManagers.Add(type, manager);
            }

            sortManagers = true;

            if (!registeredManagers.Contains(manager))
            {
                registeredManagers.Add(manager);

                manager.OnInitialize();
                manager.OnRegister();

                startManagers.Add(manager);
            }
        }

        //---------------------------------------------------------------------
        public void Unregister(Type manager_type)
        {
            var manager = storedManagers[manager_type];
            if (registeredManagers.Contains(manager))
            {
                registeredManagers.Remove(manager);

                manager.OnUnregister();
                manager.OnDispose();

                TryCreating(manager_type, true);
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
                if (registeredBehaviours.TryGetValue(type, out behaviours) == false)
                {
                    registeredBehaviours[type] = (behaviours = new List<RegistrableBehaviour>());
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
                if (registeredBehaviours.TryGetValue(type, out behaviours))
                {
                    behaviours.Remove(behaviour);
                }
                type = type.BaseType;
            }
        }

        ////-------------------------------------------------------------------
        //public NoAllocIterator<T> GetRegistered<T>() where T : MonoBehaviour
        //{
        //    List<MonoBehaviour> behaviours;
        //    if (m_registered_behaviours.TryGetValue(typeof(T), out behaviours))
        //    {
        //        return new NoAllocIterator<T>(behaviours);
        //    }
        //    else
        //    {
        //        return new NoAllocIterator<T>(m_empty_behaviours);
        //    }
        //}


        //---------------------------------------------------------------------
        public List<T> GetRegistrable<T>() where T : RegistrableBehaviour
        {
            List<T> behaviours = null;
            GetRegistrable<T>(ref behaviours);
            return behaviours;
        }

        //---------------------------------------------------------------------
        public bool GetRegistrable<T>(ref List<T> behaviours) where T : RegistrableBehaviour
        {
            List<RegistrableBehaviour> temp = null; behaviours = null;
            if (registeredBehaviours.TryGetValue(typeof(T), out temp))
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
        #endregion RegistrableBehaviour

        //---------------------------------------------------------------------
        #region Unity events
        private void Update()
        {
            if (startManagers.Count > 0)
            {
                startManagers.Sort((a, b) => a.Priority - b.Priority);
                for (int i = 0; i < startManagers.Count; i++)
                {
                    startManagers[i].OnStart();
                }
                startManagers.Clear();
            }

            if (sortManagers)
            {
                sortManagers = false;
                registeredManagers.Sort((a, b) => a.Priority - b.Priority);
            }

            UpdateManagers();
        }

        //---------------------------------------------------------------------
        private void LateUpdate() { LateUpdateManagers(); }
        private void FixedUpdate() { FixedUpdateManagers(); }
        private void OnApplicationFocus(bool focusStatus) { OnApplicationFocusManagers(focusStatus); }
        private void OnApplicationPause(bool pauseStatus) { OnApplicationPauseManagers(pauseStatus); }

        //---------------------------------------------------------------------
        protected override void OnApplicationQuit()
        {
            OnApplicationQuitManagers();

            base.OnApplicationQuit();
        }

        //---------------------------------------------------------------------
        private void OnGUI() { OnGUIManagers(); }
        #endregion Unity events

        //---------------------------------------------------------------------
        #region Manager Events
        private void UpdateManagers()
        {
            for (int i = 0; i < registeredManagers.Count; ++i)
            {
                registeredManagers[i].OnUpdate(Time.deltaTime);
                registeredManagers[i].OnTimescaleIndependantUpdate(Time.unscaledDeltaTime);
            }
        }

        //---------------------------------------------------------------------
        private void LateUpdateManagers()
        {
            for (int i = 0; i < registeredManagers.Count; ++i)
            {
                registeredManagers[i].OnLateUpdate(Time.deltaTime);
            }
        }

        //---------------------------------------------------------------------
        private void FixedUpdateManagers()
        {
            for (int i = 0; i < registeredManagers.Count; ++i)
            {
                registeredManagers[i].OnFixedUpdate(Time.fixedDeltaTime);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationFocusManagers(bool focusStatus)
        {
            for (int i = 0; i < registeredManagers.Count; ++i)
            {
                registeredManagers[i].OnApplicationFocus(focusStatus);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationPauseManagers(bool pauseStatus)
        {
            for (int i = 0; i < registeredManagers.Count; ++i)
            {
                registeredManagers[i].OnApplicationPause(pauseStatus);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationQuitManagers()
        {
            for (int i = 0; i < registeredManagers.Count; ++i)
            {
                registeredManagers[i].OnApplicationQuit();
            }
        }

        //---------------------------------------------------------------------
        private void OnGUIManagers()
        {
            for (int i = 0; i < registeredManagers.Count; ++i)
            {
                registeredManagers[i].OnGUI();
            }
        }
        #endregion Manager Events
    }
}
#pragma warning restore 618
