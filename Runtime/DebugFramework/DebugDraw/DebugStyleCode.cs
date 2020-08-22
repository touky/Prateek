//#PRATEEK:#PRATEEK_DEFINE_SECTION#

namespace Prateek.Runtime.DebugFramework
{
    //#PRATEEK:#PRATEEK_CSHARP_NAMESPACE_CODE#

    //#PRATEEK:#PRATEEK_USING_NAMESPACE#
    using Prateek.Runtime.Core.Helpers;
    using UnityEngine;

    ///---------------------------------------------------------------------
#if ACTIVE_CODE
    public struct DebugStyle
#else
    internal struct DebugStyleCode
#endif
    {
        #region InitMode enum
        ///-----------------------------------------------------------------
        public enum InitMode
        {
            Reset,
            UseLast,
        }
        #endregion

        #region Fields
        ///-----------------------------------------------------------------
        private MaskFlag flag;

        private DebugSpace debugSpace;
        private Matrix4x4 matrix;
        private Color color;
        private float duration;
        private bool depthTest;
        private int precision;
        #endregion

        #region Properties
        ///-----------------------------------------------------------------
        public MaskFlag Flag { get { return flag; } set { flag = value; } }

        public DebugSpace DebugSpace { get { return debugSpace; } set { debugSpace = value; } }
        public Matrix4x4 Matrix { get { return matrix; } set { matrix = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public float Duration { get { return duration; } set { duration = value; } }
        public bool DepthTest { get { return depthTest; } set { depthTest = value; } }
        public int Precision { get { return precision; } set { precision = value; } }
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
#if ACTIVE_CODE
        public DebugStyle(InitMode initMode)
#else
        public DebugStyleCode(InitMode initMode)
#endif
        {
            switch (initMode)
            {
                case InitMode.UseLast:
                {
#if ACTIVE_CODE
                    this = DebugDraw.ActiveSetup;
#else
                    this = default;
#endif
                    break;
                }
                default:
                {
                    flag = -1;
                    debugSpace = DebugSpace.World;
                    matrix = Matrix4x4.identity;
                    color = Color.white;
                    duration = -1;
                    depthTest = false;
                    precision = 8;
                    break;
                }
            }
        }
        #endregion

        //#PRATEEK:#PRATEEK_CODEGEN_DATA#
    }
}
