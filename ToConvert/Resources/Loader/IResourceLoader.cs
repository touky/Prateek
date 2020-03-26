namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using Assets.Prateek.ToConvert.Resources.Enums;

    public interface IResourceLoader
    {
        #region Properties
        string Location { get; }
        bool IsDone { get; }
        AsyncStatus Status { get; }
        float PercentComplete { get; }
        #endregion
    }
}
