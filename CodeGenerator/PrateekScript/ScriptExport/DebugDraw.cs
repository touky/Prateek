// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 01/04/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

//-----------------------------------------------------------------------------
#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

//-----------------------------------------------------------------------------
#region Unity
using Unity.Jobs;
using Unity.Collections;

//-----------------------------------------------------------------------------
#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

//-----------------------------------------------------------------------------
#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

#endregion Engine

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#endregion Unity

//-----------------------------------------------------------------------------
#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

//-----------------------------------------------------------------------------
#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

//-----------------------------------------------------------------------------
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.Draw.Style.QuickCTor;
#endif //PRATEEK_DEBUG

#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public  partial class Draw
    {
        
        //---------------------------------------------------------------------
        #region DrawSetup overload
        public partial struct Style
        {
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.space = space;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Matrix4x4 matrix) : this(mode)
        {
            this.space = space;
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color, float duration) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color, bool depthTest) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color, int precision) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, Color color) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, Color color) : this(mode)
        {
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, float duration, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, float duration, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, float duration, int precision) : this(mode)
        {
            this.space = space;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, float duration) : this(mode)
        {
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, bool depthTest, int precision) : this(mode)
        {
            this.space = space;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, bool depthTest) : this(mode)
        {
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space, int precision) : this(InitMode.Reset)
        {
            this.space = space;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space, int precision) : this(mode)
        {
            this.space = space;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Space space) : this(InitMode.Reset)
        {
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Space space) : this(mode)
        {
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color, float duration, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, float duration, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, float duration, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, bool depthTest, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix, int precision) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix, int precision) : this(mode)
        {
            this.matrix = matrix;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Matrix4x4 matrix) : this(mode)
        {
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color, float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color, float duration, bool depthTest, int precision) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color, float duration, bool depthTest) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color, float duration, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color, float duration, int precision) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color, float duration) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color, float duration) : this(mode)
        {
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color, bool depthTest, int precision) : this(mode)
        {
            this.color = color;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color, bool depthTest) : this(mode)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color, int precision) : this(InitMode.Reset)
        {
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color, int precision) : this(mode)
        {
            this.color = color;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(Color color) : this(InitMode.Reset)
        {
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, Color color) : this(mode)
        {
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Style(float duration, bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, float duration, bool depthTest, int precision) : this(mode)
        {
            this.duration = duration;
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, float duration, bool depthTest) : this(mode)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(float duration, int precision) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, float duration, int precision) : this(mode)
        {
            this.duration = duration;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(float duration) : this(InitMode.Reset)
        {
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, float duration) : this(mode)
        {
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Style(bool depthTest, int precision) : this(InitMode.Reset)
        {
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, bool depthTest, int precision) : this(mode)
        {
            this.depthTest = depthTest;
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(bool depthTest) : this(InitMode.Reset)
        {
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, bool depthTest) : this(mode)
        {
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Style(int precision) : this(InitMode.Reset)
        {
            this.precision = precision;
        }
        
        //---------------------------------------------------------------------
        public Style(InitMode mode, int precision) : this(mode)
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
    //-------------------------------------------------------------------------
    public  partial class Draw
    {
        
        //---------------------------------------------------------------------
        #region DrawSetup overload
        public partial struct Style
        {
            //-----------------------------------------------------------------
            public static class QuickCTor
            {
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
                { return new Style(mode, space, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, space, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, bool depthTest)
                { return new Style(mode, space, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration, int precision)
                { return new Style(mode, space, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color, float duration) 
                { return new Style(InitMode.UseLast, space, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, float duration)
                { return new Style(mode, space, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest, int precision)
                { return new Style(mode, space, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color, bool depthTest) 
                { return new Style(InitMode.UseLast, space, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, bool depthTest)
                { return new Style(mode, space, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color, int precision)
                { return new Style(mode, space, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, Color color) 
                { return new Style(InitMode.UseLast, space, matrix, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, Color color)
                { return new Style(mode, space, matrix, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest, int precision)
                { return new Style(mode, space, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, space, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, bool depthTest)
                { return new Style(mode, space, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, float duration, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration, int precision)
                { return new Style(mode, space, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, float duration) 
                { return new Style(InitMode.UseLast, space, matrix, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, float duration)
                { return new Style(mode, space, matrix, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest, int precision)
                { return new Style(mode, space, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, bool depthTest) 
                { return new Style(InitMode.UseLast, space, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, bool depthTest)
                { return new Style(mode, space, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix, int precision) 
                { return new Style(InitMode.UseLast, space, matrix, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix, int precision)
                { return new Style(mode, space, matrix, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Matrix4x4 matrix) 
                { return new Style(InitMode.UseLast, space, matrix); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Matrix4x4 matrix)
                { return new Style(mode, space, matrix); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color, float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest, int precision)
                { return new Style(mode, space, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color, float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color, float duration, bool depthTest)
                { return new Style(mode, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color, float duration, int precision) 
                { return new Style(InitMode.UseLast, space, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color, float duration, int precision)
                { return new Style(mode, space, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color, float duration) 
                { return new Style(InitMode.UseLast, space, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color, float duration)
                { return new Style(mode, space, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color, bool depthTest, int precision)
                { return new Style(mode, space, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color, bool depthTest) 
                { return new Style(InitMode.UseLast, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color, bool depthTest)
                { return new Style(mode, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color, int precision) 
                { return new Style(InitMode.UseLast, space, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color, int precision)
                { return new Style(mode, space, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, Color color) 
                { return new Style(InitMode.UseLast, space, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, Color color)
                { return new Style(mode, space, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, float duration, bool depthTest, int precision)
                { return new Style(mode, space, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, float duration, bool depthTest)
                { return new Style(mode, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, float duration, int precision) 
                { return new Style(InitMode.UseLast, space, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, float duration, int precision)
                { return new Style(mode, space, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, float duration) 
                { return new Style(InitMode.UseLast, space, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, float duration)
                { return new Style(mode, space, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, space, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, bool depthTest, int precision)
                { return new Style(mode, space, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, bool depthTest) 
                { return new Style(InitMode.UseLast, space, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, bool depthTest)
                { return new Style(mode, space, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space, int precision) 
                { return new Style(InitMode.UseLast, space, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space, int precision)
                { return new Style(mode, space, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Space space) 
                { return new Style(InitMode.UseLast, space); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Space space)
                { return new Style(mode, space); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
                { return new Style(mode, matrix, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color, float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest)
                { return new Style(mode, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color, float duration, int precision) 
                { return new Style(InitMode.UseLast, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration, int precision)
                { return new Style(mode, matrix, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color, float duration) 
                { return new Style(InitMode.UseLast, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, float duration)
                { return new Style(mode, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest, int precision)
                { return new Style(mode, matrix, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color, bool depthTest) 
                { return new Style(InitMode.UseLast, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest)
                { return new Style(mode, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color, int precision) 
                { return new Style(InitMode.UseLast, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color, int precision)
                { return new Style(mode, matrix, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, Color color) 
                { return new Style(InitMode.UseLast, matrix, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, Color color)
                { return new Style(mode, matrix, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest, int precision)
                { return new Style(mode, matrix, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest)
                { return new Style(mode, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, float duration, int precision) 
                { return new Style(InitMode.UseLast, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, float duration, int precision)
                { return new Style(mode, matrix, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, float duration) 
                { return new Style(InitMode.UseLast, matrix, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, float duration)
                { return new Style(mode, matrix, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest, int precision)
                { return new Style(mode, matrix, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, bool depthTest) 
                { return new Style(InitMode.UseLast, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, bool depthTest)
                { return new Style(mode, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix, int precision) 
                { return new Style(InitMode.UseLast, matrix, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix, int precision)
                { return new Style(mode, matrix, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Matrix4x4 matrix) 
                { return new Style(InitMode.UseLast, matrix); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Matrix4x4 matrix)
                { return new Style(mode, matrix); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color, float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color, float duration, bool depthTest, int precision)
                { return new Style(mode, color, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color, float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color, float duration, bool depthTest)
                { return new Style(mode, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color, float duration, int precision) 
                { return new Style(InitMode.UseLast, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color, float duration, int precision)
                { return new Style(mode, color, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color, float duration) 
                { return new Style(InitMode.UseLast, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color, float duration)
                { return new Style(mode, color, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color, bool depthTest, int precision)
                { return new Style(mode, color, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color, bool depthTest) 
                { return new Style(InitMode.UseLast, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color, bool depthTest)
                { return new Style(mode, color, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color, int precision) 
                { return new Style(InitMode.UseLast, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color, int precision)
                { return new Style(mode, color, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(Color color) 
                { return new Style(InitMode.UseLast, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, Color color)
                { return new Style(mode, color); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(float duration, bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, float duration, bool depthTest, int precision)
                { return new Style(mode, duration, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(float duration, bool depthTest) 
                { return new Style(InitMode.UseLast, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, float duration, bool depthTest)
                { return new Style(mode, duration, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(float duration, int precision) 
                { return new Style(InitMode.UseLast, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, float duration, int precision)
                { return new Style(mode, duration, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(float duration) 
                { return new Style(InitMode.UseLast, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, float duration)
                { return new Style(mode, duration); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(bool depthTest, int precision) 
                { return new Style(InitMode.UseLast, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, bool depthTest, int precision)
                { return new Style(mode, depthTest, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(bool depthTest) 
                { return new Style(InitMode.UseLast, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, bool depthTest)
                { return new Style(mode, depthTest); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(int precision) 
                { return new Style(InitMode.UseLast, precision); }
        
                //-------------------------------------------------------------
                public static Style DebugStyle(InitMode mode, int precision)
                { return new Style(mode, precision); }
            }
        }
        #endregion DrawSetup overload
        
    }
}
