namespace Mayfair.Core.Code.Utils.Types
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public struct Array2DKeys : IEnumerable<Vector2Int>, IEnumerable
    {
        #region Fields
        private Vector2Int size;
        #endregion

        #region Constructors
        public Array2DKeys(Vector2Int size)
        {
            this.size = size;
        }
        #endregion

        #region Class Methods
        public IEnumerator GetEnumerator(Vector2Int startOffset, Vector2Int size)
        {
            return new Array2DKeysEnumerator(startOffset, size);
        }
        #endregion

        #region IEnumerable<Vector2Int> Members
        public IEnumerator<Vector2Int> GetEnumerator()
        {
            return new Array2DKeysEnumerator(size);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Array2DKeysEnumerator(size);
        }
        #endregion
    }
}
