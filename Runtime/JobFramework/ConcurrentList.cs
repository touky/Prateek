namespace Prateek.Runtime.JobFramework
{
    using System.Collections.Generic;

    public class ConcurrentList<T>
    {
        #region Fields
        private List<T> list = new List<T>();
        private object listLock = new object();
        #endregion

        #region Properties
        public IReadOnlyList<T> Copy
        {
            get
            {
                var result = new List<T>();
                lock (listLock)
                {
                    result.AddRange(list);
                }

                return result;
            }
        }
        #endregion

        #region Class Methods
        public void Add(T item)
        {
            lock (listLock)
            {
                list.Add(item);
            }
        }
        #endregion
    }
}
