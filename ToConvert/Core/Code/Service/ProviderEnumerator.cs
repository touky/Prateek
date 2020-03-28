namespace Mayfair.Core.Code.Service
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;

    internal struct ProviderEnumerator<TProvider> : IEnumerator, IEnumerator<TProvider>
        where TProvider : Interfaces.IServiceProvider
    {
        #region Fields
        private int cursor;

        private IReadOnlyList<TProvider> providerList;
        private bool allowInvalid;
        #endregion

        #region Constructors
        public ProviderEnumerator(IReadOnlyList<TProvider> providerList, bool allowInvalid)
        {
            this.cursor = Consts.BEFORE_ZERO;
            this.providerList = providerList;
            this.allowInvalid = allowInvalid;
        }
        #endregion

        #region IEnumerator Members
        public bool MoveNext()
        {
            while (++this.cursor < this.providerList.Count)
            {
                if (!this.allowInvalid && !Current.IsValid)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public void Reset()
        {
            this.cursor = Consts.BEFORE_ZERO;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        #region IEnumerator<T> Members
        public void Dispose() { }

        public TProvider Current
        {
            get
            {
                try
                {
                    return this.providerList[this.cursor];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
        #endregion
    }
}
