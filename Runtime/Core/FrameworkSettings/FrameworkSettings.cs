namespace Prateek.Runtime.Core.FrameworkSettings
{
    /// <summary>
    ///     Base class for framework settings.
    ///     Use templated version
    /// </summary>
    public abstract class FrameworkSettings
    {
        #region Static and Constants
        public const string DEFAULT_PATH = "Settings/";
        #endregion

        #region Properties
        public abstract bool IsAvailable { get; }
        #endregion

        #region Class Methods
        internal void InternalInit()
        {
            Init();
        }

        protected abstract void Init();
        #endregion
    }
}