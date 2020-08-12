namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Mayfair.Core.Code.VisualAsset.Providers;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public sealed class VisualResourceDaemonOverseer
        : ContentAccessDaemonOverseer<VisualResourceDaemonOverseer, VisualResourceServant>
        , IPreUpdateTickable
    {
        #region Fields
        private DebugMenuNotebook debugNotebook;
        private DebugMenuPage debugMainPage;
        #endregion


        public void PreUpdate()
        {
            //PerformProviderAction(ServiceProviderUsageRule,
            //                      servant =>
            //                      {
            //                          servant.RefreshPending();
            //                      });
        }

        #region Service
        protected override void OnAwake()
        {
            SetupDebugContent();
        }
        #endregion

        #region Messaging
        public override void DefineReceptionActions(ICommandReceiver receiver)
        {
            base.DefineReceptionActions(receiver);

            receiver.SetActionFor<VisualResourceDirectCommand>(OnVisualResourceMessage);
        }
        #endregion

        #region Class Methods
        protected override void OnServantRegistered(VisualResourceServant servant)
        {
            base.OnServantRegistered(servant);

            servant.SetupDebugContent(debugNotebook, debugMainPage);
        }

        private void OnVisualResourceMessage(VisualResourceDirectCommand command)
        {
            foreach (var servant in AllAliveServants)
            {
                if (!command.AllowTransfer(servant))
                {
                    continue;
                }

                servant.OnVisualResourceMessage(command);
            }
        }

        #region Debug
        [Conditional("PRATEEK_DEBUG")]
        public void SetupDebugContent()
        {
            debugNotebook = new DebugMenuNotebook("VSLR", "Visual Resources");
            debugMainPage = new EmptyMenuPage("MAIN");
            debugNotebook.Register();
        }
        #endregion
        #endregion
    }
}