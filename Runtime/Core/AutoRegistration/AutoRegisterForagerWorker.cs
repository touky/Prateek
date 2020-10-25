namespace Prateek.Runtime.Core.AutoRegistration
{
    using Prateek.Runtime.Core.AssemblyForager;

    public abstract class AutoRegisterForagerWorker
        : AssemblyForagerWorker
    {
        #region Register/Unregister
        internal void Register(IAutoRegister instance)
        {
            if (!Validate(instance))
            {
                return;
            }

            OnRegister(instance);
        }
        #endregion

        #region Class Methods
        public override void Init()
        {
            AutoRegisterRegistry.Singleton.workers.Add(this);
        }

        internal void Unregister(IAutoRegister instance)
        {
            if (!Validate(instance))
            {
                return;
            }

            OnUnregister(instance);
        }

        protected abstract bool Validate(IAutoRegister instance);
        protected abstract void OnRegister(IAutoRegister instance);
        protected abstract void OnUnregister(IAutoRegister instance);
        #endregion
    }
}
