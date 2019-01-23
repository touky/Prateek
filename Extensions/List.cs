//
//  Prateek, a library that is "bien pratique"
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUG
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Extensions
{
    //-------------------------------------------------------------------------
    public static class ListExt
    {
        //---------------------------------------------------------------------
        public static T Last<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                return list[list.Count - 1];
            }
            return default(T);
        }

        //---------------------------------------------------------------------
        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }

        //---------------------------------------------------------------------
        public static T Pop<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                var value = list.Last();
                list.RemoveLast();
                return value;
            }
            return default(T);
        }

        //---------------------------------------------------------------------
        public static T PopFirst<T>(this List<T> list)
        {
            if (list.Count > 0)
            {
                var value = list[0];
                list.RemoveAt(0);
                return value;
            }
            return default(T);
        }

        //---------------------------------------------------------------------
        public static T GetOrInsert<T>(this List<T> list, int index) where T : new()
        {
            if (index >= list.Count)
            {
                for (int i = list.Count; i < index + 1; ++i)
                {
                    list.Add(new T());
                }
            }
            return list[index];
        }

        //---------------------------------------------------------------------
        public static int BinarySearchBy<TSource, TKey>(this IList<TSource> list, System.Func<TSource, TKey> projection, TKey key)
        {
            int start = 0;
            int end = list.Count - 1;

            while (start <= end)
            {
                int mid = (start + end) / 2;
                TKey mid_key = projection(list[mid]);
                int diff = Comparer<TKey>.Default.Compare(mid_key, key);
                if (diff == 0)
                {
                    return mid;
                }

                start = mid + (diff < 0 ? 1 : -1);
            }
            return ~start;
        }

        //---------------------------------------------------------------------
        public static int AddSorted<T>(this List<T> list, T item, System.Func<T, int> projection)
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

            int index = list.BinarySearchBy(projection, projection(item));
            if (index < 0)
            {
                index = ~index;
            }

            list.Insert(index, item);
            return index;
        }

        //---------------------------------------------------------------------
        public static int AddSorted<T>(this List<T> list, T item, System.Func<T, float> projection)
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

            int index = list.BinarySearchBy(projection, projection(item));
            if (index < 0)
            {
                index = ~index;
            }

            list.Insert(index, item);
            return index;
        }

        //---------------------------------------------------------------------
        public static V GetOrInsert<K, V>(this Dictionary<K, V> dictionary, K key) where V : new()
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                dictionary[key] = (value = new V());
                return value;
            }
        }

        //---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        public static bool AddRangeUnique<T>(this List<T> list, List<T> other)
        {
            bool one_added = false;
            for (int i = 0; i < other.Count; ++i)
            {
                one_added = (list.AddUnique(other[i]) || one_added);
            }
            return one_added;
        }

        //---------------------------------------------------------------------
        public static bool AddRangeUnique<T>(this List<T> list, T[] other)
        {
            bool one_added = false;
            for (int i = 0; i < other.Length; ++i)
            {
                one_added = (list.AddUnique(other[i]) || one_added);
            }
            return one_added;
        }
        #endregion AddUnique

        //---------------------------------------------------------------------
        public static int Compare<T>(this List<T> list, List<T> other, Comparison<T> comparison)
        {
            var size = list.Count - other.Count;
            if (size != 0)
            {
                return size;
            }

            for (int i = 0; i < list.Count; ++i)
            {
                var result = comparison.Invoke(list[i], other[i]);
                if (result != 0)
                {
                    return result;
                }
            }

            return 0;
        }

        ////---------------------------------------------------------------------
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

        //---------------------------------------------------------------------
        #region Select
        public static T SelectMin<T>(this List<T> list, Func<T, float> selector)
        {
            var min = float.MaxValue;
            T selected_item = default(T);
            for (int i = 0; i < list.Count; ++i)
            {
                var item = list[i];
                var item_value = selector(item);
                if (item_value < min)
                {
                    min = item_value;
                    selected_item = item;
                }
            }
            return selected_item;
        }

        //---------------------------------------------------------------------
        public static T SelectMin<T>(this List<T> list, Func<T, float> selector, Func<T, bool> condition)
        {
            var min = float.MaxValue;
            T selected_item = default(T);
            for (int i = 0; i < list.Count; ++i)
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

        //---------------------------------------------------------------------
        public static T SelectMax<T>(this List<T> list, Func<T, float> selector)
        {
            var max = float.MinValue;
            T selected_item = default(T);
            for (int i = 0; i < list.Count; ++i)
            {
                var item = list[i];
                var item_value = selector(item);
                if (item_value > max)
                {
                    max = item_value;
                    selected_item = item;
                }
            }
            return selected_item;
        }

        //---------------------------------------------------------------------
        public static T SelectMax<T>(this List<T> list, Func<T, float> selector, Func<T, bool> condition)
        {
            var max = float.MinValue;
            T selected_item = default(T);
            for (int i = 0; i < list.Count; ++i)
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
        #endregion Select

        //---------------------------------------------------------------------
        public static bool IsIndexInBounds<T>(this T[] array, int index)
        {
            return index >= 0 && index < array.Length;
        }

        //---------------------------------------------------------------------
        public static bool IsIndexInBounds<T>(this List<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        //---------------------------------------------------------------------
        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }

        //---------------------------------------------------------------------
        public static void Populate<T>(this T[,] arr, T value)
        {
            for (int j = 0; j < arr.GetLength(0); j++)
            {
                for (int i = 0; i < arr.GetLength(1); i++)
                {
                    arr[j, i] = value;
                }
            }
        }
    }
}