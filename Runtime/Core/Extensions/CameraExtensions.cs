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

//-----------------------------------------------------------------------------
namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static class CameraExtensions
    {
        #region Class Methods
        ///---------------------------------------------------------------------
        public static float ComputeHorizontalFov(float verticalFOV)
        {
            var vFOVInRads = verticalFOV * Mathf.Deg2Rad;
            var hFOVInRads = 2 * Mathf.Atan(Mathf.Tan(vFOVInRads / 2) * Camera.main.aspect);
            var hFOV       = hFOVInRads * Mathf.Rad2Deg;

            return hFOV;
        }

        ///---------------------------------------------------------------------
        public static Matrix4x4 LocalToCameraMatrix(this Camera camera)
        {
            return camera.worldToCameraMatrix * camera.transform.localToWorldMatrix;
        }

        ///---------------------------------------------------------------------
        public static Matrix4x4 CameraToLocalMatrix(this Camera camera)
        {
            return camera.LocalToCameraMatrix().inverse;
        }
        #endregion
    }
}
