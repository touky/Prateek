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
        #region Settings
        [SerializeField]
        private List<string> m_load_on_awake = new List<string>();

        [SerializeField, TypeRef(typeof(GlobalManager))]
        private List<string> m_create_on_awake = new List<string>();
        #endregion Settings

        #region Fields
        private bool m_sort_managers = false;
        private Dictionary<Type, List<RegistrableBehaviour>> m_registered_behaviours = new Dictionary<Type, List<RegistrableBehaviour>>();
        private List<RegistrableBehaviour> m_empty_behaviours = new List<RegistrableBehaviour>();

        private List<GlobalManager> m_registered_managers = new List<GlobalManager>();
        private List<GlobalManager> m_start_managers = new List<GlobalManager>();
        private Dictionary<Type, GlobalManager> m_stored_managers = new Dictionary<Type, GlobalManager>();
        private Dictionary<Type, GlobalManager.BuilderBase> m_stored_builder = new Dictionary<Type, GlobalManager.BuilderBase>();
        #endregion Fields

        //---------------------------------------------------------------------
        protected override void Awake()
        {
            m_sort_managers = true;

            //Load stored resources
            for (int i = 0; i < m_load_on_awake.Count; i++)
            {
                var instance = GameObject.Instantiate(Resources.Load(m_load_on_awake[i])) as GlobalManager;
                Registry.instance.Register(instance.GetType(), instance);
            }

            //Create listed classes
            for (int i = 0; i < m_create_on_awake.Count; i++)
            {
                TryCreating(Helpers.Types.GetType(m_create_on_awake[i]));
            }

            base.Awake();
        }

        //---------------------------------------------------------------------
        protected void TryCreating(Type type, bool force_null = false)
        {
            GlobalManager.BuilderBase builder = null;
            if (!m_stored_builder.ContainsKey(type))
            {
                var method_info = type.GetMethod(GlobalManager.CREATE_METHOD, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (method_info != null)
                {
                    builder = method_info.Invoke(null, null) as GlobalManager.BuilderBase;
                }
            }
            else
            {
                builder = m_stored_builder[type];
            }

            if (builder != null)
            {
                m_stored_builder[type] = builder;

                if (!force_null
                &&  builder.m_real_type != null
                && !builder.m_real_type.IsAbstract
                && !builder.m_real_type.IsInterface)
                {
                    DoCreate(builder.m_real_type);
                }
                else if (builder.m_empty_type != null
                     && !builder.m_empty_type.IsAbstract
                     && !builder.m_empty_type.IsInterface)
                {
                    DoCreate(builder.m_empty_type);
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
            Registry.instance.Register(type, instance);
        }

        //---------------------------------------------------------------------
        #region Managers
        public T GetManager<T>() where T : class, IGlobalManager
        {
            var type = typeof(T);

            if (m_stored_managers.ContainsKey(type))
                return m_stored_managers[type] as T;

#if UNITY_EDITOR
            for (int i = 0; i < m_create_on_awake.Count; i++)
            {
                var test_type = Helpers.Types.GetType(m_create_on_awake[i]);
                if (test_type.IsSubclassOf(type))
                {
                    type = test_type;
                }
            }
#endif // UNITY_EDITOR

            TryCreating(type);

            return m_stored_managers[type] as T;
        }

        //---------------------------------------------------------------------
        public void Register(Type type, GlobalManager manager)
        {
            if (type == null || manager == null)
                return;

            if (m_stored_managers.ContainsKey(type))
            {
                if (manager != null)
                {
                    Unregister(type);
                    m_stored_managers[type] = manager;
                }
            }
            else if (manager != null)
            {
                m_stored_managers.Add(type, manager);
            }

            m_sort_managers = true;

            if (!m_registered_managers.Contains(manager))
            {
                m_registered_managers.Add(manager);

                manager.OnInitialize();
                manager.OnRegister();

                m_start_managers.Add(manager);
            }
        }

        //---------------------------------------------------------------------
        public void Unregister(Type manager_type)
        {
            var manager = m_stored_managers[manager_type];
            if (m_registered_managers.Contains(manager))
            {
                m_registered_managers.Remove(manager);

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
                if (m_registered_behaviours.TryGetValue(type, out behaviours) == false)
                {
                    m_registered_behaviours[type] = (behaviours = new List<RegistrableBehaviour>());
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
                if (m_registered_behaviours.TryGetValue(type, out behaviours))
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
            if (m_registered_behaviours.TryGetValue(typeof(T), out temp))
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
            if (m_start_managers.Count > 0)
            {
                m_start_managers.Sort((a, b) => a.priority - b.priority);
                for (int i = 0; i < m_start_managers.Count; i++)
                {
                    m_start_managers[i].OnStart();
                }
                m_start_managers.Clear();
            }

            if (m_sort_managers)
            {
                m_sort_managers = false;
                m_registered_managers.Sort((a, b) => a.priority - b.priority);
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
            for (int i = 0; i < m_registered_managers.Count; ++i)
            {
                m_registered_managers[i].OnUpdate(Time.deltaTime);
                m_registered_managers[i].OnTimescaleIndependantUpdate(Time.unscaledDeltaTime);
            }
        }

        //---------------------------------------------------------------------
        private void LateUpdateManagers()
        {
            for (int i = 0; i < m_registered_managers.Count; ++i)
            {
                m_registered_managers[i].OnLateUpdate(Time.deltaTime);
            }
        }

        //---------------------------------------------------------------------
        private void FixedUpdateManagers()
        {
            for (int i = 0; i < m_registered_managers.Count; ++i)
            {
                m_registered_managers[i].OnFixedUpdate(Time.fixedDeltaTime);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationFocusManagers(bool focusStatus)
        {
            for (int i = 0; i < m_registered_managers.Count; ++i)
            {
                m_registered_managers[i].OnApplicationFocus(focusStatus);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationPauseManagers(bool pauseStatus)
        {
            for (int i = 0; i < m_registered_managers.Count; ++i)
            {
                m_registered_managers[i].OnApplicationPause(pauseStatus);
            }
        }

        //---------------------------------------------------------------------
        private void OnApplicationQuitManagers()
        {
            for (int i = 0; i < m_registered_managers.Count; ++i)
            {
                m_registered_managers[i].OnApplicationQuit();
            }
        }

        //---------------------------------------------------------------------
        private void OnGUIManagers()
        {
            for (int i = 0; i < m_registered_managers.Count; ++i)
            {
                m_registered_managers[i].OnGUI();
            }
        }
        #endregion Manager Events
    }
}
#pragma warning restore 618
