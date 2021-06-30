namespace Prateek.Runtime.GadgetFramework.Interfaces
{
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
}