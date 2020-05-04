namespace Mayfair.Core.Code.Utils.Tools
{
    public struct TimeOutTicker
    {
        #region Fields
        private int timeOutDefault;
        private int timeOut;
        #endregion

        #region Constructors
        public TimeOutTicker(int timeOut)
        {
            this.timeOutDefault = timeOut;
            this.timeOut = timeOut;
        }

        public static implicit operator TimeOutTicker(int timeOut)
        {
            return new TimeOutTicker(timeOut);
        }
        #endregion

        #region Class Methods
        public void Begin()
        {
            this.timeOut = this.timeOutDefault;
        }

        public bool CanTrigger()
        {
            if (this.timeOut-- < 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
