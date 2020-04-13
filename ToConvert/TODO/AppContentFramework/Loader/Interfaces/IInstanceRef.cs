namespace Mayfair.Core.Code.Resources.Loader
{
    public interface IInstanceRef
    {
        #region Class Methods
        void IncrementInstanceRef();
        void DecrementInstanceRef();
        #endregion
    }
}
