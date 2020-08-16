namespace Prateek.Runtime.Core.Interfaces.IPriority
{
    using System.Collections.Generic;
    using UnityEngine.Assertions;

    public static class PriorityExtensions
    {
        #region Class Methods
        public static void SortWithPriorities<TPriority>(this List<TPriority> list)
            where TPriority : IPriority
        {
            list.Sort((a, b) => { return a.DefaultPriority - b.DefaultPriority; });
        }

        public static void SortWithPriorities<TPriority>(this List<IPriority<TPriority>> list)
        {
            list.Sort((a, b) => { return a.Priority(a) - b.Priority(b); });
        }
        #endregion
    }
}
