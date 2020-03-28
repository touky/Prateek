namespace Mayfair.Core.Code.TimeService.Interfaces
{
    using System;

    public interface ITimeDependent
    {
        void UpdateTime(DateTime currentTime);
    }
}