namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.GadgetFramework;

    public abstract class ContentAccessDaemonOverseer<TDaemon, TServant>
        : ReceiverDaemonOverseer<TDaemon, TServant>
        where TDaemon : ReceiverDaemonOverseer<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
    {
        #region Class Methods
        public override void DefineReceptionActions(ICommandReceiver receiver)
        {
            receiver.SetActionFor<ContentAccessChangedResponse>(OnContentAccessChangedResponse);
        }

        protected override void OnServantRegistered(TServant servant)
        {
            base.OnServantRegistered(servant);

            this.Get<ICommandReceiver>().Send(servant.GetAccessRequest());
        }

        private void OnContentAccessChangedResponse(ContentAccessChangedResponse response)
        {
            foreach (var aliveServant in AllAliveServants)
            {
                aliveServant.ContentAccessChanged(response);
            }
        }
        #endregion
    }
}
