namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.TickableFramework.Interfaces;

    [DebuggerDisplay("Alive: {alive}, {tickable.GetType().Name} / {tickable}")]
    internal class AliveTickable
        : IPriority
        , IEquatable<ITickable>
    {
        #region Fields
        public bool alive;
        private ITickable tickable;
        #endregion

        #region Constructors
        public AliveTickable(ITickable tickable)
        {
            this.tickable = tickable;
            alive = true;
        }
        #endregion

        #region Class Methods
        public TTickable Get<TTickable>()
            where TTickable : class, ITickable
        {
            return tickable as TTickable;
        }
        #endregion

        #region IEquatable<ITickable> Members
        public bool Equals(ITickable other)
        {
            return tickable == other;
        }
        #endregion

        #region IPriority Members
        public int DefaultPriority { get{ throw new NotImplementedException();} }
        #endregion
    }
}
