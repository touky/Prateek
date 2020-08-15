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

        public static void SortWithPriorities<TPriority, TPriorityType>(this List<TPriority> list)
            where TPriority : IPriority
            where TPriorityType : class
        {
            list.Sort((a, b) =>
            {
                var aT = a as IPriority<TPriorityType>;
                var bT = b as IPriority<TPriorityType>;
                Assert.IsNotNull(aT);
                Assert.IsNotNull(bT);
                return aT.Priority(aT) - bT.Priority(bT);
            });
        }

        public static void SortWithPriorities<TType>(this List<IPriority<TType>> list)
        {
            list.Sort((a, b) => { return a.Priority(a) - b.Priority(b); });
        }
        #endregion
    }
}
