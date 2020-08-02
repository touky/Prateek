namespace Prateek.Runtime.KeynameFramework
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.KeynameFramework.Enums;
    using Prateek.Runtime.KeynameFramework.Interfaces;

    public class AssemblyLookupWorker
    {
        private List<Type> searchedTypes = new List<Type>();
        private List<Type> foundTypes = new List<Type>();

        public List<Type> FoundTypes
        {
            get { return foundTypes; }
        }

        public AssemblyLookupWorker(params Type[] types)
        {
            AssemblyForager.workers.Add(this);

            searchedTypes.AddRange(types);
        }

        internal void TryStore(Type assemblyType)
        {
            foreach (var searchedType in searchedTypes)
            {
                if (assemblyType == searchedType
                    || assemblyType.IsSubclassOf(searchedType)
                    || searchedType.IsAssignableFrom(searchedType))
                {
                    foundTypes.Add(assemblyType);
                    break;
                }
            }
        }
    }

    public static class AssemblyForager
    {
        internal static List<AssemblyLookupWorker> workers = new List<AssemblyLookupWorker>();

        public static void Execute()
        {
            foreach (Assembly domainAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = domainAssembly.GetTypes();
                foreach (Type assemblyType in assemblyTypes)
                {
                    foreach (var worker in workers)
                    {
                        worker.TryStore(assemblyType);
                    }
                }
            }

            workers.Clear();
        }
    }

    internal static class KeywordRegistry
    {
        #region Static and Constants
        private static Dictionary<string, Type> stringToKeywords = new Dictionary<string, Type>();
        private static Dictionary<Type, Type> keywordToParent = new Dictionary<Type, Type>();
        #endregion

        internal static Type MasterKeyword
        {
            get { return typeof(MasterKeyword); }
        }

        #region Class Methods
        private static void Init()
        {
            if (stringToKeywords.Count != 0)
            {
                return;
            }

            //todo move that elsewhere
            var worker = new AssemblyLookupWorker(MasterKeyword);
            AssemblyForager.Execute();

            foreach (Type type in worker.FoundTypes)
            {
                if (typeof(IReplaceWithParentKeyword).IsAssignableFrom(type))
                {
                    var baseType = type;
                    while (typeof(IReplaceWithParentKeyword).IsAssignableFrom(baseType))
                    {
                        baseType = baseType.BaseType;
                    }

                    Register(type.Name, baseType);
                    keywordToParent.Add(type, baseType);
                }
                else
                {
                    Register(type);
                }
            }
        }


        private static void Register(string name, Type type)
        {
#if DEBUG_DEV
            if (tagStorage.ContainsKey(name))
            {
                throw new Exception($"ERROR: Tag {name} with {type.Name} already exists");
            }
#endif

            stringToKeywords.Add(name, type);
        }

        private static void Register(Type type)
        {
#if DEBUG_DEV
            if (tagStorage.ContainsKey(type.Name))
            {
                throw new Exception($"ERROR: Tag {type.Name} already exists");
            }
#endif

            stringToKeywords.Add(type.Name, type);
        }
        
        internal static Type GetKeywordType(Type type)
        {
            if (keywordToParent.ContainsKey(type))
            {
                return keywordToParent[type];
            }

            return type;
        }

        public static Type GetKeywordType(string source)
        {
            Init();

            Type result = null;
            if (stringToKeywords.TryGetValue(source, out result))
            {
                return result;
            }

            return null;
        }

        internal static Keyname Convert(string source, KeynameSettingsData settings = null)
        {
            Init();

            settings = settings == null ? KeynameSettings.Instance.Data : settings;

            var keyname = new Keyname(false);
            keyname.settings = settings;
            if (string.IsNullOrEmpty(source))
            {   
                return keyname;
            }

            var keywordMatch = settings.keywordRegex.Match(source);
            if (!keywordMatch.Success)
            {
                return keyname;
            }

            var firstLoop = true;
            var keyword = new Keyword();
            var numberMatch = settings.numberRegex.Match(source);
            while (true)
            {
                var keywordIndex = keywordMatch.Success ? keywordMatch.LastGroup().Index : int.MaxValue;
                var numberIndex  = numberMatch.Success ? numberMatch.LastGroup().Index : int.MaxValue;

                if (numberIndex < keywordIndex)
                {
                    if (firstLoop)
                    {
                        throw new ArithmeticException($"Keyname cannot start with a numerical value");
                    }

                    keyname.Add(new Keyword(keyword, int.Parse(numberMatch.LastGroup().Value)));
                    keyword = new Keyword();
                    numberMatch = numberMatch.NextMatch();
                }
                else
                {
                    Type keywordType = GetKeywordType(keywordMatch.LastGroup().Value);
                    if (keywordType != null)
                    {
                        if (keyword.Status != KeywordStatus.None)
                        {
                            keyname.Add(keyword);
                        }

                        keyword = keywordType;
                    }
                    else
                    {
                        if (keyword.Status == KeywordStatus.Name)
                        {
                            keyword = keyword.Name + keywordMatch.LastGroup().Value;
                        }
                        else
                        {
                            if (keyword.Status == KeywordStatus.Type)
                            {
                                keyname.Add(keyword);
                            }

                            keyword = keywordMatch.LastGroup().Value;
                        }
                    }

                    keywordMatch = keywordMatch.NextMatch();
                }

                firstLoop = false;

                if (!keywordMatch.Success && !numberMatch.Success)
                {
                    break;
                }
            }

            if (keyword.Status != KeywordStatus.None)
            {
                keyname.Add(keyword);
            }

            return keyname;
        }
        #endregion
    }
}
