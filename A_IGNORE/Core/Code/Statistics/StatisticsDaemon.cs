namespace Mayfair.Core.Code.Statistics
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.GameAction;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils.Debug;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.KeynameFramework;

    using Prateek.Runtime.TickableFramework.Interfaces;

    public sealed class StatisticsDaemon
        : ReceiverDaemonOverseer<StatisticsDaemon, StatisticsServant>
        , IDebugMenuNotebookOwner
        , IPreUpdateTickable
    {
        #region Fields
        //private Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>> trackedProviderTags = new Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>>();
        #endregion
            

        public void PreUpdate()
        {
            foreach (var servant in AllAliveServants)
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
        public override void DefineCommandReceiverActions()
        {
            CommandReceiver.SetActionFor<GameActionCommand>(OnStatisticsMessage);
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
