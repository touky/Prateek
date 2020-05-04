namespace Mayfair.Core.Code.Utils.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class ListExtensions
    {
        #region Class Methods
        /// <summary>
        /// This shuffle uses Knuth shuffle algorithm (https://www.rosettacode.org/wiki/Knuth_shuffle)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this List<T> list)
        {
            for (int iO = 0; iO < list.Count; iO++)
            {
                int iR = Random.Range(iO, list.Count - 1);
                T src = list[iO];
                list[iO] = list[iR];
                list[iR] = src;
            }
        }

        public static void Resize<T>(this List<T> list, int newCount)
        {
            list.Resize(newCount, default);
        }

        public static void Resize<T>(this List<T> list, int newCount, T defaultValue)
        {
            if (list.Count < newCount)
            {
                if (list.Count > list.Capacity)
                {
                    list.Capacity = list.Count;
                }

                while (list.Count < newCount)
                {
                    list.Add(defaultValue);
                }
            }
            else if (list.Count > newCount)
            {
                list.RemoveRange(newCount, list.Count - newCount);
            }
        }

        public static void Resize(this IList list, int newCount)
        {
            Resize<object>(list, newCount, null);
        }

        public static void Resize<T>(this IList list, int newCount, T defaultValue)
        {
            if (list.Count < newCount)
            {
                while (list.Count < newCount)
                {
                    list.Add(defaultValue);
                }
            }
            else
            {
                while (list.Count > newCount)
                {
                    list.RemoveAt(newCount);
                }
            }
        }

        public static void AddUnique<T>(this IList list, T value)
        {
            if (list.Contains(value))
            {
                return;
            }

            list.Add(value);
        }

        public static T Last<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            return list[list.Count - 1];
        }
        #endregion
    }
}
