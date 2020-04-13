namespace Mayfair.Core.Code.Resources.Loader
{
    using System;

    public interface IContentHandle
    {
        #region Properties
        IContentLoader Loader { get; }
        Action<IContentHandle> LoadCompleted { set; }
        #endregion

        #region Class Methods
        void LoadAsync();
        #endregion
    }
}
