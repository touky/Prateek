// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 21/08/2020
//
//  Copyright � 2017-2020 "Touky" <touky at prateek dot top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

#define ACTIVE_CODE

namespace Prateek.Runtime.DebugFramework.DebugDraw
{
    // -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------

    using Prateek.Runtime.Core.Helpers;
    using UnityEngine;

    #region Prateek Code Namespaces
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-

    ///------------------------------------------------------------------------
#if ACTIVE_CODE
    public struct DebugStyle
#else
    internal struct DebugStyleCode
#endif
    {
        #region InitMode enum
        ///--------------------------------------------------------------------
        public enum InitMode
        {
            Reset,
            UseLast,
        }
        #endregion

        #region Fields
        ///--------------------------------------------------------------------
        private MaskFlag flag;

        private DebugSpace debugSpace;
        private Matrix4x4 matrix;
        private Color color;
        private float duration;
        private bool depthTest;
        private int precision;
        #endregion

        #region Properties
        ///--------------------------------------------------------------------
        public MaskFlag Flag { get { return flag; } set { flag = value; } }

        public DebugSpace DebugSpace { get { return debugSpace; } set { debugSpace = value; } }
        public Matrix4x4 Matrix { get { return matrix; } set { matrix = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public float Duration { get { return duration; } set { duration = value; } }
        public bool DepthTest { get { return depthTest; } set { depthTest = value; } }
        public int Precision { get { return precision; } set { precision = value; } }
        #endregion

        #region Constructors
        ///--------------------------------------------------------------------
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
                    this = DebugDisplayRegistry.ActiveSetup;
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

        #region DebugStyle CTor Overload
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace, int precision) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, DebugSpace debugSpace) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, DebugSpace debugSpace) : this(mode)
        {
            this.flag = flag;
            this.debugSpace = debugSpace;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color) : this(mode)
        {
            this.flag = flag;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, int precision) : this(mode)
        {
            this.flag = flag;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(MaskFlag flag) : this(InitMode.Reset)
        {
            this.flag = flag;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag) : this(mode)
        {
            this.flag = flag;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color, float duration, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color, float duration, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color, float duration) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color, float duration) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, Color color) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, Color color) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, float duration, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, float duration, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, float duration, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, float duration, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, float duration) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, float duration) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, bool depthTest, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, bool depthTest) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, bool depthTest) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace, int precision) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace, int precision) : this(mode)
        {
            this.debugSpace = debugSpace;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(DebugSpace debugSpace) : this(InitMode.Reset)
        {
            this.debugSpace = debugSpace;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, DebugSpace debugSpace) : this(mode)
        {
            this.debugSpace = debugSpace;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix) : this(mode)
        {
            this.matrix = matrix;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration, bool depthTest) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration, int precision) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color, float duration) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration) : this(mode)
        {
            this.color = color;
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, bool depthTest, int precision) : this(mode)
        {
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, bool depthTest) : this(mode)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, int precision) : this(mode)
        {
            this.color = color;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(Color color) : this(InitMode.Reset)
        {
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color) : this(mode)
        {
            this.color = color;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration, bool depthTest, int precision) : this(mode)
        {
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration, bool depthTest) : this(mode)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(float duration, int precision) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration, int precision) : this(mode)
        {
            this.duration = duration;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(float duration) : this(InitMode.Reset)
        {
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration) : this(mode)
        {
            this.duration = duration;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, bool depthTest, int precision) : this(mode)
        {
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(bool depthTest) : this(InitMode.Reset)
        {
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, bool depthTest) : this(mode)
        {
            this.depthTest = depthTest;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(int precision) : this(InitMode.Reset)
        {
            this.precision = precision;
        }
        
        ///--------------------------------------------------------------------
        public DebugStyle(InitMode mode, int precision) : this(mode)
        {
            this.precision = precision;
        }
        #endregion DrawSetup overload
        
    }
}
