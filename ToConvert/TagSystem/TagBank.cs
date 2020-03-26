namespace Assets.Prateek.ToConvert.TagSystem
{
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.CachedArray;

    internal static class TagBank
    {
        #region Static and Constants
        private const string UNDERSCORE = "_";
        private static Dictionary<string, Type> tagStorage = new Dictionary<string, Type>();
        private static List<string> matchesCache = new List<string>(10);
        #endregion

        #region Class Methods
        //TODO: benjaminh: This is *HIGHLY* temporary
        private static void Init()
        {
            if (tagStorage.Count != 0)
            {
                return;
            }

            var masterType = typeof(MasterTag);
            foreach (var domain_assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var types = domain_assembly.GetTypes();
                foreach (var type in types)
                {
                    if (!type.IsSubclassOf(masterType))
                    {
                        continue;
                    }

                    Add(type);
                }
            }
        }

        private static void Add<T>() where T : MasterTag
        {
            Add(typeof(T));
        }

        private static void Add(Type type)
        {
#if NVIZZIO_DEV
            if (tagStorage.ContainsKey(type.Name))
            {
                throw new Exception($"ERROR: Tag {type.Name} already exists");
            }
#endif

            tagStorage.Add(type.Name, type);
        }

        public static Type Get(string source)
        {
            Init();

            Type result = null;
            if (tagStorage.TryGetValue(source, out result))
            {
                return result;
            }

            return null;
        }

        public static string ToName<T>(string name = null)
            where T : MasterTag
        {
            return string.IsNullOrEmpty(name) ? typeof(T).Name : $"{typeof(T).Name}_{name}";
        }

        public static string ToName<T0, T1>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
        {
            return $"{ToName<T0>()}_{typeof(T1).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
        {
            return $"{ToName<T0, T1>()}_{typeof(T2).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2, T3>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
        {
            return $"{ToName<T0, T1, T2>()}_{typeof(T3).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2, T3, T4>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
        {
            return $"{ToName<T0, T1, T2, T3>()}_{typeof(T4).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2, T3, T4, T5>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
        {
            return $"{ToName<T0, T1, T2, T3, T4>()}_{typeof(T5).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2, T3, T4, T5, T6>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
        {
            return $"{ToName<T0, T1, T2, T3, T4, T5>()}_{typeof(T6).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2, T3, T4, T5, T6, T7>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
        {
            return $"{ToName<T0, T1, T2, T3, T4, T5, T6>()}_{typeof(T7).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
            where T8 : MasterTag
        {
            return $"{ToName<T0, T1, T2, T3, T4, T5, T6, T7>()}_{typeof(T8).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
            where T8 : MasterTag
            where T9 : MasterTag
        {
            return $"{ToName<T0, T1, T2, T3, T4, T5, T6, T7, T8>()}_{typeof(T9).Name}" + (string.IsNullOrEmpty(name) ? string.Empty : $"_{name}");
        }

        public static string ToName(IReadOnlyList<Type> tags)
        {
            if (tags.Count == 0)
            {
                return string.Empty;
            }

            var name = tags[0].Name;

            for (int i = 1, n = tags.Count; i < n; i++)
            {
                name += UNDERSCORE + tags[i].Name;
            }

            return name;
        }

        public static string Convert(string source, out StaticArray10<Type> tags)
        {
            var name = string.Empty;
            tags = new StaticArray10<Type>();
            if (string.IsNullOrEmpty(source))
            {
                return name;
            }

            Init();

            if (!RegexHelper.TryFetchingMatches(source, RegexHelper.UniqueIdTag, matchesCache))
            {
                return name;
            }

            foreach (var match in matchesCache)
            {
                var tag = Get(match);
                if (tag == null)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        name += Consts.UNDERSCORE_SINGLE;
                    }

                    name += match;
                    continue;
                }

                tags.Add(tag);
            }

            matchesCache.Clear();

            return name;
        }
        #endregion
    }
}
