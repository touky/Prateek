namespace Prateek.Runtime.AppContentFramework.Loader.Interfaces
{
    using System;

    public interface IContentHandle
        : IContentReference
    {
        #region Properties
        bool HasReferences { get; }
        IContentLoader Loader { get; }
        Action<IContentHandle> LoadCompleted { set; }

        void Load();
        void Unload();
        #endregion
    }
}
