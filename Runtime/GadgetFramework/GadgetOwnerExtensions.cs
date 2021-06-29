namespace Prateek.Runtime.GadgetFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using UnityEngine.Assertions;

    public static class GadgetOwnerExtensions
    {
        #region Class Methods
        public static TGadget Get<TGadget>(this IGadgetOwner owner)
            where TGadget : class, IGadget
        {
            Assert.IsNotNull(owner.GadgetPouch);

            //if (owner.GadgetPouch.Gadgets.TryGetValue(typeof(TGadget), out var gadget))
            //{
            //    return gadget as TGadget;
            //}

            throw new KeyNotFoundException($"Gadget of type {typeof(TGadget).Name} does not exist in the {owner.Name}'s pouch.\nCheck if the AutoRegister has been called properly, or check its gadget instantiator.");
        }
        #endregion
    }

    public static class GadgetInstantiatorExtensions
    {
        #region Class Methods
        public static void Bind<TGadget>(this IGadgetInstantiator instantiator, IGadgetOwner gadgetOwner, TGadget gadget)
            where TGadget : class, IGadget
        {
        }
        #endregion
    }
}
