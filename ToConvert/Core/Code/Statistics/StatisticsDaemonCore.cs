namespace Mayfair.Core.Code.Statistics
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.GameAction;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Prateek.NoticeFramework.Tools;

    public sealed class StatisticsDaemonCore : NoticeReceiverDaemonCore<StatisticsDaemonCore, StatisticsDaemonBranch>, IDebugMenuNotebookOwner
    {
        #region Fields
        //private Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>> trackedProviderTags = new Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>>();
        #endregion

        #region Unity Methods
        protected override void Update()
        {
            base.Update();

            IEnumerable<StatisticsDaemonBranch> providers = GetValidBranches();
            foreach (StatisticsDaemonBranch branch in providers)
            {
                if (!branch.RelevantTagsAreDirty)
                {
                    continue;
                }

                //branch.RelevantTagsAreDirty = false;
                //foreach (KeywordHolder relevantTag in branch.RelevantTags)
                //{
                //    HashSet<StatisticsServiceProvider> hash;
                //    if (!trackedProviderTags.TryGetValue(relevantTag, out hash))
                //    {
                //        hash = new HashSet<StatisticsServiceProvider>();
                //        trackedProviderTags.Add(relevantTag, hash);
                //    }

                //    if (hash.Contains(branch))
                //    {
                //        continue;
                //    }

                //    hash.Add(branch);
                //}
            }
        }
        #endregion

        #region Service
        protected override void OnAwake() { }
        #endregion

        #region Messaging
        public override void NoticeReceived() { }

        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<GameActionNotice>(OnStatisticsMessage);
        }
        #endregion

        #region Class Methods
        private void OnStatisticsMessage(GameActionNotice notice)
        {
            foreach (Keyname uniqueId in notice.Tags)
            {
                //if (!trackedProviderTags.TryGetValue(uniqueId.KeywordHolder, out HashSet<StatisticsServiceProvider> providers))
                //{
                //    return;
                //}

                DebugTools.Log($"Received {notice.ToString()}");

                //foreach (StatisticsServiceProvider branch in providers)
                //{
                //    if (!branch.IsValid)
                //    {
                //        continue;
                //    }

                //    branch.ProcessMessage(notice);
                //}
            }
        }
        #endregion
    }
}
