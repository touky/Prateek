namespace Mayfair.Core.Code.Utils.Types
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    internal struct Array2DKeysEnumerator : IEnumerator, IEnumerator<Vector2Int>
    {
        #region Fields
        private Vector2Int cursor;
        private Vector2Int size;
        private Vector2Int offset;
        #endregion

        #region Constructors
        public Array2DKeysEnumerator(Vector2Int size)
        {
            cursor = new Vector2Int(-1, 0);
            this.size = new Vector2Int(0, 0);
            offset = new Vector2Int(0, 0);

            this.size = size;
        }

        public Array2DKeysEnumerator(Vector2Int startOffset, Vector2Int size)
        {
            cursor = new Vector2Int(-1, 0);

            this.size = size;
            offset = startOffset;
        }
        #endregion

        #region IEnumerator Members
        public bool MoveNext()
        {
            cursor.x++;
            if (cursor.x >= size.x)
            {
                cursor.x = Consts.FIRST_ITEM;
                cursor.y++;
            }

            return cursor.y < size.y;
        }

        public void Reset()
        {
            cursor = new Vector2Int(-1, 0);
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        #region IEnumerator<T> Members
        public void Dispose() { }

        public Vector2Int Current
        {
            get
            {
                try
                {
                    return offset + cursor;
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
