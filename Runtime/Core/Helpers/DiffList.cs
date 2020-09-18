namespace Prateek.Runtime.Core
{
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Extensions;

    public struct DiffList<T>
    {
        #region Fields
        private List<T> removed;
        private List<T> active;
        private List<T> added;
        #endregion

        #region Properties
        public IReadOnlyList<T> Removed
        {
            get { return removed; }
        }

        public IReadOnlyList<T> Active
        {
            get { return active; }
        }

        public IReadOnlyList<T> Added
        {
            get { return added; }
        }
        #endregion

        #region Class Methods
        public void Add(T item)
        {
            if (!item.ContainedIn(active))
            {
                item.AddTo(ref added);
                item.AddTo(ref active);
            }

            item.RemoveFrom(removed);
        }

        public void Remove(T item)
        {
            if (item.ContainedIn(active))
            {
                item.AddTo(ref removed);
                item.RemoveFrom(active);
            }

            item.RemoveFrom(added);
        }

        public void FlushDiff()
        {
            added.SafeClear();
            removed.SafeClear();
        }

        public void Clear()
        {
            added.SafeClear();
            removed.SafeClear();
            active.SafeAddRange(ref removed);
            active.SafeClear();
        }
        #endregion
    }
}
