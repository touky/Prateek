namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;
    using Service;

    public abstract class BaseTimeServiceProvider : ServiceProvider
    {
        public abstract DateTime GetCurrentTime();

        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<TimeService, BaseTimeServiceProvider>(this);
        }
    }
}