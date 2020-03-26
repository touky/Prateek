namespace Assets.Prateek.ToConvert.DebugMenu
{
    using System;
    using UnityEngine;

    internal struct StyleDebug
    {
        private class Data
        {
            #region Fields
            public int borderMax = 40;
            public int overflowMax = 40;
            public int paddingMax = 40;
            #endregion
        }

        private static Data data = new Data();

        public static void DrawStyleDebug(GUIStyle style)
        {
            var s = data;
            Func<RectOffset, int, RectOffset> DrawSliders = (ro, maxValue) =>
            {
                ro.left = Mathf.RoundToInt(GUILayout.HorizontalSlider(ro.left, -maxValue, maxValue));
                ro.right = Mathf.RoundToInt(GUILayout.HorizontalSlider(ro.right, -maxValue, maxValue));
                ro.top = Mathf.RoundToInt(GUILayout.HorizontalSlider(ro.top, -maxValue, maxValue));
                ro.bottom = Mathf.RoundToInt(GUILayout.HorizontalSlider(ro.bottom, -maxValue, maxValue));

                return ro;
            };

            GUILayout.Label($"Border {style.border.ToString()}");
            {
                style.border = DrawSliders(style.border, s.borderMax);
            }

            GUILayout.Label($"Overflow {style.overflow.ToString()}");
            {
                style.overflow = DrawSliders(style.overflow, s.overflowMax);
            }

            GUILayout.Label($"Padding {style.padding.ToString()}");
            {
                style.padding = DrawSliders(style.padding, s.paddingMax);
            }
        }
    }
}
