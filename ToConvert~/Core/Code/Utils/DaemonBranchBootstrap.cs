namespace Mayfair.Core.Code.Utils.Helpers
{
    using System;
    using Prateek.DaemonCore.Code.Branches;
    using Prateek.DaemonCore.Code.Interfaces;
    using Service;
    using UnityEngine;

    public class DaemonBranchBootstrap<TDaemonBranch> : MonoBehaviour
        where TDaemonBranch : class, IDaemonBranch
    {
        private void Awake()
        {
            TDaemonBranch instance = Activator.CreateInstance<TDaemonBranch>();
            instance.Startup();
        }
    }
}