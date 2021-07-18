namespace Prateek.Runtime.Core.Statics
{
    using UnityEngine;

    public static partial class Statics
    {
        public static float lerp(float v0, float v1, float alpha) { return Mathf.Lerp(v0, v1, alpha); }
        public static float mix(float v0, float v1, float alpha) { return Mathf.Lerp(v0, v1, alpha); }
    }
}
