namespace Mayfair.Core.Code.DebugMenu
{
    using System;
    using UnityEngine;

    //internal struct TextureDebug
    //{
    //    private class Data
    //    {
    //        #region Fields
    //        public float zoomLevel = 10;
    //        public float seconds = 0.4f;
    //        public Texture2D texture2D;
    //        public GUIStyleHelper.BoxStyle boxStyle = new GUIStyleHelper.BoxStyle(Color.white, 24);
    //        public GUIStyleHelper.BorderStyle borderStyle = new GUIStyleHelper.BorderStyle(Color.black, null);
    //        #endregion
    //    }

    //    private static Data data = new Data();

    //    public static void DrawTextureDebug()
    //    {
    //        Rect screenRect = new Rect(0, 0, Screen.width / 3, Screen.height);
    //        screenRect.x = Screen.width - screenRect.width;

    //        GUI.Box(screenRect, GUIContent.none);
    //        using (new GUILayout.AreaScope(screenRect))
    //        {
    //            Data workingTexture = data;

    //            GUILayout.Label($"Zoom level {workingTexture.zoomLevel}");
    //            workingTexture.zoomLevel = Mathf.RoundToInt(GUILayout.HorizontalSlider(workingTexture.zoomLevel, 1, 10));
    //            workingTexture.seconds -= Time.deltaTime;
    //            if (workingTexture.seconds < 0)
    //            {
    //                workingTexture.seconds = 0.4f;

    //                UnityEngine.Object.Destroy(workingTexture.texture2D);
    //                workingTexture.texture2D = null;
    //            }

    //            workingTexture.boxStyle.color = Color.white;

    //            GUILayout.Label($"SIZE {(int) workingTexture.boxStyle.size.x} / {(int) workingTexture.boxStyle.size.y}");
    //            workingTexture.boxStyle.size = Vector2.one * Mathf.RoundToInt(GUILayout.HorizontalSlider(workingTexture.boxStyle.size.x, 1, 32));

    //            Func<string, float?, bool, float?> Draw = (title, value, allowInc) =>
    //            {
    //                GUILayout.Label($"{title} {(int) (value.HasValue ? value.Value : 0)}");
    //                float fvalue = value.HasValue ? value.Value : 0;
    //                fvalue = GUILayout.HorizontalSlider(fvalue, 0, 32);
    //                if (allowInc)
    //                {
    //                    fvalue = Mathf.RoundToInt(fvalue * 2f) / 2f;
    //                }
    //                else
    //                {
    //                    fvalue = Mathf.RoundToInt(fvalue);
    //                }

    //                if (fvalue == 0)
    //                {
    //                    value = null;
    //                }
    //                else
    //                {
    //                    value = fvalue;
    //                }

    //                return value;
    //            };

    //            workingTexture.boxStyle.roundness = Draw("Roudness", workingTexture.boxStyle.roundness, false);
    //            workingTexture.boxStyle.bevel = Draw("Bevel", workingTexture.boxStyle.bevel, true);

    //            workingTexture.borderStyle.size = Draw("Border size", workingTexture.borderStyle.size, true);
    //            workingTexture.borderStyle.bevel = Draw("Border bevel", workingTexture.borderStyle.bevel, false);

    //            if (workingTexture.texture2D == null)
    //            {
    //                workingTexture.texture2D = GUIStyleHelper.CreateTexture(workingTexture.boxStyle, workingTexture.borderStyle);
    //            }

    //            int borderSize = 4;
    //            float width = workingTexture.texture2D.width * workingTexture.zoomLevel + borderSize * 2;
    //            float height = workingTexture.texture2D.height * workingTexture.zoomLevel + borderSize * 2;
    //            Rect rect = GUILayoutUtility.GetRect(width, width, height, height);
    //            rect.width = Mathf.Min(rect.width, rect.height);
    //            rect.height = rect.width;
    //            rect = RectHelper.Inflate(rect, -borderSize);
    //            using (new ColorScope(Color.cyan))
    //            {
    //                GUI.DrawTexture(RectHelper.Inflate(rect, borderSize), Texture2D.whiteTexture);
    //            }

    //            using (new ColorScope(Color.magenta))
    //            {
    //                GUI.DrawTexture(rect, Texture2D.whiteTexture);
    //            }

    //            GUI.DrawTexture(rect, workingTexture.texture2D);

    //            string boxStyleCode = string.Empty;
    //            string borderStyleCode = string.Empty;
    //            {
    //                string roundText = workingTexture.boxStyle.roundness.HasValue ? workingTexture.boxStyle.roundness.Value.ToString() : "null";
    //                string bevelText = workingTexture.boxStyle.bevel.HasValue ? workingTexture.boxStyle.bevel.Value.ToString() : "null";
    //                boxStyleCode = $"var myBoxStyle = new GUIStyleHelper.BoxStyle(Color.white, {workingTexture.boxStyle.size.x}, {roundText}, {bevelText});";
    //            }
    //            {
    //                string sizeText = workingTexture.borderStyle.size.HasValue ? workingTexture.borderStyle.size.Value.ToString() : "null";
    //                string bevelText = workingTexture.borderStyle.bevel.HasValue ? workingTexture.borderStyle.bevel.Value.ToString() : "null";
    //                borderStyleCode = $"var myBorderStyle = new GUIStyleHelper.BorderStyle(Color.white, {sizeText}, {bevelText});";
    //            }

    //            GUILayout.Label("Style code");
    //            GUILayout.TextArea($"{boxStyleCode}\n{borderStyleCode}");
    //        }
    //    }
    //}
}
