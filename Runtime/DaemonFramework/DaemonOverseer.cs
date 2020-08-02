namespace Prateek.Runtime.DaemonFramework
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework.Enumerators;
    using Prateek.Runtime.DaemonFramework.Enums;
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class DaemonOverseer<TDaemon, TServant>
        : Daemon<TDaemon>, IDaemonOverseer<TServant>, ITickable
        where TDaemon : DaemonOverseer<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Fields
        private List<TServant> servants = new List<TServant>();
        #endregion

        #region Properties
        protected TServant FirstAliveServant
        {
            get
            {
                foreach (var servant in servants)
                {
                    if (servant.IsAlive)
                    {
                        return servant;
                    }
                }

                return null;
            }
        }

        protected ServantEnumerator<TServant> AllAliveServants
        {
            get { return new ServantEnumerator<TServant>(servants, false); }
        }

        protected ServantEnumerator<TServant> AllServants
        {
            get { return new ServantEnumerator<TServant>(servants, true); }
        }
        #endregion

        #region Class Methods
        internal static void SetServantStatus(TServant servant, StatusAction action)
        {
            var instance = (IDaemonOverseer<TServant>) Instance;

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

        protected virtual void OnServantRegistered(TServant servant) { }
        protected virtual void OnServantUnregistered(TServant servant) { }
        #endregion

        #region IDaemon<TServant> Members
        void IDaemonOverseer<TServant>.Register(TServant servant)
        {
            if (servants.Contains(servant))
            {
                return;
            }

            servants.Add(servant);
            servants.SortWithPriorities();

            OnServantRegistered(servant);
        }

        void IDaemonOverseer<TServant>.Unregister(TServant servant)
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
    }
}
