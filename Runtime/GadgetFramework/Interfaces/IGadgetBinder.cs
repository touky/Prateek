namespace Prateek.Runtime.GadgetFramework.Interfaces
{
    public interface IGadgetBinder
    {
        GadgetTools.IOwner Owner { get; }
        GadgetTools.IGadget GetInstance(string propertySource);
        void Bind(GadgetTools.IGadget gadget);
    }
}