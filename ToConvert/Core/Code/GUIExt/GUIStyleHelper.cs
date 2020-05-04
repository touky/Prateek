namespace Mayfair.Core.Code.GUIExt
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class GUIStyleHelper
    {
        #region BuiltinBackground enum
        public enum BuiltinBackground
        {
            GradientHue,
            GradientSaturation,
            GradientValue,

            MAX
        }
        #endregion

        #region Static and Constants
        private static readonly Storage storage = new Storage();
        #endregion

        #region Class Methods
        private static GUIStyle Get(string key)
        {
            GUIStyle style = null;
            if (storage.Styles.TryGetValue(key, out style))
            {
                return style;
            }

            return null;
        }

        private static void Store(string key, GUIStyle style)
        {
            if (storage.Styles.ContainsKey(key))
            {
                storage.Styles[key] = style;
            }
            else
            {
                storage.Styles.Add(key, style);
            }
        }

        /// <summary>
        ///     Creates a style from the built-in background enum
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="textColor">The text color</param>
        /// <param name="builtinBG">The built-in background behaviour</param>
        /// <param name="width">The width of the texture created for the style</param>
        /// <returns></returns>
        public static GUIStyle CreateStyle(string key, Color textColor, BuiltinBackground builtinBG, int width = 512)
        {
            GUIStyle style = Get(key);
            if (style != null)
            {
                return style;
            }

            style = CreateStyleWithBuiltinTexture(textColor, builtinBG, width);
            style.name = key;

            Store(key, style);
            return style;
        }

        /// <summary>
        ///     Creates a style from another style
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="other">The other style used as source</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, GUIStyle other)
        {
            GUIStyle style = Get(key);
            if (style != null)
            {
                return style;
            }

            style = new GUIStyle(other);
            style.name = key;

            Store(key, style);
            return style;
        }

        /// <summary>
        ///     Creates a style from another style
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="other">The other style used as source</param>
        /// <param name="textColor">The text color</param>
        /// <param name="backgroundColor">The center color</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, GUIStyle other, Color textColor, Color backgroundColor)
        {
            BoxStyle boxStyle = BoxStyle.Default;

            boxStyle.color = backgroundColor;
            return CreateStyle(key, other, textColor, boxStyle, BorderStyle.Default);
        }

        /// <summary>
        ///     Creates a style from another style
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="other">The other style used as source</param>
        /// <param name="textColor">The text color</param>
        /// <param name="backgroundColor">The center color</param>
        /// <param name="borderColor">The border color</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, GUIStyle other, Color textColor, Color backgroundColor, Color borderColor)
        {
            BoxStyle boxStyle = BoxStyle.Default;
            BorderStyle borderStyle = BorderStyle.Default;

            boxStyle.color = backgroundColor;
            boxStyle.size = Vector2.one * 3;
            borderStyle.color = borderColor;
            borderStyle.size = 1;
            return CreateStyle(key, other, textColor, boxStyle, borderStyle);
        }

        /// <summary>
        ///     Creates a style from another style with a texture of [bx * 2 + 1, by * 2 + 1] size
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="other">The other style used as source</param>
        /// <param name="textColor">The text color</param>
        /// <param name="boxStyle">The center box style</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, GUIStyle other, Color textColor, BoxStyle boxStyle)
        {
            return CreateStyle(key, other, textColor, boxStyle, BorderStyle.Default);
        }

        /// <summary>
        ///     Creates a style from another style with a texture of [bx * 2 + 1, by * 2 + 1] size
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="other">The other style used as source</param>
        /// <param name="textColor">The text color</param>
        /// <param name="boxStyle">The center box style</param>
        /// <param name="borderStyle">The border style</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, GUIStyle other, Color textColor, BoxStyle boxStyle, BorderStyle borderStyle)
        {
            GUIStyle style = Get(key);
            if (style != null)
            {
                return style;
            }

            style = new GUIStyle(other);
            style.name = key;

            SdfBox sdfBox = new SdfBox(boxStyle, borderStyle);

            style.normal.background = CreateTexture(sdfBox);
            style.border = sdfBox.RectOffset;
            style.padding = sdfBox.Padding;

            Store(key, style);
            return style;
        }

        /// <summary>
        ///     Creates a style with a texture of [bx * 2 + 1, by * 2 + 1] size
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="textColor">The text color</param>
        /// <param name="backgroundColor">The center color</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, Color textColor, Color backgroundColor)
        {
            BoxStyle boxStyle = BoxStyle.Default;

            boxStyle.color = backgroundColor;
            return CreateStyle(key, textColor, boxStyle, BorderStyle.Default);
        }

        /// <summary>
        ///     Creates a style with a texture of [bx * 2 + 1, by * 2 + 1] size
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="textColor">The text color</param>
        /// <param name="backgroundColor">The center color</param>
        /// <param name="borderColor">The border color</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, Color textColor, Color backgroundColor, Color borderColor)
        {
            BoxStyle boxStyle = BoxStyle.Default;
            BorderStyle borderStyle = BorderStyle.Default;

            boxStyle.color = backgroundColor;
            boxStyle.size = Vector2.one * 3;
            borderStyle.color = borderColor;
            borderStyle.size = 1;
            return CreateStyle(key, textColor, boxStyle, borderStyle);
        }

        /// <summary>
        ///     Creates a style with a texture of [bx * 2 + 1, by * 2 + 1] size
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="textColor">The text color</param>
        /// <param name="boxStyle">The center box style</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, Color textColor, BoxStyle boxStyle)
        {
            return CreateStyle(key, textColor, boxStyle, BorderStyle.Default);
        }

        /// <summary>
        ///     Creates a style with a texture of [bx * 2 + 1, by * 2 + 1] size
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="textColor">The text color</param>
        /// <param name="boxStyle">The center box style</param>
        /// <param name="borderStyle">The border style</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, Color textColor, BoxStyle boxStyle, BorderStyle borderStyle)
        {
            GUIStyle style = Get(key);
            if (style != null && style.normal.background != null)
            {
                return style;
            }

            style = CreateStyleWithTexture(textColor, boxStyle, borderStyle);
            style.name = key;

            Store(key, style);
            return style;
        }

        /// <summary>
        ///     Creates a style with this text color
        ///     Stores it in the given key slot
        /// </summary>
        /// <param name="key">The key to store the style as</param>
        /// <param name="textColor">The text color</param>
        /// <returns>The created or already existing style</returns>
        public static GUIStyle CreateStyle(string key, Color textColor)
        {
            GUIStyle style = Get(key);
            if (style != null)
            {
                return style;
            }

            style = CreateStyle(textColor, SdfBox.Default);
            style.name = key;

            Store(key, style);
            return style;
        }

        /// <summary>
        ///     Create a texture with the given parameters
        /// </summary>
        /// <param name="boxStyle"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(BoxStyle boxStyle)
        {
            return CreateTexture(new SdfBox(boxStyle, BorderStyle.Default));
        }

        /// <summary>
        ///     Create a texture with the given parameters
        /// </summary>
        /// <param name="boxStyle"></param>
        /// <param name="borderStyle"></param>
        /// <returns></returns>
        public static Texture2D CreateTexture(BoxStyle boxStyle, BorderStyle borderStyle)
        {
            return CreateTexture(new SdfBox(boxStyle, borderStyle));
        }
        #endregion

        #region Nested type: BorderStyle
        public struct BorderStyle
        {
            public Color color;
            public float? size;
            public float? bevel;

            public static readonly BorderStyle Default = new BorderStyle(Color.clear, null);

            public BorderStyle(Color color, float? size) : this(color, size, null) { }

            public BorderStyle(Color color, float? size, float? bevel)
            {
                this.color = color;
                this.size = size;
                this.bevel = bevel;
            }
        }
        #endregion

        #region Nested type: BoxStyle
        public struct BoxStyle
        {
            public Color color;
            public Vector2 size;
            public float? roundness;
            public float? bevel;

            public static readonly BoxStyle Default = new BoxStyle(Color.clear, 1);

            public BoxStyle(Color color, float size) : this(color, Vector2.one * size, null, null) { }
            public BoxStyle(Color color, float size, float? roundness) : this(color, Vector2.one * size, roundness, null) { }
            public BoxStyle(Color color, float size, float? roundness, float? bevel) : this(color, Vector2.one * size, roundness, bevel) { }

            public BoxStyle(Color color, Vector2 size, float? roundness, float? bevel)
            {
                this.color = color;
                this.size = size;
                this.roundness = roundness;
                this.bevel = bevel;
            }
        }
        #endregion

        #region Nested type: SdfBox
        public struct SdfBox
        {
            private const float PIXEL_PADDING = 1;
            private const float TEXT_PADDING = 2;
            public BoxStyle box;
            public BorderStyle border;

            public static readonly SdfBox Default = new SdfBox(BoxStyle.Default);

            private float Bevel
            {
                get { return box.bevel.HasValue ? box.bevel.Value : 0f; }
            }

            private float Roundness
            {
                get { return box.roundness.HasValue ? box.roundness.Value : 0f; }
            }

            private float Border
            {
                get
                {
                    return (border.size.HasValue ? border.size.Value : 0f)
                         + (border.bevel.HasValue ? border.bevel.Value : 0f);
                }
            }

            public Vector2 RenderSize
            {
                get
                {
                    float bevel = Bevel;
                    float roundness = Roundness;
                    float border = Mathf.Ceil(Border);
                    return (box.size + Vector2.one * (PIXEL_PADDING + Mathf.Max(0, Mathf.Max(border, bevel)))) * 2;
                }
            }

            public Vector2 Center
            {
                get { return RenderSize / 2f; }
            }

            public RectOffset RectOffset
            {
                get
                {
                    float bevel = Bevel;
                    float roundness = Roundness;
                    float border = Border;
                    int o = (int) TEXT_PADDING + (int) (PIXEL_PADDING + Mathf.Max(border * 2, border + bevel + roundness));
                    return new RectOffset(o, o, o, o);
                }
            }

            public RectOffset Padding
            {
                get
                {
                    float bevel = Bevel;
                    float roundness = Roundness;
                    float border = Border;
                    int o = (int) TEXT_PADDING + (int) (PIXEL_PADDING + Mathf.Max(border * 2, border + bevel + roundness / 2));
                    return new RectOffset(o, o, o, o);
                }
            }

            public SdfBox(BoxStyle box)
            {
                this.box = box;
                border = BorderStyle.Default;
            }

            public SdfBox(BoxStyle box, BorderStyle border)
            {
                this.box = box;
                this.border = border;
            }

            private static float BoxAlpha(Vector2 p, Vector2 b)
            {
                Vector2 d = new Vector2(Mathf.Abs(p.x), Mathf.Abs(p.y)) - b;
                Vector2 m = new Vector2(Mathf.Max(d.x, 0), Mathf.Max(d.y, 0));
                return m.magnitude + Mathf.Min(Mathf.Max(d.x, d.y), 0.0f);
            }

            public float BoxAlpha(Vector2 point)
            {
                Vector2 b = box.size;
                if (box.roundness.HasValue)
                {
                    b -= Vector2.one * box.roundness.Value;
                }

                float d = BoxAlpha(point, b);

                if (box.roundness.HasValue)
                {
                    d -= box.roundness.Value;
                }

                if (box.bevel.HasValue)
                {
                    d = Mathf.Min(d, box.bevel.Value) / (box.bevel.Value == 0 ? 1f : box.bevel.Value);
                }
                else
                {
                    d = Mathf.Ceil(Mathf.Min(1, d));
                }

                return d;
            }

            public float BorderAlpha(Vector2 point)
            {
                if (!border.size.HasValue)
                {
                    return 1;
                }

                Vector2 b = box.size;
                if (box.roundness.HasValue)
                {
                    b -= Vector2.one * box.roundness.Value;
                }

                float d = BoxAlpha(point, b);

                if (box.roundness.HasValue)
                {
                    d -= box.roundness.Value;
                }

                if (border.size.HasValue)
                {
                    d = Mathf.Abs(d) - border.size.Value;

                    if (border.bevel.HasValue && border.bevel.Value > 0)
                    {
                        d = Mathf.Clamp(d, 0, border.bevel.Value) / border.bevel.Value;
                    }
                    else
                    {
                        d = Mathf.Ceil(Mathf.Clamp01(d));
                    }
                }

                return d;
            }
        }
        #endregion

        #region Nested type: Storage
        [Serializable]
        private class Storage : ISerializationCallbackReceiver
        {
            #region Settings
            [SerializeField]
            private List<StyleSave> styleSaves = new List<StyleSave>();
            #endregion

            #region Properties
            public Dictionary<string, GUIStyle> Styles { get; private set; } = new Dictionary<string, GUIStyle>();
            #endregion

            #region Nested type: StyleSave
            [Serializable]
            private struct StyleSave
            {
                public string key;
                public GUIStyle value;
                public Texture2D normalBackground;
                public Texture2D onNormalBackground;
                public Texture2D hoverBackground;
                public Texture2D onHoverBackground;
                public Texture2D activeBackground;
                public Texture2D onActiveBackground;
                public Texture2D focusedBackground;
                public Texture2D onFocusedBackground;
            }
            #endregion

            #region ISerializationCallbackReceiver
            public void OnBeforeSerialize()
            {
                styleSaves.Clear();

                foreach (KeyValuePair<string, GUIStyle> kvp in Styles)
                {
                    styleSaves.Add(new StyleSave
                    {
                        key = kvp.Key,
                        value = kvp.Value,
                        normalBackground = kvp.Value.normal.background,
                        onNormalBackground = kvp.Value.onNormal.background,
                        hoverBackground = kvp.Value.hover.background,
                        onHoverBackground = kvp.Value.onHover.background,
                        activeBackground = kvp.Value.active.background,
                        onActiveBackground = kvp.Value.onActive.background,
                        focusedBackground = kvp.Value.focused.background,
                        onFocusedBackground = kvp.Value.onFocused.background
                    });
                }
            }

            public void OnAfterDeserialize()
            {
                Styles = new Dictionary<string, GUIStyle>();

                for (int i = 0; i < styleSaves.Count; i++)
                {
                    StyleSave styleSave = styleSaves[i];
                    styleSave.value.normal.background = styleSave.normalBackground;
                    styleSave.value.onNormal.background = styleSave.onNormalBackground;
                    styleSave.value.hover.background = styleSave.hoverBackground;
                    styleSave.value.onHover.background = styleSave.onHoverBackground;
                    styleSave.value.active.background = styleSave.activeBackground;
                    styleSave.value.onActive.background = styleSave.onActiveBackground;
                    styleSave.value.focused.background = styleSave.focusedBackground;
                    styleSave.value.onFocused.background = styleSave.onFocusedBackground;
                    Styles.Add(styleSave.key, styleSave.value);
                }
            }
            #endregion ISerializationCallbackReceiver
        }
        #endregion

        #region Internals
        private static GUIStyle CreateStyleWithBuiltinTexture(Color textColor, BuiltinBackground builtinBG, int width = 512)
        {
            Texture2D texture = new Texture2D(width, 1);
            texture.filterMode = FilterMode.Bilinear;
            Color[] pixels = texture.GetPixels();
            for (int x = 0; x < width; x++)
            {
                switch (builtinBG)
                {
                    case BuiltinBackground.GradientHue:
                    {
                        pixels[x] = Color.HSVToRGB(x / (float) (width - 1), 1, 1);
                        break;
                    }
                    case BuiltinBackground.GradientSaturation:
                    {
                        pixels[x] = Color.Lerp(new Color(1, 1, 1, 0), Color.white, x / (float) (width - 1));
                        break;
                    }
                    case BuiltinBackground.GradientValue:
                    {
                        pixels[x] = Color.HSVToRGB(0, 0, x / (float) (width - 1));
                        break;
                    }
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            GUIStyle style = CreateStyle(textColor, SdfBox.Default);
            style.normal.background = texture;

            return style;
        }

        private static GUIStyle CreateStyleWithTexture(Color textColor, BoxStyle boxStyle, BorderStyle borderStyle)
        {
            SdfBox sdfBox = new SdfBox(boxStyle, borderStyle);

            Texture2D texture = CreateTexture(sdfBox);
            GUIStyle style = CreateStyle(textColor, sdfBox);

            style.normal.background = texture;

            return style;
        }

        private static GUIStyle CreateStyle(Color textColor, SdfBox sdfBox)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = textColor;
            style.border = sdfBox.RectOffset;
            style.alignment = TextAnchor.MiddleCenter;
            style.padding = sdfBox.Padding;

            return style;
        }

        private static Texture2D CreateTexture(SdfBox sdfBox)
        {
            Vector2 size = sdfBox.RenderSize;

            Texture2D texture = new Texture2D(Mathf.CeilToInt(size.x), Mathf.CeilToInt(size.y));
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            Vector2 center = sdfBox.Center;
            BoxStyle box = sdfBox.box;

            Color[] pixels = texture.GetPixels();
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    Vector2 p = new Vector2(x + 0.5f, y + 0.5f) - center;
                    float aBox = Mathf.Clamp01(sdfBox.BoxAlpha(p));
                    float aBorder = sdfBox.BorderAlpha(p);

                    Color boxColor = Color.Lerp(sdfBox.box.color, Color.clear, aBox);
                    if (sdfBox.border.size != null)
                    {
                        boxColor = Color.Lerp(sdfBox.border.color, boxColor, aBorder);
                    }

                    pixels[y * (int) size.x + x] = boxColor;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
        #endregion
    }
}
