namespace Mayfair.Core.Code.Statistics
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.GameAction;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Debug;
    using Mayfair.Core.Code.Utils.Types.UniqueId;

    public sealed class StatisticsService : ServiceCommunicatorBehaviour<StatisticsService, StatisticsServiceProvider>, IDebugMenuNotebookOwner
    {
        #region Fields
        //private Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>> trackedProviderTags = new Dictionary<KeywordHolder, HashSet<StatisticsServiceProvider>>();
        #endregion

        #region Unity Methods
        protected override void Update()
        {
            base.Update();

            IEnumerable<StatisticsServiceProvider> providers = GetAllValidProviders();
            foreach (StatisticsServiceProvider provider in providers)
            {
                if (!provider.RelevantTagsAreDirty)
                {
                    continue;
                }

                //provider.RelevantTagsAreDirty = false;
                //foreach (KeywordHolder relevantTag in provider.RelevantTags)
                //{
                //    HashSet<StatisticsServiceProvider> hash;
                //    if (!trackedProviderTags.TryGetValue(relevantTag, out hash))
                //    {
                //        hash = new HashSet<StatisticsServiceProvider>();
                //        trackedProviderTags.Add(relevantTag, hash);
                //    }

                //    if (hash.Contains(provider))
                //    {
                //        continue;
                //    }

                //    hash.Add(provider);
                //}
            }
        }
        #endregion

        #region Service
        protected override void OnAwake() { }
        #endregion

        #region Messaging
        public override void MessageReceived() { }

        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<GameActionMessage>(OnStatisticsMessage);
        }
        #endregion

        #region Class Methods
        private void OnStatisticsMessage(GameActionMessage message)
        {
            foreach (Keyname uniqueId in message.Tags)
            {
                //if (!trackedProviderTags.TryGetValue(uniqueId.KeywordHolder, out HashSet<StatisticsServiceProvider> providers))
                //{
                //    return;
                //}

                DebugTools.Log($"Received {message.ToString()}");

                //foreach (StatisticsServiceProvider provider in providers)
                //{
                //    if (!provider.IsValid)
                //    {
                //        continue;
                //    }

                //    provider.ProcessMessage(message);
                //}
            }
        }
        #endregion
    }
}
