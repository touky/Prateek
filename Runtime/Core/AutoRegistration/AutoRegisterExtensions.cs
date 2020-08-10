namespace Prateek.Runtime.Core.AutoRegistration
{
    public static class AutoRegisterExtensions
    {
        #region Class Methods
        public static void AutoRegister(this IAutoRegister instance)
        {
            AutoRegisterRegistry.Register(instance);
        }

        public static void AutoUnregister(this IAutoRegister instance)
        {
            AutoRegisterRegistry.Unregister(instance);
        }
        #endregion
    }
}
