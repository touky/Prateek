namespace Mayfair.Core.Code.Input.Providers
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Service;
    using Prateek.Runtime.DaemonFramework.Servants;
    using UnityEngine;

    /// <summary>
    /// Abstract root for the service servant
    /// TODO: Fill the Touch servant with logic
    /// TODO: Make service providers for Mouse (Mouse-to-touch feature should a servant
    /// </summary>
    public abstract class InputServant : Servant<InputDaemon, InputServant>
    {
        public abstract void GatherInput(List<Touch> touches);
    }
}
