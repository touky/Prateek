namespace Prateek.Runtime.DebugFramework.DebugMenu.Gadgets
{
    using JetBrains.Annotations;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public abstract class DebugMenu
        : GadgetTools
    {
        [UsedImplicitly]
        internal class Instantiator
            : IInstantiator
        {
            #region IGadgetInstantiator Members
            public int DefaultPriority
            {
                get { return typeof(Instantiator).GetHashCode(); }
            }
                        
            public void Declare(IInstantiatorBinder binder)
            {
                binder.BindTo<IDocumentOwner>();
                //todo binder.InjectGadgetTo<DebugMenuDocument>();
                binder.AddGadgetAs<DebugMenuDocument>();
            }

            public void Bind(IGadgetBinder binder)
            {
                binder.Bind(new DebugMenuDocument());
            }
            #endregion
        }

        public interface IDebugMenuOwner { }

        public interface IDocumentOwner
            : IOwner
                , IDebugMenuOwner
        {
            #region Class Methods
            void SetupDebugDocument(DebugMenuDocument document, out string title);
            #endregion
        }

        public interface IDocumentServant
            : IDebugMenuOwner
        {
            #region Class Methods
            void SetupDebugDocument();
            void SetupDebugDocument(DebugMenuDocument document);
            #endregion
        }
    }
}