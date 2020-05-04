namespace Mayfair.Core.Code.GameData.Cards
{
    using System;
    using Utils.Types;
    using Utils.Types.UniqueId;

    [Serializable]
    public class CardData
    {
        public UniqueId uniqueId;
        public int level;
        public int copiesCount;
    }
}