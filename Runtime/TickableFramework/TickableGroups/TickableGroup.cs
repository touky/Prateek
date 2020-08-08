namespace Prateek.Runtime.TickableFramework.TickableGroups
{
    internal abstract class TickableGroup
    {
        #region Fields
        protected string name;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public abstract bool InjectAtTheEnd { get; }
        #endregion

        #region Register/Unregister
        public abstract void Register(AliveTickable tickable);
        #endregion

        #region Class Methods
        public abstract void Tick();
        public abstract void Unregister(AliveTickable tickable);
        #endregion
    }
}
