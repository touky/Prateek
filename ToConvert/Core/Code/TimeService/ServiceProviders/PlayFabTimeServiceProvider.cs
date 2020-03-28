namespace Mayfair.Core.Code.TimeService.ServiceProviders
{
    using System;
    using UnityEngine;

    public class PlayFabTimeServiceProvider : BaseTimeServiceProvider
    {
        public override bool IsValid
        {
            get => true;
        }

        public override int Priority
        {
            get => (int)TimeServiceProviderPriority.Server;
        }

        private DateTime serverTime;
        private float timeWhenServerTimeSet;

        public PlayFabTimeServiceProvider(DateTime serverTime)
        {
            this.serverTime = serverTime;
            timeWhenServerTimeSet = Time.time;
        }

        public override DateTime GetCurrentTime()
        {
            // It is safe to assume that we can remain in sync with Server Time by tracking
            // the seconds that have elapsed since we retrieved it.
            return serverTime.AddSeconds(timeWhenServerTimeSet);
        }
    }
}