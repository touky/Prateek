namespace Mayfair.Core.Code.Utils.Helpers
{
    using System;
    using Prateek.DaemonCore.Code.Branches;
    using Service;
    using UnityEngine;

    public class DaemonBranchBootstrap<TDaemonBranch> : MonoBehaviour
        where TDaemonBranch : DaemonBranch
    {
        private void Awake()
        {
            DaemonBranch instance = Activator.CreateInstance<TDaemonBranch>();
            instance.Startup();
        }
    }
}