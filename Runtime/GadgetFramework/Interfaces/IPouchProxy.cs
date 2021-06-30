namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    internal interface IPouchProxy
    {
        void AddToPouch<TGadget>(TGadget gadget, IGadgetPouch pouch)
            where TGadget : class, IGadget;
    }
}