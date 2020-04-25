namespace Mayfair.CoreContent.Code.UniqueIds
{
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Mayfair.CoreContent.Code.Tags;

    /// <summary>
    /// Consts filters are const UniqueIds that can be used as bases to test UniqueId.Match()
    /// </summary>
    public static class ConstsFilter
    {
        #region Static and Constants
        public static readonly Keyname Pack = Keyname.Create<Pack>();
        public static readonly Keyname Mission = Keyname.Create<Mission>();
        public static readonly Keyname Resource = Keyname.Create<Resource>();

        public static readonly Keyname Buildings = Keyname.Create<BuildingType>();
        public static readonly Keyname BuildingRarity = Keyname.Create<BuildingType, Rarity>();

        public static readonly Keyname Business = Keyname.Create<Business>();
        public static readonly Keyname Residential = Keyname.Create<Residential>();
        public static readonly Keyname Decoration = Keyname.Create<Decoration>();
        public static readonly Keyname Utility = Keyname.Create<Utility>();

        public static readonly Keyname Size = Keyname.Create<Tags.SizeKeyword>();
        #endregion
    }
}
