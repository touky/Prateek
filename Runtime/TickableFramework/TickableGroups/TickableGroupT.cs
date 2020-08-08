namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.TickableFramework.Interfaces;

    internal abstract class TickableGroup<TTickable> : TickableGroup
        where TTickable : class, ITickable
    {
        #region Fields
        private List<AliveTickable> tickables = new List<AliveTickable>();
        #endregion

        #region Properties
        public Type Type
        {
            get { return typeof(TTickable); }
        }
        #endregion

        #region Constructors
        public TickableGroup()
        {
            name = typeof(TTickable).Name.TrimStart('I').Replace("Tickable", string.Empty);
        }
        #endregion

        #region Register/Unregister
        public override void Register(AliveTickable tickable)
        {
            if (tickable.Get<TTickable>() == null)
            {
                return;
            }

            tickables.Add(tickable);
            tickables.SortWithPriorities();
        }
        #endregion

        #region Class Methods
        public override void Unregister(AliveTickable tickable)
        {
            if (tickable.Get<TTickable>() == null)
            {
                return;
            }

            tickables.Remove(tickable);
        }

        public override void Tick()
        {
            foreach (var tickable in tickables)
            {
                if (!tickable.alive)
                {
                    continue;
                }

                Tick(tickable.Get<TTickable>());
            }
        }

        public abstract void Tick(TTickable tickable);
        #endregion
    }
}
