namespace Mayfair.Core.Code.Utils
{
    using System;
    using System.Collections.Generic;

    public static class EnumHelper
    {
        #region Class Methods
        public static void CacheEnum<T>(Dictionary<T, string> dictionary, string format = null) where T : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(T));
            for (int e = 0; e < values.Length; e++)
            {
                T value = (T) values.GetValue(e);
                dictionary[value] = format == null ? value.ToString() : string.Format(format, value);
            }
        }
        #endregion
    }
}
