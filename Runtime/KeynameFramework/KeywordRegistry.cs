namespace Prateek.Runtime.KeynameFramework
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Singleton;
    using Prateek.Runtime.KeynameFramework.Enums;
    using Prateek.Runtime.KeynameFramework.Interfaces;
    using Prateek.Runtime.KeynameFramework.Settings;

    internal class KeywordRegistry
        : Registry<KeywordRegistry>
    {
        #region Fields
        internal KeywordForagerWorker foragerWorker;
        private Dictionary<string, Type> stringToKeywords = new Dictionary<string, Type>();
        private Dictionary<Type, Type> keywordToParent = new Dictionary<Type, Type>();
        #endregion

        #region Properties
        internal static KeywordRegistry Singleton { get { return Instance; } }

        internal static Type MasterKeyword { get { return typeof(MasterKeyword); } }
        #endregion

        #region Register/Unregister
        protected override void OnAwake() { }

        private void Register(string keyName, Type keyType)
        {
#if DEBUG_DEV
            if (stringToKeywords.ContainsKey(keyName))
            {
                throw new Exception($"ERROR: Tag {name} with {keyType.Name} already exists");
            }
#endif

            stringToKeywords.Add(keyName, keyType);
        }

        private void Register(Type keyType)
        {
#if DEBUG_DEV
            if (stringToKeywords.ContainsKey(keyType.Name))
            {
                throw new Exception($"ERROR: Tag {keyType.Name} already exists");
            }
#endif

            stringToKeywords.Add(keyType.Name, keyType);
        }
        #endregion

        #region Class Methods
        internal void BuildRegistry()
        {
            if (stringToKeywords.Count != 0 || foragerWorker == null)
            {
                return;
            }

            foreach (var type in foragerWorker.FoundTypes)
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

        internal Type GetKeywordType(Type type)
        {
            if (keywordToParent.ContainsKey(type))
            {
                return keywordToParent[type];
            }

            return type;
        }

        public Type GetKeywordType(string source)
        {
            Type result = null;
            if (stringToKeywords.TryGetValue(source, out result))
            {
                return result;
            }

            return null;
        }

        internal Keyname Convert(string source, KeynameSettingsData settings = null)
        {
            settings = settings == null ? KeynameSettings.Default.Data : settings;

            var keyname = new Keyname(false);
            keyname.settings = settings;
            if (string.IsNullOrEmpty(source))
            {
                return keyname;
            }

            var keywordMatch = settings.KeywordRegex.Match(source);
            if (!keywordMatch.Success)
            {
                return keyname;
            }

            var firstLoop   = true;
            var keyword     = new Keyword();
            var numberMatch = settings.NumberRegex.Match(source);
            while (true)
            {
                var keywordIndex = keywordMatch.Success ? keywordMatch.Index : int.MaxValue;
                var numberIndex  = numberMatch.Success ? numberMatch.Index : int.MaxValue;

                if (numberIndex < keywordIndex)
                {
                    if (firstLoop)
                    {
                        throw new ArithmeticException("Keyname cannot start with a numerical value");
                    }

                    keyname.Add(new Keyword(keyword, int.Parse(numberMatch.Value)));
                    keyword = new Keyword();
                    numberMatch = numberMatch.NextMatch();
                }
                else
                {
                    var keywordType = GetKeywordType(keywordMatch.Value);
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
                            keyword = keyword.Name + keywordMatch.Value;
                        }
                        else
                        {
                            if (keyword.Status == KeywordStatus.Type)
                            {
                                keyname.Add(keyword);
                            }

                            keyword = keywordMatch.Value;
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
