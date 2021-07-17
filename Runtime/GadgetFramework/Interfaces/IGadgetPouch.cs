namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    public interface IGadgetPouch
    {
        TGadget Get<TGadget>()
            where TGadget : class, GadgetTools.IGadget;
    }
}