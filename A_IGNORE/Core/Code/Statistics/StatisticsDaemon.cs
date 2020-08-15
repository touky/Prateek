namespace Mayfair.Core.Code.Statistics
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.GameAction;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils.Debug;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.KeynameFramework;

    using Prateek.Runtime.TickableFramework.Interfaces;

    public sealed class StatisticsDaemon
        : DaemonOverseer<StatisticsDaemon, StatisticsServant>
        , IDebugMenuNotebookOwner
        , ICommandReceiverOwner
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
        public void DefineReceptionActions(ICommandReceiver receiver)
        {
            receiver.SetActionFor<GameActionCommand>(OnStatisticsMessage);
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

        public int Priority(IPriority<IApplicationFeedbackTickable> type)
        {
            throw new System.NotImplementedException();
        }

        public int Priority(IPriority<IPreUpdateTickable> type)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
