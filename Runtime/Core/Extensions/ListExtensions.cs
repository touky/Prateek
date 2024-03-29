// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------

#region Prateek Ifdefs
//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG
#endregion Prateek Ifdefs

// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.Runtime.Core.Extensions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static class ListExtensions
    {
        #region Static and Constants
        private static Dictionary<Type, IList> emptyLists = new Dictionary<Type, IList>();
        #endregion

        #region Class Methods
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void DomainReload()
        {
            emptyLists = new Dictionary<Type, IList>();
        }

        ///---------------------------------------------------------------------
        public static IReadOnlyList<T> SafeReadOnly<T>(this List<T> list)
        {
            if (list != null)
            {
                return list;
            }

            var type = typeof(T);
            if (!emptyLists.TryGetValue(type, out var empty))
            {
                empty = new List<T>();
                emptyLists.Add(type, empty);
            }

            return empty as IReadOnlyList<T>;
        }
        
        ///---------------------------------------------------------------------
        public static int SafeCount(this IList list)
        {
            if (list == null)
            {
                return 0;
            }

            return list.Count;
        }

        ///---------------------------------------------------------------------
        public static void SafeClear<T>(this List<T> list)
        {
            if (list == null)
            {
                return;
            }

            list.Clear();
        }

        ///---------------------------------------------------------------------
        public static void SafeAddRange<T>(this List<T> list, ref List<T> other)
        {
            if (list == null)
            {
                return;
            }

            if (other == null)
            {
                other = new List<T>();
            }

            other.AddRange(list);
        }

        ///---------------------------------------------------------------------
        public static T Last<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return list[list.Count - 1];
            }

            return default;
        }

        ///---------------------------------------------------------------------
        public static void Last<T>(this List<T> list, T value)
        {
            if (list.Count > 0)
            {
                list[list.Count - 1] = value;
            }
        }

        ///---------------------------------------------------------------------
        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        ///---------------------------------------------------------------------
        public static T Pop<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                var value = list.Last();
                list.RemoveLast();
                return value;
            }

            return default;
        }

        ///---------------------------------------------------------------------
        public static T PopFirst<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                var value = list[0];
                list.RemoveAt(0);
                return value;
            }

            return default;
        }

        ///---------------------------------------------------------------------
        public static T First<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return list[0];
            }

            return default;
        }

        ///---------------------------------------------------------------------
        public static T GetOrInsert<T>(this List<T> list, int index)
            where T : new()
        {
            if (index >= list.Count)
            {
                for (var i = list.Count; i < index + 1; ++i)
                {
                    list.Add(new T());
                }
            }

            return list[index];
        }

        ///---------------------------------------------------------------------
        public static int BinarySearchBy<TSource, TKey>(this IList<TSource> list, Func<TSource, TKey> projection, TKey key)
        {
            var start = 0;
            var end   = list.Count - 1;

            while (start <= end)
            {
                var mid     = (start + end) / 2;
                var mid_key = projection(list[mid]);
                var diff    = Comparer<TKey>.Default.Compare(mid_key, key);
                if (diff == 0)
                {
                    return mid;
                }

                start = mid + (diff < 0 ? 1 : -1);
            }

            return ~start;
        }

        ///---------------------------------------------------------------------
        public static int AddSorted<T>(this List<T> list, T item, Func<T, int> projection)
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return 0;
            }

            if (projection(list[list.Count - 1]) - projection(item) <= 0)
            {
                list.Add(item);
                return list.Count - 1;
            }

            if (projection(list[0]) - projection(item) >= 0)
            {
                list.Insert(0, item);
                return 0;
            }

            var index = list.BinarySearchBy(projection, projection(item));
            if (index < 0)
            {
                index = ~index;
            }

            list.Insert(index, item);
            return index;
        }

        ///---------------------------------------------------------------------
        public static int AddSorted<T>(this List<T> list, T item, Func<T, float> projection)
        {
            if (list.Count == 0)
            {
                list.Add(item);
                return 0;
            }

            if (projection(list[list.Count - 1]) - projection(item) <= 0)
            {
                list.Add(item);
                return list.Count - 1;
            }

            if (projection(list[0]) - projection(item) >= 0)
            {
                list.Insert(0, item);
                return 0;
            }

            var index = list.BinarySearchBy(projection, projection(item));
            if (index < 0)
            {
                index = ~index;
            }

            list.Insert(index, item);
            return index;
        }

        ///---------------------------------------------------------------------
        public static V GetOrInsert<K, V>(this Dictionary<K, V> dictionary, K key)
            where V : new()
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                dictionary[key] = value = new V();
                return value;
            }
        }

        ///---------------------------------------------------------------------
        public static int Compare<T>(this List<T> list, List<T> other, Comparison<T> comparison)
        {
            var size = list.Count - other.Count;
            if (size != 0)
            {
                return size;
            }

            for (var i = 0; i < list.Count; ++i)
            {
                var result = comparison.Invoke(list[i], other[i]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        ///---------------------------------------------------------------------
        public static bool IsIndexInBounds<T>(this T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        ///---------------------------------------------------------------------
        public static bool IsIndexInBounds<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        ///---------------------------------------------------------------------
        public static void Populate<T>(this T[] arr, T value)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }

        ///---------------------------------------------------------------------
        public static void Populate<T>(this T[,] arr, T value)
        {
            for (var j = 0; j < arr.GetLength(0); j++)
            {
                for (var i = 0; i < arr.GetLength(1); i++)
                {
                    arr[j, i] = value;
                }
            }
        }
        #endregion

        ///---------------------------------------------------------------------

        #region AddUnique
        public static bool AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
                return true;
            }

            return false;
        }

        ///---------------------------------------------------------------------
        public static bool AddRangeUnique<T>(this List<T> list, List<T> other)
        {
            var one_added = false;
            for (var i = 0; i < other.Count; ++i)
            {
                one_added = list.AddUnique(other[i]) || one_added;
            }

            return one_added;
        }

        ///---------------------------------------------------------------------
        public static bool AddRangeUnique<T>(this List<T> list, T[] other)
        {
            var one_added = false;
            for (var i = 0; i < other.Length; ++i)
            {
                one_added = list.AddUnique(other[i]) || one_added;
            }

            return one_added;
        }
        #endregion AddUnique

        ////-------------------------------------------------------------------
        //public static void Resize<T>(this List<T> list, int new_size, T default_value = default(T))
        //{
        //    int count = list.Count;

        //    if (new_size < count)
        //    {
        //        list.RemoveRange(new_size, count - new_size);
        //    }
        //    else if (new_size > count)
        //    {
        //        if (new_size > list.Capacity)
        //        {
        //            list.Capacity = new_size;
        //        }
        //        list.AddRange(Enumerable.Repeat(default_value, new_size - count));
        //    }
        //}

        ///---------------------------------------------------------------------

        #region Select
        public static T SelectMin<T>(this List<T> list, Func<T, float> selector)
        {
            var min           = float.MaxValue;
            var selected_item = default(T);
            for (var i = 0; i < list.Count; ++i)
            {
                var item       = list[i];
                var item_value = selector(item);
                if (item_value < min)
                {
                    min = item_value;
                    selected_item = item;
                }
            }

            return selected_item;
        }

        ///---------------------------------------------------------------------
        public static T SelectMin<T>(this List<T> list, Func<T, float> selector, Func<T, bool> condition)
        {
            var min           = float.MaxValue;
            var selected_item = default(T);
            for (var i = 0; i < list.Count; ++i)
            {
                var item = list[i];
                if (condition == null || condition(item))
                {
                    var item_value = selector(item);
                    if (item_value < min)
                    {
                        min = item_value;
                        selected_item = item;
                    }
                }
            }

            return selected_item;
        }

        ///---------------------------------------------------------------------
        public static T SelectMax<T>(this List<T> list, Func<T, float> selector)
        {
            var max           = float.MinValue;
            var selected_item = default(T);
            for (var i = 0; i < list.Count; ++i)
            {
                var item       = list[i];
                var item_value = selector(item);
                if (item_value > max)
                {
                    max = item_value;
                    selected_item = item;
                }
            }

            return selected_item;
        }

        ///---------------------------------------------------------------------
        public static T SelectMax<T>(this List<T> list, Func<T, float> selector, Func<T, bool> condition)
        {
            var max           = float.MinValue;
            var selected_item = default(T);
            for (var i = 0; i < list.Count; ++i)
            {
                var item = list[i];
                if (condition == null || condition(item))
                {
                    var item_value = selector(item);
                    if (item_value > max)
                    {
                        max = item_value;
                        selected_item = item;
                    }
                }
            }

            return selected_item;
        }

        ///---------------------------------------------------------------------
        public static bool TryAdd<TList, TItem>(this IList<TList> list, TItem item)
        {
            if (item is TList tItem)
            {
                list.Add(tItem);
                return true;
            }

            return false;
        }
        #endregion Select
    }
}
