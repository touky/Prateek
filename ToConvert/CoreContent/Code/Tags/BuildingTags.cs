namespace Mayfair.CoreContent.Code.Tags
{
    public abstract partial class Tags { }

    public abstract class BuildingType : CommonType { }

    public abstract class Business : BuildingType { }
    public abstract class Residential : BuildingType { }
    public abstract class Decoration : BuildingType { }
    public abstract class Utility : BuildingType { }
    public abstract class Railroad : BuildingType { }
    public abstract class Transport : BuildingType { }
}
