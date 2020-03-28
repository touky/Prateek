namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Code.Utils.Types;

    internal static class TagBank
    {
        #region Static and Constants
        private const string UNDERSCORE = "_";
        private static Dictionary<string, Type> tagStorage = new Dictionary<string, Type>();
        private static List<string> matchesCache = new List<string>(10);
        #endregion

        internal static Type RootTagType
        {
            get { return typeof(MasterTag); }
        }

        #region Class Methods
        //TODO: benjaminh: This is *HIGHLY* temporary
        private static void Init()
        {
            if (tagStorage.Count != 0)
            {
                return;
            }

            Type masterType = typeof(MasterTag);
            foreach (Assembly domain_assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types = domain_assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (!type.IsSubclassOf(masterType))
                    {
                        continue;
                    }

                    //todo Temporary solution for type spoofing, need to change when doing the 2nd pass
                    if (type.GetField(MasterTag.TYPE_USE_PARENT_SPOOF, BindingFlags.NonPublic | BindingFlags.Static) != null)
                    {
                        Add(type.Name, type.BaseType);
                    }
                    else
                    {
                        Add(type);
                    }
                }
            }
        }
        

        internal static bool TagMatches(Type child, Type parent)
        {
            return parent == child || parent.IsSubclassOf(child);
        }

        private static void Add<T>() where T : MasterTag
        {
            Add(typeof(T));
        }
        
        private static void Add(string name, Type type)
        {
#if NVIZZIO_DEV
            if (tagStorage.ContainsKey(name))
            {
                throw new Exception($"ERROR: Tag {name} with {type.Name} already exists");
            }
#endif

            tagStorage.Add(name, type);
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

        public static string ToString(IReadOnlyList<Type> tags, string name = null)
        {
            if (tags.Count == 0)
            {
                return string.Empty;
            }

            string result = tags[0].Name;

            for (int i = 1, n = tags.Count; i < n; i++)
            {
                result += UNDERSCORE + tags[i].Name;
            }

            return string.IsNullOrEmpty(name) ? result : $"{result}_{name}";
        }

        public static string Convert(string source, out StaticArray10<Type> tags)
        {
            string name = string.Empty;
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

            foreach (string match in matchesCache)
            {
                Type tag = Get(match);
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
