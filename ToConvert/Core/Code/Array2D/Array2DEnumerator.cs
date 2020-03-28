namespace Mayfair.Core.Code.Utils.Types
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal struct Array2DEnumerator<T> : IEnumerator, IEnumerator<T>
    {
        #region Fields
        private Vector2Int cursor;
        private Vector2Int size;
        private Vector2Int offset;

        public Array2D<T> array;
        #endregion

        #region Constructors
        public Array2DEnumerator(Array2D<T> array)
        {
            this.cursor = new Vector2Int(-1, 0);
            this.size = new Vector2Int(0, 0);
            this.offset = new Vector2Int(0, 0);

            this.array = array;
            this.size = array.Size;
        }

        public Array2DEnumerator(Array2D<T> array, Vector2Int startOffset, Vector2Int size)
        {
            this.cursor = new Vector2Int(-1, 0);

            this.array = array;
            this.size = array.Size;
            this.size = size;
            this.offset = startOffset;
        }
        #endregion

        #region IEnumerator Members
        public bool MoveNext()
        {
            this.cursor.x++;
            if (this.cursor.x >= this.size.x)
            {
                this.cursor.x = Consts.FIRST_ITEM;
                this.cursor.y++;
            }

            return this.cursor.y < this.size.y;
        }

        public void Reset()
        {
            this.cursor = new Vector2Int(-1, 0);
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
                    return this.array[this.offset + this.cursor];
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
