namespace Mayfair.Core.Code.Resources.Loader
{
    using System;

    public interface IAbstractResourceReference
    {
        #region Properties
        IResourceLoader Loader { get; }
        Action<IAbstractResourceReference> LoadCompleted { set; }
        #endregion

        #region Class Methods
        void LoadAsync();
        #endregion
    }
}
