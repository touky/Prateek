namespace Prateek.Runtime.StateMachineFramework.Interfaces
{
    public interface IEnumComparer<TEnum>
    {
        #region Class Methods
        /// <summary>
        ///     This Compare method is needed because C# generic are a pain to make work without GC
        /// </summary>
        /// <param name="enum0"></param>
        /// <param name="enum1"></param>
        /// <returns></returns>
        bool Compare(TEnum enum0, TEnum enum1);
        #endregion
    }
}