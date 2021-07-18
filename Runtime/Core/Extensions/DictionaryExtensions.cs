namespace Prateek.Runtime.Core.Extensions
{
    using System.Collections.Generic;

    ///-------------------------------------------------------------------------
    public static class DictionaryExtensions
    {
        ///-------------------------------------------------------------------------
        public static void SafeAdd<T0, T1>(this Dictionary<T0, T1> dictionary, T0 key, T1 value)
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

        ///-------------------------------------------------------------------------
        public static void SafeAdd<T0, T1>(this Dictionary<T0, List<T1>> dictionary, T0 key, T1 value)
        {
            if (!dictionary.TryGetValue(key, out var list))
            {
                list = new List<T1>();
                dictionary.Add(key, list);
            }

            list.Add(value);
        }

        ///-------------------------------------------------------------------------
        public static T1 SafeGet<T0, T1>(this Dictionary<T0, T1> dictionary, T0 key)
            where T1 : class, new()
        {
            if (!dictionary.TryGetValue(key, out var list))
            {
                list = new T1();
                dictionary.Add(key, list);
            }

            return list;
        }
    }
}
