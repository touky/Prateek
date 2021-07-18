namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    public interface IInstantiatorBinder
    {
        void BindGadgetTo<TOwner>()
            where TOwner : GadgetTools.IOwner;
        
        void InjectGadgetTo<TGadget>()
            where TGadget : class, GadgetTools.IGadget;

        void InjectGadgetTo(string propertyName);

        void AddGadgetAs<TGadget>()
            where TGadget : class, GadgetTools.IGadget;

        void AddGadgetAs<TGadget1, TGadget2>()
            where TGadget1 : class, GadgetTools.IGadget
            where TGadget2 : class, GadgetTools.IGadget;
    }
}