namespace Prateek.Runtime.KeynameFramework
{
    using Prateek.Runtime.Core.AssemblyForager;

    public class KeywordForagerWorker : AssemblyForagerWorker
    {
        #region Class Methods
        public override void PrepareSearch()
        {
            Search(KeywordRegistry.MasterKeyword);

            KeywordRegistry.Singleton.foragerWorker = this;
        }

        public override void WorkDone()
        {
            base.WorkDone();

            KeywordRegistry.Singleton.BuildRegistry();
        }
        #endregion
    }
}
