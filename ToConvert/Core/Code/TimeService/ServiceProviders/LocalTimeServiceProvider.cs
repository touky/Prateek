namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;

    public class LocalTimeServiceProvider : BaseTimeServiceProvider
    {
        public override bool IsValid
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