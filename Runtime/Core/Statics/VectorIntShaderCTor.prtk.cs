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
namespace Prateek.Runtime.Core.Statics
{
    // -BEGIN_PRATEEK_CSHARP_NAMESPACE_CODE-
    ///------------------------------------------------------------------------
    #region Prateek Code Namespaces
    using UnityEngine;
    #endregion Prateek Code Namespaces
// -END_PRATEEK_CSHARP_NAMESPACE_CODE-
    
    

    ///------------------------------------------------------------------------
    public static partial class Statics
    {
        
        //--
        #region Mixed Ctor Vector2Int
        public static Vector2Int vec2i(int n_0, int n_1) { return new Vector2Int(n_0, n_1); }
        public static Vector2Int vec2i(Vector2Int v_0) { return new Vector2Int(v_0.x, v_0.y); }
        public static Vector2Int vec2i(int n_0) { return new Vector2Int(n_0, n_0); }
        #endregion Mixed Ctor Vector2Int
        
        //--
        #region Mixed Ctor Vector3Int
        public static Vector3Int vec3i(int n_0, int n_1, int n_2) { return new Vector3Int(n_0, n_1, n_2); }
        public static Vector3Int vec3i(int n_0, Vector2Int v_0) { return new Vector3Int(n_0, v_0.x, v_0.y); }
        public static Vector3Int vec3i(Vector2Int v_0, int n_0) { return new Vector3Int(v_0.x, v_0.y, n_0); }
        public static Vector3Int vec3i(Vector3Int v_0) { return new Vector3Int(v_0.x, v_0.y, v_0.z); }
        public static Vector3Int vec3i(int n_0) { return new Vector3Int(n_0, n_0, n_0); }
        #endregion Mixed Ctor Vector3Int
        
    }
}
