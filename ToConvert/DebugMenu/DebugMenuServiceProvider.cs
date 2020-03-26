namespace Assets.Prateek.ToConvert.DebugMenu
{
    using Assets.Prateek.ToConvert.DebugMenu.Content;
    using Assets.Prateek.ToConvert.DebugMenu.Pages;
    using Assets.Prateek.ToConvert.Service;

    public abstract class DebugMenuServiceProvider : ServiceProviderBehaviour
    {
        #region Properties
        public override bool IsProviderValid
        {
            get { return true; }
        }

        public override int Priority
        {
            get { return 0; }
        }
        #endregion

        #region Class Methods
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<DebugMenuService, DebugMenuServiceProvider>(this);
        }

        public abstract void AddDebugContent(DebugMenuNotebook notebook, DebugMenuPage rootPage);
        #endregion
    }
}
