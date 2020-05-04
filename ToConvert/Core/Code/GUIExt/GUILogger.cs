namespace Mayfair.Core.Code.GUIExt
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.Utils.Helpers;
    using UnityEngine;

    [Serializable]
    public class GUILogger : ISerializationCallbackReceiver
    {
        #region Static and Constants
        private const float BACKGROUND_PADDING = 8;
        private const float LINE_PADDING = 0;

        private static Color DEFAULT_COLOR = new Color(0, 0.9f, 0);
        #endregion

        #region Settings
        [SerializeField]
        private List<TintPrefix> tintPrefixes = new List<TintPrefix>();

        [SerializeField]
        private List<string> saveLines = new List<string>();
        #endregion

        #region Fields
        private string lineFormat = "{0:D3}> {1}";
        private Vector2 maxLineSize = Vector2.zero;
        private int maxOutputLines; //Needed for OnGUI event purposes

        private int maxLogMemory;
        private List<string> lines = new List<string>();

        private bool stickyScroll = true;
        private Vector2 scrollPosition = Vector2.zero;

        private GUIStyle styleText;
        private GUIStyle styleBackground;
        #endregion

        #region Constructors
        public GUILogger(int maxLogMemory = 1000)
        {
            this.maxLogMemory = maxLogMemory;

            this.styleBackground = GUIStyleHelper.CreateStyle("GUILogger.styleBackground", ColorHelper.green.dark25, Color.black, ColorHelper.green.dark25);
            this.styleText = GUIStyleHelper.CreateStyle("GUILogger.styleText", Color.white, Color.clear, Color.clear);
            this.styleText.font = Font.CreateDynamicFontFromOSFont("Consolas", 12);
            this.styleText.alignment = TextAnchor.UpperLeft;
            this.styleText.wordWrap = false;
            this.styleText.richText = true;
        }
        #endregion

        #region Unity EditorOnly Methods
        public void OnGUI(Rect rectWindow)
        {
            this.maxLineSize.y = this.styleText.CalcSize(new GUIContent("ABCDEFGHIJKLMNOPQRSTUVWXYZ")).y + LINE_PADDING;

            Rect rectView = new Rect(0, 0,
                                     Mathf.Max(rectWindow.width, this.maxLineSize.x + BACKGROUND_PADDING * 2),
                                     Mathf.Max(rectWindow.height, this.maxOutputLines * this.maxLineSize.y + BACKGROUND_PADDING * 2));

            Rect buttonLine = RectHelper.TruncateY(ref rectWindow, this.maxLineSize.y);
            Rect[] buttonRects = RectHelper.SplitX(ref buttonLine, 10, Split.FixedSize(100), Split.FixedSize(10), Split.FixedSize(60));
            {
                int br = 1;
                if (GUI.Button(buttonRects[br++], this.stickyScroll ? "Unstick scroll" : "Stick scroll"))
                {
                    this.stickyScroll = !this.stickyScroll;
                }

                br++;
                if (GUI.Button(buttonRects[br++], "Clear"))
                {
                    ClearLog();
                }
            }

            Rect rectBG = rectWindow;
            rectBG.width -= GUI.skin.verticalScrollbar.fixedWidth;
            rectBG.height -= GUI.skin.horizontalScrollbar.fixedHeight;
            GUI.Box(rectBG, GUIContent.none, this.styleBackground);

            using (GUI.ScrollViewScope scroll = new GUI.ScrollViewScope(rectWindow, this.scrollPosition, rectView, true, true))
            {
                RectHelper.Inflate(ref rectView, -BACKGROUND_PADDING);

                int lineCount = Mathf.Max(1, Mathf.Min(this.maxOutputLines, this.lines.Count));
                for (int s = 0; s < lineCount; s++)
                {
                    string line = s < this.lines.Count ? this.lines[s] : string.Empty;

                    Vector2 size = this.styleText.CalcSize(new GUIContent(line));
                    this.maxLineSize.x = Mathf.Max(this.maxLineSize.x, size.x);

                    //maxLineSize.y = Mathf.Max(maxLineSize.y, size.y + linePadding);

                    Rect rectLabel = rectView;
                    rectLabel.y += this.maxLineSize.y * s;
                    rectLabel.height = this.maxLineSize.y;

                    if (rectLabel.y < this.scrollPosition.y - this.maxLineSize.y * 2
                     || rectLabel.y > this.scrollPosition.y + rectWindow.height + this.maxLineSize.y * 2)
                    {
                        continue;
                    }

                    Color color = DEFAULT_COLOR;
                    foreach (TintPrefix prefix in this.tintPrefixes)
                    {
                        if (line.StartsWith(prefix.value))
                        {
                            color = prefix.color;
                            break;
                        }
                    }

                    using (ColorScope scpColor = new ColorScope(color))
                    {
                        GUI.Label(rectLabel, string.Format(this.lineFormat, s, line), this.styleText);
                    }
                }

                this.scrollPosition = scroll.scrollPosition;

                if (Event.current.type == EventType.Repaint)
                {
                    this.maxOutputLines = Mathf.Max(1, this.lines.Count);

                    if (this.stickyScroll)
                    {
                        float bottomScroll = Mathf.Max(0, rectView.height - (rectWindow.height - (GUI.skin.horizontalScrollbar.fixedHeight + 1)));
                        this.scrollPosition.y = bottomScroll;
                    }
                }
            }
        }
        #endregion

        #region Class Methods
        #region Tint Prefix Methods
        public void ClearAllTintPrefix()
        {
            this.tintPrefixes.Clear();
        }

        public void ClearTintPrefix(string prefix)
        {
            for (int p = 0; p < this.tintPrefixes.Count; p++)
            {
                if (this.tintPrefixes[p].value == prefix)
                {
                    this.tintPrefixes.RemoveAt(p);
                    break;
                }
            }
        }

        public TintPrefix GetTintPrefix(int index)
        {
            if (index < 0 || index >= this.tintPrefixes.Count)
            {
                return new TintPrefix {color = Color.white};
            }

            return this.tintPrefixes[index];
        }

        public void AddTintPrefix(params TintPrefix[] prefix)
        {
            this.tintPrefixes.AddRange(prefix);
        }

        public TintPrefix AddTintPrefix(string prefix, Color color)
        {
            TintPrefix newPrefix = new TintPrefix {value = prefix, color = color};
            this.tintPrefixes.Add(newPrefix);
            return newPrefix;
        }
        #endregion

        #region Log Methods
        public void Log(string format, params object[] args)
        {
            if (this.lines.Count == this.maxLogMemory)
            {
                this.lines.RemoveAt(0);
            }

            for (int a = 0; a < args.Length; a++)
            {
                if (!(args[a] is TintPrefix))
                {
                    continue;
                }

                args[a] = ((TintPrefix) args[a]).value;
            }

            this.lines.Add(string.Format(format, args));
        }

        public void Log(TintPrefix prefix, string line)
        {
            if (this.lines.Count == this.maxLogMemory)
            {
                this.lines.RemoveAt(0);
            }

            this.lines.Add(string.Format("{0} {1}", prefix.value, line));
        }

        public void Log(string line)
        {
            if (this.lines.Count == this.maxLogMemory)
            {
                this.lines.RemoveAt(0);
            }

            this.lines.Add(line);
        }

        public void Log(string[] log)
        {
            if (log.Length == 0)
            {
                return;
            }

            int max = this.lines.Count + log.Length;
            if (max >= this.maxLogMemory)
            {
                this.lines.RemoveRange(0, 1 + (max - this.maxLogMemory));
            }

            this.lines.AddRange(log);
        }

        public void ClearLog()
        {
            this.lines.Clear();
        }
        #endregion
        #endregion

        #region Nested type: TintPrefix
        [Serializable]
        public struct TintPrefix
        {
            public string value;
            public Color color;
        }
        #endregion

        #region ISerializationCallbackReceiver
        public void OnBeforeSerialize()
        {
            this.saveLines.Clear();
            this.saveLines.AddRange(this.lines);
        }

        public void OnAfterDeserialize()
        {
            this.lines.Clear();
            this.lines.AddRange(this.saveLines);
            this.saveLines.Clear();
        }
        #endregion ISerializationCallbackReceiver
    }
}
