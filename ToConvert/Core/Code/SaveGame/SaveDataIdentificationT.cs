namespace Mayfair.Core.Code.SaveGame
{
    public abstract class SaveDataIdentification<TData> : SaveDataIdentification
        where TData : class
    {
        #region Fields
        protected TData data;
        #endregion

        #region Properties
        public TData Data
        {
            get { return this.data; }
        }
        #endregion
    }
}
