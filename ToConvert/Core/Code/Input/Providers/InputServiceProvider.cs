namespace Mayfair.Core.Code.Input.Providers
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Service;
    using UnityEngine;

    /// <summary>
    /// Abstract root for the service provider
    /// TODO: Fill the Touch provider with logic
    /// TODO: Make service providers for Mouse (Mouse-to-touch feature should a provider
    /// </summary>
    public abstract class InputServiceProvider : ServiceProviderBehaviour
    {
        public abstract void GatherInput(List<Touch> touches);
    }
}
