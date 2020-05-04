namespace Mayfair.Core.Code.Statistics
{
    public class StatisticReport
    {
        #region Fields
        public Statistic total;
        public int delta;
        #endregion

        #region Constructors
        public StatisticReport(Statistic total)
        {
            this.total = total;
            delta = 0;
        }
        #endregion

        #region Class Methods
        public override int GetHashCode()
        {
            return total.GetHashCode();
        }
        #endregion
    }
}
