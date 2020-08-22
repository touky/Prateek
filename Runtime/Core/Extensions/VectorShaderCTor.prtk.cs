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





///----------------------------------------------------------------------------
namespace Prateek.Runtime.Core.Extensions
{
    // -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------
    #region Prateek Code Namespaces
    using System;
    using System.Collections;
    using System.Collections.Generic;
    
    using UnityEngine;
    
    using Prateek;
    using static Prateek.Runtime.Core.Extensions.Statics;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-
    
    

    ///------------------------------------------------------------------------
    public static partial class Statics
    {
        
        //--
        #region Mixed Ctor Vector2
        public static Vector2 vec2(float n_0, float n_1) { return new Vector2(n_0, n_1); }
        public static Vector2 vec2(Vector2 v_0) { return new Vector2(v_0.x, v_0.y); }
        public static Vector2 vec2(float n_0) { return new Vector2(n_0, n_0); }
        #endregion Mixed Ctor Vector2
        
        //--
        #region Mixed Ctor Vector3
        public static Vector3 vec3(float n_0, float n_1, float n_2) { return new Vector3(n_0, n_1, n_2); }
        public static Vector3 vec3(float n_0, Vector2 v_0) { return new Vector3(n_0, v_0.x, v_0.y); }
        public static Vector3 vec3(Vector2 v_0, float n_0) { return new Vector3(v_0.x, v_0.y, n_0); }
        public static Vector3 vec3(Vector3 v_0) { return new Vector3(v_0.x, v_0.y, v_0.z); }
        public static Vector3 vec3(float n_0) { return new Vector3(n_0, n_0, n_0); }
        #endregion Mixed Ctor Vector3
        
        //--
        #region Mixed Ctor Vector4
        public static Vector4 vec4(float n_0, float n_1, float n_2, float n_3) { return new Vector4(n_0, n_1, n_2, n_3); }
        public static Vector4 vec4(float n_0, float n_1, Vector2 v_0) { return new Vector4(n_0, n_1, v_0.x, v_0.y); }
        public static Vector4 vec4(float n_0, Vector2 v_0, float n_1) { return new Vector4(n_0, v_0.x, v_0.y, n_1); }
        public static Vector4 vec4(float n_0, Vector3 v_0) { return new Vector4(n_0, v_0.x, v_0.y, v_0.z); }
        public static Vector4 vec4(Vector2 v_0, float n_0, float n_1) { return new Vector4(v_0.x, v_0.y, n_0, n_1); }
        public static Vector4 vec4(Vector2 v_0, Vector2 v_1) { return new Vector4(v_0.x, v_0.y, v_1.x, v_1.y); }
        public static Vector4 vec4(Vector3 v_0, float n_0) { return new Vector4(v_0.x, v_0.y, v_0.z, n_0); }
        public static Vector4 vec4(Vector4 v_0) { return new Vector4(v_0.x, v_0.y, v_0.z, v_0.w); }
        public static Vector4 vec4(float n_0) { return new Vector4(n_0, n_0, n_0, n_0); }
        #endregion Mixed Ctor Vector4
        
    }
}
