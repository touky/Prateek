namespace Prateek.Runtime.GadgetFramework.Binding
{
    using System;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using UnityEditor.VersionControl;
    using UnityEngine.Assertions;

    internal class GadgetBinder
        : IGadgetBinder
    {
        public InstantiatorBinder instantiator;
        public GadgetTools.IOwner owner;
        public SetterCache cache;
        private object[] injectedData = new object[1];

        public GadgetTools.IOwner Owner { get { return owner; } }
        
        public GadgetTools.IGadget GetInstance(string propertySource)
        {
            var gadgetType = typeof(GadgetTools.IGadget);
            if (cache.ownerSetters.TryGetValue(owner.GetType(), out var setters))
            {
                foreach (var propertyInfo in setters)
                {
                    if (propertyInfo.SetMethod != null
                        && propertyInfo.Name == propertySource
                        && gadgetType.IsAssignableFrom(propertyInfo.PropertyType)
                        && !propertyInfo.PropertyType.IsInterface
                        && !propertyInfo.PropertyType.IsAbstract)
                    {
                        return Activator.CreateInstance(propertyInfo.PropertyType) as GadgetTools.IGadget;
                    }
                }
            }

            Assert.IsTrue(false, $"Could not find a property with name '{propertySource}' in type '{owner.GetType().Name}'");
            return null;
        }

        public void Bind(GadgetTools.IGadget gadget)
        {
            injectedData[0] = owner;
            if (!cache.gadgetSetters.TryGetValue(gadget.GetType(), out var propertyInfo) || propertyInfo.SetMethod == null)
            {
                Assert.IsTrue(false, $"Gadget of Type '{gadget.GetType().Name}' does contains a proper setter for its owner");
            }

            propertyInfo.SetMethod.Invoke(gadget, injectedData);

            instantiator.gadgetInjection.SafeInvoke(gadget, this);

            for (var ga = 0; ga < instantiator.GadgetAddition.Count; ga++)
            {
                instantiator.GadgetAddition[ga](gadget, owner);
            }

            gadget.Awake();
        }
    }
}