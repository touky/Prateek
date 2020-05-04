namespace Mayfair.Core.Code.Types.Extensions
{
    using Unity.Collections;

    public static class NativeArrayExtensions
    {
        #region Class Methods
        /// <summary>
        /// Safely dispose of the NativeArray by checking if it's valid
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void SafeDispose<T>(this NativeArray<T> array)
            where T : struct
        {
            if (array.IsCreated)
            {
                array.Dispose();
            }
        }
        #endregion
    }
}
