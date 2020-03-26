namespace Assets.Prateek.ToConvert.Input.Providers
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Service;
    using UnityEngine;

    /// <summary>
    ///     Abstract root for the service provider
    ///     TODO: Fill the Touch provider with logic
    ///     TODO: Make service providers for Mouse (Mouse-to-touch feature should a provider
    /// </summary>
    public abstract class InputServiceProvider : ServiceProviderBehaviour
    {
        #region Class Methods
        public abstract void GatherInput(List<Touch> touches);
        #endregion
    }
}
