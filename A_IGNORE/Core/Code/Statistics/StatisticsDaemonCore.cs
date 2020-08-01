namespace Mayfair.Core.Code.Statistics
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.GameAction;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils.Debug;
    using Prateek.KeynameFramework;
    using Prateek.CommandFramework.Tools;
    using Prateek.TickableFramework.Code.Enums;

    public sealed class StatisticsDaemon : CommandReceiverDaemon<StatisticsDaemon, StatisticsServant>, IDebugMenuNotebookOwner
    {
        #region Fields
        //private Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>> trackedProviderTags = new Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>>();
        #endregion
            
        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }

        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            IEnumerable<StatisticsServant> providers = GetValidServants();
            foreach (StatisticsServant servant in providers)
            {
                if (!servant.RelevantTagsAreDirty)
                {
                    continue;
                }

                //servant.RelevantTagsAreDirty = false;
                //foreach (KeywordHolder relevantTag in servant.RelevantTags)
                //{
                //    HashSet<StatisticsServiceProvider> hash;
                //    if (!trackedProviderTags.TryGetValue(relevantTag, out hash))
                //    {
                //        hash = new HashSet<StatisticsServiceProvider>();
                //        trackedProviderTags.Add(relevantTag, hash);
                //    }

                //    if (hash.Contains(servant))
                //    {
                //        continue;
                //    }

                //    hash.Add(servant);
                //}
            }
        }

        #region Service
        protected override void OnAwake() { }
        #endregion

        #region Messaging
        public override void CommandReceived() { }

        protected override void SetupCommandReceiverCallback()
        {
            CommandReceiver.AddCallback<GameActionCommand>(OnStatisticsMessage);
        }
        #endregion

        #region Class Methods
        private void OnStatisticsMessage(GameActionCommand command)
        {
            foreach (Keyname uniqueId in command.Tags)
            {
                //if (!trackedProviderTags.TryGetValue(uniqueId.KeywordHolder, out HashSet<StatisticsServiceProvider> providers))
                //{
                //    return;
                //}

                DebugTools.Log($"Received {command.ToString()}");

                //foreach (StatisticsServiceProvider servant in providers)
                //{
                //    if (!servant.IsValid)
                //    {
                //        continue;
                //    }

                //    servant.ProcessMessage(notice);
                //}
            }
        }
        #endregion
    }
}
