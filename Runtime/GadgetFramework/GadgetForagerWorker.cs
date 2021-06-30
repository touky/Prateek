namespace Prateek.Runtime.GadgetFramework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DebugFramework.Reflection;
    using Prateek.Runtime.GadgetFramework.Binding;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using UnityEngine;
    using UnityEngine.Assertions;

    public class GadgetForagerWorker
        : AutoRegisterForagerWorker
        , IPouchProxy
    {
        private const string FIELD_OWNER = "Owner";
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public;
        private static readonly HashSet<Type> highestParents = new HashSet<Type>()
        {
            typeof(MonoBehaviour), typeof(Component), typeof(UnityEngine.Object)
        };

        #region Fields
        private SetterCache cache = new SetterCache();
        private List<IGadgetInstantiator> instantiators = new List<IGadgetInstantiator>();
        private List<InstantiatorBinder> binders = new List<InstantiatorBinder>();
        private GadgetBinder gadgetBinder = new GadgetBinder();
        private object[] injectedData = new object[1];
        #endregion

        #region Class Methods
        public override void PrepareSearch()
        {
            base.PrepareSearch();

            Search<IGadget>(SearchFlag.Interface);
            Search<IGadgetOwner>();
            Search<IGadgetInstantiator>();
        }

        public override void WorkDone()
        {
            base.WorkDone();

            var pouchSetters = new List<PropertyInfo>();
            var ownerType = typeof(IGadgetOwner);
            var pouchType = typeof(IGadgetPouch);
            var gadgetType = typeof(IGadget);
            foreach (var foundType in FoundTypes)
            {
                if (!foundType.IsInterface && ownerType.IsAssignableFrom(foundType))
                {
                    cache.pouchSetters.Add(foundType, null);

                    var ownerSetters = new List<PropertyInfo>();
                    if (ReflectionHelper.FindProperties<IGadget>(foundType, ownerSetters, BINDING_FLAGS, highestParents, true))
                    {
                        cache.ownerSetters.Add(foundType, ownerSetters);
                    }

                    pouchSetters.Clear();
                    if (ReflectionHelper.FindProperties<IGadgetPouch>(foundType, pouchSetters, BINDING_FLAGS, highestParents, true))
                    {
                        cache.pouchSetters[foundType] = pouchSetters[0];
                    }

                    Assert.IsNotNull(cache.pouchSetters[foundType], $"Gadget Owner of type {foundType.Name} does not implement a setter for {pouchType.Name}");
                }
                else if (gadgetType.IsAssignableFrom(foundType))
                {
                    var setter = foundType.GetProperty(FIELD_OWNER, BINDING_FLAGS);

                    Assert.IsNotNull(setter, $"Gadget of type '{foundType}' does not have a private setter for its owner.");

                    cache.gadgetSetters.Add(foundType, setter);
                }
                else if (!foundType.IsAbstract && !foundType.IsInterface)
                {
                    instantiators.Add(Activator.CreateInstance(foundType) as IGadgetInstantiator);
                }
            }

            instantiators.SortWithPriorities();

            foreach (var instantiator in instantiators)
            {
                var binder = new InstantiatorBinder();
                binder.pouchProxy = this;
                binders.Add(binder);

                instantiator.Declare(binder);
            }
        }

        protected override bool Validate(IAutoRegister instance)
        {
            return instance is IGadgetOwner;
        }

        protected override void OnRegister(IAutoRegister instance)
        {
            if (!(instance is IGadgetOwner owner))
            {
                return;
            }

            for (int i = 0; i < instantiators.Count; i++)
            {
                var binder = binders[i];
                if (!binder.ownerValidation(owner))
                {
                    continue;
                }

                if (owner.GadgetPouch == null)
                {
                    var pouch = new GadgetPouch(owner);

                    injectedData[0] = pouch;
                    cache.pouchSetters[owner.GetType()].SetMethod.Invoke(owner, injectedData);

                }

                var instantiator = instantiators[i];

                gadgetBinder.owner = owner;
                gadgetBinder.instantiator = binder;
                gadgetBinder.cache = cache;

                instantiator.Bind(gadgetBinder);
            }
        }

        protected override void OnUnregister(IAutoRegister instance) { }

        public void AddToPouch<TGadget>(TGadget gadget, IGadgetPouch pouch)
            where TGadget : class, IGadget
        {
            Assert.IsNotNull(pouch as GadgetPouch);
            (pouch as GadgetPouch).Add(gadget);
        }
        #endregion
    }
}
