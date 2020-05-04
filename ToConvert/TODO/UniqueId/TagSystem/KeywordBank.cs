namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Code.Utils.Types;

    internal static class KeywordBank
    {
        #region Static and Constants
        private const string UNDERSCORE = "_";
        private static Dictionary<string, Type> stringToKeywords = new Dictionary<string, Type>();
        private static Dictionary<Type, Type> keywordToParent = new Dictionary<Type, Type>();
        private static List<string> matchesCache = new List<string>(10);
        #endregion

        internal static Type RootTagType
        {
            get { return typeof(MasterKeyword); }
        }

        #region Class Methods
        //TODO: benjaminh: This is *HIGHLY* temporary
        private static void Init()
        {
            if (stringToKeywords.Count != 0)
            {
                return;
            }

            Type masterType = typeof(MasterKeyword);
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
                    if (type.GetField(MasterKeyword.TYPE_USE_PARENT_SPOOF, BindingFlags.NonPublic | BindingFlags.Static) != null)
                    {
                        Add(type.Name, type.BaseType);
                        keywordToParent.Add(type, type.BaseType);
                    }
                    else
                    {
                        Add(type);
                    }
                }
            }
        }
        

        internal static bool TagMatches(Type parent, Type child)
        {
            return child == parent || child.IsSubclassOf(parent) || parent.IsAssignableFrom(child);
        }

        private static void Add(string name, Type type)
        {
#if DEBUG_DEV
            if (tagStorage.ContainsKey(name))
            {
                throw new Exception($"ERROR: Tag {name} with {type.Name} already exists");
            }
#endif

            stringToKeywords.Add(name, type);
        }

        private static void Add(Type type)
        {
#if DEBUG_DEV
            if (tagStorage.ContainsKey(type.Name))
            {
                throw new Exception($"ERROR: Tag {type.Name} already exists");
            }
#endif

            stringToKeywords.Add(type.Name, type);
        }
        
        internal static Type Get(Type type)
        {
            if (keywordToParent.ContainsKey(type))
            {
                return keywordToParent[type];
            }

            return type;
        }

        public static Type Get(string source)
        {
            Init();

            Type result = null;
            if (stringToKeywords.TryGetValue(source, out result))
            {
                return result;
            }

            return null;
        }

        public static string ToString(IReadOnlyList<Type> tags, string name = null)
        {
            if (tags.Count == 0)
            {
                return name;
            }

            string result = tags[0].Name;

            for (int i = 1, n = tags.Count; i < n; i++)
            {
                result += $"{UNDERSCORE}{tags[i].Name}";
            }

            if (!string.IsNullOrEmpty(name))
            {
                result += $"{UNDERSCORE}{name}";
            }

            return result;
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
