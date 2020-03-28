namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.VisualAsset.Providers;

    public abstract class VisualResourceDirectMessage<TProvider> : VisualResourceDirectMessage
        where TProvider : VisualResourceServiceProvider
    {
        #region Class Methods
        public override bool AllowTransfer(VisualResourceServiceProvider provider)
        {
            return provider.GetType() == typeof(TProvider);
        }
        #endregion
    }
}
