namespace Mayfair.CoreContent.Code.Tags
{
    using Prateek.Runtime.KeynameFramework.Interfaces;

    public abstract partial class Tags
    {
        public interface IGameActionVerb{}

        public abstract class Move    : MasterKeyword, IGameActionVerb { }
        public abstract class Rotate  : MasterKeyword, IGameActionVerb { }
        public abstract class ZoomIn  : MasterKeyword, IGameActionVerb { }
        public abstract class ZoomOut : MasterKeyword, IGameActionVerb { }
        public abstract class Collect : MasterKeyword, IGameActionVerb { }
        public abstract class Open    : MasterKeyword, IGameActionVerb { }
        public abstract class Upgrade : MasterKeyword, IGameActionVerb { }
        public abstract class Acquire : MasterKeyword, IGameActionVerb { }
        public abstract class Own     : MasterKeyword, IGameActionVerb { }
        public abstract class Reach   : MasterKeyword, IGameActionVerb { }
        public abstract class Place   : MasterKeyword, IGameActionVerb { }
    }
}
