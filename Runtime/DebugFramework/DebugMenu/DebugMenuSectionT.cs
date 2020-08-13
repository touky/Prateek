namespace Prateek.Runtime.DebugFramework.DebugMenu
{
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.DebugFramework.Reflection;

    public abstract class DebugMenuSection<TOwnerType>
        : DebugMenuSection
        where TOwnerType : class, IDebugMenuOwner
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
