namespace Prateek.Runtime.AppContentFramework.Daemons.Debug
{
    using Prateek.Runtime.DebugFramework.DebugMenu;

    public abstract class RegistryServantSection<TServantType>
        : DebugMenuSection<TServantType>
        where TServantType : ContentRegistryServant
    {
        #region Fields
        private TServantType owner;
        #endregion

        #region Constructors
        protected RegistryServantSection(TServantType owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context) { }
        #endregion
    }
}
