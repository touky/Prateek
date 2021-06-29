namespace Prateek.Runtime.GadgetFramework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using ICSharpCode.NRefactory.Ast;
    using JetBrains.Annotations;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public interface IInstantiatorBinder
    {
        void BindTo<TOwner>()
            where TOwner : IGadgetOwner;
        
        void InjectGadgetTo<TGadget>()
            where TGadget : class, IGadget;

        void AddGadgetAs<TGadget>()
            where TGadget : class, IGadget;

        void AddGadgetAs<TGadget1, TGadget2>()
            where TGadget1 : class, IGadget
            where TGadget2 : class, IGadget;
    }

    public interface IGadgetBinder
    {
        void Bind(IGadget gadget);
    }

    public class GadgetForagerWorker
        : AutoRegisterForagerWorker
    {
        #region Fields
        private Dictionary<Type, PropertyInfo> ownerSetters = new Dictionary<Type, PropertyInfo>(50);
        private Dictionary<Type, PropertyInfo> gadgetSetters = new Dictionary<Type, PropertyInfo>(50);
        private List<IGadgetInstantiator> instantiators = new List<IGadgetInstantiator>();
        private List<InstantiatorBinder> binders = new List<InstantiatorBinder>();
        private GadgetBinder gadgetBinder = new GadgetBinder();
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

            var ownerType = typeof(IGadgetOwner);
            var gadgetType = typeof(IGadget);
            foreach (var foundType in FoundTypes)
            {
                if (ownerType.IsAssignableFrom(foundType))
                {
                    ownerSetters.Add(foundType, null);
                }
                else if (gadgetType.IsAssignableFrom(foundType))
                {
                    var setter = foundType.GetProperty("Owner", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (setter == null)
                    {
                        throw new KeyNotFoundException($"Gadget of type '{foundType}' does not have a private setter for its owner.");
                    }
                    gadgetSetters.Add(foundType, setter);
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
                instantiator.Declare(binder);
                binders.Add(binder);
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

                var instantiator = instantiators[i];

                gadgetBinder.owner = owner;
                gadgetBinder.binder = binder;

                instantiator.Bind(gadgetBinder);
            }
        }

        protected override void OnUnregister(IAutoRegister instance) { }
        #endregion
        
        private class InstantiatorBinder
            : IInstantiatorBinder
        {
            public Type ownerType;
            public Func<IGadgetOwner, bool> ownerValidation;
            public Action<IGadget, IGadgetOwner> gadgetInjection;
            public List<Action<IGadget, IGadgetOwner>> GadgetAddition = new List<Action<IGadget, IGadgetOwner>>();
            private object[] injectedGadget = new object[1];

            public void BindTo<TOwner>()
                where TOwner : IGadgetOwner
            {
                ownerType = typeof(TOwner);
                ownerValidation = (owner) => { return owner is TOwner; };
            }
            
            public void InjectGadgetTo<TGadget>()
                where TGadget : class, IGadget
            {
                var foundProperty = default(PropertyInfo);
                foreach (var propertyInfo in ownerType.GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.NonPublic))
                {
                    if (propertyInfo.SetMethod.ReturnType == typeof(TGadget))
                    {
                        foundProperty = propertyInfo;
                        break;
                    }
                }

                if (foundProperty == null)
                {
                    throw new KeyNotFoundException($"Gadget Owner of type '{ownerType}' does not have a private setter for '{typeof(TGadget)}'.\nFix the owner or do not use InjectGadgetTo()");
                }
                
                gadgetInjection = (gadget, owner) =>
                {
                    injectedGadget[0] = gadget;
                    foundProperty.SetMethod.Invoke(owner, injectedGadget);
                };
            }

            public void AddGadgetAs<TGadget>()
                where TGadget : class, IGadget
            {
                GadgetAddition.Add((gadget, owner) => { owner.GadgetPouch.Add(gadget as TGadget); });
            }

            public void AddGadgetAs<TGadget1, TGadget2>()
                where TGadget1 : class, IGadget
                where TGadget2 : class, IGadget
            {
                AddGadgetAs<TGadget1>();
                AddGadgetAs<TGadget2>();
            }
        }

        private class GadgetBinder
            : IGadgetBinder
        {
            public InstantiatorBinder binder;
            public IGadgetOwner owner;
            public Dictionary<Type, PropertyInfo> gadgetSetters;
            private object[] injectedOwner = new object[1];

            public void Bind(IGadget gadget)
            {
                injectedOwner[0] = owner;
                gadgetSetters[gadget.GetType()].SetMethod.Invoke(gadget, injectedOwner);

                binder.gadgetInjection.SafeInvoke(gadget, owner);

                for (var ga = 0; ga < binder.GadgetAddition.Count; ga++)
                {
                    binder.GadgetAddition[ga](gadget, owner);
                }

                gadget.Awake();
            }
        }
    }
}
