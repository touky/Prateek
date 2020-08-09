namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using UnityEngine;

    public static class ReflectionHelper
    {
        #region Static and Constants
        private static readonly BindingFlags BINDING_FLAGS =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.FlattenHierarchy;
        #endregion

        #region Class Methods
        public static FieldInfo[] GetAllFieldInfo<TFieldType>(Type classType, bool allHierarchy = false)
        {
            var searchType = typeof(TFieldType);
            if (allHierarchy)
            {
                var fieldInfos = new List<FieldInfo>();
                var baseType   = classType;
                while (baseType != null)
                {
                    fieldInfos.AddRange(baseType.GetFields(BINDING_FLAGS));

                    baseType = baseType.BaseType;
                }

                for (var f = 0; f < fieldInfos.Count; f++)
                {
                    var fieldType = fieldInfos[f].FieldType;
                    if (fieldType.IsSubclassOf(searchType)
                     || searchType.IsAssignableFrom(fieldType))
                    {
                        continue;
                    }

                    fieldInfos.RemoveAt(f--);
                }

                return fieldInfos.ToArray();
            }

            return classType.GetFields(BINDING_FLAGS);
        }

        public static FieldInfo SearchFieldInfo(Type containerType, string fieldName, bool exploreInheritance = false)
        {
            var result = (FieldInfo) null;
            do
            {
                result = containerType.GetField(fieldName, BINDING_FLAGS);
                containerType = containerType.BaseType;

                if (!exploreInheritance)
                {
                    break;
                }
            } while (result == null && containerType != null);

            return result;
        }

        public static object Invoke<TOwner>(this MethodInfo methodInfo, TOwner methodOwner, object[] parameters)
        {
            if (methodInfo == null)
            {
                Debug.LogError($"methodInfo is null in {nameof(ReflectionHelper)}.Invoke()");
                return null;
            }

            return methodInfo.Invoke(methodOwner, parameters);
        }
        #endregion
    }
}
