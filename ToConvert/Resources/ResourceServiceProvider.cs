namespace Assets.Prateek.ToConvert.Resources
{
    using Assets.Prateek.ToConvert.Resources.Enums;
    using Assets.Prateek.ToConvert.Service;

    public abstract class ResourceServiceProvider : ServiceProviderBehaviour
    {
        #region Class Methods
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<ResourceService, ResourceServiceProvider>(this);
        }

        public abstract void ExecuteState(ResourceService service, ServiceState state);
        #endregion
    }
}
