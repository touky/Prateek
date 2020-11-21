namespace Prateek.Runtime.DaemonFramework
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework.Enumerators;
    using Prateek.Runtime.DaemonFramework.Enums;
    using Prateek.Runtime.DaemonFramework.Interfaces;

    /// <summary>
    /// This Daemon automatically creates itself when a <typeparam name="TServant"/> servant's method <see cref="IServant.Startup()"/> is called
    /// and it registers itself to this Daemon
    /// </summary>
    /// <typeparam name="TDaemon">The Deamon class type inheriting from this class</typeparam>
    /// <typeparam name="TServant">The Base class for this Daemon Overseer</typeparam>
    public abstract class DaemonOverseer<TDaemon, TServant>
        : Daemon<TDaemon>, IDaemonOverseer<TServant>
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

        protected ServantEnumerator<TServant> AllAliveServants { get { return new ServantEnumerator<TServant>(servants, false); } }

        protected ServantEnumerator<TServant> AllServants { get { return new ServantEnumerator<TServant>(servants, true); } }
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

        /// <summary>
        /// Callback to indicate a new servant registered
        /// - It is always called after adding it to the <see cref="servants"/>
        /// - This call also performs the <seealso cref="PriorityExtensions.SortWithPriorities{TPriority}(List{TPriority})"/>
        /// </summary>
        /// <param name="servant">The registering servant</param>
        protected virtual void OnServantRegistered(TServant servant)
        {
            servants.SortWithPriorities();
        }

        /// <summary>
        /// Callback to indicate a new servant registered
        /// - It is always called after removing it to the <see cref="servants"/>
        /// - This call also performs the <seealso cref="PriorityExtensions.SortWithPriorities{TPriority}(List{TPriority})"/>
        /// </summary>
        /// <param name="servant">The registering servant</param>
        protected virtual void OnServantUnregistered(TServant servant)
        {
            servants.SortWithPriorities();
        }
        #endregion

        #region IDaemonOverseer<TServant> Members
        void IDaemonOverseer<TServant>.Register(TServant servant)
        {
            if (servants.Contains(servant))
            {
                return;
            }

            if (servant is IServantInternal servantInternal)
            {
                servantInternal.Overseer = this;
            }

            servants.Add(servant);

            OnServantRegistered(servant);
        }

        void IDaemonOverseer<TServant>.Unregister(TServant servant)
        {
            if (!servants.Contains(servant))
            {
                return;
            }

            if (servant is IServantInternal servantInternal)
            {
                servantInternal.Overseer = null;
            }

            servants.Remove(servant);

            OnServantUnregistered(servant);
        }
        #endregion
    }
}
