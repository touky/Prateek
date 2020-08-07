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
namespace Prateek.Runtime.TickableFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Singleton;
    using Prateek.Runtime.TickableFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.TickableGroups;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.LowLevel;

    ///-------------------------------------------------------------------------
    internal sealed class TickableRegistry : SingletonBehaviour<TickableRegistry>
    {
        #region Fields
        ///---------------------------------------------------------------------
        private List<AliveTickable> allTickables = new List<AliveTickable>();

        private List<AliveTickable> pendingRegistry = new List<AliveTickable>();

        private ApplicationFeedbackTickableGroup applicationGroup = null;

        private List<TickableGroup> tickableGroups = new List<TickableGroup>
        {
            new RegistryCleanupGroup(),
            new EarlyUpdateTickableGroup(),
            new PreUpdateTickableGroup(),
            new PreLateUpdateTickableGroup(),
            new PostLateUpdateTickableGroup(),
            new ApplicationFeedbackTickableGroup()
        };
        #endregion

        #region Unity Methods
        ///---------------------------------------------------------------------
        private void OnDestroy()
        {
            SetPauseFeedback(false);
        }
        #endregion

        #region Unity Application Methods
        ///---------------------------------------------------------------------
        protected override void OnApplicationQuit()
        {
            base.OnApplicationQuit();

            applicationGroup.ApplicationQuit();
        }

        ///---------------------------------------------------------------------
        private void OnApplicationFocus(bool focusStatus)
        {
            applicationGroup.ApplicationFocus(focusStatus);
        }

        ///---------------------------------------------------------------------
        private void OnApplicationPause(bool pauseStatus)
        {
            applicationGroup.OnApplicationPause(pauseStatus);
        }
        #endregion

        #region Register/Unregister
        ///---------------------------------------------------------------------
        protected override void OnAwake() { }

        ///---------------------------------------------------------------------
        internal static void Register(ITickable tickable)
        {
            var aliveTickable = new AliveTickable(tickable);
            Instance.pendingRegistry.Add(aliveTickable);
            Instance.allTickables.Add(aliveTickable);
        }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        [RuntimeInitializeOnLoadMethod]
        private static void RuntimeInitialize()
        {
            Instance.InitializeApplicationGroup();

            Instance.InjectTickableLoop();
        }

        ///---------------------------------------------------------------------
        private void InitializeApplicationGroup()
        {
            SetPauseFeedback(true);
            applicationGroup = Instance.tickableGroups.Find(x =>
            {
                return x.GetType() == typeof(ApplicationFeedbackTickableGroup);
            }) as ApplicationFeedbackTickableGroup;
        }

        ///---------------------------------------------------------------------
        private void InjectTickableLoop()
        {
            //Get the default loop
            var playerLoop = PlayerLoop.GetDefaultPlayerLoop();
            for (var s = 0; s < playerLoop.subSystemList.Length; s++)
            {
                var system = playerLoop.subSystemList[s];
                foreach (var tickableGroup in Instance.tickableGroups)
                {
                    //Locate a tickable group with the same name
                    if (tickableGroup.Name != system.type.Name)
                    {
                        continue;
                    }

                    //Create a new job in the sub system list
                    var tickableGroupSystem = new PlayerLoopSystem
                    {
                        type = tickableGroup.GetType(),
                        updateDelegate = tickableGroup.Tick
                    };

                    //Inject the new tickable group in the subsystem list
                    var subSystemList = new List<PlayerLoopSystem>(system.subSystemList);
                    if (tickableGroup.InjectAtTheEnd)
                    {
                        subSystemList.Add(tickableGroupSystem);
                    }
                    else
                    {
                        subSystemList.Insert(0, tickableGroupSystem);
                    }

                    //Restore the system where it was before
                    system.subSystemList = subSystemList.ToArray();
                    playerLoop.subSystemList[s] = system;
                    break;
                }
            }

            //Set the modified playerLoop back into unity
            PlayerLoop.SetPlayerLoop(playerLoop);
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

        ///---------------------------------------------------------------------
        internal static void Unregister(ITickable tickable)
        {
            var aliveTickable = Instance.allTickables.Find(x =>
            {
                return x.Equals(tickable);
            });

            if (aliveTickable == null)
            {
                return;
            }

            aliveTickable.alive = false;
        }

        ///---------------------------------------------------------------------
        internal static void FlushPendingRegistry()
        {
            Instance.InternalFlushPendingRegistry();
        }

        ///---------------------------------------------------------------------
        internal static void ClearDeadTickables()
        {
            Instance.InternalClearDeadTickables();
        }

        ///---------------------------------------------------------------------
        private void InternalFlushPendingRegistry()
        {
            foreach (var aliveTickable in pendingRegistry)
            {
                if (!aliveTickable.alive)
                {
                    allTickables.Remove(aliveTickable);
                    continue;
                }

                foreach (var tickableGroup in tickableGroups)
                {
                    tickableGroup.Register(aliveTickable);
                }
            }

            pendingRegistry.Clear();
        }

        ///---------------------------------------------------------------------
        private void InternalClearDeadTickables()
        {
            for (var t = 0; t < allTickables.Count; t++)
            {
                var deadtickable = allTickables[t];
                if (deadtickable.alive)
                {
                    continue;
                }

                foreach (var tickableGroup in tickableGroups)
                {
                    tickableGroup.Unregister(deadtickable);
                }

                allTickables.RemoveAt(t--);
            }
        }
        #endregion
    }
}
