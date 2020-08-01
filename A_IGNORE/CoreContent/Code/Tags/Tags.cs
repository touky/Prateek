namespace Mayfair.CoreContent.Code.Tags
{
    using Prateek.KeynameFramework.Interfaces;

    //Temp class to fool the unit test
    public abstract partial class Tags { }

    public abstract class CityName : MasterKeyword { }

    public abstract class GameplayName : MasterKeyword { }
    public abstract class GameCamera : GameplayName { }

    public abstract class MenuName : MasterKeyword { }
    public abstract class BuildMenu : MenuName { }
    public abstract class PropertyInventoryMenu : MenuName { }

    public abstract class AC : CityName, ICityDatabaseName { }
    public abstract class AtlanticCity : AC { }

    public interface ICityDatabaseName { }

    public abstract class CommonType : MasterKeyword { }

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

    public abstract class Rarity : MasterKeyword { }

    public abstract class s1 : Rarity { }
    public abstract class s2 : Rarity { }
    public abstract class s3 : Rarity { }
    public abstract class s4 : Rarity { }
    public abstract class s5 : Rarity { }

    public abstract class FP : MasterKeyword { }
	
    public abstract class Wealth : MasterKeyword { }
    public abstract class Prosperity : MasterKeyword { }
    public abstract class Population : MasterKeyword { }


    public abstract class Tutorial : MasterKeyword { }
    public abstract class Beginner : MasterKeyword { }

    public abstract class AuctionHouse : CommonType { }

    public abstract class GameSettings : MasterKeyword { }
}