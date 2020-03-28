namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;

    public class LocalTimeDaemonBranch : BaseTimeDaemonBranch
    {
        public override bool IsAlive
        {
            get => true;
        }

        public override int Priority
        {
            get => (int)TimeServiceProviderPriority.Local;
        }

        public override DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}