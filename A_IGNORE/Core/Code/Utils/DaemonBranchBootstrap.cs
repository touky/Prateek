namespace Mayfair.Core.Code.Utils.Helpers
{
    using System;
    using Prateek.DaemonFramework.Code.Interfaces;
    using Service;
    using UnityEngine;

    public class ServantBootstrap<TServant> : MonoBehaviour
        where TServant : class, IServant
    {
        private void Awake()
        {
            TServant instance = Activator.CreateInstance<TServant>();
            instance.Startup();
        }
    }
}