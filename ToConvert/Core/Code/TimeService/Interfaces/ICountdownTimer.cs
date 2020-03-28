namespace Mayfair.Core.Code.TimeService.Interfaces
{
    using System;

    public interface ICountdownTimer : ITimeInterest
    {
        bool TimeReached { get; }
        TimeSpan TimeRemaining { get; }

        void SetCountdownDuration(int seconds);
        void PreserveCountdownProgress(DateTime newReferenceTime);
        void Clear();
    }
}