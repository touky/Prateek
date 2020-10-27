namespace Prateek.Runtime.Core.Extensions
{
    using System.Collections.Generic;

    ///-------------------------------------------------------------------------
    public static class DictionaryExtensions
    {
        ///-------------------------------------------------------------------------
        public static void Set<T0, T1>(this Dictionary<T0, T1> dictionary, T0 key, T1 value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
            else
            {
                dictionary[key] = value;
            }
        }
    }
}
