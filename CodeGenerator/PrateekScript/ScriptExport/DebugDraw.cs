// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
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
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

#region Unity
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public partial class Draw
    {
        
        //---------------------------------------------------------------------
        #region DrawSetup overload
        public partial struct Setup
        {
        
        //---------------------------------------------------------------------
        public Setup(bool depthTest) : this(InitMode.Reset)
        {
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, bool depthTest) : this(mode)
        {
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(float duration) : this(InitMode.Reset)
        {
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, float duration) : this(mode)
        {
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, float duration, bool depthTest) : this(mode)
        {
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Color color) : this(InitMode.Reset)
        {
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Color color) : this(mode)
        {
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Color color, bool depthTest) : this(mode)
        {
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Color color, float duration) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Color color, float duration) : this(mode)
        {
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Color color, float duration, bool depthTest) : this(mode)
        {
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space) : this(InitMode.Reset)
        {
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space) : this(mode)
        {
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space, bool depthTest) : this(mode)
        {
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space, float duration) : this(mode)
        {
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space, Color color) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space, Color color) : this(mode)
        {
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space, Color color, bool depthTest) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space, Color color, float duration) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space, Color color, float duration) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(Space space, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Space space, Color color, float duration, bool depthTest) : this(mode)
        {
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix) : this(InitMode.Reset)
        {
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix) : this(mode)
        {
            this.matrix = matrix;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Color color) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Color color) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Color color, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Color color, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space, Color color) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space, Color color) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space, Color color, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space, Color color, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space, Color color, float duration) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space, Color color, float duration) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
            this.duration = duration;
        }
        
        //---------------------------------------------------------------------
        public Setup(Matrix4x4 matrix, Space space, Color color, float duration, bool depthTest) : this(InitMode.Reset)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        
        //---------------------------------------------------------------------
        public Setup(InitMode mode, Matrix4x4 matrix, Space space, Color color, float duration, bool depthTest) : this(mode)
        {
            this.matrix = matrix;
            this.space = space;
            this.color = color;
            this.duration = duration;
            this.depthTest = depthTest;
        }
        }
        #endregion DrawSetup overload
        
    }
}

//-----------------------------------------------------------------------------
namespace Prateek.Debug
{
    //-------------------------------------------------------------------------
    public partial class Draw
    {
        
        //---------------------------------------------------------------------
        #region DrawSetup overload
        public partial struct Setup
        {
            //-----------------------------------------------------------------
            public partial struct QuickCTor
            {
        
                //-------------------------------------------------------------
                public Setup DebugSetup(bool depthTest) 
                { return new Setup(InitMode.Reset, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, bool depthTest)
                { return new Setup(mode, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(float duration) 
                { return new Setup(InitMode.Reset, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, float duration)
                { return new Setup(mode, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, float duration, bool depthTest)
                { return new Setup(mode, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Color color) 
                { return new Setup(InitMode.Reset, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Color color)
                { return new Setup(mode, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Color color, bool depthTest) 
                { return new Setup(InitMode.Reset, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Color color, bool depthTest)
                { return new Setup(mode, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Color color, float duration) 
                { return new Setup(InitMode.Reset, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Color color, float duration)
                { return new Setup(mode, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Color color, float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Color color, float duration, bool depthTest)
                { return new Setup(mode, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space) 
                { return new Setup(InitMode.Reset, space); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space)
                { return new Setup(mode, space); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space, bool depthTest) 
                { return new Setup(InitMode.Reset, space, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space, bool depthTest)
                { return new Setup(mode, space, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space, float duration) 
                { return new Setup(InitMode.Reset, space, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space, float duration)
                { return new Setup(mode, space, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space, float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space, float duration, bool depthTest)
                { return new Setup(mode, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space, Color color) 
                { return new Setup(InitMode.Reset, space, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space, Color color)
                { return new Setup(mode, space, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space, Color color, bool depthTest) 
                { return new Setup(InitMode.Reset, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space, Color color, bool depthTest)
                { return new Setup(mode, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space, Color color, float duration) 
                { return new Setup(InitMode.Reset, space, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space, Color color, float duration)
                { return new Setup(mode, space, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Space space, Color color, float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Space space, Color color, float duration, bool depthTest)
                { return new Setup(mode, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix) 
                { return new Setup(InitMode.Reset, matrix); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix)
                { return new Setup(mode, matrix); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, bool depthTest)
                { return new Setup(mode, matrix, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, float duration) 
                { return new Setup(InitMode.Reset, matrix, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, float duration)
                { return new Setup(mode, matrix, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, float duration, bool depthTest)
                { return new Setup(mode, matrix, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Color color) 
                { return new Setup(InitMode.Reset, matrix, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Color color)
                { return new Setup(mode, matrix, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Color color, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Color color, bool depthTest)
                { return new Setup(mode, matrix, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Color color, float duration) 
                { return new Setup(InitMode.Reset, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Color color, float duration)
                { return new Setup(mode, matrix, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Color color, float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest)
                { return new Setup(mode, matrix, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space) 
                { return new Setup(InitMode.Reset, matrix, space); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space)
                { return new Setup(mode, matrix, space); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, space, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space, bool depthTest)
                { return new Setup(mode, matrix, space, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space, float duration) 
                { return new Setup(InitMode.Reset, matrix, space, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space, float duration)
                { return new Setup(mode, matrix, space, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space, float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space, float duration, bool depthTest)
                { return new Setup(mode, matrix, space, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space, Color color) 
                { return new Setup(InitMode.Reset, matrix, space, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space, Color color)
                { return new Setup(mode, matrix, space, color); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space, Color color, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space, Color color, bool depthTest)
                { return new Setup(mode, matrix, space, color, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space, Color color, float duration) 
                { return new Setup(InitMode.Reset, matrix, space, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space, Color color, float duration)
                { return new Setup(mode, matrix, space, color, duration); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(Matrix4x4 matrix, Space space, Color color, float duration, bool depthTest) 
                { return new Setup(InitMode.Reset, matrix, space, color, duration, depthTest); }
        
                //-------------------------------------------------------------
                public Setup DebugSetup(InitMode mode, Matrix4x4 matrix, Space space, Color color, float duration, bool depthTest)
                { return new Setup(mode, matrix, space, color, duration, depthTest); }
            }
        }
        #endregion DrawSetup overload
        
    }
}
