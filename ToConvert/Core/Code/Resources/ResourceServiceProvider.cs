namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Resources.Enums;

    public abstract class ResourceServiceProvider : Service.ServiceProviderBehaviour
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
