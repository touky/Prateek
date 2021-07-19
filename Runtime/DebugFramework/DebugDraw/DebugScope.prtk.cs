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
    public class DebugScope : GUI.Scope
#else
    public class DebugScopeCode : GUI.Scope
#endif
    {
        ///--------------------------------------------------------------------
        #region Fields
        private DebugStyle setup;
        #endregion Fields

        ///--------------------------------------------------------------------
        #region Properties
        public DebugStyle Setup { get { return setup; } }
        #endregion Properties

        ///--------------------------------------------------------------------
        #region Scope
#if ACTIVE_CODE
        private DebugScope(DebugStyle setup) : base()
#else
        private DebugScopeCode(DebugStyle setup) : base()
#endif
        {
            this.setup = setup;
#if ACTIVE_CODE
            DebugDisplayRegistry.Add(this);
#endif
        }

#if PRATEEK_DEBUG
        public static DebugScope Open(DebugStyle setup)
#else
        public static DebugScopeCode Open(DebugStyle setup)
#endif
        {
#if PRATEEK_DEBUG
            return new DebugScope(setup);
#else
            return null;
#endif
        }

        ///--------------------------------------------------------------------
        protected override void CloseScope()
        {
#if ACTIVE_CODE
            DebugDisplayRegistry.Remove(this);
#endif
        }
        #endregion Scope

        #region Open overload
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, Color color)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, float duration)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Matrix4x4 matrix)
        { return Open(new DebugStyle(mode, flag, debugSpace, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, float duration)
        { return Open(new DebugStyle(mode, flag, debugSpace, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, Color color)
        { return Open(new DebugStyle(mode, flag, debugSpace, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, float duration)
        { return Open(new DebugStyle(mode, flag, debugSpace, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, bool depthTest)
        { return Open(new DebugStyle(mode, flag, debugSpace, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace, int precision)
        { return Open(new DebugStyle(mode, flag, debugSpace, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, DebugSpace debugSpace) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, debugSpace)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, DebugSpace debugSpace)
        { return Open(new DebugStyle(mode, flag, debugSpace)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, float duration)
        { return Open(new DebugStyle(mode, flag, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, flag, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, Color color)
        { return Open(new DebugStyle(mode, flag, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, float duration)
        { return Open(new DebugStyle(mode, flag, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, bool depthTest)
        { return Open(new DebugStyle(mode, flag, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix, int precision)
        { return Open(new DebugStyle(mode, flag, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Matrix4x4 matrix) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Matrix4x4 matrix)
        { return Open(new DebugStyle(mode, flag, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color, float duration)
        { return Open(new DebugStyle(mode, flag, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, flag, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color, int precision)
        { return Open(new DebugStyle(mode, flag, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, Color color)
        { return Open(new DebugStyle(mode, flag, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, flag, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, float duration, int precision)
        { return Open(new DebugStyle(mode, flag, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, float duration)
        { return Open(new DebugStyle(mode, flag, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, flag, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, bool depthTest)
        { return Open(new DebugStyle(mode, flag, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag, int precision)
        { return Open(new DebugStyle(mode, flag, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(MaskFlag flag) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, flag)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, MaskFlag flag)
        { return Open(new DebugStyle(mode, flag)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, float duration)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, Color color)
        { return Open(new DebugStyle(mode, debugSpace, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, float duration)
        { return Open(new DebugStyle(mode, debugSpace, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix, int precision)
        { return Open(new DebugStyle(mode, debugSpace, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Matrix4x4 matrix) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Matrix4x4 matrix)
        { return Open(new DebugStyle(mode, debugSpace, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, debugSpace, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color, float duration)
        { return Open(new DebugStyle(mode, debugSpace, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color, int precision)
        { return Open(new DebugStyle(mode, debugSpace, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, Color color)
        { return Open(new DebugStyle(mode, debugSpace, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, float duration, int precision)
        { return Open(new DebugStyle(mode, debugSpace, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, float duration)
        { return Open(new DebugStyle(mode, debugSpace, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, debugSpace, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, bool depthTest)
        { return Open(new DebugStyle(mode, debugSpace, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace, int precision)
        { return Open(new DebugStyle(mode, debugSpace, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugSpace debugSpace) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, debugSpace)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, DebugSpace debugSpace)
        { return Open(new DebugStyle(mode, debugSpace)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, matrix, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, matrix, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, matrix, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color, float duration)
        { return Open(new DebugStyle(mode, matrix, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, matrix, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, matrix, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color, int precision)
        { return Open(new DebugStyle(mode, matrix, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, Color color)
        { return Open(new DebugStyle(mode, matrix, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, matrix, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, matrix, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, float duration, int precision)
        { return Open(new DebugStyle(mode, matrix, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, float duration)
        { return Open(new DebugStyle(mode, matrix, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, matrix, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, bool depthTest)
        { return Open(new DebugStyle(mode, matrix, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix, int precision)
        { return Open(new DebugStyle(mode, matrix, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Matrix4x4 matrix) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Matrix4x4 matrix)
        { return Open(new DebugStyle(mode, matrix)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color, float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, color, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color, float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, color, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color, float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color, float duration, int precision)
        { return Open(new DebugStyle(mode, color, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color, float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color, float duration)
        { return Open(new DebugStyle(mode, color, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, color, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color, bool depthTest)
        { return Open(new DebugStyle(mode, color, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color, int precision)
        { return Open(new DebugStyle(mode, color, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(Color color) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, Color color)
        { return Open(new DebugStyle(mode, color)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(float duration, bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, float duration, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, duration, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(float duration, bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, float duration, bool depthTest)
        { return Open(new DebugStyle(mode, duration, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(float duration, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, float duration, int precision)
        { return Open(new DebugStyle(mode, duration, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(float duration) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, float duration)
        { return Open(new DebugStyle(mode, duration)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(bool depthTest, int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, bool depthTest, int precision)
        { return Open(new DebugStyle(mode, depthTest, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(bool depthTest) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, bool depthTest)
        { return Open(new DebugStyle(mode, depthTest)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(int precision) 
        { return Open(new DebugStyle(DebugStyle.InitMode.UseLast, precision)); }
        
        ///--------------------------------------------------------------------
        public static DebugScope Open(DebugStyle.InitMode mode, int precision)
        { return Open(new DebugStyle(mode, precision)); }
        #endregion Open overload
        
    }
}
