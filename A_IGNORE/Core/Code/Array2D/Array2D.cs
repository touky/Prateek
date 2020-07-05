namespace Mayfair.Core.Code.Utils.Types
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public struct Array2D<T> : IEnumerable<T>, IEnumerable, IReadOnlyList<T>
    {
        #region Fields
        private int width;
        private int height;
        private T[] datas;
        #endregion

        #region Properties
        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        public Vector2Int Size
        {
            get { return new Vector2Int(width, height); }
        }

        public Array2DKeys Keys
        {
            get { return new Array2DKeys(Size); }
        }

        public T this[int x, int y]
        {
            get
            {
                Debug.Assert(datas != null);
                return this[CoordToIndex(x, y)];
            }
            set
            {
                Debug.Assert(datas != null);
                this[CoordToIndex(x, y)] = value;
            }
        }

        public T this[Vector2Int index]
        {
            get
            {
                Debug.Assert(datas != null);
                return datas[CoordToIndex(index)];
            }
            set
            {
                Debug.Assert(datas != null);
                datas[CoordToIndex(index)] = value;
            }
        }
        #endregion

        #region Constructors
        public Array2D(int width, int height) : this(width, height, true) { }

        public Array2D(int width, int height, bool doCreate)
        {
            this.width = width;
            this.height = height;
            datas = null;

            if (doCreate)
            {
                datas = InternalCreate();
            }
        }
        #endregion

        #region Class Methods
        public void Create()
        {
            datas = InternalCreate();
        }

        public void Resize(int width, int height, bool copyData = false)
        {
            Vector2Int oldSize = new Vector2Int(this.width, this.height);
            T[] oldArray = datas;

            this.width = width;
            this.height = height;

            Create();

            if (copyData)
            {
                Vector2Int newSize = new Vector2Int(this.width, this.height);
                Vector2Int copySize = new Vector2Int(Mathf.Min(oldSize.x, newSize.x), Mathf.Min(oldSize.y, newSize.y));
                InternalCopy(copySize, oldArray, Vector2Int.zero, oldSize, datas, Vector2Int.zero, new Vector2Int(this.width, this.height));
            }
        }

        private T[] InternalCreate()
        {
            return new T[width * height];
        }

        private void InternalCopy(Vector2Int copySize, T[] oldArray, Vector2Int oldPosition, Vector2Int oldSize, T[] newArray, Vector2Int newPosition, Vector2Int newSize)
        {
            Debug.Assert(oldArray != null);
            Debug.Assert(newArray != null);
            Debug.Assert(oldPosition.x >= 0 && oldPosition.y >= 0 && oldPosition.x < oldSize.x && oldPosition.y < oldSize.y && oldPosition.x + copySize.x < oldSize.x && oldPosition.y + copySize.y < oldSize.y);
            Debug.Assert(newPosition.x >= 0 && newPosition.y >= 0 && newPosition.x < newSize.x && newPosition.y < newSize.y && newPosition.x + copySize.x < newSize.x && newPosition.y + copySize.y < newSize.y);

            for (int y = 0; y < copySize.y; y++)
            {
                Vector2Int pos = new Vector2Int(0, y);
                Array.ConstrainedCopy(oldArray, Array2DHelpers.CoordToIndex(oldPosition + pos, oldSize),
                                      newArray, Array2DHelpers.CoordToIndex(newPosition + pos, new Vector2Int()), copySize.x);
            }
        }

        public void Clear()
        {
            for (int d = 0; d < datas.Length; d++)
            {
                datas[d] = default;
            }
        }

        public void Dispose()
        {
            width = 0;
            height = 0;
            datas = null;
        }

        public void CopyTo(Vector2Int copySize, Array2D<T> source, Vector2Int sourcePosition, Array2D<T> destination, Vector2Int destinationPosition)
        {
            InternalCopy(copySize, source.datas, sourcePosition, source.Size, destination.datas, destinationPosition, destination.Size);
        }

        public Vector2Int IndexToCoord(int index)
        {
            return Array2DHelpers.IndexToCoord(index, new Vector2Int(width, height));
        }

        public int CoordToIndex(Vector2Int position)
        {
            return Array2DHelpers.CoordToIndex(position, new Vector2Int(width, height));
        }

        public int CoordToIndex(int x, int y)
        {
            return Array2DHelpers.CoordToIndex(x, y, new Vector2Int(width, height));
        }

        public IEnumerator GetEnumerator(Vector2Int startOffset, Vector2Int size)
        {
            return new Array2DEnumerator<T>(this, startOffset, size);
        }
        #endregion

        #region IEnumerable<T> Members
        public IEnumerator<T> GetEnumerator()
        {
            return new Array2DEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Array2DEnumerator<T>(this);
        }
        #endregion

        #region IReadOnlyList<T> Members
        public int Count
        {
            get { return datas.Length; }
        }

        public T this[int index]
        {
            get
            {
                Debug.Assert(datas != null);
                Debug.Assert(index >= 0 && index < Count);

                return datas[index];
            }
            set
            {
                Debug.Assert(datas != null);
                Debug.Assert(index >= 0 && index < Count);

                datas[index] = value;
            }
        }
        #endregion
    }
}
