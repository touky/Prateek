namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine.Assertions;

    [DebuggerDisplay("'{name}' ({ownerType.Name})")]
    public abstract class DebugField
    {
        #region Fields
        private string name;
        protected object owner;
        private Type ownerType;
        private SerializedObject serializedOwner;
        private MemberInfo memberInfo;
        protected FieldInfo fieldInfo;
        protected PropertyInfo propertyInfo;
        #endregion

        #region Properties
        public string Name { get { return name; } }

        public bool IsValid { get { return memberInfo != null; } }

#if UNITY_EDITOR
        public SerializedProperty SerializedProperty
        {
            get
            {
                if (!(owner is UnityEngine.Object unityOwner))
                {
                    return null;
                }

                if (serializedOwner == null)
                {
                    serializedOwner = new SerializedObject(unityOwner);
                }

                return serializedOwner.FindProperty(name);
            }
        }
#endif
        #endregion

        #region Class Methods
        protected void SetName(string name)
        {
            this.name = name.Trim();

            if (owner != null)
            {
                SetOwner(owner);
            }
        }

        public virtual void SetOwner(object owner)
        {
            if (owner == null)
            {
                return;
            }

            this.owner = owner;

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            ownerType = this.owner.GetType();
            memberInfo = fieldInfo = ReflectionHelper.SearchFieldInfo(ownerType, name, true);
            if (fieldInfo == null)
            {
                memberInfo = propertyInfo = ReflectionHelper.SearchPropertyInfo(ownerType, name, true);

                if (memberInfo == null)
                {
                    UnityEngine.Debug.LogError($"{name} could not be found in {ownerType.Name}");
                }
            }
        }

        public static void SetOwnerToAllDebugFields(object debugFieldOwner, object contentOwner)
        {
            //Doing this to allow renaming of the interface method but keep the correct one as the first
            var methods = typeof(DebugField).GetMethods();
            Assert.IsTrue(methods.Length >= 1);

            var methodInfo = (MethodInfo) null;
            foreach (var method in methods)
            {
                if ((method.Attributes & MethodAttributes.Virtual) != 0
                 && (method.Attributes & MethodAttributes.SpecialName) == 0)
                {
                    methodInfo = method;
                    break;
                }
            }

            Assert.IsNotNull(methodInfo, $"{nameof(DebugField)} class has changed, cannot find its abstract SetOwner !");

            var arguments = methodInfo.GetParameters();
            Assert.IsTrue(arguments.Length == 1);
            Assert.IsTrue(arguments[0].ParameterType == typeof(object));

            var parameters = new object[1];
            var fieldInfos = ReflectionHelper.GetAllFieldInfo<DebugField>(debugFieldOwner.GetType(), true);
            foreach (var fieldInfo in fieldInfos)
            {
                parameters[0] = contentOwner;

                var debugField = fieldInfo.GetValue(debugFieldOwner);
                methodInfo.Invoke(debugField, parameters);
            }
        }

        public static void SetOwner<T0, T1>(object owner, DebugField<T0> field0, DebugField<T1> field1)
        {
            field0.SetOwner(owner);
            field1.SetOwner(owner);
        }

        public static void SetOwner<T0, T1, T2>(object owner, DebugField<T0> field0, DebugField<T1> field1, DebugField<T2> field2)
        {
            SetOwner(owner, field0, field1);
            field2.SetOwner(owner);
        }

        public static void SetOwner<T0, T1, T2, T3>(object owner, DebugField<T0> field0, DebugField<T1> field1, DebugField<T2> field2, DebugField<T3> field3)
        {
            SetOwner(owner, field0, field1);
            SetOwner(owner, field2, field3);
        }

        public static void SetOwner<T0, T1, T2, T3, T4>(object owner, DebugField<T0> field0, DebugField<T1> field1, DebugField<T2> field2, DebugField<T3> field3, DebugField<T4> field4)
        {
            SetOwner(owner, field0, field1);
            SetOwner(owner, field2, field3);
            field4.SetOwner(owner);
        }
        #endregion
    }
}
