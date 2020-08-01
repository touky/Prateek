namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.VisualAsset.Providers;

    public abstract class VisualResourceDirectCommand<TServant> : VisualResourceDirectCommand
        where TServant : VisualResourceServant
    {
        #region Class Methods
        public override bool AllowTransfer(VisualResourceServant servant)
        {
            return servant.GetType() == typeof(TServant);
        }
        #endregion
    }
}
