namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using Prateek.Runtime.Core.Consts;

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

                if (result != null && !exploreInheritance)
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
