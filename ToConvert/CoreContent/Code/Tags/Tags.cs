namespace Mayfair.CoreContent.Code.Tags
{
    using Mayfair.Core.Code.TagSystem;

    //Temp class to fool the unit test
    public abstract partial class Tags { }

    public abstract class CityName : MasterTag { }

    public abstract class GameplayName : MasterTag { }
    public abstract class GameCamera : GameplayName { }

    public abstract class MenuName : MasterTag { }
    public abstract class BuildMenu : MenuName { }
    public abstract class PropertyInventoryMenu : MenuName { }

    public abstract class AC : CityName, ICityDatabaseName { }
    public abstract class AtlanticCity : AC { }

    public interface ICityDatabaseName { }

    public abstract class CommonType : MasterTag { }

    public abstract class Template : CommonType { }
    public abstract class TEMPLATE : Template { }

    public abstract class Resource : CommonType { }
    public abstract class Need : CommonType { }
    public abstract class Deed : CommonType { }
    public abstract class Pack : CommonType { }
    public abstract class Gacha : CommonType { }
    public abstract class Card : CommonType { }
    public abstract class Property : CommonType { }
    public abstract class Mission : CommonType { }

    public abstract class Rarity : MasterTag { }

    public abstract class s1 : Rarity { }
    public abstract class s2 : Rarity { }
    public abstract class s3 : Rarity { }
    public abstract class s4 : Rarity { }
    public abstract class s5 : Rarity { }

    public abstract class FP : MasterTag { }
	
    public abstract class Wealth : MasterTag { }
    public abstract class Prosperity : MasterTag { }
    public abstract class Population : MasterTag { }


    public abstract class Tutorial : MasterTag { }
    public abstract class Beginner : MasterTag { }

    public abstract class AuctionHouse : CommonType { }

    public abstract class GameSettings : MasterTag { }
}