namespace Mayfair.Core.Code.Utils.Helpers
{
    using System;
    using Service;
    using UnityEngine;

    public class ServiceProviderBootstrap<T> : MonoBehaviour where T : ServiceProvider
    {
        private void Awake()
        {
            ServiceProvider instance = Activator.CreateInstance<T>();
            instance.Startup();
        }
    }
}