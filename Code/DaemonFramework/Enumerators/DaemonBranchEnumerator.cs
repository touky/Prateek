namespace Prateek.DaemonFramework.Code.Enumerators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Core.Code.Consts;
    using Prateek.DaemonFramework.Code.Interfaces;

    internal struct ServantEnumerator<TServant> : IEnumerator, IEnumerator<TServant>
        where TServant : IServant
    {
        #region Fields
        private int cursor;

        private IReadOnlyList<TServant> providerList;
        private bool allowInvalid;
        #endregion

        #region Constructors
        public ServantEnumerator(IReadOnlyList<TServant> providerList, bool allowInvalid)
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

        public TServant Current
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
