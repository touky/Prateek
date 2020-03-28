namespace Mayfair.Core.Code.Utils.Extensions
{
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static bool ChangeKey<TKey, TValue>(IDictionary<TKey, TValue> dict, 
            TKey oldKey, TKey newKey)
        {
            TValue value;
            if (!dict.TryGetValue(oldKey, out value))
                return false;

            dict.Remove(oldKey); 
            dict[newKey] = value; 
            return true;
        }
    }
}
