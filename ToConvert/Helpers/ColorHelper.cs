namespace Assets.Prateek.ToConvert.Helpers
{
    using System;
    using UnityEngine;

    public static class ColorHelper
    {
        #region Static and Constants
        private static int PI2_IN_DEGREE = 360;
        #endregion

        #region Class Methods
        /// <summary>
        ///     Convert the given degree angle into a hue value usable in an HSV color
        ///     Then use it to create and return said color
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static Color DegreeToColor(float degree)
        {
            return Color.HSVToRGB(DegreeToHue(degree), 1, 1);
        }

        /// <summary>
        ///     Convert the given degree angle into a hue value usable in an HSV color
        ///     Hue == angle in degree / 360f
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static float DegreeToHue(float degree)
        {
            float mod = PI2_IN_DEGREE;
            var   add = mod * 8f;
            return (degree + add) % mod / mod;
        }

        public static float TypeToHue(Type type, byte offset = 1)
        {
            return HashCodeToHue(type.GetHashCode(), offset);
        }

        public static Color TypeToColor(Type type, byte offset = 1)
        {
            return Color.HSVToRGB(HashCodeToHue(type.GetHashCode(), offset), 1, 1);
        }

        public static float HashCodeToHue(int hashCode, byte offset = 1)
        {
            long longHash = hashCode;
            longHash = longHash * offset % PI2_IN_DEGREE;
            return DegreeToHue(longHash);
        }

        public static Color HashCodeToColor(int hashCode, byte offset = 1)
        {
            return Color.HSVToRGB(HashCodeToHue(hashCode, offset), 1, 1);
        }

        public static Color Alpha(this Color color, float alpha)
        {
            var c = color;
            c.a = alpha;
            return c;
        }

        public static Color Lighten(this Color color, float alpha)
        {
            return new Color(color.r * alpha, color.g * alpha, color.b * alpha, color.a);
        }
        #endregion

        #region Nested colors
        //This does not follow code standard on purpose, it follows Color struct logic
        public struct editor
        {
            public static readonly Color backgroundDark = new Color(20f / 255f, 20f / 255f, 20f / 255f);
            public static readonly Color background = new Color(56f / 255f, 56f / 255f, 56f / 255f);
            public static readonly Color text = new Color(180f / 255f, 180f / 255f, 180f / 255f);
        }

        public struct red
        {
            /// <summary>
            ///     <para>Pastel. RGB is (1, .412f, .38f).</para>
            /// </summary>
            public static readonly Color pastel = new Color(1, .412f, .38f);

            /// <summary>
            ///     <para>Pastel. RGB is Pastel * 0.5f.</para>
            /// </summary>
            public static readonly Color pastelDark = new Color(1 * 0.5f, .412f * 0.5f, .38f * 0.5f);

            /// <summary>
            ///     <para>Custom Dark. RGB is (percent, 0, 0).</para>
            /// </summary>
            public static Color ToDark(float percent)
            {
                return new Color(percent, 0, 0);
            }

            /// <summary>
            ///     <para>Custom Light. RGB is (red, percent, percent).</para>
            /// </summary>
            public static Color ToLight(float percent, float red = 1)
            {
                return new Color(red, percent, percent);
            }

            /// <summary>
            ///     <para>Dark 25%. RGB is (0.25f, 0, 0).</para>
            /// </summary>
            public static readonly Color dark25 = new Color(0.25f, 0, 0);

            /// <summary>
            ///     <para>Dark 50%. RGB is (0.50f, 0, 0).</para>
            /// </summary>
            public static readonly Color dark50 = new Color(0.50f, 0, 0);

            /// <summary>
            ///     <para>Dark 75%. RGB is (0.75f, 0, 0).</para>
            /// </summary>
            public static readonly Color dark75 = new Color(0.75f, 0, 0);

            /// <summary>
            ///     <para>Light 25%. RGB is (1, 0.25f, 0.25f).</para>
            /// </summary>
            public static readonly Color light25 = new Color(1, 0.25f, 0.25f);

            /// <summary>
            ///     <para>Light 50%. RGB is (1, 0.50f, 0.50f).</para>
            /// </summary>
            public static readonly Color light50 = new Color(1, 0.50f, 0.50f);

            /// <summary>
            ///     <para>Light 75%. RGB is (1, 0.75f, 0.75f).</para>
            /// </summary>
            public static readonly Color light75 = new Color(1, 0.75f, 0.75f);
        }

        public struct green
        {
            /// <summary>
            ///     <para>Pastel. RGB is (1, .412f, .38f).</para>
            /// </summary>
            public static readonly Color pastel = new Color(.467f, .867f, .467f);

            /// <summary>
            ///     <para>Pastel. RGB is Pastel * 0.5f.</para>
            /// </summary>
            public static readonly Color pastelDark = new Color(.467f * 0.5f, .867f * 0.5f, .467f * 0.5f);

            /// <summary>
            ///     <para>Custom Dark. RGB is (0, percent, 0).</para>
            /// </summary>
            public static Color ToDark(float percent)
            {
                return new Color(0, percent, 0);
            }

            /// <summary>
            ///     <para>Custom Light. RGB is (percent, green, percent).</para>
            /// </summary>
            public static Color ToLight(float percent, float green = 1)
            {
                return new Color(percent, green, percent);
            }

            /// <summary>
            ///     <para>Dark 25%. RGB is (0, 0.25f, 0).</para>
            /// </summary>
            public static readonly Color dark25 = new Color(0, 0.25f, 0);

            /// <summary>
            ///     <para>Dark 50%. RGB is (0, 0.50f, 0).</para>
            /// </summary>
            public static readonly Color dark50 = new Color(0, 0.50f, 0);

            /// <summary>
            ///     <para>Dark 75%. RGB is (0, 0.75f, 0).</para>
            /// </summary>
            public static readonly Color dark75 = new Color(0, 0.75f, 0);

            /// <summary>
            ///     <para>Light 25%. RGB is (0.25f, 1, 0.25f).</para>
            /// </summary>
            public static readonly Color light25 = new Color(0.25f, 1, 0.25f);

            /// <summary>
            ///     <para>Light 50%. RGB is (0.50f, 1, 0.50f).</para>
            /// </summary>
            public static readonly Color light50 = new Color(0.50f, 1, 0.50f);

            /// <summary>
            ///     <para>Light 75%. RGB is (0.75f, 1, 0.75f).</para>
            /// </summary>
            public static readonly Color light75 = new Color(0.75f, 1, 0.75f);
        }

        public struct blue
        {
            /// <summary>
            ///     <para>Pastel. RGB is (1, .412f, .38f).</para>
            /// </summary>
            public static readonly Color pastel = new Color(.682f, .776f, .812f);

            /// <summary>
            ///     <para>Pastel. RGB is Pastel * 0.5f.</para>
            /// </summary>
            public static readonly Color pastelDark = new Color(.682f * 0.5f, .776f * 0.5f, .812f * 0.5f);

            /// <summary>
            ///     <para>Custom Dark. RGB is (0, 0, percent).</para>
            /// </summary>
            public static Color ToDark(float percent)
            {
                return new Color(0, 0, percent);
            }

            /// <summary>
            ///     <para>Custom Light. RGB is (percent, percent, blue).</para>
            /// </summary>
            public static Color ToLight(float percent, float blue = 1)
            {
                return new Color(percent, percent, blue);
            }

            /// <summary>
            ///     <para>Dark 25%. RGB is (0, 0, 0.25f).</para>
            /// </summary>
            public static readonly Color dark25 = new Color(0, 0, 0.25f);

            /// <summary>
            ///     <para>Dark 50%. RGB is (0, 0, 0.50f).</para>
            /// </summary>
            public static readonly Color dark50 = new Color(0, 0, 0.50f);

            /// <summary>
            ///     <para>Dark 75%. RGB is (0, 0, 0.75f).</para>
            /// </summary>
            public static readonly Color dark75 = new Color(0, 0, 0.75f);

            /// <summary>
            ///     <para>Light 25%. RGB is (0.25f, 0.25f, 1).</para>
            /// </summary>
            public static readonly Color light25 = new Color(0.25f, 0.25f, 1);

            /// <summary>
            ///     <para>Light 50%. RGB is (0.50f, 0.50f, 1).</para>
            /// </summary>
            public static readonly Color light50 = new Color(0.50f, 0.50f, 1);

            /// <summary>
            ///     <para>Light 75%. RGB is (0.75f, 0.75f, 1).</para>
            /// </summary>
            public static readonly Color light75 = new Color(0.75f, 0.75f, 1);
        }

        public struct grey
        {
            /// <summary>
            ///     <para>Custom Dark. RGB is (0, 0, percent).</para>
            /// </summary>
            public static Color ToDark(float percent)
            {
                return new Color(percent, percent, percent);
            }

            /// <summary>
            ///     <para>Dark 25%. RGB is (0, 0, 0.25f).</para>
            /// </summary>
            public static readonly Color dark25 = new Color(0.25f, 0.25f, 0.25f);

            /// <summary>
            ///     <para>Equals Color.grey.</para>
            /// </summary>
            public static readonly Color dark50 = new Color(0.50f, 0.50f, 0.50f);

            /// <summary>
            ///     <para>Dark 75%. RGB is (0, 0, 0.75f).</para>
            /// </summary>
            public static readonly Color dark75 = new Color(0.75f, 0.75f, 0.75f);
        }

        public struct cyan
        {
            /// <summary>
            ///     <para>Custom Dark. RGB is (0, percent, percent).</para>
            /// </summary>
            public static Color ToDark(float percent)
            {
                return new Color(0, percent, percent);
            }

            /// <summary>
            ///     <para>Custom Light. RGB is (percent, 1, 1).</para>
            /// </summary>
            public static Color ToLight(float percent)
            {
                return new Color(percent, 1, 1);
            }

            /// <summary>
            ///     <para>Dark 25%. RGB is (0, 0.25f, 0.25f).</para>
            /// </summary>
            public static readonly Color dark25 = new Color(0, 0.25f, 0.25f);

            /// <summary>
            ///     <para>Dark 50%. RGB is (0, 0.50f, 0.50f).</para>
            /// </summary>
            public static readonly Color dark50 = new Color(0, 0.50f, 0.50f);

            /// <summary>
            ///     <para>Dark 75%. RGB is (0, 0.75f, 0.75f).</para>
            /// </summary>
            public static readonly Color dark75 = new Color(0, 0.75f, 0.75f);

            /// <summary>
            ///     <para>Light 25%. RGB is (0.25f, 1, 1).</para>
            /// </summary>
            public static readonly Color light25 = new Color(0.25f, 1, 1);

            /// <summary>
            ///     <para>Light 50%. RGB is (0.50f, 1, 1).</para>
            /// </summary>
            public static readonly Color light50 = new Color(0.50f, 1, 1);

            /// <summary>
            ///     <para>Light 75%. RGB is (0.75f, 1, 1).</para>
            /// </summary>
            public static readonly Color light75 = new Color(0.75f, 1, 1);
        }

        public struct magenta
        {
            /// <summary>
            ///     <para>Custom Dark. RGB is (percent, 0, percent).</para>
            /// </summary>
            public static Color ToDark(float percent)
            {
                return new Color(percent, 0, percent);
            }

            /// <summary>
            ///     <para>Custom Light. RGB is (1, percent, 1).</para>
            /// </summary>
            public static Color ToLight(float percent)
            {
                return new Color(1, percent, 1);
            }

            /// <summary>
            ///     <para>Dark 25%. RGB is (0.25f, 0, 0.25f).</para>
            /// </summary>
            public static readonly Color dark25 = new Color(0.25f, 0, 0.25f);

            /// <summary>
            ///     <para>Dark 50%. RGB is (0.50f, 0, 0.50f).</para>
            /// </summary>
            public static readonly Color dark50 = new Color(0.50f, 0, 0.50f);

            /// <summary>
            ///     <para>Dark 75%. RGB is (0.75f, 0, 0.75f).</para>
            /// </summary>
            public static readonly Color dark75 = new Color(0.75f, 0, 0.75f);

            /// <summary>
            ///     <para>Light 25%. RGB is (1, 0.25f, 1).</para>
            /// </summary>
            public static readonly Color light25 = new Color(1, 0.25f, 1);

            /// <summary>
            ///     <para>Light 50%. RGB is (1, 0.50f, 1).</para>
            /// </summary>
            public static readonly Color light50 = new Color(1, 0.50f, 1);

            /// <summary>
            ///     <para>Light 75%. RGB is (1, 0.75f, 1).</para>
            /// </summary>
            public static readonly Color light75 = new Color(1, 0.75f, 1);
        }

        public struct yellow
        {
            /// <summary>
            ///     <para>Custom Dark. RGB is (percent, percent, 0).</para>
            /// </summary>
            public static Color ToDark(float percent)
            {
                return new Color(percent, 0.92f * percent, 0);
            }

            /// <summary>
            ///     <para>Custom Light. RGB is (1, 1, percent).</para>
            /// </summary>
            public static Color ToLight(float percent)
            {
                return new Color(1, 0.92f, percent);
            }

            /// <summary>
            ///     <para>Dark 25%. RGB is (0.25f, 0.25f, 0).</para>
            /// </summary>
            public static readonly Color dark25 = new Color(0.25f, 0.92f * 0.25f, 0);

            /// <summary>
            ///     <para>Dark 50%. RGB is (0.50f, 0.50f, 0).</para>
            /// </summary>
            public static readonly Color dark50 = new Color(0.50f, 0.92f * 0.50f, 0);

            /// <summary>
            ///     <para>Dark 75%. RGB is (0.75f, 0.75f, 0).</para>
            /// </summary>
            public static readonly Color dark75 = new Color(0.75f, 0.92f * 0.75f, 0);

            /// <summary>
            ///     <para>Light 25%. RGB is (1, 0.92f, 0.25f).</para>
            /// </summary>
            public static readonly Color light25 = new Color(1, 0.92f, 0.25f);

            /// <summary>
            ///     <para>Light 50%. RGB is (1, 0.92f, 0.50f).</para>
            /// </summary>
            public static readonly Color light50 = new Color(1, 0.92f, 0.50f);

            /// <summary>
            ///     <para>Light 75%. RGB is (1, 0.92f, 0.75f).</para>
            /// </summary>
            public static readonly Color light75 = new Color(1, 0.92f, 0.75f);
        }
        #endregion
    }
}
