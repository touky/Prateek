// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 05/07/2020
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


///----------------------------------------------------------------------------
namespace Prateek.Debug.Code
{
// -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------
    #region Prateek Code Namespaces
    using UnityEngine;
    
    using Prateek.Core.Code.Helpers;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-

    ///------------------------------------------------------------------------
    public partial class DebugDraw
    {
        
        ///--------------------------------------------------------------------
        #region DebugStyle CTor Overload
        public partial struct DebugStyle
        {
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, Color color) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space, int precision) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Space space) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.space = space;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Space space) : this(mode)
            {
                    this.flag = flag;
                this.space = space;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, int precision) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Matrix4x4 matrix) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix) : this(mode)
            {
                    this.flag = flag;
                this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color, int precision) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, Color color) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, Color color) : this(mode)
            {
                    this.flag = flag;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, float duration, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, float duration, int precision) : this(mode)
            {
                    this.flag = flag;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, float duration) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, float duration) : this(mode)
            {
                    this.flag = flag;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, bool depthTest, int precision) : this(mode)
            {
                    this.flag = flag;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, bool depthTest) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, bool depthTest) : this(mode)
            {
                    this.flag = flag;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag, int precision) : this(InitMode.Reset)
            {
                this.flag = flag;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag, int precision) : this(mode)
            {
                    this.flag = flag;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(MaskFlag flag) : this(InitMode.Reset)
            {
                this.flag = flag;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, MaskFlag flag) : this(mode)
            {
                    this.flag = flag;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, int precision) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Matrix4x4 matrix) : this(InitMode.Reset)
            {
                this.space = space;
                this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix) : this(mode)
            {
                    this.space = space;
                this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color, float duration, int precision) : this(mode)
            {
                    this.space = space;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color, float duration) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color, float duration) : this(mode)
            {
                    this.space = space;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color, int precision) : this(mode)
            {
                    this.space = space;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, Color color) : this(InitMode.Reset)
            {
                this.space = space;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, Color color) : this(mode)
            {
                    this.space = space;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, float duration, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, float duration, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, float duration, int precision) : this(mode)
            {
                    this.space = space;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, float duration) : this(InitMode.Reset)
            {
                this.space = space;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, float duration) : this(mode)
            {
                    this.space = space;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, bool depthTest, int precision) : this(mode)
            {
                    this.space = space;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, bool depthTest) : this(InitMode.Reset)
            {
                this.space = space;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, bool depthTest) : this(mode)
            {
                    this.space = space;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space, int precision) : this(InitMode.Reset)
            {
                this.space = space;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space, int precision) : this(mode)
            {
                    this.space = space;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Space space) : this(InitMode.Reset)
            {
                this.space = space;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Space space) : this(mode)
            {
                    this.space = space;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, Color color) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color) : this(mode)
            {
                    this.matrix = matrix;
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
            {
                    this.matrix = matrix;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, float duration) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration) : this(mode)
            {
                    this.matrix = matrix;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest) : this(mode)
            {
                    this.matrix = matrix;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix, int precision) : this(InitMode.Reset)
            {
                this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix, int precision) : this(mode)
            {
                    this.matrix = matrix;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Matrix4x4 matrix) : this(InitMode.Reset)
            {
                this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Matrix4x4 matrix) : this(mode)
            {
                    this.matrix = matrix;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color, float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color, float duration, bool depthTest) : this(mode)
            {
                    this.color = color;
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color, float duration, int precision) : this(InitMode.Reset)
            {
                this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color, float duration, int precision) : this(mode)
            {
                    this.color = color;
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color, float duration) : this(InitMode.Reset)
            {
                this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color, float duration) : this(mode)
            {
                    this.color = color;
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color, bool depthTest, int precision) : this(mode)
            {
                    this.color = color;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color, bool depthTest) : this(InitMode.Reset)
            {
                this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color, bool depthTest) : this(mode)
            {
                    this.color = color;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color, int precision) : this(InitMode.Reset)
            {
                this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color, int precision) : this(mode)
            {
                    this.color = color;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(Color color) : this(InitMode.Reset)
            {
                this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, Color color) : this(mode)
            {
                    this.color = color;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(float duration, bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, float duration, bool depthTest, int precision) : this(mode)
            {
                    this.duration = duration;
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(float duration, bool depthTest) : this(InitMode.Reset)
            {
                this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, float duration, bool depthTest) : this(mode)
            {
                    this.duration = duration;
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(float duration, int precision) : this(InitMode.Reset)
            {
                this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, float duration, int precision) : this(mode)
            {
                    this.duration = duration;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(float duration) : this(InitMode.Reset)
            {
                this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, float duration) : this(mode)
            {
                    this.duration = duration;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(bool depthTest, int precision) : this(InitMode.Reset)
            {
                this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, bool depthTest, int precision) : this(mode)
            {
                    this.depthTest = depthTest;
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(bool depthTest) : this(InitMode.Reset)
            {
                this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, bool depthTest) : this(mode)
            {
                    this.depthTest = depthTest;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(int precision) : this(InitMode.Reset)
            {
                this.precision = precision;
            }
        
            ///----------------------------------------------------------------
            public DebugStyle(InitMode mode, int precision) : this(mode)
            {
                    this.precision = precision;
            }
        }
        #endregion DrawSetup overload
        
    }
}
