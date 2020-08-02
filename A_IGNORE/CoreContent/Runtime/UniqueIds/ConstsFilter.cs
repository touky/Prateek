namespace Mayfair.CoreContent.Code.UniqueIds
{
    using Mayfair.CoreContent.Code.Tags;
    using Prateek.Runtime.KeynameFramework;

    /// <summary>
    /// Consts filters are const UniqueIds that can be used as bases to test UniqueId.Match()
    /// </summary>
    public static class ConstsFilter
    {
        #region Static and Constants
        public static readonly Keyname Pack = string.Empty;//todo Keyname.Create<Pack>();
        public static readonly Keyname Mission = string.Empty;//todo Keyname.Create<Mission>();
        public static readonly Keyname Resource = string.Empty;//todo Keyname.Create<Resource>();

        public static readonly Keyname Buildings = string.Empty;//todo Keyname.Create<BuildingType>();
        public static readonly Keyname BuildingRarity = string.Empty;//todo Keyname.Create<BuildingType, Rarity>();

        public static readonly Keyname Business = string.Empty;//todo Keyname.Create<Business>();
        public static readonly Keyname Residential = string.Empty;//todo Keyname.Create<Residential>();
        public static readonly Keyname Decoration = string.Empty;//todo Keyname.Create<Decoration>();
        public static readonly Keyname Utility = string.Empty;//todo Keyname.Create<Utility>();

        public static readonly Keyname Size = string.Empty;//todo Keyname.Create<Tags.SizeKeyword>();
        #endregion
    }
}
