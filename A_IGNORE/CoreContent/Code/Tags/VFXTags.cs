namespace Mayfair.CoreContent.Code.Tags
{
    using Mayfair.Core.Code.TagSystem;

    public abstract partial class Tags
    {
        public abstract class VFX : MasterKeyword { }

        public abstract class SizeKeyword : MasterKeyword { }

        //Width tag in cell units
        public abstract class Width1 : SizeKeyword { }
        public abstract class Width2 : SizeKeyword { }
        public abstract class Width3 : SizeKeyword { }
        public abstract class Width4 : SizeKeyword { }
        public abstract class Width5 : SizeKeyword { }
        public abstract class Width6 : SizeKeyword { }
        public abstract class Width7 : SizeKeyword { }
        public abstract class Width8 : SizeKeyword { }
        public abstract class Width9 : SizeKeyword { }

        //Length tag in cell units
        public abstract class Length1 : SizeKeyword { }
        public abstract class Length2 : SizeKeyword { }
        public abstract class Length3 : SizeKeyword { }
        public abstract class Length4 : SizeKeyword { }
        public abstract class Length5 : SizeKeyword { }
        public abstract class Length6 : SizeKeyword { }
        public abstract class Length7 : SizeKeyword { }
        public abstract class Length8 : SizeKeyword { }
        public abstract class Length9 : SizeKeyword { }

        //Width tag in cell units
        public abstract class W1 : Width1 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W2 : Width2 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W3 : Width3 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W4 : Width4 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W5 : Width5 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W6 : Width6 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W7 : Width7 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W8 : Width8 { private const bool USE_PARENT_TYPE = true; }
        public abstract class W9 : Width9 { private const bool USE_PARENT_TYPE = true; }

        //Length tag in cell units
        public abstract class L1 : Length1 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L2 : Length2 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L3 : Length3 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L4 : Length4 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L5 : Length5 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L6 : Length6 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L7 : Length7 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L8 : Length8 { private const bool USE_PARENT_TYPE = true; }
        public abstract class L9 : Length9 { private const bool USE_PARENT_TYPE = true; }
    }
}