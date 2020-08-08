namespace Prateek.Runtime.Core.Extensions
{
    using System.Collections.Generic;

    public static class ListItemExtensions
    {
        #region Class Methods
        public static void AddTo<T>(this T item, ref List<T> list)
        {
            if (list == null)
            {
                list = new List<T>();
            }

            list.Add(item);
        }

        public static void RemoveFrom<T>(this T item, List<T> list)
        {
            if (list == null)
            {
                return;
            }

            list.Remove(item);
        }

        public static bool ContainedIn<T>(this T item, List<T> list)
        {
            if (list == null)
            {
                return false;
            }

            return list.Contains(item);
        }
        #endregion
    }
}
