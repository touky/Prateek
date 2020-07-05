namespace Mayfair.Core.Code.UpdateService
{
    public enum UpdateFrequency
    {
        /// <summary>
        /// This should never be used to register for updates
        /// </summary>
        Never = 0,

        EveryFrame = 1,
        EverySecondFrame = 2,
        EveryFifthFrame = 5,
        EveryFifteenthFrame = 15,
        EveryThirtiethFrame = 30,

        /// <summary>
        /// This should never be used to register for updates
        /// </summary>
        Max = EveryThirtiethFrame
    }
}