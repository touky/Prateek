namespace Assets.Prateek.ToConvert.Resources.Loader
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
