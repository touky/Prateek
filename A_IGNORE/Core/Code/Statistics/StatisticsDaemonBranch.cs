namespace Mayfair.Core.Code.Statistics
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameAction;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Debug;
    using Prateek.DaemonFramework.Code.Servants;
    using Prateek.KeynameFramework.Interfaces;

    public abstract class StatisticsServant : ServantBehaviour<StatisticsDaemon, StatisticsServant>
    {
        #region StatisticFlushType enum
        [Flags]
        public enum StatisticFlushType
        {
            BuildReport = 1 << 0,
            IncludeUnchanged = 1 << 1
        }
        #endregion

        #region Fields
        private bool relevantTagsAreDirty = false;
        //protected HashSet<KeywordHolder> relevantTags = new HashSet<KeywordHolder>();

        private List<Statistic> totalStatistics = new List<Statistic>();
        private List<Statistic> pendingStatistics = new List<Statistic>();
        protected List<StatisticReport> reports = new List<StatisticReport>();
        #endregion

        #region Properties
        public bool RelevantTagsAreDirty
        {
            get { return relevantTagsAreDirty; }
            internal set { relevantTagsAreDirty = value; }
        }

        //public HashSet<KeywordHolder> RelevantTags
        //{
        //    get { return relevantTags; }
        //}
        #endregion

        #region Class Methods
        public abstract void ProcessMessage(GameActionCommand command);

        public void AddRelevantTag<T>()
            where T : MasterKeyword
        {
            //todo
            //KeywordHolder tag = new KeywordHolder();
            //tag.Add<T>();
            //if (!relevantTags.Contains(tag))
            //{
            //    relevantTagsAreDirty = true;
            //    relevantTags.Add(tag);
            //}
        }

        protected void AddPendingStatistic(Statistic statistic)
        {
            //DebugTools.Log($"Adding Statistic: {statistic.Count} of {statistic.Tags}");

            int index = pendingStatistics.FindIndex(s => { return s.GetHashCode() == statistic.GetHashCode(); });
            if (index == Consts.INDEX_NONE)
            {
                pendingStatistics.Add(statistic);
                return;
            }

            Statistic otherStatistic = pendingStatistics[index];
            {
                otherStatistic.MergePending(statistic);
            }
            pendingStatistics[index] = otherStatistic;
        }

        protected virtual bool FlushPendingStatistics(StatisticFlushType flushType)
        {
            if (pendingStatistics.Count == 0)
            {
                return false;
            }

            if (flushType.HasFlag(StatisticFlushType.BuildReport) && flushType.HasFlag(StatisticFlushType.IncludeUnchanged))
            {
                reports.Clear();
                foreach (Statistic total in totalStatistics)
                {
                    reports.Add(new StatisticReport(total));
                }
            }

            //foreach (Statistic pending in pendingStatistics)
            //{
            //    Statistic total = new Statistic(pending.Tags);
            //    int index = totalStatistics.FindIndex(s => { return s.GetHashCode() == pending.GetHashCode(); });
            //    if (index == Consts.INDEX_NONE)
            //    {
            //        index = totalStatistics.Count;
            //        totalStatistics.Add(total);
            //    }
            //    else
            //    {
            //        total = totalStatistics[index];
            //    }

            //    StatisticReport report = new StatisticReport(total);
            //    int reportIndex = Consts.INDEX_NONE;
            //    if (flushType.HasFlag(StatisticFlushType.BuildReport))
            //    {
            //        reportIndex = reports.FindIndex(r => { return r.GetHashCode() == pending.GetHashCode(); });
            //        if (reportIndex == Consts.INDEX_NONE)
            //        {
            //            reportIndex = reports.Count;
            //            reports.Add(report);
            //        }
            //        else
            //        {
            //            report = reports[reportIndex];
            //        }
            //    }

            //    total.MergePending(pending);

            //    if (flushType.HasFlag(StatisticFlushType.BuildReport))
            //    {
            //        report.total = total;
            //        report.delta = pending.Count;
            //        reports[reportIndex] = report;
            //    }

            //    totalStatistics[index] = total;
            //}

            //pendingStatistics.Clear();

            return true;
        }
        #endregion
    }
}
