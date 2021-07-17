namespace Prateek.Runtime.GadgetFramework.Binding
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    internal class InstantiatorBinder
        : IInstantiatorBinder
    {
        public Type ownerType;
        public Func<GadgetTools.IOwner, bool> ownerValidation;
        public Action<GadgetTools.IGadget, GadgetBinder> gadgetInjection;
        public IPouchProxy pouchProxy;
        public List<Action<GadgetTools.IGadget, GadgetTools.IOwner>> GadgetAddition = new List<Action<GadgetTools.IGadget, GadgetTools.IOwner>>();
        private object[] injectedGadget = new object[1];

        public void BindTo<TOwner>()
            where TOwner : GadgetTools.IOwner
        {
            ownerType = typeof(TOwner);
            ownerValidation = (owner) => { return owner is TOwner; };
        }
            
        public void InjectGadgetTo<TGadget>()
            where TGadget : class, GadgetTools.IGadget
        {
            gadgetInjection = (gadget, binder) =>
            {
                var gadgetType = typeof(TGadget);
                if (binder.cache.ownerSetters.TryGetValue(binder.owner.GetType(), out var setters))
                {
                    foreach (var propertyInfo in setters)
                    {
                        if (propertyInfo.GetMethod.ReturnType == gadgetType)
                        {
                            injectedGadget[0] = gadget;
                            propertyInfo.SetMethod.Invoke(binder.owner, injectedGadget);
                            break;
                        }
                    }
                    return;
                }

                throw new KeyNotFoundException($"Can't inject Gadget of type '{gadgetType.Name}' in owner of type {binder.owner.GetType().Name}.");
            };
        }

        public void AddGadgetAs<TGadget>()
            where TGadget : class, GadgetTools.IGadget
        {
            GadgetAddition.Add((gadget, owner) => { pouchProxy.AddToPouch(gadget as TGadget, owner.GadgetPouch); });
        }

        public void AddGadgetAs<TGadget1, TGadget2>()
            where TGadget1 : class, GadgetTools.IGadget
            where TGadget2 : class, GadgetTools.IGadget
        {
            AddGadgetAs<TGadget1>();
            AddGadgetAs<TGadget2>();
        }
    }
}