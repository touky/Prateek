namespace Prateek.Runtime.GadgetFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using UnityEngine.Assertions;

    public static class GadgetOwnerExtensions
    {
        #region Class Methods
        public static TGadget Get<TGadget>(this GadgetTools.IOwner owner)
            where TGadget : class, GadgetTools.IGadget
        {
            Assert.IsNotNull(owner);
            Assert.IsNotNull(owner.GadgetPouch);

            return owner.GadgetPouch.Get<TGadget>();
        }
        #endregion
    }
}
