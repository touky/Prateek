namespace Prateek.Runtime.DebugFramework.Reflection
{
    using System.Reflection;
    using UnityEngine.Assertions;

    public abstract class DebugField
    {
        #region Properties
        public abstract string Name { get; }
        public abstract bool IsValid { get; }
        #endregion

        #region Class Methods
        public abstract void SetOwner(object owner);

        public static void SetOwnerToAllDebugFields(object debugFieldOwner, object contentOwner)
        {
            //Doing this to allow renaming of the interface method but keep the correct one as the first
            var methods = typeof(DebugField).GetMethods();
            Assert.IsTrue(methods.Length >= 1);

            var methodInfo = (MethodInfo) null;
            foreach (var method in methods)
            {
                if ((method.Attributes & MethodAttributes.Abstract) != 0
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
