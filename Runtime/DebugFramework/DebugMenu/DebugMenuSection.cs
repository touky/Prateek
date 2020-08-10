namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.DebugFramework.Reflection;

    public abstract class DebugMenuSection
        : DebugMenuObject
    {
        #region Fields
        private IDebugMenuDocumentOwner owner;
        #endregion

        #region Constructors
        protected DebugMenuSection(string title) : base(title) { }
        #endregion

        #region Class Methods
        internal void SetOwner(IDebugMenuDocumentOwner owner)
        {
            this.owner = owner;
            DebugField.SetOwnerToAllDebugFields(this, owner);
        }

        protected TOwner GetOwner<TOwner>()
            where TOwner : class, IDebugMenuDocumentOwner
        {
            return owner as TOwner;
        }
        #endregion
    }
}
