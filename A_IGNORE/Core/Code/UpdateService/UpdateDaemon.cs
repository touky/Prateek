namespace Mayfair.Core.Code.UpdateService
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Mayfair.Core.Code.LoadingProcess;
    using Messages;
    using Prateek.Runtime.CommandFramework;
    using Service;
    using UnityEngine;
    using Utils.Debug;

    /// <summary>
    /// This should not be used as an example of Service/Provider/Messaging interaction
    /// as it violates the asynchronicity goals of our systems.
    /// </summary>
    public class UpdateDaemon : ReceiverDaemonOverseer<UpdateDaemon, UpdateProvider>
    {
        private Dictionary<UpdateFrequency, List<HashSet<IUpdatable>>> registeredUpdatables;
        private Dictionary<UpdateFrequency, int> updateFrequencyIndex;
        private Dictionary<IUpdatable, UpdateFrequency> registeredUpdatablesToFrequency;

        protected override void OnServantRegistered(UpdateProvider servant)
        {
            base.OnServantRegistered(servant);

            foreach (var updateProvider in AllServants)
            {
                updateProvider.updateAction = null;
            }

            FirstAliveServant.updateAction += OnUpdate;
        }

        protected override void OnAwake()
        {
            registeredUpdatables = new Dictionary<UpdateFrequency, List<HashSet<IUpdatable>>>();
            AddUpdatablesList(UpdateFrequency.EveryFrame);
            AddUpdatablesList(UpdateFrequency.EverySecondFrame);
            AddUpdatablesList(UpdateFrequency.EveryFifthFrame);
            AddUpdatablesList(UpdateFrequency.EveryFifteenthFrame);
            AddUpdatablesList(UpdateFrequency.EveryThirtiethFrame);

            updateFrequencyIndex = new Dictionary<UpdateFrequency, int>
            {
                { UpdateFrequency.EveryFrame, 0 },
                { UpdateFrequency.EverySecondFrame, 0 },
                { UpdateFrequency.EveryFifthFrame, 0 },
                { UpdateFrequency.EveryFifteenthFrame, 0 },
                { UpdateFrequency.EveryThirtiethFrame, 0 }
            };

            registeredUpdatablesToFrequency = new Dictionary<IUpdatable, UpdateFrequency>();
        }

        public override void DefineCommandReceiverActions()
        {
            CommandReceiver.SetActionFor<RegisterForUpdate>(OnRegisterForUpdate);
            CommandReceiver.SetActionFor<UnregisterForUpdate>(OnUnregisterForUpdate);
        }

        private void AddUpdatablesList(UpdateFrequency updateFrequency)
        {
            List<HashSet<IUpdatable>> newUpdatablesList = new List<HashSet<IUpdatable>>((int)updateFrequency);
            for (int i = 0; i < (int)updateFrequency; i++)
            {
                newUpdatablesList.Add(new HashSet<IUpdatable>());
            }

            registeredUpdatables.Add(updateFrequency, newUpdatablesList);
        }

        private void OnRegisterForUpdate(RegisterForUpdate notice)
        {
            RemoveExistingRegistrationIfExists(notice.UpdatableObject, notice.UpdateFrequency);

            List<HashSet<IUpdatable>> updatablesList = registeredUpdatables[notice.UpdateFrequency];

            int leastPopulatedIndex = GetLeastPopulatedIndex(updatablesList);

            updatablesList[leastPopulatedIndex].Add(notice.UpdatableObject);
            registeredUpdatablesToFrequency.Add(notice.UpdatableObject, notice.UpdateFrequency);
        }

        private int GetLeastPopulatedIndex(List<HashSet<IUpdatable>> updatablesList)
        {
            int leastPopulatedIndex = 0;
            for (int i = 0; i < updatablesList.Count; i++)
            {
                HashSet<IUpdatable> list = updatablesList[i];
                if (list.Count < updatablesList[leastPopulatedIndex].Count)
                {
                    leastPopulatedIndex = i;

                    if (updatablesList[leastPopulatedIndex].Count == 0)
                    {
                        break;
                    }
                }
            }

            Debug.Assert(leastPopulatedIndex != int.MaxValue);

            return leastPopulatedIndex;
        }

        private void OnUnregisterForUpdate(UnregisterForUpdate notice)
        {
            if (!registeredUpdatablesToFrequency.TryGetValue(notice.UpdatableObject, out UpdateFrequency updateFrequency))
            {
                throw new ArgumentException("Updatable object is not registered to receive updates");
            }

            RemoveRegisteredUpdatable(notice.UpdatableObject, updateFrequency);
        }

        private void RemoveExistingRegistrationIfExists(IUpdatable updatableObject, UpdateFrequency newUpdateFrequency)
        {
            if (registeredUpdatablesToFrequency.ContainsKey(updatableObject))
            {
                UpdateFrequency oldUpdateFrequency = registeredUpdatablesToFrequency[updatableObject];
                DebugTools.LogWarning($"An Updatable object may only be registered once. Changing update frequency from {oldUpdateFrequency} to {newUpdateFrequency}");

                RemoveRegisteredUpdatable(updatableObject, oldUpdateFrequency);
            }
        }

        private void RemoveRegisteredUpdatable(IUpdatable updatableObject, UpdateFrequency updateFrequency)
        {
            List<HashSet<IUpdatable>> updatablesGroup = registeredUpdatables[updateFrequency];
            foreach (HashSet<IUpdatable> updatables in updatablesGroup)
            {
                if (updatables.Remove(updatableObject))
                {
                    break;
                }
            }

            registeredUpdatablesToFrequency.Remove(updatableObject);
        }

        private void OnUpdate()
        {
            UpdateRegisteredUpdatables(UpdateFrequency.EveryFrame);
            UpdateRegisteredUpdatables(UpdateFrequency.EverySecondFrame);
            UpdateRegisteredUpdatables(UpdateFrequency.EveryFifthFrame);
            UpdateRegisteredUpdatables(UpdateFrequency.EveryFifteenthFrame);
            UpdateRegisteredUpdatables(UpdateFrequency.EveryThirtiethFrame);
        }

        private void UpdateRegisteredUpdatables(UpdateFrequency updateFrequency)
        {
            int updateIndex = updateFrequencyIndex[updateFrequency];

            foreach (IUpdatable updatable in registeredUpdatables[updateFrequency][updateIndex])
            {
                updatable.OnUpdate();
            }

            updateFrequencyIndex[updateFrequency] = (updateIndex + 1) % (int)updateFrequency;
        }
    }
}