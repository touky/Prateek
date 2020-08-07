namespace Prateek.Runtime.TickableFramework
{
    using Prateek.Runtime.TickableFramework.Interfaces;

    public static class TickableExtensions
    {
        #region Class Methods
        public static void RegisterTickable(this ITickable tickable)
        {
            TickableRegistry.Register(tickable);
        }

        public static void UnregisterTickable(this ITickable tickable)
        {
            TickableRegistry.Unregister(tickable);
        }
        #endregion
    }
}
