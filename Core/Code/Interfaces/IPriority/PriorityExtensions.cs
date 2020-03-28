namespace Mayfair.Core.Code.Utils.Types.Priority
{
    using System.Collections.Generic;

    public static class PriorityExtensions
    {
        #region Class Methods
        public static void SortWithPriorities<TPriority>(this List<TPriority> list)
            where TPriority : IPriority
        {
            list.Sort((a, b) => { return a.Priority - b.Priority; });
        }
        #endregion
    }
}
