namespace Mayfair.Core.Code.BaseBehaviour
{
    using System.Collections.Generic;
    using Prateek.DaemonFramework.Code;
    using Prateek.TickableFramework.Code.Enums;
    using UnityEngine;

    public sealed class AutoDisableDaemon : Daemon<AutoDisableDaemon, AutoDisableServant>
    {
        #region Static and Constants
        //10 frames alives
        private const double MAX_TIME_AWAKE = 10.0 / 30.0;
        #endregion

        #region Fields
        private List<DisablerContainer> disablerContainers = new List<DisablerContainer>();
        #endregion

        #region Properties
        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateEndLate; }
        }
        #endregion

        #region Register/Unregister
        protected override void OnAwake() { }
        #endregion

        #region Class Methods
        public override void TickLate(TickableFrame tickableFrame, float seconds)
        {
            base.TickLate(tickableFrame, seconds);

            RefreshAwaken();
        }

        private void RefreshAwaken()
        {
            //Remove all the behaviour that need disabling
            //All of them are ordered, so we can stop asap
            while (disablerContainers.Count > 0)
            {
                var container = disablerContainers[0];
                if (Time.realtimeSinceStartup - container.lastAwakeTimeMark > MAX_TIME_AWAKE)
                {
                    if (container.behaviour != null)
                    {
                        container.behaviour.enabled = false;
                    }

                    disablerContainers.RemoveAt(0);
                    continue;
                }

                break;
            }
        }

        private void InternalWakeUp(AutoDisableBehaviour behaviour)
        {
            var index = disablerContainers.FindIndex(x =>
            {
                return x.behaviour == behaviour;
            });

            if (index < 0)
            {
                disablerContainers.Add(new DisablerContainer
                {
                    lastAwakeTimeMark = Time.realtimeSinceStartup,
                    behaviour = behaviour
                });
            }
            else
            {
                var container = disablerContainers[index];
                container.lastAwakeTimeMark = Time.realtimeSinceStartup;

                disablerContainers.RemoveAt(index);
                disablerContainers.Add(container);
            }

            behaviour.enabled = true;
        }

        private void InternalRemove(AutoDisableBehaviour behaviour)
        {
            var index = disablerContainers.FindIndex(x =>
            {
                return x.behaviour == behaviour;
            });

            if (index > 0)
            {
                disablerContainers.RemoveAt(index);
            }
        }

        public static void WakeUp(AutoDisableBehaviour behaviour)
        {
            if (IsApplicationQuitting)
            {
                return;
            }

            Instance.InternalWakeUp(behaviour);
        }

        public static void Remove(AutoDisableBehaviour behaviour)
        {
            if (IsApplicationQuitting)
            {
                return;
            }

            Instance.InternalRemove(behaviour);
        }
        #endregion

        #region Nested type: DisablerContainer
        private struct DisablerContainer
        {
            public double lastAwakeTimeMark;
            public AutoDisableBehaviour behaviour;
        }
        #endregion
    }
}
