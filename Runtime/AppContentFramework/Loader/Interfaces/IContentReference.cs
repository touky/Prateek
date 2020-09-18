namespace Prateek.Runtime.AppContentFramework.Loader.Interfaces
{
    public interface IContentReference
    {
        #region Class Methods
        void IncrementReferences();
        void DecrementReferences();
        #endregion
    }
}
