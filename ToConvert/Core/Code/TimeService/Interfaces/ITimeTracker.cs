namespace Mayfair.Core.Code.TimeService.Interfaces
{
    public interface ITimeTracker<out TInterest> where TInterest : ITimeInterest
    {
        TInterest Interest { get; }
    }
}