namespace Mayfair.CoreContent.Code.Tags
{
    using Mayfair.Core.Code.TagSystem;

    public abstract partial class Tags
    {
        public interface IGameActionVerb{}

        public abstract class Move    : MasterTag, IGameActionVerb { }
        public abstract class Rotate  : MasterTag, IGameActionVerb { }
        public abstract class ZoomIn  : MasterTag, IGameActionVerb { }
        public abstract class ZoomOut : MasterTag, IGameActionVerb { }
        public abstract class Collect : MasterTag, IGameActionVerb { }
        public abstract class Open    : MasterTag, IGameActionVerb { }
        public abstract class Upgrade : MasterTag, IGameActionVerb { }
        public abstract class Acquire : MasterTag, IGameActionVerb { }
        public abstract class Own     : MasterTag, IGameActionVerb { }
        public abstract class Reach   : MasterTag, IGameActionVerb { }
        public abstract class Place   : MasterTag, IGameActionVerb { }
    }
}
