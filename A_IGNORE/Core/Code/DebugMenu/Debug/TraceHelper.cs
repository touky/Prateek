namespace Mayfair.Core.Code.Utils.Debug
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    public static class TraceHelper
    {
        #region Class Methods
        [Conditional("NVIZZIO_DEV")]
        public static void EnsureTrace<T>(string callerName)
        {
            //benjaminh: limiting to the editor until I have time to double check on device
#if UNITY_EDITOR
            Type callerType = typeof(T);
            StackTrace trace = new StackTrace();
            for (int f = 0; f < trace.FrameCount; f++)
            {
                StackFrame frame = trace.GetFrame(f);
                MethodBase method = frame.GetMethod();
                Type containerType = method.ReflectedType;
                if (containerType == callerType && method.Name == callerName)
                {
                    return;
                }
            }

            StackFrame currentFrame = trace.GetFrame(1);//todo Consts.SECOND_ITEM);
            MethodBase currentMethod = currentFrame.GetMethod();
            Type currentContainerType = currentMethod.ReflectedType;

            throw new Exception($"{currentContainerType.Name}{currentMethod.Name}() is only allowed to be called by {callerType.Name}.{callerName}");
#endif //UNITY_EDITOR
        }
        #endregion
    }
}
