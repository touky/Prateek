namespace Prateek.DaemonFramework.Code.Enumerators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.DaemonFramework.Code.Interfaces;

    internal struct DaemonBranchEnumerator<TDaemonBranch> : IEnumerator, IEnumerator<TDaemonBranch>
        where TDaemonBranch : IDaemonBranch
    {
        #region Fields
        private int cursor;

        private IReadOnlyList<TDaemonBranch> providerList;
        private bool allowInvalid;
        #endregion

        #region Constructors
        public DaemonBranchEnumerator(IReadOnlyList<TDaemonBranch> providerList, bool allowInvalid)
        {
            cursor = Const.CURSOR_RESET;
            this.providerList = providerList;
            this.allowInvalid = allowInvalid;
        }
        #endregion

        #region IEnumerator Members
        public bool MoveNext()
        {
            while (++cursor < providerList.Count)
            {
                if (!allowInvalid && !Current.IsAlive)
                {
                    continue;
                }

                return true;
            }

            return false;
        }

        public void Reset()
        {
            cursor = Const.CURSOR_RESET;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        #region IEnumerator<T> Members
        public void Dispose() { }

        public TDaemonBranch Current
        {
            get
            {
                try
                {
                    return providerList[cursor];
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
