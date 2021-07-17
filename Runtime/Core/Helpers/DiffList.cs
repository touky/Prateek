namespace Prateek.Runtime.Core.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.Runtime.Core.Extensions;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct DiffList<T>
    {
        #region Fields
        //Those are auto-created when needed
        private List<T> active;
        private List<T> added;
        private List<T> removed;
        #endregion

        #region Properties
        public IReadOnlyList<T> Active { get { return active.SafeReadOnly(); } }

        public IReadOnlyList<T> Added { get { return added.SafeReadOnly(); } }

        public IReadOnlyList<T> Removed { get { return removed.SafeReadOnly(); } }

        private string DebuggerDisplay { get { return $"<{typeof(T).Name}>, active = {active.SafeCount()}, added = {added.SafeCount()}, removed = {removed.SafeCount()}"; } }
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
