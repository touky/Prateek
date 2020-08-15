namespace Prateek.Runtime.Core.Interfaces.IPriority
{
    public interface IPriority<TType>
    {
        #region Class Methods
        int Priority(IPriority<TType> type);
        #endregion
    }
}
