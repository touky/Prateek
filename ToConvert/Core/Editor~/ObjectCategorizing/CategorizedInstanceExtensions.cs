namespace Mayfair.Core.Editor.ObjectCategorizing
{
    using System.Collections.Generic;

    public static class CategorizedInstanceExtensions
    {
        #region Class Methods
        public static void ClearBuiltDatas(this List<CategorizedInstance> list)
        {
            foreach (CategorizedInstance instance in list)
            {
                instance.ClearBuiltDatas();
            }
        }
        #endregion
    }
}
