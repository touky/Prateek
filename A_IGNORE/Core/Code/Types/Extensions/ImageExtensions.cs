namespace Mayfair.Core.Code.Types.Extensions
{
    using UnityEngine;
    using UnityEngine.UI;

    public static class ImageExtensions
    {
        public static void SetTransparency(this Image image, float alpha)
        {
            Color alphaColor = image.color;
            alphaColor.a = alpha;
            image.color = alphaColor;
        }
    }
}