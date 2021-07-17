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
        private GadgetTools.IOwner owner;
        internal Dictionary<Type, GadgetTools.IGadget> gadgets = new Dictionary<Type, GadgetTools.IGadget>();
        #endregion

        #region Constructors
        public GadgetPouch(GadgetTools.IOwner owner)
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
            where TGadget : GadgetTools.IGadget
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
            where TGadget : class, GadgetTools.IGadget
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
