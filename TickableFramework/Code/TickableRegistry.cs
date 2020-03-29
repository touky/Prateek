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
    using Mayfair.Core.Code.Utils.Types.Priority;
    using Prateek.DaemonCore.Code;
    using Prateek.TickableFramework.Code.Enums;
    using Prateek.TickableFramework.Code.Interfaces;
    using Prateek.TickableFramework.Code.Internal;
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public sealed class TickableRegistry : SingletonBehaviour<TickableRegistry>
    {
        //---------------------------------------------------------------------
        #region Fields
        private GameObject tickerObject;
        private TickableRegistryFrameBegin tickerBegin;
        private TickableRegistryFrameEnd tickerEnd;

        private TickableRegistryStarter tickableStarter = null;
        private List<ITickable> startingTickables = new List<ITickable>();
        private HashSet<ITickable> allTickables = new HashSet<ITickable>();
        private Dictionary<TickType, List<ITickable>> aliveTickables = new Dictionary<TickType, List<ITickable>>();
        private List<TickType> tickValues = new List<TickType>();
        #endregion

        //---------------------------------------------------------------------
        #region Unity Methods
        private void OnDestroy()
        {
            SetPauseFeedback(false);
        }
        #endregion

        //---------------------------------------------------------------------
        #region Unity Application Methods
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            Execute(TickType.OnApplicationQuit);

            tickerBegin = null;
            tickerEnd = null;
        }

        //---------------------------------------------------------------------
        private void OnApplicationFocus(bool focusStatus)
        {
            Execute(TickType.OnApplicationFocus, focusStatus);
        }

        //---------------------------------------------------------------------
        private void OnApplicationPause(bool pauseStatus)
        {
            Execute(TickType.OnApplicationPause, pauseStatus);
        }
        #endregion

        //---------------------------------------------------------------------
        #region Unity EditorOnly Methods
        private void OnGUI()
        {
            Execute(TickType.OnGUI);
        }
        #endregion

        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        private void InitTickValues()
        {
            var tickMask = (TickType) 1;
            while (true)
            {
                if (tickMask == TickType.MAX)
                {
                    break;
                }

                tickValues.Add(tickMask);
                aliveTickables.Add(tickMask, new List<ITickable>());

                tickMask = (TickType) ((int) tickMask << 1);
            }
        }

        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        internal void OnStarterStart()
        {
            foreach (var tickable in startingTickables)
            {
                tickable.Start();
            }

            startingTickables.Clear();

            Destroy(tickableStarter);
            tickableStarter = null;
        }

        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        private void Execute(TickType tickType, float seconds, float unscaledSeconds = 0, bool status = false)
        {
            if (aliveTickables.TryGetValue(tickType, out var list))
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
        #endregion

        #region Service
        //---------------------------------------------------------------------
        protected override void OnAwake()
        {
            Initialize();
        }

        //---------------------------------------------------------------------
        public void Register(ITickable tickable)
        {
            if (allTickables.Contains(tickable))
            {
                throw new Exception();
            }

            RegisterInStarter(tickable);

            allTickables.Add(tickable);

            var tickType = tickable.TickType;
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

        //---------------------------------------------------------------------
        public void Unregister(ITickable tickable)
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
