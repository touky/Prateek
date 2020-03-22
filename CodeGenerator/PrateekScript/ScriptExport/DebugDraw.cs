// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.Debug
{
    using Prateek.Helpers;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public  partial class DebugDraw
    {
        
        //---------------------------------------------------------------------
        #region DrawSetup overload
        public partial struct DebugStyle
        {
        
        //---------------------------------------------------------------------
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
        
        //---------------------------------------------------------------------
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
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space, int precision) : this(mode)
        {
            this.flag = flag;
            this.space = space;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Space space) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Space space) : this(mode)
        {
            this.flag = flag;
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix) : this(mode)
        {
            this.flag = flag;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color, int precision) : this(mode)
        {
            this.flag = flag;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, Color color) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, Color color) : this(mode)
        {
            this.flag = flag;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration, int precision) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, float duration) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, float duration) : this(mode)
        {
            this.flag = flag;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, bool depthTest, int precision) : this(mode)
        {
            this.flag = flag;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, bool depthTest) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, bool depthTest) : this(mode)
        {
            this.flag = flag;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag, int precision) : this(InitMode.Reset)
        {
            this.flag = flag;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag, int precision) : this(mode)
        {
            this.flag = flag;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(MaskFlag flag) : this(InitMode.Reset)
        {
            this.flag = flag;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, MaskFlag flag) : this(mode)
        {
            this.flag = flag;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Matrix4x4 matrix) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color, float duration) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color, bool depthTest) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, Color color) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, Color color) : this(mode)
        {
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, float duration) : this(mode)
        {
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, bool depthTest) : this(mode)
        {
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space, int precision) : this(mode)
        {
            this.space = space;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Space space) : this(InitMode.Reset)
        {
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Space space) : this(mode)
        {
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Matrix4x4 matrix) : this(mode)
        {
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration, bool depthTest) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration, int precision) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color, float duration) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, float duration) : this(mode)
        {
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, bool depthTest, int precision) : this(mode)
        {
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, bool depthTest) : this(mode)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color, int precision) : this(mode)
        {
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(Color color) : this(InitMode.Reset)
        {
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, Color color) : this(mode)
        {
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration, bool depthTest, int precision) : this(mode)
        {
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration, bool depthTest) : this(mode)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(float duration, int precision) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration, int precision) : this(mode)
        {
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(float duration) : this(InitMode.Reset)
        {
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, float duration) : this(mode)
        {
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, bool depthTest, int precision) : this(mode)
        {
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(bool depthTest) : this(InitMode.Reset)
        {
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, bool depthTest) : this(mode)
        {
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(int precision) : this(InitMode.Reset)
        {
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public DebugStyle(InitMode mode, int precision) : this(mode)
        {
            this.precision = precision;
        }
        }
        #endregion DrawSetup overload
        
    }
}

//-----------------------------------------------------------------------------
namespace Prateek.Debug
{
    using Prateek.Helpers;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public  partial class DebugDraw
    {
        
        //---------------------------------------------------------------------
        #region DrawSetup overload
        public partial struct DebugStyle
        {
            //-----------------------------------------------------------------
            public static class QuickCTor
            {
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, space, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration, int precision)
                { return new DebugStyle(mode, flag, space, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, float duration)
                { return new DebugStyle(mode, flag, space, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, bool depthTest)
                { return new DebugStyle(mode, flag, space, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color, int precision)
                { return new DebugStyle(mode, flag, space, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, Color color) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, Color color)
                { return new DebugStyle(mode, flag, space, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, space, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration, int precision)
                { return new DebugStyle(mode, flag, space, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, float duration)
                { return new DebugStyle(mode, flag, space, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, bool depthTest)
                { return new DebugStyle(mode, flag, space, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix, int precision)
                { return new DebugStyle(mode, flag, space, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Matrix4x4 matrix) 
                { return new DebugStyle(InitMode.UseLast, flag, space, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Matrix4x4 matrix)
                { return new DebugStyle(mode, flag, space, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration, int precision)
                { return new DebugStyle(mode, flag, space, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, float duration)
                { return new DebugStyle(mode, flag, space, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, bool depthTest)
                { return new DebugStyle(mode, flag, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color, int precision)
                { return new DebugStyle(mode, flag, space, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, Color color) 
                { return new DebugStyle(InitMode.UseLast, flag, space, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, Color color)
                { return new DebugStyle(mode, flag, space, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration, int precision)
                { return new DebugStyle(mode, flag, space, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, space, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, float duration)
                { return new DebugStyle(mode, flag, space, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, space, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, space, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, bool depthTest)
                { return new DebugStyle(mode, flag, space, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, space, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space, int precision)
                { return new DebugStyle(mode, flag, space, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Space space) 
                { return new DebugStyle(InitMode.UseLast, flag, space); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Space space)
                { return new DebugStyle(mode, flag, space); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision)
                { return new DebugStyle(mode, flag, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration)
                { return new DebugStyle(mode, flag, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest)
                { return new DebugStyle(mode, flag, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, int precision)
                { return new DebugStyle(mode, flag, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, Color color) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color)
                { return new DebugStyle(mode, flag, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, int precision)
                { return new DebugStyle(mode, flag, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration)
                { return new DebugStyle(mode, flag, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest)
                { return new DebugStyle(mode, flag, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix, int precision)
                { return new DebugStyle(mode, flag, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Matrix4x4 matrix) 
                { return new DebugStyle(InitMode.UseLast, flag, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Matrix4x4 matrix)
                { return new DebugStyle(mode, flag, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration, int precision)
                { return new DebugStyle(mode, flag, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color, float duration)
                { return new DebugStyle(mode, flag, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color, bool depthTest)
                { return new DebugStyle(mode, flag, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color, int precision)
                { return new DebugStyle(mode, flag, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, Color color) 
                { return new DebugStyle(InitMode.UseLast, flag, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, Color color)
                { return new DebugStyle(mode, flag, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, float duration, bool depthTest)
                { return new DebugStyle(mode, flag, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, float duration, int precision)
                { return new DebugStyle(mode, flag, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, float duration) 
                { return new DebugStyle(InitMode.UseLast, flag, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, float duration)
                { return new DebugStyle(mode, flag, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, bool depthTest, int precision)
                { return new DebugStyle(mode, flag, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, flag, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, bool depthTest)
                { return new DebugStyle(mode, flag, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag, int precision) 
                { return new DebugStyle(InitMode.UseLast, flag, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag, int precision)
                { return new DebugStyle(mode, flag, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(MaskFlag flag) 
                { return new DebugStyle(InitMode.UseLast, flag); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, MaskFlag flag)
                { return new DebugStyle(mode, flag); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, space, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, space, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, int precision)
                { return new DebugStyle(mode, space, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration)
                { return new DebugStyle(mode, space, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, space, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest)
                { return new DebugStyle(mode, space, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, int precision)
                { return new DebugStyle(mode, space, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, Color color) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color)
                { return new DebugStyle(mode, space, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, space, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest)
                { return new DebugStyle(mode, space, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, int precision)
                { return new DebugStyle(mode, space, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, float duration) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration)
                { return new DebugStyle(mode, space, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest, int precision)
                { return new DebugStyle(mode, space, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest)
                { return new DebugStyle(mode, space, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, int precision)
                { return new DebugStyle(mode, space, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Matrix4x4 matrix) 
                { return new DebugStyle(InitMode.UseLast, space, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Matrix4x4 matrix)
                { return new DebugStyle(mode, space, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, space, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color, float duration, int precision)
                { return new DebugStyle(mode, space, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, space, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color, float duration)
                { return new DebugStyle(mode, space, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, space, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color, bool depthTest)
                { return new DebugStyle(mode, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color, int precision)
                { return new DebugStyle(mode, space, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, Color color) 
                { return new DebugStyle(InitMode.UseLast, space, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, Color color)
                { return new DebugStyle(mode, space, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, space, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, float duration, bool depthTest)
                { return new DebugStyle(mode, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, float duration, int precision)
                { return new DebugStyle(mode, space, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, float duration) 
                { return new DebugStyle(InitMode.UseLast, space, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, float duration)
                { return new DebugStyle(mode, space, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, bool depthTest, int precision)
                { return new DebugStyle(mode, space, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, space, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, bool depthTest)
                { return new DebugStyle(mode, space, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space, int precision) 
                { return new DebugStyle(InitMode.UseLast, space, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space, int precision)
                { return new DebugStyle(mode, space, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Space space) 
                { return new DebugStyle(InitMode.UseLast, space); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Space space)
                { return new DebugStyle(mode, space); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, int precision)
                { return new DebugStyle(mode, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration)
                { return new DebugStyle(mode, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest)
                { return new DebugStyle(mode, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, int precision)
                { return new DebugStyle(mode, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, Color color) 
                { return new DebugStyle(InitMode.UseLast, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, Color color)
                { return new DebugStyle(mode, matrix, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest)
                { return new DebugStyle(mode, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, int precision)
                { return new DebugStyle(mode, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, float duration) 
                { return new DebugStyle(InitMode.UseLast, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, float duration)
                { return new DebugStyle(mode, matrix, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest, int precision)
                { return new DebugStyle(mode, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest)
                { return new DebugStyle(mode, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix, int precision) 
                { return new DebugStyle(InitMode.UseLast, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix, int precision)
                { return new DebugStyle(mode, matrix, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Matrix4x4 matrix) 
                { return new DebugStyle(InitMode.UseLast, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Matrix4x4 matrix)
                { return new DebugStyle(mode, matrix); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color, float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color, float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color, float duration, bool depthTest)
                { return new DebugStyle(mode, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color, float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color, float duration, int precision)
                { return new DebugStyle(mode, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color, float duration) 
                { return new DebugStyle(InitMode.UseLast, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color, float duration)
                { return new DebugStyle(mode, color, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color, bool depthTest, int precision)
                { return new DebugStyle(mode, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color, bool depthTest)
                { return new DebugStyle(mode, color, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color, int precision) 
                { return new DebugStyle(InitMode.UseLast, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color, int precision)
                { return new DebugStyle(mode, color, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(Color color) 
                { return new DebugStyle(InitMode.UseLast, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, Color color)
                { return new DebugStyle(mode, color); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(float duration, bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, float duration, bool depthTest, int precision)
                { return new DebugStyle(mode, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(float duration, bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, float duration, bool depthTest)
                { return new DebugStyle(mode, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(float duration, int precision) 
                { return new DebugStyle(InitMode.UseLast, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, float duration, int precision)
                { return new DebugStyle(mode, duration, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(float duration) 
                { return new DebugStyle(InitMode.UseLast, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, float duration)
                { return new DebugStyle(mode, duration); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(bool depthTest, int precision) 
                { return new DebugStyle(InitMode.UseLast, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, bool depthTest, int precision)
                { return new DebugStyle(mode, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(bool depthTest) 
                { return new DebugStyle(InitMode.UseLast, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, bool depthTest)
                { return new DebugStyle(mode, depthTest); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(int precision) 
                { return new DebugStyle(InitMode.UseLast, precision); }
        
                //-------------------------------------------------------------
                public static DebugStyle DebugStyle(InitMode mode, int precision)
                { return new DebugStyle(mode, precision); }
            }
        }
        #endregion DrawSetup overload
        
    }
}
