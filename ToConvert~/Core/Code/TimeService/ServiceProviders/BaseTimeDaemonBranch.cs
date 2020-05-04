namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;
    using Prateek.DaemonCore.Code.Branches;
    using Service;

    public abstract class BaseTimeDaemonBranch : DaemonBranch<TimeDaemonCore, BaseTimeDaemonBranch>
    {
        public override string Name { get { return GetType().Name; } }
        public override bool IsAlive { get => true; }

        public abstract DateTime GetCurrentTime();
    }
}