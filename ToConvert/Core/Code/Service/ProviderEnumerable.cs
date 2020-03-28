namespace Mayfair.Core.Code.Service
{
    using System.Collections;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Service.Interfaces;

    internal struct ProviderEnumerable<TProvider> : IEnumerable<TProvider>
        where TProvider : IServiceProvider
    {
        private IReadOnlyList<TProvider> providerList;
        private bool allowInvalid;

        public ProviderEnumerable(IReadOnlyList<TProvider> providerList)
        {
            this.providerList = providerList;
            this.allowInvalid = false;
        }

        public ProviderEnumerable(IReadOnlyList<TProvider> providerList, bool allowInvalid)
        {
            this.providerList = providerList;
            this.allowInvalid = allowInvalid;
        }

        public IEnumerator<TProvider> GetEnumerator()
        {
            return new ProviderEnumerator<TProvider>(this.providerList, this.allowInvalid);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ProviderEnumerator<TProvider>(this.providerList, this.allowInvalid);
        }
    }
}
