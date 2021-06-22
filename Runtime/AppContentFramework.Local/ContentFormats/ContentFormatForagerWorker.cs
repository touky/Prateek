namespace Prateek.Runtime.AppContentFramework.Local.ContentFormats
{
    using System.Reflection;
    using Prateek.Runtime.Core.AssemblyForager;

    internal class ContentFormatForagerWorker
        : AssemblyForagerWorker<ContentFormatForagerWorker>
    {
        #region Class Methods
        public override void PrepareSearch()
        {
            Search<ContentFormat>(SearchFlag.Abstract);
        }
        #endregion
    }
}
