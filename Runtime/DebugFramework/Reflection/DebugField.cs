namespace Prateek.Runtime.DebugFramework.Reflection
{
    using UnityEngine.Assertions;

    public abstract class DebugField
    {
        #region Class Methods
        public static void SetOwnerToAllDebugFields(object debugFieldOwner, object contentOwner)
        {
            //Doing this to allow renaming of the interface method but keep the correct one as the first
            var methods = typeof(DebugField).GetMethods();
            Assert.IsTrue(methods.Length >= 1);

            var arguments = methods[0].GetParameters();
            Assert.IsTrue(arguments.Length == 1);
            Assert.IsTrue(arguments[0].ParameterType == typeof(object));

            var parameters = new object[1];
            var fieldInfos = ReflectionHelper.GetAllFieldInfo<DebugField>(debugFieldOwner.GetType(), true);
            foreach (var fieldInfo in fieldInfos)
            {
                parameters[0] = contentOwner;

                var debugField = fieldInfo.GetValue(debugFieldOwner);
                methods[0].Invoke(debugField, parameters);
            }
        }

        public abstract void SetOwner(object owner);

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
