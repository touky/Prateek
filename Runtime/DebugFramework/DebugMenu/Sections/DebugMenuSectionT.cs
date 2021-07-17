namespace Prateek.Runtime.DebugFramework.DebugMenu.Sections
{
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;

    public abstract class DebugMenuSection<TOwnerType>
        : DebugMenuSection
        where TOwnerType : class, DebugMenu.IDebugMenuOwner
    {
        #region Properties
        public TOwnerType Owner
        {
            get { return GetOwner<TOwnerType>(); }
        }
        #endregion

        #region Constructors
        protected DebugMenuSection(string title) : base(title) { }

        protected DebugMenuSection(TOwnerType owner, string title) : base(title)
        {
            SetOwner(owner);
        }
        #endregion
    }
}
