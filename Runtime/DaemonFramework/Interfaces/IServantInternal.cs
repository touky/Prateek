namespace Prateek.Runtime.DaemonFramework.Interfaces
{
    internal interface IServantInternal
    {
        #region Properties
        IDaemon Overseer { set; }
        string Name { set; }
        #endregion
    }
}
