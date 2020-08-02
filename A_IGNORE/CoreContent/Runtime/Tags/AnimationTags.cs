namespace Mayfair.CoreContent.Code.Tags
{
    using Prateek.Runtime.KeynameFramework.Interfaces;

    public abstract partial class Tags
    {
        public interface IAnimationTag { }

        public abstract class Placement : MasterKeyword, IAnimationTag { }
        public abstract class Selection : MasterKeyword, IAnimationTag { }
    }
}
