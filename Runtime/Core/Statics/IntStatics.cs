namespace Prateek.Runtime.Core.Extensions
{
    public static class IntExtensions
    {
        #region Class Methods
        public static string ToHex(this short value)
        {
            return $"{value:X4}";
        }

        public static string ToHex(this byte value)
        {
            return $"{value:X2}";
        }

        public static string ToHex(this int value)
        {
            return $"{value:X8}";
        }

        public static string ToHex(this long value)
        {
            return $"{value:X16}";
        }

        public static string ToHex(this ushort value)
        {
            return $"{value:X4}";
        }

        public static string ToHex(this uint value)
        {
            return $"{value:X8}";
        }

        public static string ToHex(this ulong value)
        {
            return $"{value:X16}";
        }
        #endregion
    }
}
