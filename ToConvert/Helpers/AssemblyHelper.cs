﻿namespace Assets.Prateek.ToConvert.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class AssemblyHelper
    {
        #region GetTypeFromAnyAssembly
        public static bool GetAllTypeMatchingAnyAssembly(string startWith, string endWith, List<Type> resultTypes)
        {
            FindTypeFromAnyAssembly(false, startWith, endWith, resultTypes);
            return resultTypes.Count > 0;
        }

        public static Type GetTypeMatchingAnyAssembly(string startWith, string endWith)
        {
            return FindTypeFromAnyAssembly(false, startWith, endWith, null);
        }

        public static Type GetTypeFromAnyAssembly(string name)
        {
            return FindTypeFromAnyAssembly(true, name, string.Empty, null);
        }

        private static Type FindTypeFromAnyAssembly(bool testEqual, string startWith, string endWith, List<Type> resultTypes)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Type resultType = null;
                var  types      = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (testEqual)
                    {
                        if (type.Name != startWith)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (startWith != string.Empty && !type.Name.StartsWith(startWith))
                        {
                            continue;
                        }

                        if (endWith != string.Empty && !type.Name.EndsWith(endWith))
                        {
                            continue;
                        }
                    }

                    if (resultTypes == null)
                    {
                        resultType = type;
                        break;
                    }
                    else
                    {
                        resultTypes.Add(type);
                    }
                }

                if (resultTypes == null)
                {
                    if (resultType == null)
                    {
                        continue;
                    }

                    return resultType;
                }
            }

            return null;
        }

        public static List<Assembly> GetAssembliesMatching(string startWith)
        {
            return GetAssembliesMatching(startWith, string.Empty);
        }

        public static List<Assembly> GetAssembliesMatching(string startWith, string endWith)
        {
            var result     = new List<Assembly>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var name = assembly.FullName.Substring(0, assembly.FullName.IndexOf(","));
                if (!name.StartsWith(startWith))
                {
                    continue;
                }

                if (endWith != string.Empty && !name.EndsWith(endWith))
                {
                    continue;
                }

                result.Add(assembly);
            }

            return result;
        }
        #endregion
    }
}
