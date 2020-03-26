namespace Assets.Prateek.ToConvert.Helpers
{
    using System;
    using Assets.Prateek.ToConvert.Service;
    using UnityEngine;

    public class ServiceProviderBootstrap<T> : MonoBehaviour where T : ServiceProvider
    {
        #region Unity Methods
        private void Awake()
        {
            ServiceProvider instance = Activator.CreateInstance<T>();
            instance.Startup();
        }
        #endregion
    }
}
