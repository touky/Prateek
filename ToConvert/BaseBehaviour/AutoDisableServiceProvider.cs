namespace Assets.Prateek.ToConvert.BaseBehaviour
{
    using Assets.Prateek.ToConvert.Service;

    public sealed class AutoDisableServiceProvider : ServiceProviderBehaviour
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
            SendIdentificationFor<AutoDisableService, AutoDisableServiceProvider>(this);
        }
        #endregion
    }
}
