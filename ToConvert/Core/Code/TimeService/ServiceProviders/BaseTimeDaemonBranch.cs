namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;
    using Prateek.DaemonCore.Code.Branches;
    using Service;

    public abstract class BaseTimeDaemonBranch : DaemonBranch<TimeDaemonCore, BaseTimeDaemonBranch>
    {
        public abstract DateTime GetCurrentTime();
    }
}