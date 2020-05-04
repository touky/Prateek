namespace Mayfair.Core.Code.MathExt
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class RectHelper
    {
        #region Nested type: SplitHelper
        public class SplitHelper
        {
            #region Fields
            public Rect rect;
            public Vector2 size;
            #endregion

            #region implicit
            public static implicit operator SplitHelper(Vector2 v)
            {
                return new SplitHelper {size = v};
            }

            public static implicit operator SplitHelper(float v)
            {
                return new SplitHelper {size = Vector2.one * v};
            }
            #endregion
        }
        #endregion

        #region Split
        private static void Split(ref Rect srcRect, SplitHelper[] helpers, bool onlyX, bool onlyY)
        {
            int length = helpers != null ? helpers.Length : 0;
            Vector2 total = Vector2.zero;
            for (int h = 0; h < length; h++)
            {
                total += new Vector2(helpers[h].size.x, helpers[h].size.y);
            }

            Vector2 alpha = new Vector2(total.x <= 0 || onlyY ? -1 : srcRect.width / total.x,
                                        total.y <= 0 || onlyX ? -1 : srcRect.height / total.y);
            for (int h = 0; h < helpers.Length; h++)
            {
                helpers[h].rect = Truncate(ref srcRect,
                                           alpha.x < 0 ? -1 : alpha.x * helpers[h].size.x,
                                           alpha.y < 0 ? -1 : alpha.y * helpers[h].size.y);
            }
        }

        /// <summary>
        ///     <para>Split the given rect by the given width/heights</para>
        /// </summary>
        public static void Split(ref Rect srcRect, SplitHelper[] helpers)
        {
            Split(ref srcRect, helpers, false, false);
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the X-axis with the given pixel weights
        ///         A negative weight means the max size of the return rect will be abs(NegWeight)
        ///         Use SplitOptions.MaxSize() to take advantage of that feature
        ///     </para>
        /// </summary>
        public static void SplitX(ref Rect srcRect, params SplitHelper[] helpers)
        {
            Split(ref srcRect, helpers, true, false);
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the Y-axis with the given pixel weights
        ///         A negative weight means the max size of the return rect will be abs(NegWeight)
        ///         Use SplitOptions.MaxSize() to take advantage of that feature
        ///     </para>
        /// </summary>
        public static void SplitY(ref Rect srcRect, params SplitHelper[] helpers)
        {
            Split(ref srcRect, helpers, false, true);
        }

        /// <summary>
        ///     <para>Split the given rect by the given width/heights</para>
        /// </summary>
        public static Rect[] Split(ref Rect srcRect, float[] widths, float[] heights)
        {
            Vector2 alpha = new Vector2(ConvertWeightsIntoRealSizes(srcRect.width, ref widths),
                                        ConvertWeightsIntoRealSizes(srcRect.height, ref heights));
            int length = Mathf.Max(SafeLength(widths), SafeLength(heights));
            Rect[] result = new Rect[length];
            for (int r = 0; r < result.Length; r++)
            {
                result[r] = Truncate(ref srcRect,
                                     alpha.x < 0 ? -1 : Mathf.Abs(widths[r]),
                                     alpha.y < 0 ? -1 : Mathf.Abs(heights[r]));
            }

            return result;
        }

        /// <summary>
        ///     <para>Split the given rect by the given width/heights</para>
        /// </summary>
        public static Rect[] Split(ref Rect srcRect, List<float> widths, List<float> heights)
        {
            Vector2 alpha = new Vector2(ConvertWeightsIntoRealSizes(srcRect.width, widths),
                                        ConvertWeightsIntoRealSizes(srcRect.height, heights));
            int length = Mathf.Max(SafeLength(widths), SafeLength(heights));
            Rect[] result = new Rect[length];
            for (int r = 0; r < result.Length; r++)
            {
                result[r] = Truncate(ref srcRect,
                                     alpha.x < 0 ? -1 : Mathf.Abs(widths[r]),
                                     alpha.y < 0 ? -1 : Mathf.Abs(heights[r]));
            }

            return result;
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the X-axis with the given pixel weights
        ///         A negative weight means the max size of the return rect will be abs(NegWeight)
        ///         Use SplitOptions.MaxSize() to take advantage of that feature
        ///     </para>
        /// </summary>
        public static Rect[] SplitX(ref Rect srcRect, List<float> pixelWeights)
        {
            return Split(ref srcRect, pixelWeights, null);
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the Y-axis with the given pixel weights
        ///         A negative weight means the max size of the return rect will be abs(NegWeight)
        ///         Use SplitOptions.MaxSize() to take advantage of that feature
        ///     </para>
        /// </summary>
        public static Rect[] SplitY(ref Rect srcRect, List<float> pixelWeights)
        {
            return Split(ref srcRect, null, pixelWeights);
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the X-axis with the given pixel weights
        ///         A negative weight means the max size of the return rect will be abs(NegWeight)
        ///         Use SplitOptions.MaxSize() to take advantage of that feature
        ///     </para>
        /// </summary>
        public static Rect[] SplitX(ref Rect srcRect, params float[] pixelWeights)
        {
            return Split(ref srcRect, pixelWeights, null);
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the Y-axis with the given pixel weights
        ///         A negative weight means the max size of the return rect will be abs(NegWeight)
        ///         Use SplitOptions.MaxSize() to take advantage of that feature
        ///     </para>
        /// </summary>
        public static Rect[] SplitY(ref Rect srcRect, params float[] pixelWeights)
        {
            return Split(ref srcRect, null, pixelWeights);
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the X-axis with the given number of splits
        ///     </para>
        /// </summary>
        public static Rect[] SplitX(ref Rect srcRect, int count)
        {
            float[] sizes = new float[count];
            for (int s = 0; s < sizes.Length; s++)
            {
                sizes[s] = 1;
            }

            return Split(ref srcRect, sizes, null);
        }

        /// <summary>
        ///     <para>
        ///         Split the given rect by the axis y with the given number of splits
        ///     </para>
        /// </summary>
        public static Rect[] SplitY(ref Rect srcRect, int count)
        {
            float[] sizes = new float[count];
            for (int s = 0; s < sizes.Length; s++)
            {
                sizes[s] = 1;
            }

            return Split(ref srcRect, null, sizes);
        }

        private static int SafeLength<T>(List<T> array)
        {
            return array != null ? array.Count : 0;
        }

        private static int SafeLength<T>(T[] array)
        {
            return array != null ? array.Length : 0;
        }

        /// <summary>
        ///     <para>
        ///         Convert the given pixel weight in actual weigthed size depending on the given rectSize
        ///     </para>
        /// </summary>
        public static float ConvertWeightsIntoRealSizes(float rectSize, ref float[] pixelWeights)
        {
            if (pixelWeights == null)
            {
                return -1;
            }

            float total = 0f;
            for (int s = 0; s < pixelWeights.Length; s++)
            {
                total += Mathf.Abs(pixelWeights[s]);
            }

            if (total >= rectSize)
            {
                float alpha = rectSize / total;
                for (int s = 0; s < pixelWeights.Length; s++)
                {
                    pixelWeights[s] = Mathf.Abs(pixelWeights[s]) * alpha;
                }

                return alpha;
            }

            float rest = rectSize;
            float posTotal = 0f;
            for (int s = 0; s < pixelWeights.Length; s++)
            {
                float size = pixelWeights[s];
                if (size < 0)
                {
                    rest -= Mathf.Abs(size);
                }
                else
                {
                    posTotal += size;
                }
            }

            for (int s = 0; s < pixelWeights.Length; s++)
            {
                float size = pixelWeights[s];
                if (size < 0)
                {
                    pixelWeights[s] = Mathf.Abs(size);
                }
                else
                {
                    pixelWeights[s] = size / posTotal * rest;
                }
            }

            return 1;
        }

        /// <summary>
        ///     <para>
        ///         Convert the given pixel weight in actual weigthed size depending on the given rectSize
        ///     </para>
        /// </summary>
        public static float ConvertWeightsIntoRealSizes(float rectSize, List<float> pixelWeights)
        {
            if (pixelWeights == null)
            {
                return -1;
            }

            float total = 0f;
            for (int s = 0; s < pixelWeights.Count; s++)
            {
                total += Mathf.Abs(pixelWeights[s]);
            }

            if (total >= rectSize)
            {
                return rectSize / total;
            }

            float rest = rectSize;
            float posTotal = 0f;
            for (int s = 0; s < pixelWeights.Count; s++)
            {
                float size = pixelWeights[s];
                if (size < 0)
                {
                    rest -= Mathf.Abs(size);
                }
                else
                {
                    posTotal += size;
                }
            }

            for (int s = 0; s < pixelWeights.Count; s++)
            {
                float size = pixelWeights[s];
                if (size < 0)
                {
                    pixelWeights[s] = Mathf.Abs(size);
                }
                else
                {
                    pixelWeights[s] = size / posTotal * rest;
                }
            }

            return 1;
        }
        #endregion

        #region Inflate
        /// <summary>
        ///     <para>
        ///         Inflate rect by the given size on the X-axis
        ///     </para>
        /// </summary>
        public static void InflateX(ref Rect rect, float size)
        {
            Inflate(ref rect, new Vector2(size, 0));
        }

        /// <summary>
        ///     <para>
        ///         Inflate rect by the given size on the Y-axis
        ///     </para>
        /// </summary>
        public static void InflateY(ref Rect rect, float size)
        {
            Inflate(ref rect, new Vector2(0, size));
        }

        /// <summary>
        ///     <para>
        ///         Inflate rect by the given size
        ///     </para>
        /// </summary>
        public static void Inflate(ref Rect rect, float size)
        {
            Inflate(ref rect, new Vector2(size, size));
        }

        /// <summary>
        ///     <para>
        ///         Inflate rect by the given width/height
        ///     </para>
        /// </summary>
        public static void Inflate(ref Rect rect, float width, float height)
        {
            Inflate(ref rect, new Vector2(width, height));
        }

        /// <summary>
        ///     <para>
        ///         returns an Inflated rect by the given size on the X-axis
        ///     </para>
        /// </summary>
        public static Rect InflateX(Rect rect, float size)
        {
            Rect other = rect;
            Inflate(ref rect, new Vector2(size, 0));
            return other;
        }

        /// <summary>
        ///     <para>
        ///         returns an Inflated rect by the given size on the Y-axis
        ///     </para>
        /// </summary>
        public static Rect InflateY(Rect rect, float size)
        {
            Rect other = rect;
            Inflate(ref rect, new Vector2(0, size));
            return other;
        }

        /// <summary>
        ///     <para>
        ///         returns an Inflated rect by the given size
        ///     </para>
        /// </summary>
        public static Rect Inflate(Rect rect, float size)
        {
            Rect other = rect;
            Inflate(ref other, new Vector2(size, size));
            return other;
        }

        /// <summary>
        ///     <para>
        ///         returns an Inflated rect by the given width/height
        ///     </para>
        /// </summary>
        public static Rect Inflate(Rect rect, float width, float height)
        {
            Rect other = rect;
            Inflate(ref other, new Vector2(width, height));
            return other;
        }

        /// <summary>
        ///     <para>
        ///         returns an Inflated rect by the given size
        ///     </para>
        /// </summary>
        public static void Inflate(ref Rect rect, Vector2 size)
        {
            rect = new Rect(
                (Mathf.Abs(rect.position.x) - size.x) * Mathf.Sign(rect.position.x),
                (Mathf.Abs(rect.position.y) - size.y) * Mathf.Sign(rect.position.y),
                (Mathf.Abs(rect.size.x) + size.x * 2) * Mathf.Sign(rect.size.x),
                (Mathf.Abs(rect.size.y) + size.y * 2) * Mathf.Sign(rect.size.y));
        }
        #endregion

        #region Truncate
        /// <summary>
        ///     <para>
        ///         Truncate from the left the given rect by the given width/height
        ///         <returns>The removed rect</returns>
        ///     </para>
        /// </summary>
        public static Rect Truncate(ref Rect srcRect, float width, float height)
        {
            float wResult = width < 0 ? srcRect.width : Mathf.Clamp(width, 0, srcRect.width);
            float hResult = height < 0 ? srcRect.height : Mathf.Clamp(height, 0, srcRect.height);

            Rect result = new Rect(srcRect.position, new Vector2(wResult, hResult));
            Vector2 offset = new Vector2(width < 0 ? 0 : result.width, height < 0 ? 0 : result.height);

            srcRect.size -= offset;
            srcRect.position += offset;
            return result;
        }

        /// <summary>
        ///     <para>
        ///         Truncate from the left the given rect on the X-axis by the given width
        ///         <returns>The removed rect</returns>
        ///     </para>
        /// </summary>
        public static Rect TruncateX(ref Rect srcRect, float width)
        {
            return Truncate(ref srcRect, width, -1);
        }

        /// <summary>
        ///     <para>
        ///         Truncate from the left the given rect on the Y-axis by the given width
        ///         <returns>The removed rect</returns>
        ///     </para>
        /// </summary>
        public static Rect TruncateY(ref Rect srcRect, float height)
        {
            return Truncate(ref srcRect, -1, height);
        }

        /// <summary>
        ///     <para>
        ///         Trim the given rect by the given width/height
        ///     </para>
        /// </summary>
        public static void Trim(ref Rect rect, float width, float height)
        {
            rect.width = Mathf.Max(0, rect.width - width);
            rect.height = Mathf.Max(0, rect.height - height);
        }
        #endregion
    }
}
