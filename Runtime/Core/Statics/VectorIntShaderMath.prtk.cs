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
        #region Mixed Func int
        public static int clamp(int n_0, int n_1, int n_2) { return Mathf.Clamp(n_0, n_1, n_2); }
        public static int random(int n_0, int n_1) { return UnityEngine.Random.Range(n_0, n_1); }
        public static int max(int n_0, int n_1) { return Mathf.Max(n_0, n_1); }
        public static int min(int n_0, int n_1) { return Mathf.Min(n_0, n_1); }
        public static int mul(int n_0, int n_1) { return n_0 * n_1; }
        public static int div(int n_0, int n_1) { return n_0 / n_1; }
        public static int mod(int n_0, int n_1) { return (n_0 + n_1) % n_1; }
        public static int abs(int n_0) { return Mathf.Abs(n_0); }
        public static int sign(int n_0) { return System.Math.Sign(n_0); }
        public static int exp(int n_0) { return (int)Mathf.Exp(n_0); }
        #endregion Mixed Func int
        
        //--
        #region Mixed Func Vector2Int
        public static Vector2Int clamp(Vector2Int v_0, Vector2Int v_1, Vector2Int v_2) { return new Vector2Int(Mathf.Clamp(v_0.x, v_1.x, v_2.x), Mathf.Clamp(v_0.y, v_1.y, v_2.y)); }
        public static Vector2Int clamp(Vector2Int v_0, int n_1, int n_2) { return new Vector2Int(Mathf.Clamp(v_0.x, n_1, n_2), Mathf.Clamp(v_0.y, n_1, n_2)); }
        public static Vector2Int random(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(UnityEngine.Random.Range(v_0.x, v_1.x), UnityEngine.Random.Range(v_0.y, v_1.y)); }
        public static Vector2Int random(Vector2Int v_0, int n_1) { return new Vector2Int(UnityEngine.Random.Range(v_0.x, n_1), UnityEngine.Random.Range(v_0.y, n_1)); }
        public static Vector2Int max(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(Mathf.Max(v_0.x, v_1.x), Mathf.Max(v_0.y, v_1.y)); }
        public static Vector2Int max(Vector2Int v_0, int n_1) { return new Vector2Int(Mathf.Max(v_0.x, n_1), Mathf.Max(v_0.y, n_1)); }
        public static Vector2Int min(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(Mathf.Min(v_0.x, v_1.x), Mathf.Min(v_0.y, v_1.y)); }
        public static Vector2Int min(Vector2Int v_0, int n_1) { return new Vector2Int(Mathf.Min(v_0.x, n_1), Mathf.Min(v_0.y, n_1)); }
        public static Vector2Int mul(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(v_0.x * v_1.x, v_0.y * v_1.y); }
        public static Vector2Int mul(Vector2Int v_0, int n_1) { return new Vector2Int(v_0.x * n_1, v_0.y * n_1); }
        public static Vector2Int div(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int(v_0.x / v_1.x, v_0.y / v_1.y); }
        public static Vector2Int div(Vector2Int v_0, int n_1) { return new Vector2Int(v_0.x / n_1, v_0.y / n_1); }
        public static Vector2Int mod(Vector2Int v_0, Vector2Int v_1) { return new Vector2Int((v_0.x + v_1.x) % v_1.x, (v_0.y + v_1.y) % v_1.y); }
        public static Vector2Int mod(Vector2Int v_0, int n_1) { return new Vector2Int((v_0.x + n_1) % n_1, (v_0.y + n_1) % n_1); }
        public static Vector2Int abs(Vector2Int v_0) { return new Vector2Int(Mathf.Abs(v_0.x), Mathf.Abs(v_0.y)); }
        public static Vector2Int sign(Vector2Int v_0) { return new Vector2Int(System.Math.Sign(v_0.x), System.Math.Sign(v_0.y)); }
        public static Vector2Int exp(Vector2Int v_0) { return new Vector2Int((int)Mathf.Exp(v_0.x), (int)Mathf.Exp(v_0.y)); }
        #endregion Mixed Func Vector2Int
        
        //--
        #region Mixed Func Vector3Int
        public static Vector3Int clamp(Vector3Int v_0, Vector3Int v_1, Vector3Int v_2) { return new Vector3Int(Mathf.Clamp(v_0.x, v_1.x, v_2.x), Mathf.Clamp(v_0.y, v_1.y, v_2.y), Mathf.Clamp(v_0.z, v_1.z, v_2.z)); }
        public static Vector3Int clamp(Vector3Int v_0, int n_1, int n_2) { return new Vector3Int(Mathf.Clamp(v_0.x, n_1, n_2), Mathf.Clamp(v_0.y, n_1, n_2), Mathf.Clamp(v_0.z, n_1, n_2)); }
        public static Vector3Int random(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(UnityEngine.Random.Range(v_0.x, v_1.x), UnityEngine.Random.Range(v_0.y, v_1.y), UnityEngine.Random.Range(v_0.z, v_1.z)); }
        public static Vector3Int random(Vector3Int v_0, int n_1) { return new Vector3Int(UnityEngine.Random.Range(v_0.x, n_1), UnityEngine.Random.Range(v_0.y, n_1), UnityEngine.Random.Range(v_0.z, n_1)); }
        public static Vector3Int max(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(Mathf.Max(v_0.x, v_1.x), Mathf.Max(v_0.y, v_1.y), Mathf.Max(v_0.z, v_1.z)); }
        public static Vector3Int max(Vector3Int v_0, int n_1) { return new Vector3Int(Mathf.Max(v_0.x, n_1), Mathf.Max(v_0.y, n_1), Mathf.Max(v_0.z, n_1)); }
        public static Vector3Int min(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(Mathf.Min(v_0.x, v_1.x), Mathf.Min(v_0.y, v_1.y), Mathf.Min(v_0.z, v_1.z)); }
        public static Vector3Int min(Vector3Int v_0, int n_1) { return new Vector3Int(Mathf.Min(v_0.x, n_1), Mathf.Min(v_0.y, n_1), Mathf.Min(v_0.z, n_1)); }
        public static Vector3Int mul(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(v_0.x * v_1.x, v_0.y * v_1.y, v_0.z * v_1.z); }
        public static Vector3Int mul(Vector3Int v_0, int n_1) { return new Vector3Int(v_0.x * n_1, v_0.y * n_1, v_0.z * n_1); }
        public static Vector3Int div(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int(v_0.x / v_1.x, v_0.y / v_1.y, v_0.z / v_1.z); }
        public static Vector3Int div(Vector3Int v_0, int n_1) { return new Vector3Int(v_0.x / n_1, v_0.y / n_1, v_0.z / n_1); }
        public static Vector3Int mod(Vector3Int v_0, Vector3Int v_1) { return new Vector3Int((v_0.x + v_1.x) % v_1.x, (v_0.y + v_1.y) % v_1.y, (v_0.z + v_1.z) % v_1.z); }
        public static Vector3Int mod(Vector3Int v_0, int n_1) { return new Vector3Int((v_0.x + n_1) % n_1, (v_0.y + n_1) % n_1, (v_0.z + n_1) % n_1); }
        public static Vector3Int abs(Vector3Int v_0) { return new Vector3Int(Mathf.Abs(v_0.x), Mathf.Abs(v_0.y), Mathf.Abs(v_0.z)); }
        public static Vector3Int sign(Vector3Int v_0) { return new Vector3Int(System.Math.Sign(v_0.x), System.Math.Sign(v_0.y), System.Math.Sign(v_0.z)); }
        public static Vector3Int exp(Vector3Int v_0) { return new Vector3Int((int)Mathf.Exp(v_0.x), (int)Mathf.Exp(v_0.y), (int)Mathf.Exp(v_0.z)); }
        #endregion Mixed Func Vector3Int
        
    }
}
