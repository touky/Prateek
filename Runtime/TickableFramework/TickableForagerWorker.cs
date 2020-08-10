namespace Prateek.Runtime.TickableFramework
{
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public class TickableForagerWorker
        : AutoRegisterForagerWorker
    {
        #region Class Methods
        protected override bool Validate(IAutoRegister instance)
        {
            return instance is ITickable;
        }

        protected override void OnRegister(IAutoRegister instance)
        {
            TickableRegistry.Register(instance as ITickable);
        }

        protected override void OnUnregister(IAutoRegister instance)
        {
            TickableRegistry.Unregister(instance as ITickable);
        }
        #endregion
    }
}
