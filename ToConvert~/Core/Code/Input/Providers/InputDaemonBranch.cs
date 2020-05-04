namespace Mayfair.Core.Code.Input.Providers
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonCore.Code.Branches;
    using UnityEngine;

    /// <summary>
    /// Abstract root for the service branch
    /// TODO: Fill the Touch branch with logic
    /// TODO: Make service providers for Mouse (Mouse-to-touch feature should a branch
    /// </summary>
    public abstract class InputDaemonBranch : DaemonBranchBehaviour<InputDaemonCore, InputDaemonBranch>
    {
        public abstract void GatherInput(List<Touch> touches);
    }
}
