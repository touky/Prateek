namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;

    public class LocalTimeServant : BaseTimeServant
    {
        public override string Name
        {
            get => GetType().Name;
        }

        public override bool IsAlive
        {
            get => true;
        }

        public override int DefaultPriority
        {
            get => (int)TimeServiceProviderPriority.Local;
        }

        public override DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}