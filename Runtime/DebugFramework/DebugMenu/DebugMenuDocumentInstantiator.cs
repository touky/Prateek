namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class DebugMenuDocumentInstantiator
        : IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        public void Create(IGadgetOwner owner)
        {
            if (!(owner is IDebugMenuDocumentOwner receiverOwner))
            {
                return;
            }

            var document = new DebugMenuDocument(receiverOwner);
            owner.GadgetPouch.Add(document);
            receiverOwner.SetupDebugDocument(document, out var title);
            document.Register(title);
        }
        #endregion
    }
}
