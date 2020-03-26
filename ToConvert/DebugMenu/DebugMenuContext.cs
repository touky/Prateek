namespace Assets.Prateek.ToConvert.DebugMenu
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using UnityEngine;

    public class DebugMenuContext
    {
        #region Fields
        private int indent = 0;
        private bool useShortNames = false;
        private DebugMenuStyle style;
        private List<TitleField> titles = new List<TitleField>();
        private List<DebugField> fields = new List<DebugField>();
        private List<float> weightList = new List<float>();
        #endregion

        #region Properties
        //@formatter:off
        public bool HasFields
        {
            get { return fields.Count > 0; }
        }

        public bool UseShortNames
        {
            get { return useShortNames; }
        }

        public int Indent
        {
            get { return indent; }
            set { indent = value; }
        }

        public int MaxLineCount
        {
            get { return style.ValidLineCount; }
            set { style.ValidLineCount = Mathf.Max(1, value); }
        }

        public List<float> WeightList
        {
            get { return weightList; }
        }

        public float LineHeight
        {
            get { return style.LineHeight; }
        }

        public GUIStyle Background
        {
            get { return style.background; }
        }

        public GUIStyle Close
        {
            get { return style.close; }
        }

        public GUIStyle Extend
        {
            get { return style.extend; }
        }

        public GUIStyle[] ProgressBar
        {
            get { return style.progressBars; }
        }

        public GUIStyle Button
        {
            get { return style.button; }
        }

        public GUIStyle InputField
        {
            get { return style.inputField; }
        }

        public GUIStyle Page
        {
            get { return style.title[DebugMenuStyle.PAGE_INDEX]; }
        }

        public GUIStyle LabelLeft
        {
            get { return style.labels[DebugMenuStyle.LABEL_LEFT]; }
        }

        public GUIStyle LabelCenter
        {
            get { return style.labels[DebugMenuStyle.LABEL_CENTER]; }
        }

        public GUIStyle LabelRight
        {
            get { return style.labels[DebugMenuStyle.LABEL_RIGHT]; }
        }

        public GUIStyle Toggle
        {
            get { return style.toggle; }
        }

        public GUIStyle[] Sliders
        {
            get { return style.sliders; }
        }

        public GUIStyle VerticalScrollBar
        {
            get { return style.verticalScrollbar; }
        }

        public GUIStyle HorizontalScrollBar
        {
            get { return style.horizontalScrollbar; }
        }
        #endregion

        #region Constructors
        public DebugMenuContext()
        {
            style = new DebugMenuStyle();
        }
        #endregion

        #region Class Methods
        public void CheckProperInit()
        {
            if (style != null && !style.ShouldReInit)
            {
                return;
            }

            if (style == null)
            {
                style = new DebugMenuStyle();
            }
            else
            {
                style.Init();
            }
        }

        public GUIStyle Title(bool showContent)
        {
            return style.title[showContent ? 1 : 0];
        }

        public GUIStyle Category(int index)
        {
            return style.title[Mathf.Min(Mathf.Max(index, 1) + DebugMenuStyle.PAGE_INDEX, style.title.Length - 1)];
        }

        public static void DrawTextureDebug()
        {
            TextureDebug.DrawTextureDebug();
        }

        public static void DrawStyleDebug(GUIStyle style)
        {
            StyleDebug.DrawStyleDebug(style);
        }

        public void Reset(bool useShortNames)
        {
            this.useShortNames = useShortNames;

            titles.Clear();
            fields.Clear();
            style.VerifyFontSize();
        }

        public void Draw(DebugField field)
        {
            var title = field as TitleField;
            if (title != null && title.IsTitle)
            {
                titles.Add(title);
            }
            else
            {
                field.Indent = indent;

                fields.Add(field);
            }
        }

        public float GetTitlesSize()
        {
            float size = 0;
            for (var r = 0; r < titles.Count; r++)
            {
                var title = titles[r];

                size = Mathf.Max(size, title.GetSize(this));
            }

            return size;
        }

        public void DrawTitles()
        {
#if NVIZZIO_DEV
            using (new ProfilerScope("DrawTitles"))
#endif
            {
                for (var r = 0; r < titles.Count; r++)
                {
                    var title = titles[r];

                    title.OnGUI(this);
                }
            }
        }

        public void DrawFields()
        {
#if NVIZZIO_DEV
            using (new ProfilerScope("DrawFields"))
#endif
            {
                for (var r = 0; r < fields.Count; r++)
                {
#if NVIZZIO_DEV
                    using (new ProfilerScope("field.OnGUI(this);"))
#endif
                    {
                        var field = fields[r];

                        field.OnGUI(this);
                    }
                }
            }
        }
        #endregion
    }
}
