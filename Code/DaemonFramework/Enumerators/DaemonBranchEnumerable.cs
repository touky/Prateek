namespace Prateek.DaemonFramework.Code.Enumerators
{
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.DaemonFramework.Code.Interfaces;

    internal struct ServantEnumerable<TServant> : IEnumerable<TServant>
        where TServant : IServant
    {
        #region Fields
        private IReadOnlyList<TServant> providerList;
        private bool allowInvalid;
        #endregion

        #region Constructors
        public ServantEnumerable(IReadOnlyList<TServant> providerList, bool allowInvalid)
        {
            this.providerList = providerList;
            this.allowInvalid = allowInvalid;
        }
        #endregion

        #region IEnumerable<TServant> Members
        public IEnumerator<TServant> GetEnumerator()
        {
            return new ServantEnumerator<TServant>(providerList, allowInvalid);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ServantEnumerator<TServant>(providerList, allowInvalid);
        }
        #endregion
    }
}
