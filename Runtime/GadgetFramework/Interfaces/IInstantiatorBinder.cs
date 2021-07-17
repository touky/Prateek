namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    public interface IInstantiatorBinder
    {
        void BindTo<TOwner>()
            where TOwner : GadgetTools.IOwner;
        
        void InjectGadgetTo<TGadget>()
            where TGadget : class, GadgetTools.IGadget;

        void AddGadgetAs<TGadget>()
            where TGadget : class, GadgetTools.IGadget;

        void AddGadgetAs<TGadget1, TGadget2>()
            where TGadget1 : class, GadgetTools.IGadget
            where TGadget2 : class, GadgetTools.IGadget;
    }
}