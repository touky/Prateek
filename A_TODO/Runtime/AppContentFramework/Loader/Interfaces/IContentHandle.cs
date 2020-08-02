namespace Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces
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
