namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.VisualAsset.Providers;
    using Prateek.KeynameFramework.Interfaces;
    using Commands.Core;

    public abstract class VisualResourceDirectCommand : DirectCommand
    {
        #region Fields
        private IAssignableVisualResource instance;
        #endregion

        #region Properties
        public override long CommandID
        {
            get { return ConvertToId(typeof(VisualResourceDirectCommand)); }
        }

        public IAssignableVisualResource Instance
        {
            get { return instance; }
        }
        #endregion

        #region Class Methods
        public abstract bool AllowTransfer(VisualResourceServant servant);

        //todo benjaminh: this is not pratical, we can't have multiple uniqueids for one assignable
        //todo benjaminh: This should be re-evaluated at some point. Maybe split the two, or require the assignable to return the needed IIdentifiable
        public void Init<T>(T instance)
            where T : IAssignableVisualResource, IKeynameUser
        {
            this.instance = instance;
        }
        #endregion
    }
}
