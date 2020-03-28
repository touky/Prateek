namespace Mayfair.CoreContent.Code.Tags
{
    using Mayfair.Core.Code.TagSystem;

    public abstract partial class Tags
    {
        public interface IAnimationTag { }

        public abstract class Placement : MasterTag, IAnimationTag { }
        public abstract class Selection : MasterTag, IAnimationTag { }
    }
}
