namespace Prateek.Runtime.GadgetFramework
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using UnityEngine.Assertions;

    internal class GadgetPouch
        : IGadgetPouch
    {
        #region Fields
        private IGadgetOwner owner;
        internal Dictionary<Type, IGadget> gadgets = new Dictionary<Type, IGadget>();
        #endregion

        #region Constructors
        public GadgetPouch(IGadgetOwner owner)
        {
            this.owner = owner;
        }

        ~GadgetPouch()
        {
            foreach (var gadget in gadgets.Values)
            {
                if (gadget == null)
                {
                    continue;
                }

                gadget.Kill();
            }
        }
        #endregion

        #region Class Methods
        internal void Add<TGadget>(TGadget gadget)
            where TGadget : IGadget
        {
            var key = typeof(TGadget);
            if (gadgets.ContainsKey(key))
            {
                gadgets[key] = gadget;
            }
            else
            {
                gadgets.Add(key, gadget);
            }
        }

        public TGadget Get<TGadget>()
            where TGadget : class, IGadget
        {
            if (gadgets.TryGetValue(typeof(TGadget), out var gadget))
            {
                return gadget as TGadget;
            }

            throw new KeyNotFoundException($"Gadget of type {typeof(TGadget).Name} does not exist in the {owner.Name}'s pouch.\nCheck if the AutoRegister has been called properly, or check its gadget instantiator.");
        }
        #endregion
    }
}
