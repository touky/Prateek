namespace Prateek.Core.Code.Helpers
{
    public static class IntExtensions
    {
        #region Class Methods
        public static string ToHexString(this short value)
        {
            return $"{value:X}";
        }

        public static string ToHexString(this byte value)
        {
            return $"{value:X}";
        }

        public static string ToHexString(this int value)
        {
            return $"{value:X}";
        }

        public static string ToHexString(this long value)
        {
            return $"{value:X}";
        }

        public static string ToHexString(this ushort value)
        {
            return $"{value:X}";
        }

        public static string ToHexString(this uint value)
        {
            return $"{value:X}";
        }

        public static string ToHexString(this ulong value)
        {
            return $"{value:X}";
        }
        #endregion
    }
}
