namespace Mayfair.Core.Code.Database.Interfaces
{
    using System;
    using Prateek.Runtime.KeynameFramework;

    /// <summary>
    /// Use this class to define how database entries need to be joined together
    /// Join is the same term used by SQL to link two table together
    /// </summary>
    public interface ICompositeIdentifier
    {
        Func<IDatabaseEntry, bool> RootValidationCondition { get; set; }
        /// <summary>
        /// Join the TJoiner type with the ICompositeIdentifier root one
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <returns></returns>
        bool Join<TJoiner>();
        /// <summary>
        /// Join the TJoiner type with the TSource one
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        bool Join<TJoiner, TSource>();
        /// <summary>
        /// Join the TJoiner type with the TSource one with custom UniqueId getter
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sourceIdFunc">UniqueId getter for TSource</param>
        /// <returns></returns>
        bool Join<TJoiner, TSource>(Func<TSource, Keyname> sourceIdFunc);
        /// <summary>
        /// Join the TJoiner type with the TSource one with custom UniqueId getter
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="joinerIdFunc">UniqueId getter for TSource</param>
        /// <param name="sourceIdFunc">UniqueId getter for TJoiner</param>
        /// <returns></returns>
        bool Join<TJoiner, TSource>(Func<TJoiner, Keyname> joinerIdFunc, Func<TSource, Keyname> sourceIdFunc);

        /// <summary>
        /// Join the TJoiner type with the ICompositeIdentifier root one
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <param name="status">Use it to define the joint as optional</param>
        /// <returns></returns>
        bool Join<TJoiner>(IdentifierStatus status);
        /// <summary>
        /// Join the TJoiner type with the TSource one
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="status">Use it to define the joint as optional</param>
        /// <returns></returns>
        bool Join<TJoiner, TSource>(IdentifierStatus status);
        /// <summary>
        /// Join the TJoiner type with the TSource one with custom UniqueId getter
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="status">Use it to define the joint as optional</param>
        /// <param name="sourceIdFunc">UniqueId getter for TSource</param>
        /// <returns></returns>
        bool Join<TJoiner, TSource>(IdentifierStatus status, Func<TSource, Keyname> sourceIdFunc);
        /// <summary>
        /// Join the TJoiner type with the TSource one with custom UniqueId getter
        /// </summary>
        /// <typeparam name="TJoiner"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="status">Use it to define the joint as optional</param>
        /// <param name="joinerIdFunc">UniqueId getter for TSource</param>
        /// <param name="sourceIdFunc">UniqueId getter for TJoiner</param>
        /// <returns></returns>
        bool Join<TJoiner, TSource>(IdentifierStatus status, Func<TJoiner, Keyname> joinerIdFunc, Func<TSource, Keyname> sourceIdFunc);
    }
}
