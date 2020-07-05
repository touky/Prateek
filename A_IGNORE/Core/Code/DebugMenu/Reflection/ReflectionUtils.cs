namespace Mayfair.Core.Code.Utils.Debug.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;

    public static class ReflectionUtils
    {
        #region Static and Constants
        private static readonly BindingFlags DEFAULT_BINDING_FLAGS =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.Static |
            BindingFlags.FlattenHierarchy;
        #endregion

        #region Class Methods
        public static void InitAllReflectedFields(object fieldContainer, object instance)
        {
            //Doing this to allow renaming of the interface method but keep the correct one as the first
            MethodInfo[] interfaceMethods = typeof(IReflectedField).GetMethods();
            Debug.Assert(interfaceMethods.Length >= 1);
            ParameterInfo[] interfaceArguments = interfaceMethods[0].GetParameters();
            Debug.Assert(interfaceArguments.Length == 1);
            Debug.Assert(interfaceArguments[0].ParameterType == typeof(object));

            object[] parameters = new object[1];
            FieldInfo[] fieldInfos = GetMembers(fieldContainer.GetType(), true);
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                if (!typeof(IReflectedField).IsAssignableFrom(fieldInfo.FieldType))
                {
                    continue;
                }

                parameters[0] = instance;

                CallMethodInfo(interfaceMethods[0].Name, interfaceMethods[0], fieldInfo.GetValue(fieldContainer), parameters);
            }
        }

        private static MethodInfo FindMethodInTheHierarchy(string methodName, Type type, BindingFlags bindingFlags)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, bindingFlags);
            if (methodInfo == null && bindingFlags.HasFlag(BindingFlags.NonPublic))
            {
                Type parent = type;
                while (parent != null)
                {
                    methodInfo = parent.GetMethod(methodName, bindingFlags);
                    if (methodInfo != null)
                    {
                        break;
                    }

                    parent = parent.BaseType;
                }
            }

            return methodInfo;
        }

        public static object CallMethod<TOwner>(string methodName, TOwner instance, params object[] parameters)
        {
            Type type = instance.GetType();
            MethodInfo methodInfo = FindMethodInTheHierarchy(methodName, type, DEFAULT_BINDING_FLAGS);
            return CallMethodInfo(methodName, methodInfo, instance, parameters);
        }

        public static object CallMethod<TOwner>(string methodName, TOwner instance, BindingFlags bindingFlags, params object[] parameters)
        {
            Type type = instance.GetType();
            MethodInfo methodInfo = FindMethodInTheHierarchy(methodName, type, bindingFlags);
            return CallMethodInfo(methodName, methodInfo, instance, parameters);
        }

        public static object CallMethod(string methodName, Type type, params object[] parameters)
        {
            MethodInfo methodInfo = FindMethodInTheHierarchy(methodName, type, DEFAULT_BINDING_FLAGS);
            return CallMethodInfo(methodName, methodInfo, parameters);
        }

        public static FieldInfo[] GetMembers<TOwner>()
        {
            Type type = typeof(TOwner);
            return type.GetFields(DEFAULT_BINDING_FLAGS);
        }

        public static FieldInfo[] GetMembers(Type classType, bool allHierarchy = false)
        {
            if (allHierarchy)
            {
                List<FieldInfo> fieldInfos = new List<FieldInfo>();
                Type parent = classType;
                while (parent != null)
                {
                    fieldInfos.AddRange(parent.GetFields(DEFAULT_BINDING_FLAGS));

                    parent = parent.BaseType;
                }

                return fieldInfos.ToArray();
            }

            return classType.GetFields(DEFAULT_BINDING_FLAGS);
        }

        public static FieldInfo GetMember<TOwner>(string memberName)
        {
            Type type = typeof(TOwner);
            return GetMember(type, memberName);
        }

        public static FieldInfo GetMember(Type type, string memberName)
        {
            return type.GetField(memberName, DEFAULT_BINDING_FLAGS);
        }

        /// <typeparam name="TMainType">The Type that owns the nested Type</typeparam>
        /// <param name="nestedTypeName">The (case-sensitive) name of the nested type</param>
        /// <param name="memberName">The name of the member you are trying to retrieve</param>
        public static FieldInfo GetMemberOfNestedType<TMainType>(string nestedTypeName, string memberName)
        {
            Type nestedType = typeof(TMainType).GetNestedType(nestedTypeName, DEFAULT_BINDING_FLAGS);
            return nestedType.GetField(memberName, DEFAULT_BINDING_FLAGS);
        }

        public static TMember GetMemberValue<TOwner, TMember>(string memberName, TOwner instance)
        {
            Type type = typeof(TOwner);
            FieldInfo field = type.GetField(memberName, DEFAULT_BINDING_FLAGS);
            return (TMember) field.GetValue(instance);
        }

        public static TMember GetMemberValue<TOwner, TMember>(string memberName, TOwner instance, BindingFlags bindingFlags)
        {
            Type type = typeof(TOwner);
            FieldInfo field = type.GetField(memberName, bindingFlags);
            return (TMember) field.GetValue(instance);
        }

        /// <typeparam name="TMainType">The Type that owns the nested Type</typeparam>
        /// <typeparam name="TMember">The Type of the member you are retrieving</typeparam>
        /// <param name="nestedTypeName">The (case-sensitive) name of the nested type</param>
        /// <param name="memberName">The name of the member whose value you are trying to retrieve</param>
        /// <param name="instance">The instance of the object that you are trying to receive tha value from</param>
        public static TMember GetMemberValueOfNestedType<TMainType, TMember>(string nestedTypeName, string memberName, object instance)
        {
            Type nestedType = typeof(TMainType).GetNestedType(nestedTypeName, DEFAULT_BINDING_FLAGS);
            try
            {
                FieldInfo field = nestedType.GetField(memberName, DEFAULT_BINDING_FLAGS);
                return (TMember) field.GetValue(instance);
            }
            catch
            {
                PropertyInfo property = nestedType.GetProperty(memberName, DEFAULT_BINDING_FLAGS);
                return (TMember) property.GetValue(instance);
            }
        }

        public static void SetMemberValue<TOwner>(string memberName, TOwner instance, object value)
        {
            Type type = typeof(TOwner);
            FieldInfo field = type.GetField(memberName, DEFAULT_BINDING_FLAGS);
            field.SetValue(instance, value);
        }

        public static void SetMemberValue<TOwner>(string memberName, TOwner instance, object value, BindingFlags bindingFlags)
        {
            Type type = typeof(TOwner);
            FieldInfo field = type.GetField(memberName, bindingFlags);
            field.SetValue(instance, value);
        }

        /// <typeparam name="TMainType">The Type that owns the nested Type</typeparam>
        /// <param name="nestedTypeName">The (case-sensitive) name of the nested type</param>
        /// <param name="memberName">The name of the member whose value you are trying to retrieve</param>
        /// <param name="instance">The instance of the object that you are trying to receive tha value from</param>
        /// <param name="value">The value that you wish to set to the member</param>
        public static void SetMemberValueOfNestedType<TMainType>(string nestedTypeName, string memberName, object instance, object value)
        {
            Type nestedType = typeof(TMainType).GetNestedType(nestedTypeName, DEFAULT_BINDING_FLAGS);
            FieldInfo field = nestedType.GetField(memberName, DEFAULT_BINDING_FLAGS);
            field.SetValue(instance, value);
        }

        private static object CallMethodInfo<TOwner>(string methodName, MethodInfo methodInfo, TOwner instance, object[] parameters)
        {
            if (methodInfo != null)
            {
                return methodInfo.Invoke(instance, parameters);
            }

            //todo DebugTools.LogError($"Couldn't find {methodName} in {instance.GetType().Name}");
            return null;
        }

        private static object CallMethodInfo(string methodName, MethodInfo methodInfo, object[] parameters)
        {
            if (methodInfo != null)
            {
                //todo DebugTools.LogError($"Couldn't find {methodName}");
                return methodInfo.Invoke(null, parameters);
            }

            return null;
        }

        public static void Init<T0, T1>(object instance, ReflectedField<T0> field0, ReflectedField<T1> field1)
        {
            field0.Init(instance);
            field1.Init(instance);
        }

        public static void Init<T0, T1, T2>(object instance, ReflectedField<T0> field0, ReflectedField<T1> field1, ReflectedField<T2> field2)
        {
            field0.Init(instance);
            field1.Init(instance);
            field2.Init(instance);
        }

        public static void Init<T0, T1, T2, T3>(object instance, ReflectedField<T0> field0, ReflectedField<T1> field1, ReflectedField<T2> field2, ReflectedField<T3> field3)
        {
            field0.Init(instance);
            field1.Init(instance);
            field2.Init(instance);
            field3.Init(instance);
        }

        public static void Init<T0, T1, T2, T3, T4>(object instance, ReflectedField<T0> field0, ReflectedField<T1> field1, ReflectedField<T2> field2, ReflectedField<T3> field3, ReflectedField<T4> field4)
        {
            field0.Init(instance);
            field1.Init(instance);
            field2.Init(instance);
            field3.Init(instance);
            field4.Init(instance);
        }
        #endregion
    }
}
