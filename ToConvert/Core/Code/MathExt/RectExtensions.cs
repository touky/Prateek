namespace Mayfair.Core.Code.MathExt
{
    using UnityEngine;

    public static class RectExtensions
    {
        #region RectExtensions
        public static Vector2 Distance(this Rect rect, Vector2 point)
        {
            Vector2 c = point - rect.center;
            return new Vector2(Mathf.Max(0, Mathf.Abs(c.x) - rect.width * 0.5f) * Mathf.Sign(c.x),
                               Mathf.Max(0, Mathf.Abs(c.y) - rect.height * 0.5f) * Mathf.Sign(c.y));
        }
        #endregion
    }
}
