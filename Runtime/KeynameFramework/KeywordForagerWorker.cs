namespace Prateek.Runtime.KeynameFramework
{
    using Prateek.Runtime.Core.AssemblyForager;

    public class KeywordForagerWorker : AssemblyForagerWorker
    {
        #region Class Methods
        public override void Init()
        {
            Search(KeywordRegistry.MasterKeyword);

            KeywordRegistry.foragerWorker = this;
        }

        public override void WorkDone()
        {
            base.WorkDone();

            KeywordRegistry.Init();
        }
        #endregion
    }
}
