namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;
    using Prateek.DaemonFramework.Code.Servants;
    using Service;

    public abstract class BaseTimeServant : Servant<TimeDaemon, BaseTimeServant>
    {
        public override string Name { get { return GetType().Name; } }
        public override bool IsAlive { get => true; }

        public abstract DateTime GetCurrentTime();
    }
}