namespace Prateek.Core.Code.CachedList {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Prateek.Core.Code.CachedList.Interfaces;
    using Prateek.Core.Code.Consts;

    ///------------------------------------------------------------------------
    internal struct CachedListEnumerator<T> : IEnumerator, IEnumerator<T>
    {
        #region Fields
        private int cursor;

        public ICachedList<T> list;
        #endregion

        #region Constructors
        public CachedListEnumerator(ICachedList<T> list)
        {
            this.list = list;
            this.cursor = Const.CURSOR_RESET;
        }
        #endregion

        #region IEnumerator Members
        public bool MoveNext()
        {
            this.cursor++;
            return this.cursor < this.list.Count;
        }

        public void Reset()
        {
            this.cursor = Const.CURSOR_RESET;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        #region IEnumerator<T> Members
        public void Dispose() { }

        public T Current
        {
            get
            {
                try
                {
                    return this.list[this.cursor];
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