namespace Prateek.Editor.Settings
{
    using Prateek.Runtime.Core.AssemblyForager;
    using Prateek.Runtime.Core.FrameworkSettings;

    internal class FrameworkSettingsForagerWorker
        : AssemblyForagerWorker<FrameworkSettingsForagerWorker>
    {
        #region Class Methods
        public override void PrepareSearch()
        {
            Search<FrameworkSettings>(SearchFlag.Abstract);
        }
        #endregion
    }
}
