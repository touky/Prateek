namespace Prateek.Runtime.DaemonFramework.Enumerators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.DaemonFramework.Interfaces;

    public struct ServantEnumerator<TServant>
        : IEnumerator
        , IEnumerator<TServant>
        , IEnumerable<TServant>
        where TServant : IServant
    {
        #region Fields
        private int cursor;

        private IReadOnlyList<TServant> servants;
        private bool allowInvalid;
        #endregion

        #region Constructors
        internal ServantEnumerator(IReadOnlyList<TServant> servants, bool allowInvalid)
        {
            cursor = Const.CURSOR_RESET;
            this.servants = servants;
            this.allowInvalid = allowInvalid;
        }
        #endregion

        #region IEnumerator Members
        public bool MoveNext()
        {
            while (++cursor < servants.Count)
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

        public IEnumerator<TServant> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public TServant Current
        {
            get
            {
                try
                {
                    return servants[cursor];
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
