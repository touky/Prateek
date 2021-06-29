namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public class DebugMenuDocumentInstantiator
        //: IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        public int DefaultPriority
        {
            get { return typeof(DebugMenuDocumentInstantiator).GetHashCode(); }
        }

        public void Create(IGadgetOwner owner)
        {
            if (!(owner is IDebugMenuDocumentOwner documentOwner))
            {
                return;
            }

            var document = new DebugMenuDocument(documentOwner);
            owner.GadgetPouch.Add(document);
            documentOwner.SetupDebugDocument(document, out var title);
            document.Register(title);
        }
        #endregion
    }
}
