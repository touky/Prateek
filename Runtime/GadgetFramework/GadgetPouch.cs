namespace Prateek.Runtime.GadgetFramework
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public interface IGadgetPouch
    {
        void Add<TGadget>(TGadget gadget)
            where TGadget : IGadget;
    }

    internal class GadgetPouch
        : IGadgetPouch
    {
        #region Fields
        internal Dictionary<Type, IGadget> gadgets = new Dictionary<Type, IGadget>();
        #endregion

        #region Constructors
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
        public void Add<TGadget>(TGadget gadget)
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
        #endregion
    }
}
