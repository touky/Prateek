// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

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
        //-----------------------------------------------------------------------------------------
        #region Declarations
        public struct LineData
        {
            public GameObject Root;
            public LineRenderer Line;
        }
        #endregion //Declarations

        //-----------------------------------------------------------------------------------------
        #region Fields
        private PersonalLoggerManager m_logger_manager = new PersonalLoggerManager();

        private Stack<LineData> m_line_pool = new Stack<LineData>();
        private Stack<LineData> m_lines = new Stack<LineData>();
        private Shader m_line_shader = null;
        private int m_get_line_count = 0;
        private int m_new_line_count = 0;

        private List<Helpers.StringBlurp> m_blurps = new List<Helpers.StringBlurp>();

        #endregion //Fields

        //-----------------------------------------------------------------------------------------
        #region Settings
        [SerializeField]
        private float m_debug_line_renderer_width = 0.0025f;
        #endregion //Settings

        //-----------------------------------------------------------------------------------------
        #region Properties
        public float DebugLineRendererWidth { get { return m_debug_line_renderer_width; } }
        #endregion //Properties


        //---------------------------------------------------------------------------------------
        #region Unity Defaults
        void LateUpdate()
        {
            Draw.EndFrame();

            if (m_lines.Count > 0)
            {
                StartCoroutine(RefreshPool());
            }

            m_logger_manager.DisplayDebug();
        }

        //-----------------------------------------------------------------------------------------
        void OnGUI()
        {
            m_logger_manager.DisplayGUI();
        }
        #endregion //Unity Defaults

        //---------------------------------------------------------------------------------------
        #region OCP Black Box
        public void Register(Helpers.PersonalLogger logger)
        {
            if (m_logger_manager == null)
                m_logger_manager = new PersonalLoggerManager();
            m_logger_manager.Register(logger);
        }

        //---------------------------------------------------------------------------------------
        public void Unregister(Helpers.PersonalLogger logger)
        {
            m_logger_manager.Unregister(logger);
        }
        #endregion //OCP Black Box

        //---------------------------------------------------------------------------------------
        #region Lines Pool
        public IEnumerator RefreshPool()
        {
            yield return new WaitForEndOfFrame();

            while (m_lines.Count > 0)
            {
                var data = m_lines.Pop();
                data.Root.SetActive(false);
                m_line_pool.Push(data);
                m_get_line_count--;
            }
        }

        //---------------------------------------------------------------------------------------
        public LineRenderer GetLine()
        {
            if (m_line_shader == null)
            {
                m_line_shader = Shader.Find("Particles/Alpha Blended Premultiply");
            }

            LineData data = new LineData();
            if (m_line_pool.Count > 0)
            {
                m_get_line_count++;
                data = m_line_pool.Pop();
            }
            else
            {
                m_new_line_count++;
                m_get_line_count++;
                data.Root = new GameObject();
                data.Root.transform.SetParent(gameObject.transform);
                data.Line = data.Root.AddComponent<LineRenderer>();
                data.Line.material = new Material(m_line_shader);
                data.Line.startWidth = 0.01f;
                data.Line.endWidth = 0.01f;
            }

            data.Root.SetActive(true);

            m_lines.Push(data);

            return data.Line;
        }
        #endregion //Lines Pool
    }

    public static class LineRendererUtils
    {
        //---------------------------------------------------------------------------------------
        public static void SetColor(this LineRenderer line, Color color)
        {
            line.startColor = color;
            line.endColor = color;
        }

        //---------------------------------------------------------------------------------------
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
