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
namespace Prateek.TickableFramework.Code
{
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code.Singleton;
    using Prateek.Core.Code.Interfaces.IPriority;
    using Prateek.TickableFramework.Code.Enums;
    using Prateek.TickableFramework.Code.Interfaces;
    using Prateek.TickableFramework.Code.Internal;
    using UnityEditor;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public sealed class TickableRegistry : SingletonBehaviour<TickableRegistry>
    {
        ///---------------------------------------------------------------------
        #region Fields
        private GameObject tickerObject;
        private TickableRegistryFrameBegin tickerBegin;
        private TickableRegistryFrameEnd tickerEnd;

        private TickableRegistryStarter tickableStarter = null;
        private List<ITickable> startingTickables = new List<ITickable>();
        private HashSet<ITickable> allTickables = new HashSet<ITickable>();
        private Dictionary<TickableSetup, List<ITickable>> aliveTickables = new Dictionary<TickableSetup, List<ITickable>>();
        private List<TickableSetup> tickValues = new List<TickableSetup>();
        #endregion

        ///---------------------------------------------------------------------
        #region Unity Methods
        private void OnDestroy()
        {
            SetPauseFeedback(false);
        }
        #endregion

        ///---------------------------------------------------------------------
        #region Unity Application Methods
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            Execute(TickableSetup.OnApplicationQuit);

            tickerBegin = null;
            tickerEnd = null;
        }

        ///---------------------------------------------------------------------
        private void OnApplicationFocus(bool focusStatus)
        {
            Execute(TickableSetup.OnApplicationFocus, focusStatus);
        }

        ///---------------------------------------------------------------------
        private void OnApplicationPause(bool pauseStatus)
        {
            Execute(TickableSetup.OnApplicationPause, pauseStatus);
        }
        #endregion

        ///---------------------------------------------------------------------
        #region Unity EditorOnly Methods
        private void OnGUI()
        {
            Execute(TickableSetup.OnGUI);
        }
        #endregion

        ///---------------------------------------------------------------------
        #region Class Methods
        private void Initialize()
        {
            if (tickerBegin != null || tickerEnd != null)
            {
                return;
            }

            InitTickValues();

            SetPauseFeedback(true);

            tickerObject = new GameObject("Registry Tickers");
            tickerObject.transform.SetParent(transform);
            tickerObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            tickerBegin = tickerObject.AddComponent<TickableRegistryFrameBegin>();
            tickerBegin.registry = this;
            tickerEnd = tickerObject.AddComponent<TickableRegistryFrameEnd>();
            tickerEnd.registry = this;

            var setup = Resources.Load<RegistrySetup>("RegistrySetup");
            if (setup != null)
            {
                setup.Initialize();
            }
        }

        ///---------------------------------------------------------------------
        private void InitTickValues()
        {
            var tickMask = (TickableSetup) 1;
            while (true)
            {
                if (tickMask == TickableSetup.MAX)
                {
                    break;
                }

                tickValues.Add(tickMask);
                aliveTickables.Add(tickMask, new List<ITickable>());

                tickMask = (TickableSetup) ((int) tickMask << 1);
            }
        }

        ///---------------------------------------------------------------------
        private void RegisterInStarter(ITickable tickable)
        {
            if (tickableStarter == null)
            {
                tickableStarter = gameObject.AddComponent<TickableRegistryStarter>();
                tickableStarter.registry = this;
            }

            startingTickables.Add(tickable);
            startingTickables.SortWithPriorities();
        }

        ///---------------------------------------------------------------------
        internal void StartTickables()
        {
            foreach (var tickable in startingTickables)
            {
                tickable.InitializeTickable();
            }

            startingTickables.Clear();

            Destroy(tickableStarter);
            tickableStarter = null;
        }

        ///---------------------------------------------------------------------
        internal void Execute(TickableSetup tickableSetup, bool status = false)
        {
            switch (tickableSetup)
            {
                case TickableSetup.UpdateBegin:
                case TickableSetup.UpdateEnd:
                {
                    Execute(tickableSetup, Time.deltaTime, Time.unscaledDeltaTime);
                    break;
                }
                case TickableSetup.UpdateBeginFixed:
                case TickableSetup.UpdateEndFixed:
                {
                    Execute(tickableSetup, Time.fixedDeltaTime);
                    break;
                }
                case TickableSetup.UpdateBeginLate:
                case TickableSetup.UpdateEndLate:
                {
                    Execute(tickableSetup, Time.deltaTime);
                    break;
                }
                default:
                {
                    Execute(tickableSetup, 0, 0, status);
                    break;
                }
            }
        }

        ///---------------------------------------------------------------------
        private void Execute(TickableSetup tickableSetup, float seconds, float unscaledSeconds = 0, bool status = false)
        {
            if (aliveTickables.TryGetValue(tickableSetup, out var list))
            {
                switch (tickableSetup)
                {
                    case TickableSetup.UpdateBegin:
                    case TickableSetup.UpdateEnd:
                    {
                        var frameEvent = tickableSetup == TickableSetup.UpdateBegin ? TickableFrame.FrameBegin : TickableFrame.FrameEnd;
                        foreach (var tickable in list)
                        {
                            tickable.Tick(frameEvent, seconds, unscaledSeconds);
                        }

                        break;
                    }
                    case TickableSetup.UpdateBeginFixed:
                    case TickableSetup.UpdateEndFixed:
                    {
                        var frameEvent = tickableSetup == TickableSetup.UpdateBeginFixed ? TickableFrame.FrameBegin : TickableFrame.FrameEnd;
                        foreach (var tickable in list)
                        {
                            tickable.TickFixed(frameEvent, seconds);
                        }

                        break;
                    }
                    case TickableSetup.UpdateBeginLate:
                    case TickableSetup.UpdateEndLate:
                    {
                        var frameEvent = tickableSetup == TickableSetup.UpdateBegin ? TickableFrame.FrameBegin : TickableFrame.FrameEnd;
                        foreach (var tickable in list)
                        {
                            tickable.TickLate(frameEvent, seconds);
                        }

                        break;
                    }
                    case TickableSetup.OnApplicationFocus:
                    {
                        foreach (var tickable in list)
                        {
                            tickable.ApplicationChangeFocus(status);
                        }

                        break;
                    }
                    case TickableSetup.OnApplicationPause:
                    {
                        foreach (var tickable in list)
                        {
                            tickable.ApplicationChangePause(status);
                        }

                        break;
                    }
                    case TickableSetup.OnApplicationQuit:
                    {
                        foreach (var tickable in list)
                        {
                            tickable.ApplicationIsQuitting();
                        }

                        break;
                    }
                    case TickableSetup.OnGUI:
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

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        private void PauseStateChanged(PauseState state)
        {
#if UNITY_EDITOR
            OnApplicationPause(state == PauseState.Paused);
#endif //UNITY_EDITOR
        }
        #endregion

        #region Service
        ///---------------------------------------------------------------------
        protected override void OnAwake()
        {
            Initialize();
        }

        ///---------------------------------------------------------------------
        public static void Register(ITickable tickable)
        {
            Instance.InternalRegister(tickable);
        }

        ///---------------------------------------------------------------------
        public static void Unregister(ITickable tickable)
        {
            Instance.InternalUnregister(tickable);
        }

        ///---------------------------------------------------------------------
        private void InternalRegister(ITickable tickable)
        {
            if (allTickables.Contains(tickable))
            {
                throw new Exception();
            }

            RegisterInStarter(tickable);

            allTickables.Add(tickable);

            var tickType = tickable.TickableSetup;
            foreach (var value in tickValues)
            {
                if (!tickType.HasFlag(value))
                {
                    continue;
                }

                var list = aliveTickables[value];
                list.Add(tickable);
                list.SortWithPriorities();
            }
        }

        ///---------------------------------------------------------------------
        private void InternalUnregister(ITickable tickable)
        {
            if (!allTickables.Contains(tickable))
            {
                throw new Exception();
            }

            allTickables.Remove(tickable);
            startingTickables.Remove(tickable);

            foreach (var tickablesValue in aliveTickables.Values)
            {
                tickablesValue.Remove(tickable);
            }
        }
        #endregion
    }
}
