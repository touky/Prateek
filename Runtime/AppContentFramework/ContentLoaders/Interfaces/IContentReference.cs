namespace Prateek.Runtime.AppContentFramework.ContentLoaders.Interfaces
{
    public interface IContentReference
    {
        #region Class Methods
        void IncrementReferences();
        void DecrementReferences();
        #endregion
    }
}
