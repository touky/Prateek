// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
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

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    public class DebugDisplayManager : MonoBehaviour
    {
        //---------------------------------------------------------------------
        #region Declarations
        public struct LineData
        {
            public GameObject root;
            public LineRenderer line;
        }
        #endregion //Declarations

        //---------------------------------------------------------------------
        #region Fields
        private PersonalLoggerManager loggerManager = new PersonalLoggerManager();

        private Stack<LineData> linePool = new Stack<LineData>();
        private Stack<LineData> lineActive = new Stack<LineData>();
        private Shader lineShader = null;
        private int getCallCount = 0;
        private int newCallCount = 0;

        private List<Helpers.StringBlurp> blurps = new List<Helpers.StringBlurp>();

        #endregion //Fields

        //---------------------------------------------------------------------
        #region Settings
        [SerializeField]
        private float lineRendererWidth = 0.0025f;
        #endregion //Settings

        //---------------------------------------------------------------------
        #region Properties
        public float LineRendererWidth { get { return lineRendererWidth; } }
        #endregion //Properties


        //---------------------------------------------------------------------
        #region Unity Defaults
        void LateUpdate()
        {
            Draw.EndFrame();

            if (lineActive.Count > 0)
            {
                StartCoroutine(RefreshPool());
            }

            loggerManager.DisplayDebug();
        }

        //---------------------------------------------------------------------
        void OnGUI()
        {
            loggerManager.DisplayGUI();
        }
        #endregion //Unity Defaults

        //---------------------------------------------------------------------
        #region OCP Black Box
        public void Register(Helpers.PersonalLogger logger)
        {
            if (loggerManager == null)
                loggerManager = new PersonalLoggerManager();
            loggerManager.Register(logger);
        }

        //---------------------------------------------------------------------
        public void Unregister(Helpers.PersonalLogger logger)
        {
            loggerManager.Unregister(logger);
        }
        #endregion //OCP Black Box

        //---------------------------------------------------------------------
        #region Lines Pool
        public IEnumerator RefreshPool()
        {
            yield return new WaitForEndOfFrame();

            while (lineActive.Count > 0)
            {
                var data = lineActive.Pop();
                data.root.SetActive(false);
                linePool.Push(data);
                getCallCount--;
            }
        }

        //---------------------------------------------------------------------
        public LineRenderer GetLine()
        {
            if (lineShader == null)
            {
                lineShader = Shader.Find("Particles/Alpha Blended Premultiply");
            }

            LineData data = new LineData();
            if (linePool.Count > 0)
            {
                getCallCount++;
                data = linePool.Pop();
            }
            else
            {
                newCallCount++;
                getCallCount++;
                data.root = new GameObject();
                data.root.transform.SetParent(gameObject.transform);
                data.line = data.root.AddComponent<LineRenderer>();
                data.line.material = new Material(lineShader);
                data.line.startWidth = 0.01f;
                data.line.endWidth = 0.01f;
            }

            data.root.SetActive(true);

            lineActive.Push(data);

            return data.line;
        }
        #endregion //Lines Pool
    }

    public static class LineRendererUtils
    {
        //---------------------------------------------------------------------
        public static void SetColor(this LineRenderer line, Color color)
        {
            line.startColor = color;
            line.endColor = color;
        }

        //---------------------------------------------------------------------
        public static void SetDebugLine(this LineRenderer line, Vector3 start, Vector3 end)
        {
            line.SetPosition(0, start);
            line.SetPosition(1, end);

            var camera = UnityEngine.Camera.current;
            var screenStart = camera.WorldToScreenPoint(start);
            var screenEnd = camera.WorldToScreenPoint(end);
            //TODO BHU
            line.startWidth = Mathf.Max(0, 5f /*MainManager.Instance.DebugDisplayManager.DebugLineRendererWidth*/ * screenStart.z);
            line.endWidth = Mathf.Max(0, 5f /*MainManager.Instance.DebugDisplayManager.DebugLineRendererWidth*/ * screenEnd.z);
        }
    }
}
#endif //PRATEEK_DEBUG
