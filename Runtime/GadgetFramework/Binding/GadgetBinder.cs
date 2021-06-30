namespace Prateek.Runtime.GadgetFramework.Binding
{
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    internal class GadgetBinder
        : IGadgetBinder
    {
        public InstantiatorBinder instantiator;
        public IGadgetOwner owner;
        public SetterCache cache;
        private object[] injectedData = new object[1];

        public void Bind(IGadget gadget)
        {
            injectedData[0] = owner;
            cache.gadgetSetters[gadget.GetType()].SetMethod.Invoke(gadget, injectedData);

            instantiator.gadgetInjection.SafeInvoke(gadget, this);

            for (var ga = 0; ga < instantiator.GadgetAddition.Count; ga++)
            {
                instantiator.GadgetAddition[ga](gadget, owner);
            }

            gadget.Awake();
        }
    }
}