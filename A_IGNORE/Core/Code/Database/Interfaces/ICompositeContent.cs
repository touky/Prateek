namespace Mayfair.Core.Code.Database.Interfaces
{
    using Mayfair.Core.Code.Utils.Types;
    using System;
    using Prateek.Runtime.KeynameFramework;

    public interface ICompositeContent
    {
        /// <summary>
        /// Get the UniqueId use to identify the ICompositeContent
        /// </summary>
        Keyname Keyname { get; }
        /// <summary>
        /// Check if the ICompositeContent contains that type of data
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        bool Contains<TType>() where TType : IDatabaseEntry;
        /// <summary>
        /// Check if the ICompositeContent contains both type of data
        /// and can join them using the ICompositeIdentifier
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TJoiner"></typeparam>
        /// <returns></returns>
        bool Contains<TSource, TJoiner>() where TSource : IDatabaseEntry
                                         where TJoiner : IDatabaseEntry;
        /// <summary>
        /// Get the first data of the given type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        TType Get<TType>() where TType : IDatabaseEntry;
        /// <summary>
        /// Get the first data of the given TJoiner type that can be joined with TSource
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TJoiner"></typeparam>
        /// <returns></returns>
        TJoiner Get<TSource, TJoiner>() where TSource : IDatabaseEntry
                                      where TJoiner : IDatabaseEntry;
        /// <summary>
        /// Get all the datas of the given type
        /// </summary>
        /// <typeparam name="TType"></typeparam>
        /// <returns></returns>
        TType[] GetAll<TType>() where TType : IDatabaseEntry;
        /// <summary>
        /// Get all the datas of the given TJoiner type that can be joined with a TSource
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TJoiner"></typeparam>
        /// <returns></returns>
        TJoiner[] GetAll<TSource, TJoiner>() where TSource : IDatabaseEntry
                                           where TJoiner : IDatabaseEntry;
        /// <summary>
        /// Get all the datas of the given type
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IDatabaseEntry Get(Type source);
        /// <summary>
        /// Get the first data of the given TJoiner type that can be joined with TSource
        /// </summary>
        /// <param name="source"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        IDatabaseEntry Get(Type source, Type linked);
        /// <summary>
        /// Get all the datas of the given type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IDatabaseEntry[] GetAll(Type type);
        /// <summary>
        /// Get all the datas of the given TJoiner type that can be joined with a TSource
        /// </summary>
        /// <param name="source"></param>
        /// <param name="linked"></param>
        /// <returns></returns>
        IDatabaseEntry[] GetAll(Type source, Type linked);
    }
}
