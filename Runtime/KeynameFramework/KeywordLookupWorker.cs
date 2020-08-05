namespace Prateek.Runtime.KeynameFramework
{
    using Prateek.Runtime.Core.AssemblyForager;

    public class KeywordLookupWorker : AssemblyLookupWorker
    {
        #region Class Methods
        public override void Init()
        {
            Search(KeywordRegistry.MasterKeyword);

            KeywordRegistry.lookupWorker = this;
        }

        public override void WorkDone()
        {
            base.WorkDone();

            KeywordRegistry.Init();
        }
        #endregion
    }
}
