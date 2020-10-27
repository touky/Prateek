namespace Prateek.Runtime.AppContentFramework.Local.ContentFormats
{
    using Prateek.Runtime.Core.AssemblyForager;

    internal class ContentFormatForagerWorker : AssemblyForagerWorker
    {
        #region Static and Constants
        private static ContentFormatForagerWorker instance;
        #endregion

        #region Properties
        public static ContentFormatForagerWorker Instance { get { return instance; } }

        public override bool IgnoreAbstract { get { return true; } }
        #endregion

        #region Class Methods
        public override void Init()
        {
            instance = this;

            Search<ContentFormat>();
        }
        #endregion
    }
}
