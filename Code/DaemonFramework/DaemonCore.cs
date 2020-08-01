namespace Prateek.DaemonFramework.Code
{
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code.Interfaces.IPriority;
    using Prateek.Core.Code.Singleton;
    using Prateek.DaemonFramework.Code.Enumerators;
    using Prateek.DaemonFramework.Code.Enums;
    using Prateek.DaemonFramework.Code.Interfaces;
    using Prateek.TickableFramework.Code.Enums;
    using Prateek.TickableFramework.Code.Interfaces;
    using UnityEngine;

    public abstract class Daemon<TDaemon, TServant>
        : SingletonBehaviour<TDaemon>, IDaemon<TServant>, ITickable
        where TDaemon : Daemon<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Fields
        private List<TServant> servants = new List<TServant>();
        #endregion

        #region Class Methods
        internal static void ChangeStatus(StatusAction action, TServant servant)
        {
            IDaemon<TServant> instance = Instance;

            //This will only happen when OnApplicationQuit has been called
            if (instance == null)
            {
                return;
            }

            switch (action)
            {
                case StatusAction.Register:
                {
                    instance.Register(servant);
                    break;
                }
                case StatusAction.Unregister:
                {
                    instance.Unregister(servant);
                    break;
                }
                default:
                {
                    throw new Exception($"{servant.GetType().Name} sent idenfication without the action setup.");
                }
            }
        }

        protected TServant GetFirstAliveBranch()
        {
            foreach (var servant in servants)
            {
                if (servant.IsAlive)
                {
                    return servant;
                }
            }

            return default;
        }

        protected IEnumerable<TServant> GetValidServants(bool includeInvalid = false)
        {
            return new ServantEnumerable<TServant>(servants, includeInvalid);
        }

        protected virtual void OnServantRegistered(TServant servant) { }
        protected virtual void OnServantUnregistered(TServant servant) { }
        #endregion

        #region IDaemon<TServant> Members
        void IDaemon<TServant>.Register(TServant servant)
        {
            if (servants.Contains(servant))
            {
                return;
            }

            servants.Add(servant);
            servants.SortWithPriorities();

            OnServantRegistered(servant);
        }

        void IDaemon<TServant>.Unregister(TServant servant)
        {
            if (!servants.Contains(servant))
            {
                return;
            }

            servants.Remove(servant);
            servants.SortWithPriorities();

            OnServantUnregistered(servant);
        }
        #endregion

        #region ITickable Members
        public virtual int Priority
        {
            get { return 0; }
        }

        public virtual TickableSetup TickableSetup
        {
            get { return TickableSetup.Nothing; }
        }

        public virtual void InitializeTickable() { }

        public virtual void TickFixed(TickableFrame tickableFrame, float seconds) { }

        public virtual void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds) { }

        public virtual void TickLate(TickableFrame tickableFrame, float seconds) { }

        public virtual void ApplicationIsQuitting() { }

        public virtual void ApplicationChangeFocus(bool appStatus) { }

        public virtual void ApplicationChangePause(bool appStatus) { }

        public virtual void DrawGUI() { }
        #endregion
    }
}
