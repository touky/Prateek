namespace Mayfair.Core.Code.Gameplay
{
    public interface ICurrencyCollectable
    {
        #region Properties
        ulong MinimumAmountCollectable { get; }
        ulong MaximumAmountCollectable { get; }
        ulong AmountCollectable { get; }
        #endregion
    }
}
