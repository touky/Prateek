namespace Prateek.DaemonCore.Code.Enumerators
{
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.DaemonCore.Code.Interfaces;

    internal struct DaemonBranchEnumerable<TDaemonBranch> : IEnumerable<TDaemonBranch>
        where TDaemonBranch : IDaemonBranch
    {
        #region Fields
        private IReadOnlyList<TDaemonBranch> providerList;
        private bool allowInvalid;
        #endregion

        #region Constructors
        public DaemonBranchEnumerable(IReadOnlyList<TDaemonBranch> providerList, bool allowInvalid)
        {
            this.providerList = providerList;
            this.allowInvalid = allowInvalid;
        }
        #endregion

        #region IEnumerable<TDaemonBranch> Members
        public IEnumerator<TDaemonBranch> GetEnumerator()
        {
            return new DaemonBranchEnumerator<TDaemonBranch>(providerList, allowInvalid);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new DaemonBranchEnumerator<TDaemonBranch>(providerList, allowInvalid);
        }
        #endregion
    }
}
