namespace Prateek.Runtime.AppContentFramework.Daemons
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.CommandFramework;

    public abstract class ContentAccessDaemonOverseer<TDaemon, TServant>
        : ReceiverDaemonOverseer<TDaemon, TServant>
        where TDaemon : ReceiverDaemonOverseer<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
    {
        #region Class Methods
        public override void DefineCommandReceiverActions()
        {
            CommandReceiver.SetActionFor<ContentAccessChangedResponse>(OnContentAccessChangedResponse);
        }

        protected override void OnServantRegistered(TServant servant)
        {
            base.OnServantRegistered(servant);

            CommandReceiver.Send(servant.GetAccessRequest());
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
