namespace Assets.Prateek.ToConvert.DebugMenu
{
    using System.Collections.Generic;
    using UnityEngine;

    internal class DebugMenuStyle
    {
        #region Static and Constants
        public const int SLIDER_BG = 0;
        public const int SLIDER_THUMB = 1;
        public const int LABEL_LEFT = 0;
        public const int LABEL_CENTER = 1;
        public const int LABEL_RIGHT = 2;
        public const int LABEL_COUNT = 3;
        public const int TITLE_COUNT = 3;
        public const int PAGE_INDEX = 2;
        public const int CATEGORY_COUNT = 8;

        private static readonly TextSettings EditorTextSettings = new TextSettings
        {
            fontSize = 15, linePadding = 15, validLineCount = 40
        };

        private static readonly TextSettings MobileTextSettings = new TextSettings
        {
            fontSize = 10, linePadding = 20, validLineCount = 15
        };

        private static TextSettings Settings = EditorTextSettings;
        #endregion

        #region Fields
        private float lineHeight = 80;
        private Font font;
        private readonly List<GUIStyleProxy> proxies = new List<GUIStyleProxy>();
        public GUIStyle background;
        public GUIStyle close;
        public GUIStyle extend;
        public GUIStyle inputField;

        public GUIStyle button;
        public GUIStyle[] title;
        public GUIStyle[] labels;
        public GUIStyle toggle;
        public GUIStyle[] sliders;
        public GUIStyle[] progressBars;
        public GUIStyle verticalScrollbar;
        public GUIStyle horizontalScrollbar;
        #endregion

        #region Properties
        public bool ShouldReInit
        {
            get { return background == null || background.normal.background == null; }
        }

        public float LineHeight
        {
            get { return lineHeight; }
        }

        public int ValidLineCount
        {
            get { return Settings.validLineCount; }
            set { Settings.validLineCount = value; }
        }
        #endregion

        #region Constructors
        public DebugMenuStyle()
        {
            Init();
        }
        #endregion

        #region Class Methods
        public void Init()
        {
            if (Application.isEditor)
            {
                Settings = EditorTextSettings;
            }
            else
            {
                Settings = MobileTextSettings;
            }

            var key = $"{typeof(DebugMenuService).Name}.{typeof(DebugMenuContext).Name}";
            font =
#if UNITY_EDITOR
                Font.CreateDynamicFontFromOSFont("Consolas", Settings.fontSize);
#else
                Resources.GetBuiltinResource<Font>("Arial.ttf");
#endif
            background = GUIStyleHelper.CreateStyle($"{key}.Background", Color.black, Color.black.Alpha(0.8f), Color.black);
            proxies.Add(background);
            lineHeight = font.lineHeight + Settings.linePadding;

            CreateCloseButton(key);
            CreateExtendButton(key);
            CreateInputField(key);

            CreateTitleAndCategories(key);
            CreateProgressBar(key);
            CreateLabel(key);
            CreateToggle(key);
            CreateSlider(key);
            CreateButton(key);
            CreateVerticalScrollbar(key);
            CreateHorizontalScrollbar(key);

            VerifyFontSize();
        }

        public void VerifyFontSize()
        {
            lineHeight = font.lineHeight;
            var lineCount = Screen.height / lineHeight;
            var fontSize  = Mathf.Max(1, Mathf.RoundToInt(Settings.fontSize * (lineCount / Settings.validLineCount)));
            if (Mathf.Abs(font.fontSize - fontSize) <= 2)
            {
                return;
            }

            for (var s = 0; s < proxies.Count; s++)
            {
                var proxy = proxies[s];
                proxy.style.font = font;
                proxy.style.fontSize = fontSize;
                proxy.style.fontSize = Mathf.RoundToInt(fontSize * proxy.fontSizeMultiplier);
            }

            lineHeight = font.lineHeight + Settings.linePadding;
        }

        private void CreateCloseButton(string key)
        {
            GUIStyleHelper.BoxStyle[]  boxStyles    = null;
            GUIStyleHelper.BorderStyle borderStyles = new GUIStyleHelper.BorderStyle(Color.black, 1);

            boxStyles = new[]
            {
                //Normal
                new GUIStyleHelper.BoxStyle(Color.white, 12, 2, 2),

                //onHover
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(354f), .92f, .90f), 12, 2, 1),

                //onActive
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(355f), .53f, .94f).Lighten(1.2f), 12, 2, 1)
            };

            close = GUIStyleHelper.CreateStyle($"{key}.Close", Color.white, boxStyles[0], borderStyles);
            close.font = font;
            close.alignment = TextAnchor.MiddleCenter;
            close.wordWrap = false;
            close.normal.textColor = Color.black;
            close.hover.textColor = Color.white;
            close.hover.background = GUIStyleHelper.CreateTexture(boxStyles[1], borderStyles);
            close.active.textColor = Color.white;
            close.active.background = GUIStyleHelper.CreateTexture(boxStyles[2], borderStyles);
            proxies.Add(new GUIStyleProxy {style = close, fontSizeMultiplier = 2});
        }

        private void CreateExtendButton(string key)
        {
            GUIStyleHelper.BoxStyle[]  boxStyles    = null;
            GUIStyleHelper.BorderStyle borderStyles = new GUIStyleHelper.BorderStyle(Color.black, 1);

            boxStyles = new[]
            {
                //Normal
                new GUIStyleHelper.BoxStyle(Color.white, 12, 2, 2),

                //onHover
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(207), .1f, 1), 12, 2, 1),

                //onActive
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(355f), .19f, 1).Lighten(1.2f), 12, 2, 1)
            };

            extend = GUIStyleHelper.CreateStyle($"{key}.Extend", Color.white, boxStyles[0], borderStyles);
            extend.font = font;
            extend.alignment = TextAnchor.MiddleCenter;
            extend.wordWrap = false;
            extend.normal.textColor = Color.black;
            extend.hover.textColor = Color.black;
            extend.hover.background = GUIStyleHelper.CreateTexture(boxStyles[1], borderStyles);
            extend.active.textColor = Color.black;
            extend.active.background = GUIStyleHelper.CreateTexture(boxStyles[2], borderStyles);
            proxies.Add(new GUIStyleProxy {style = extend, fontSizeMultiplier = 2});
        }

        private void CreateInputField(string key)
        {
            GUIStyleHelper.BoxStyle[]  boxStyles    = null;
            GUIStyleHelper.BorderStyle borderStyles = new GUIStyleHelper.BorderStyle(Color.black, 1);

            boxStyles = new[]
            {
                //Normal
                new GUIStyleHelper.BoxStyle(Color.gray, 12, 2, 2),

                //onHover
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(207), .1f, 1), 12, 2, 1),

                //onActive
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(355f), .19f, 1).Lighten(1.2f), 12, 2, 1)
            };

            inputField = GUIStyleHelper.CreateStyle($"{key}.Extend", Color.white, boxStyles[0], borderStyles);
            inputField.font = font;
            inputField.alignment = TextAnchor.MiddleCenter;
            inputField.wordWrap = false;
            inputField.normal.textColor = Color.black;
            inputField.hover.textColor = Color.black;
            inputField.hover.background = GUIStyleHelper.CreateTexture(boxStyles[1], borderStyles);
            inputField.active.textColor = Color.black;
            inputField.active.background = GUIStyleHelper.CreateTexture(boxStyles[2], borderStyles);
            proxies.Add(new GUIStyleProxy {style = extend, fontSizeMultiplier = 2});
        }

        private void CreateButton(string key)
        {
            GUIStyleHelper.BoxStyle[]  boxStyles    = null;
            GUIStyleHelper.BorderStyle borderStyles = new GUIStyleHelper.BorderStyle(Color.white, 1);

            boxStyles = new[]
            {
                //Normal
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(0, 0, .14f), 12, 2, 2),

                //onHover
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(240f), .01f, .22f), 12, 2, 1),

                //onActive
                new GUIStyleHelper.BoxStyle(Color.HSVToRGB(ColorHelper.DegreeToHue(210f), .01f, .39f), 12, 2, 1)
            };

            button = GUIStyleHelper.CreateStyle($"{key}.Button", Color.white, boxStyles[0], borderStyles);
            button.font = font;
            button.alignment = TextAnchor.MiddleCenter;
            button.wordWrap = false;
            button.normal.textColor = Color.white;
            button.hover.textColor = Color.white;
            button.hover.background = GUIStyleHelper.CreateTexture(boxStyles[1], borderStyles);
            button.active.textColor = Color.white;
            button.active.background = GUIStyleHelper.CreateTexture(boxStyles[2], borderStyles);
            proxies.Add(button);
        }

        private void CreateTitleAndCategories(string key)
        {
            title = new GUIStyle[TITLE_COUNT + CATEGORY_COUNT];
            var degree = 13f;
            for (var t = 0; t < title.Length; t++)
            {
                var                        titleColor   = Color.magenta;
                GUIStyleHelper.BoxStyle[]  boxStyles    = null;
                GUIStyleHelper.BorderStyle borderStyles = GUIStyleHelper.BorderStyle.Default;
                if (t < PAGE_INDEX)
                {
                    titleColor = t == 0
                        ? Color.HSVToRGB(ColorHelper.DegreeToHue(240f), 0.43f, 0.6f)
                        : Color.HSVToRGB(ColorHelper.DegreeToHue(24f), 0.8f, .8f);
                    boxStyles = new[]
                    {
                        //Normal
                        new GUIStyleHelper.BoxStyle(titleColor, 12, 8, 2),

                        //onHover
                        new GUIStyleHelper.BoxStyle(titleColor.Lighten(1.2f), 12, 8, 2),

                        //onActive
                        new GUIStyleHelper.BoxStyle(titleColor.Lighten(0.6f), 12, 8, 2)
                    };
                }
                else
                {
                    var alpha = 1 - Mathf.Clamp01((t - 2) / (title.Length - 1));
                    titleColor = Color.HSVToRGB(ColorHelper.DegreeToHue(degree), 0.43f, 0.3f + 0.3f * alpha);
                    var roundness = 6f * alpha;
                    boxStyles = new[]
                    {
                        //Normal
                        new GUIStyleHelper.BoxStyle(titleColor, 10, roundness, 2),

                        //onHover
                        new GUIStyleHelper.BoxStyle(titleColor.Lighten(1.2f), 10, roundness, 2),

                        //onActive
                        new GUIStyleHelper.BoxStyle(titleColor.Lighten(0.6f), 10, roundness, 2)
                    };

                    degree = (degree + 133f) % 360f;
                }

                title[t] = GUIStyleHelper.CreateStyle($"{key}.Title[{t}]", Color.white, boxStyles[0]);
                title[t].font = font;
                title[t].alignment = TextAnchor.MiddleLeft;
                title[t].wordWrap = false;
                title[t].hover.textColor = Color.white;
                title[t].hover.background = GUIStyleHelper.CreateTexture(boxStyles[1]);
                title[t].active.textColor = Color.white;
                title[t].active.background = GUIStyleHelper.CreateTexture(boxStyles[2], borderStyles);

                proxies.Add(title[t]);
            }
        }

        private void CreateProgressBar(string key)
        {
            GUIStyleHelper.BoxStyle[]    boxStyles    = null;
            GUIStyleHelper.BorderStyle[] borderStyles = null;

            progressBars = new GUIStyle[2];

            boxStyles = new[]
            {
                //Normal
                new GUIStyleHelper.BoxStyle(Color.grey, 12, 2, 2),

                //Normal
                new GUIStyleHelper.BoxStyle(Color.white, 12, 2, 2)
            };
            borderStyles = new[]
            {
                new GUIStyleHelper.BorderStyle(Color.black, 2),
                new GUIStyleHelper.BorderStyle(Color.clear, 3)
            };

            for (var p = 0; p < progressBars.Length; p++)
            {
                progressBars[p] = GUIStyleHelper.CreateStyle($"{key}.ProgressBar{p}", Color.white, boxStyles[p], borderStyles[p]);
                progressBars[p].font = font;
                progressBars[p].alignment = TextAnchor.MiddleCenter;
                progressBars[p].wordWrap = false;
                progressBars[p].normal.textColor = Color.black;
                proxies.Add(new GUIStyleProxy {style = progressBars[p]});
            }
        }

        private void CreateLabel(string key)
        {
            labels = new GUIStyle[LABEL_COUNT];

            for (var l = 0; l < labels.Length; l++)
            {
                var anchor = TextAnchor.MiddleLeft;
                if (l > LABEL_LEFT)
                {
                    anchor = l == LABEL_CENTER ? TextAnchor.MiddleCenter : TextAnchor.MiddleRight;
                }

                labels[l] = GUIStyleHelper.CreateStyle($"{key}.{anchor}", Color.white);
                labels[l].font = font;
                labels[l].alignment = anchor;
                labels[l].wordWrap = false;
                labels[l].padding = new RectOffset(0, 0, Settings.linePadding / 2, Settings.linePadding / 2);
                proxies.Add(labels[l]);
            }
        }

        private void CreateToggle(string key)
        {
            Color toggleOffColor = ColorHelper.red.ToLight(0.1f, 0.6f);
            Color toggleOnColor  = ColorHelper.green.ToLight(0.4f, 0.7f);
            GUIStyleHelper.BoxStyle[] toggleRoundBox =
            {
                //Normal
                new GUIStyleHelper.BoxStyle(toggleOffColor.Lighten(0.4f), 10, 3, 2),
                new GUIStyleHelper.BoxStyle(toggleOnColor.Lighten(1f), 10, 3, 2),

                //onHover
                new GUIStyleHelper.BoxStyle(toggleOffColor.Lighten(0.7f), 10, 3, 2),
                new GUIStyleHelper.BoxStyle(toggleOnColor.Lighten(1.2f), 10, 3, 2),

                //onActive
                new GUIStyleHelper.BoxStyle(toggleOnColor.Lighten(1.2f), 10, 3, 2),
                new GUIStyleHelper.BoxStyle(toggleOffColor.Lighten(0.7f), 10, 3, 2)
            };

            GUIStyleHelper.BorderStyle[] toggleActiveBorder =
            {
                //onActive
                new GUIStyleHelper.BorderStyle(toggleOffColor.Lighten(1.2f), 1),
                new GUIStyleHelper.BorderStyle(toggleOnColor.Lighten(0.6f), 1)
            };

            toggle = GUIStyleHelper.CreateStyle($"{key}.Toggle", title[0], Color.white, toggleRoundBox[0], toggleActiveBorder[0]);
            if (toggle.active.background != null)
            {
                toggle.border = new RectOffset(toggle.active.background.width, 0, 6, 6);
                toggle.padding = new RectOffset(toggle.active.background.width, 0, 0, 0);
                toggle.overflow = new RectOffset(0, 0, 3, 3);
                toggle.onNormal.background = GUIStyleHelper.CreateTexture(toggleRoundBox[1], toggleActiveBorder[1]);
                toggle.onNormal.textColor = Color.white;
                toggle.hover.background = GUIStyleHelper.CreateTexture(toggleRoundBox[2], toggleActiveBorder[0]);
                toggle.hover.textColor = Color.white;
                toggle.onHover.background = GUIStyleHelper.CreateTexture(toggleRoundBox[3], toggleActiveBorder[1]);
                toggle.onHover.textColor = Color.white;
                toggle.active.background = GUIStyleHelper.CreateTexture(toggleRoundBox[4], toggleActiveBorder[0]);
                toggle.active.textColor = Color.white;
                toggle.onActive.background = GUIStyleHelper.CreateTexture(toggleRoundBox[5], toggleActiveBorder[1]);
                toggle.onActive.textColor = Color.white;
            }

            proxies.Add(toggle);
        }

        private void CreateSlider(string key)
        {
            var                        sliderColor        = Color.black;
            GUIStyleHelper.BoxStyle    sliderStyles       = new GUIStyleHelper.BoxStyle(sliderColor, 6, 2, 2);
            GUIStyleHelper.BorderStyle sliderBorderStyles = new GUIStyleHelper.BorderStyle(Color.white, 0.5f);
            Color                      thumbColor         = ColorHelper.grey.dark50;
            GUIStyleHelper.BoxStyle[] thumbStyles =
            {
                //Normal
                new GUIStyleHelper.BoxStyle(thumbColor, 10, 6, 2),

                //onHover
                new GUIStyleHelper.BoxStyle(thumbColor.Lighten(1.2f), 10, 6, 2),

                //onActive
                new GUIStyleHelper.BoxStyle(thumbColor.Lighten(0.6f), 10, 6, 2)
            };

            GUIStyleHelper.BorderStyle thumbBorderStyles = new GUIStyleHelper.BorderStyle(Color.white, 1, 1);
            sliders = new GUIStyle[2];
            sliders[0] = GUIStyleHelper.CreateStyle($"{key}.sliders[0]", Color.white, sliderStyles, sliderBorderStyles);
            sliders[0].overflow.left = -2;
            sliders[0].overflow.right = -2;
            sliders[0].padding.top = -1;
            sliders[0].padding.bottom = -1;
            sliders[1] = GUIStyleHelper.CreateStyle($"{key}.sliders[1]", Color.white, thumbStyles[0], thumbBorderStyles);
            sliders[1].hover.textColor = Color.white;
            sliders[1].hover.background = GUIStyleHelper.CreateTexture(thumbStyles[1], thumbBorderStyles);
            sliders[1].active.textColor = Color.white;
            sliders[1].active.background = GUIStyleHelper.CreateTexture(thumbStyles[2], thumbBorderStyles);
            sliders[1].border = new RectOffset(7, 7, 0, 0);
            sliders[1].padding = new RectOffset(20, 20, 13, 13);
        }

        private void CreateVerticalScrollbar(string key)
        {
            verticalScrollbar = new GUIStyle(GUI.skin.verticalScrollbar);
            verticalScrollbar.fixedWidth = Screen.width * 0.025f;
            GUI.skin.verticalScrollbarThumb.fixedWidth = verticalScrollbar.fixedWidth;
        }

        private void CreateHorizontalScrollbar(string key)
        {
            horizontalScrollbar = new GUIStyle(GUI.skin.horizontalScrollbar);
            horizontalScrollbar.fixedHeight = verticalScrollbar.fixedWidth; // horizontal & vertical should match
            GUI.skin.horizontalScrollbarThumb.fixedHeight = verticalScrollbar.fixedWidth;
        }
        #endregion

        #region Nested type: GUIStyleProxy
        private struct GUIStyleProxy
        {
            public GUIStyle style;
            public float fontSizeMultiplier;

            public static implicit operator GUIStyleProxy(GUIStyle style)
            {
                return new GUIStyleProxy {style = style, fontSizeMultiplier = 1};
            }
        }
        #endregion

        #region Nested type: TextSettings
        private struct TextSettings
        {
            public int fontSize;
            public int linePadding;
            public int validLineCount;
        }
        #endregion
    }
}
