namespace Assets.Prateek.ToConvert.Service
{
    using System.Collections;
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Service.Interfaces;

    internal struct ProviderEnumerable<TProvider> : IEnumerable<TProvider>
        where TProvider : IServiceProvider
    {
        private IReadOnlyList<TProvider> providerList;
        private bool allowInvalid;

        public ProviderEnumerable(IReadOnlyList<TProvider> providerList)
        {
            this.providerList = providerList;
            allowInvalid = false;
        }

        public ProviderEnumerable(IReadOnlyList<TProvider> providerList, bool allowInvalid)
        {
            this.providerList = providerList;
            this.allowInvalid = allowInvalid;
        }

        public IEnumerator<TProvider> GetEnumerator()
        {
            return new ProviderEnumerator<TProvider>(providerList, allowInvalid);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ProviderEnumerator<TProvider>(providerList, allowInvalid);
        }
    }
}
