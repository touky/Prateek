namespace Prateek.Runtime.AppContentFramework.Local
{
    using Prateek.Runtime.DaemonFramework.Servants;

    public sealed class LocalRegistryBoostrap
#if UNITY_STANDALONE || UNITY_EDITOR
        : ServantBootstrap<LocalRegistryServant>
#endif
    {
    }
}
