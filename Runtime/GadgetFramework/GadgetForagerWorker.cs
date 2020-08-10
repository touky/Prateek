namespace Prateek.Runtime.GadgetFramework
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class GadgetForagerWorker
        : AutoRegisterForagerWorker
    {
        #region Fields
        private List<Type> owners = new List<Type>(50);
        private List<IGadgetInstantiator> instantiators = new List<IGadgetInstantiator>();
        #endregion

        #region Class Methods
        public override void Init()
        {
            base.Init();

            Search<IGadgetOwner>();
            Search<IGadgetInstantiator>();
        }

        public override void WorkDone()
        {
            base.WorkDone();

            var ownerType = typeof(IGadgetOwner);
            foreach (var foundType in FoundTypes)
            {
                if (ownerType.IsAssignableFrom(foundType))
                {
                    owners.Add(ownerType);
                }
                else if (!foundType.IsAbstract && !foundType.IsInterface)
                {
                    instantiators.Add(Activator.CreateInstance(foundType) as IGadgetInstantiator);
                }
            }
        }

        protected override bool Validate(IAutoRegister instance)
        {
            return instance is IGadgetOwner;
        }

        protected override void OnRegister(IAutoRegister instance)
        {
            foreach (var instantiator in instantiators)
            {
                instantiator.Create(instance as IGadgetOwner);
            }
        }

        protected override void OnUnregister(IAutoRegister instance) { }
        #endregion
    }
}
