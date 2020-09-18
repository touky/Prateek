namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    public static partial class Statics
    {
        public static Color lerp(Color v0, Color v1, float alpha) { return Color.Lerp(v0, v1, alpha); }
        public static Color mix(Color v0, Color v1, float alpha) { return Color.Lerp(v0, v1, alpha); }
        
    }
}
