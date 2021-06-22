namespace Prateek.Runtime.Core.Extensions
{
    using System;

    public static class FuncExtensions
    {
        #region Class Methods
        /// <summary>
        ///     Performs a null check before invoking the Func
        /// </summary>
        public static TR SafeInvoke<TR>(this Func<TR> func)
        {
            if (func != null)
            {
                return func();
            }

            return default;
        }

        /// <summary>
        ///     Performs a null check before invoking the Func
        /// </summary>
        public static TR SafeInvoke<T1, TR>(this Func<T1, TR> func, T1 arg)
        {
            if (func != null)
            {
                return func(arg);
            }

            return default;
        }

        /// <summary>
        ///     Performs a null check before invoking the Func
        /// </summary>
        public static TR SafeInvoke<T1, T2, TR>(this Func<T1, T2, TR> func, T1 arg1, T2 arg2)
        {
            if (func != null)
            {
                return func(arg1, arg2);
            }

            return default;
        }

        /// <summary>
        ///     Performs a null check before invoking the Func
        /// </summary>
        public static TR SafeInvoke<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func, T1 arg1, T2 arg2, T3 arg3)
        {
            if (func != null)
            {
                return func(arg1, arg2, arg3);
            }

            return default;
        }
        #endregion
    }
}
