namespace Mayfair.Core.Code.Utils
{
    using UnityEngine;

    public static class MathUtils
    {
        #region Class Methods

        /// <summary>
        /// Time independent damping
        /// from ma-good-ol-friend http://lolengine.net/blog/2015/05/03/damping-with-delta-time
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="coefficient"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static float Damp(float current, float target, float coefficient)
        {
            return Damp(current, target, coefficient, Time.deltaTime);
        }

        /// <summary>
        /// Time independent damping
        /// from ma-good-ol-friend http://lolengine.net/blog/2015/05/03/damping-with-delta-time
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="coefficient"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector2 Damp(Vector2 current, Vector2 target, float coefficient)
        {
            return Damp(current, target, coefficient, Time.deltaTime);
        }

        /// <summary>
        /// Time independent damping
        /// from ma-good-ol-friend http://lolengine.net/blog/2015/05/03/damping-with-delta-time
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="coefficient"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector3 Damp(Vector3 current, Vector3 target, float coefficient)
        {
            return Damp(current, target, coefficient, Time.deltaTime);
        }

        /// <summary>
        /// Time independent damping
        /// from ma-good-ol-friend http://lolengine.net/blog/2015/05/03/damping-with-delta-time
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="coefficient"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static float Damp(float current, float target, float coefficient, float deltaTime)
        {
            // Exponentiation base for velocity damping
            float D2 = Mathf.Pow(1f - coefficient / 60f, 60f);

            // Damp velocity (framerate-independent)
            return Mathf.Lerp(current, target, Mathf.Clamp01(1f - Mathf.Pow(D2, deltaTime)));
        }

        /// <summary>
        /// Time independent damping
        /// from ma-good-ol-friend http://lolengine.net/blog/2015/05/03/damping-with-delta-time
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="coefficient"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector2 Damp(Vector2 current, Vector2 target, float coefficient, float deltaTime)
        {
            // Exponentiation base for velocity damping
            float D2 = Mathf.Pow(1f - coefficient / 60f, 60f);

            // Damp velocity (framerate-independent)
            return Vector2.Lerp(current, target, Mathf.Clamp01(1f - Mathf.Pow(D2, deltaTime)));
        }

        /// <summary>
        /// Time independent damping
        /// from ma-good-ol-friend http://lolengine.net/blog/2015/05/03/damping-with-delta-time
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="coefficient"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static Vector3 Damp(Vector3 current, Vector3 target, float coefficient, float deltaTime)
        {
            // Exponentiation base for velocity damping
            float D2 = Mathf.Pow(1f - coefficient / 60f, 60f);

            // Damp velocity (framerate-independent)
            return Vector3.Lerp(current, target, Mathf.Clamp01(1f - Mathf.Pow(D2, deltaTime)));
        }
        #endregion
    }
}
