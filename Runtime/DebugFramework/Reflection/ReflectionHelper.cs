namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Extensions;

    public static class ReflectionHelper
    {
        #region Static and Constants
        private const BindingFlags BINDING_FLAGS =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.FlattenHierarchy;
        #endregion

        #region Class Methods
        public static bool FindProperties<TType>(Type owner, List<PropertyInfo> foundProperties, BindingFlags bindingFlags = BINDING_FLAGS, HashSet<Type> highestParents = null, bool searchParent = false)
        {
            return FindProperties(typeof(TType), owner, foundProperties, bindingFlags, highestParents, searchParent);
        }

        public static bool FindProperties(Type searchType, Type owner, List<PropertyInfo> foundProperties, BindingFlags bindingFlags = BINDING_FLAGS, HashSet<Type> highestParents = null, bool searchParent = false)
        {
            var parentType = owner;
            while (parentType != null)
            {
                var properties = parentType.GetProperties(bindingFlags);
                foreach (var propertyInfo in properties)
                {
                    if (bindingFlags.HasFlag(BindingFlags.SetProperty) && propertyInfo.SetMethod == null)
                    {
                        continue;
                    }

                    if (propertyInfo.GetMethod == null)
                    {
                        continue;
                    }

                    if (propertyInfo.GetMethod.ReturnType.IsSubclassOf(searchType)
                     || searchType.IsAssignableFrom(propertyInfo.GetMethod.ReturnType))
                    {
                        foundProperties.AddUnique(propertyInfo);
                    }
                }

                parentType = parentType.BaseType;

                if (highestParents.Contains(parentType))
                {
                    break;
                }
            }

            return foundProperties.Count > 0;
        }
        
        public static bool FindProperties(string name, Type owner, List<PropertyInfo> foundProperties, BindingFlags bindingFlags = BINDING_FLAGS, HashSet<Type> highestParents = null, bool searchParent = false)
        {
            var parentType = owner;
            while (parentType != null)
            {
                var properties = parentType.GetProperties(bindingFlags);
                foreach (var propertyInfo in properties)
                {
                    if (bindingFlags.HasFlag(BindingFlags.SetProperty) && propertyInfo.SetMethod == null)
                    {
                        continue;
                    }

                    if (propertyInfo.GetMethod == null)
                    {
                        continue;
                    }

                    if (propertyInfo.Name == name)
                    {
                        foundProperties.AddUnique(propertyInfo);
                    }
                }

                parentType = parentType.BaseType;

                if (highestParents.Contains(parentType))
                {
                    break;
                }
            }

            return foundProperties.Count > 0;
        }
        public static FieldInfo[] GetAllFieldInfo<TFieldType>(Type classType, bool searchParent = false)
        {
            var searchType = typeof(TFieldType);
            if (searchParent)
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
        
        public static PropertyInfo SearchPropertyInfo(Type containerType, string fieldName, bool searchParent = false)
        {
            var result = (PropertyInfo) null;
            do
            {
                result = containerType.GetProperty(fieldName, BINDING_FLAGS);
                if (result != null && !searchParent)
                {
                    break;
                }

                containerType = containerType.BaseType;
            } while (result == null && containerType != null);

            return result;
        }

        public static FieldInfo SearchFieldInfo(Type containerType, string fieldName, bool searchParent = false)
        {
            var result = (FieldInfo) null;
            do
            {
                result = containerType.GetField(fieldName, BINDING_FLAGS);

                if (result != null && !searchParent)
                {
                    break;
                }

                containerType = containerType.BaseType;
            } while (result == null && containerType != null);

            return result;
        }

        public static object Invoke<TOwner>(this MethodInfo methodInfo, TOwner methodOwner, object[] parameters)
        {
            if (methodInfo == null)
            {
                UnityEngine.Debug.LogError($"methodInfo is null in {nameof(ReflectionHelper)}.Invoke()");
                return null;
            }

            return methodInfo.Invoke(methodOwner, parameters);
        }

        public static void AssertStackTrace<TOwnerClass>(string ownerMethod)
        {
            AssertStackTrace((TraceInfo<TOwnerClass>) ownerMethod);
        }

        public static void AssertStackTrace(params ITraceInfo[] expectedStack)
        {
            //benjaminh: limiting to the editor until I have time to double check on device
            var stackIndex = expectedStack.Length - 1;
            var trace      = new StackTrace();
            for (var f = 0; f < trace.FrameCount; f++)
            {
                var frame     = trace.GetFrame(f);
                var method    = frame.GetMethod();
                var ownerType = method.ReflectedType;
                if (expectedStack[stackIndex].Validate(ownerType, method))
                {
                    stackIndex--;
                    if (stackIndex < 0)
                    {
                        return;
                    }
                }
            }

            var currentFrame     = trace.GetFrame(Const.SECOND_ITEM);
            var currentMethod    = currentFrame.GetMethod();
            var currentOwnerType = currentMethod.ReflectedType;

            throw new Exception($"{currentOwnerType.Name}{currentMethod.Name}() is only allowed to be called by {expectedStack[stackIndex].Info.Key}.{expectedStack[stackIndex].Info.Value}");
        }
        #endregion
    }
}
