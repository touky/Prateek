namespace Prateek.Core.Code.Extensions
{
    using System;

    public static class ActionExtensions
    {
        #region Class Methods
        /// <summary>
        ///     Performs a null check before invoking the Action
        /// </summary>
        public static void SafeInvoke(this Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        ///     Performs a null check before invoking the Action
        /// </summary>
        public static void SafeInvoke<T>(this Action<T> action, T arg)
        {
            if (action != null)
            {
                action(arg);
            }
        }

        /// <summary>
        ///     Performs a null check before invoking the Action
        /// </summary>
        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            if (action != null)
            {
                action(arg1, arg2);
            }
        }

        /// <summary>
        ///     Performs a null check before invoking the Action
        /// </summary>
        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            if (action != null)
            {
                action(arg1, arg2, arg3);
            }
        }
        #endregion
    }
}
