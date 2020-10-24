namespace Prateek.Runtime.AppContentFramework.Unity.Addressables
{
    using Prateek.Runtime.DaemonFramework.Servants;

    public sealed class LocalRegistryBoostrap
#if UNITY_STANDALONE || UNITY_EDITOR
        : ServantBootstrap<LocalRegistryServant>
#endif
    {
    }
}
