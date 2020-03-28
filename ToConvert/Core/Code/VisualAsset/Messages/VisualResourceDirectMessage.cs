namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Mayfair.Core.Code.VisualAsset.Providers;

    public abstract class VisualResourceDirectMessage : DirectMessage
    {
        #region Fields
        private IAssignableVisualResource instance;
        #endregion

        #region Properties
        public override long MessageID
        {
            get { return ConvertToId(typeof(VisualResourceDirectMessage)); }
        }

        public IAssignableVisualResource Instance
        {
            get { return instance; }
        }
        #endregion

        #region Class Methods
        public abstract bool AllowTransfer(VisualResourceServiceProvider provider);

        //todo benjaminh: this is not pratical, we can't have multiple uniqueids for one assignable
        //todo benjaminh: This should be re-evaluated at some point. Maybe split the two, or require the assignable to return the needed IIdentifiable
        public void Init<T>(T instance)
            where T : IAssignableVisualResource, IIdentifiable
        {
            this.instance = instance;
        }
        #endregion
    }
}
