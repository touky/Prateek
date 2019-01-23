//
//  Prateek, a library that is "bien pratique"
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Attributes;

#if PRATEEK_DEBUG
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public static class Cameras
    {
        //---------------------------------------------------------------------
        public static float ComputeHorizontalFOV(float verticalFOV)
        {
            float vFOVInRads = verticalFOV * Mathf.Deg2Rad;
            float hFOVInRads = 2 * Mathf.Atan(Mathf.Tan(vFOVInRads / 2) * UnityEngine.Camera.main.aspect);
            float hFOV = hFOVInRads * Mathf.Rad2Deg;

            return hFOV;
        }
    }
}