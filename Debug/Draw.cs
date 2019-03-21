// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 20/03/2019
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

#if UNITY_EDITOR
using Prateek.ScriptTemplating;
#endif //UNITY_EDITOR

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
    //-------------------------------------------------------------------------
    public static partial class Draw
    {
        //---------------------------------------------------------------------
#region Declarations
        public enum SpaceType
        {
            World,
            CameraLocal,
            CameraRef,
            CameraViewRatio,
            CameraViewPixel,
        }

        //---------------------------------------------------------------------
        public struct RenderSetup
        {
            private SpaceType m_space;
            private bool m_use_matrix;
            private Matrix4x4 m_matrix;
            private bool m_use_color;
            private Color m_color;
            private bool m_use_duration;
            private float m_duration;
            private bool m_use_depth_test;
            private bool m_depth_test;

            public SpaceType space { get { return m_space; } set { m_space = value; } }
            public bool use_matrix { get { return m_use_matrix; } set { m_use_matrix = value; if (!m_use_matrix) m_matrix = Matrix4x4.identity; } }
            public Matrix4x4 matrix { get { return m_use_matrix ? m_matrix : Matrix4x4.identity; } set { m_use_matrix = true; m_matrix = value; } }
            public bool use_color { get { return m_use_color; } set { m_use_color = value; if (!m_use_color) m_color = Color.white; } }
            public Color color { get { return m_use_color ? m_color : Color.white; } set { m_use_color = true; m_color = value; } }
            public bool use_duration { get { return m_use_duration; } set { m_use_duration = value; if (!m_use_duration) m_duration = 0.0f; } }
            public float duration { get { return m_use_duration ? m_duration : 0.0f; } set { m_use_duration = true; m_duration = value; } }
            public bool use_depth_test { get { return m_use_depth_test; } set { m_use_depth_test = value; if (!m_use_depth_test) m_depth_test = true; } }
            public bool depth_test { get { return m_use_depth_test ? m_depth_test : true; } set { m_use_depth_test = true; m_depth_test = value; } }

            public RenderSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test, Matrix4x4 new_matrix)
                : this(new_space, new_color, new_duration, new_depth_test)
            {
                m_use_matrix = true;
                m_matrix = new_matrix;
            }

            public RenderSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test)
                : this(new_space, new_color, new_duration)
            {
                m_use_depth_test = true;
                m_depth_test = new_depth_test;
            }

            public RenderSetup(SpaceType new_space, Color new_color, float new_duration)
                : this(new_space, new_color)
            {
                m_use_duration = true;
                m_duration = new_duration;
            }

            public RenderSetup(SpaceType new_space, Color new_color) : this(new_space)
            {
                m_use_color = true;
                m_color = new_color;
            }

            public RenderSetup(SpaceType new_space)
            {
                m_space = new_space;
                m_use_color = false;
                m_color = Color.white;
                m_use_duration = false;
                m_duration = 0.0f;
                m_use_depth_test = false;
                m_depth_test = false;
                m_use_matrix = false;
                m_matrix = Matrix4x4.identity;
            }

            public RenderSetup(RenderSetup other) : this(other.m_space, other.m_color, other.m_duration, other.m_depth_test, other.m_matrix) { }
        }

        //---------------------------------------------------------------------
        public struct LineSetup
        {
            public Vector3 start;
            public Vector3 end;
            public RenderSetup setup;
        }
        
        //---------------------------------------------------------------------
        public class ContextSetup
        {
            private RenderSetup m_previous_setup = new RenderSetup(SpaceType.World);

            public SpaceType space { get { return m_current_setup.space; } set { m_current_setup.space = value; } }

            public bool use_matrix { get { return m_current_setup.use_matrix; } set { m_current_setup.use_matrix = value; } }
            public Matrix4x4 matrix { get { return m_current_setup.matrix; } set { m_current_setup.matrix = value; } }
            public bool use_color { get { return m_current_setup.use_color; } set { m_current_setup.use_color = value; } }
            public Color color { get { return m_current_setup.color; } set { m_current_setup.color = value; } }
            public bool use_duration { get { return m_current_setup.use_duration; } set { m_current_setup.use_duration = value; } }
            public float duration { get { return m_current_setup.duration; } set { m_current_setup.duration = value; } }
            public bool use_depth_test { get { return m_current_setup.use_depth_test; } set { m_current_setup.use_depth_test = value; } }
            public bool depth_test { get { return m_current_setup.depth_test; } set { m_current_setup.depth_test = value; } }

            public ContextSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test, Matrix4x4 new_matrix)
                : this(new_space, new_color, new_duration, new_depth_test)
            {
                m_current_setup.matrix = new_matrix;
            }

            public ContextSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test)
                : this(new_space, new_color, new_duration)
            {
                m_current_setup.depth_test = new_depth_test;
            }

            public ContextSetup(SpaceType new_space, Color new_color, float new_duration)
                : this(new_space, new_color)
            {
                m_current_setup.duration = new_duration;
            }

            public ContextSetup(SpaceType new_space, Color new_color) : this(new_space)
            {
                m_current_setup.color = new_color;
            }
            public ContextSetup(SpaceType new_space)
            {
                //Store previous datas
                m_previous_setup = m_current_setup;

                //Store new ones
                m_current_setup = new RenderSetup(new_space);
            }

            public void ReleaseContext()
            {
                m_current_setup = m_previous_setup;
            }
        }
#endregion Declarations

        //---------------------------------------------------------------------
#region Fields
        private static RenderSetup m_current_setup = new RenderSetup(SpaceType.World);
        private static List<LineSetup> m_screen_space_lines = new List<LineSetup>();
#endregion Fields

        //---------------------------------------------------------------------
        public static void EndFrame()
        {
            if (!FrameRecorder.playback_active || FrameRecorder.frame_count == 0)
            {
                for (int i = 0; i < m_screen_space_lines.Count; ++i)
                {
                    var data = m_screen_space_lines[i];
                    DelayedLine(ref data);
                }
                m_screen_space_lines.Clear();
            }

            FrameRecorder.EndFrame();
            if (FrameRecorder.playback_active)
            {
                var currentFrame = FrameRecorder.CurrentFrame;
                for (int i = 0; i < currentFrame.m_lines.Count; ++i)
                {
                    var data = currentFrame.m_lines[i];
                    DelayedLine(ref data);
                }
            }
        }

        //---------------------------------------------------------------------
#region Line operations
        private static void DelayedLine(ref LineSetup newData, bool useDuration = true)
        {
            var camera = UnityEngine.Camera.current;
            var data = newData;
            Matrix4x4 mx = (data.setup.use_matrix ? data.setup.matrix : Matrix4x4.identity);
            switch (data.setup.space)
            {
                case SpaceType.CameraLocal: { mx = camera.transform.localToWorldMatrix * mx; break; }
                case SpaceType.CameraRef: { mx = camera.cameraToWorldMatrix * mx; break; }
                case SpaceType.CameraViewRatio:
                {
                    var mxInv = camera.localToCameraMatrix().inverse;
                    var scale = (float)camera.pixelHeight / (float)camera.pixelWidth;
                    var vScale = new Vector3(scale, 1, -1);
                    data.start = mxInv.MultiplyPoint(Vector3.Scale(data.start, vScale));
                    data.end = mxInv.MultiplyPoint(Vector3.Scale(data.end, vScale));
                    //TODO BHU -------------------------------
                    data.start.z = Mathf.Max(-data.start.z, .01f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    data.end.z = Mathf.Max(-data.end.z, 10000f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    mx = camera.transform.localToWorldMatrix * mx;
                    break;
                }
                case SpaceType.CameraViewPixel:
                {
                    var mxInv = camera.localToCameraMatrix().inverse;
                    var vScale = new Vector3(1.0f / (float)camera.pixelWidth, 1.0f / (float)camera.pixelHeight, -1);
                    data.start = mxInv.MultiplyPoint(Vector3.Scale(data.start, vScale));
                    data.end = mxInv.MultiplyPoint(Vector3.Scale(data.end, vScale));
                    data.start.z = Mathf.Max(-data.start.z, .01f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    data.end.z = Mathf.Max(-data.end.z, 10000f /*MainManager.Instance.Objects.MainCamera.nearClipPlane*/);
                    mx = camera.transform.localToWorldMatrix * mx;
                    break;
                }
            }

            data.start = mx.MultiplyPoint(data.start);
            data.end = mx.MultiplyPoint(data.end);

            data.setup.space = SpaceType.World;
            data.setup.use_matrix = false;

            FrameRecorder.AddLine(data);

            data.setup.duration = useDuration ? data.setup.duration : -1f;
            ImmediateLine(ref data);

        }

        //---------------------------------------------------------------------
#if UNITY_EDITOR
        private static void ImmediateLine(ref LineSetup data)
        {
            UnityEngine.Debug.DrawLine(data.start, data.end, data.setup.color, data.setup.duration, data.setup.depth_test);
        }
#else //UNITY_EDITOR
        private static void ImmediateLine(ref LineSetup data)
        {
            var line = MainManager.Instance.DebugDisplayManager.GetLine();
            line.SetColor(data.setup.Color);
            line.SetDebugLine(data.setup.start, data.setup.end);
        }
#endif //UNITY_EDITOR
#endregion Line operations

        //---------------------------------------------------------------------
#region Base primitives
        public static void Line(Vector3 start, Vector3 end, RenderSetup? custom_setup = null)
        {
            RenderSetup local_setup = new RenderSetup(custom_setup != null ? custom_setup.Value.space : m_current_setup.space);
            local_setup.color = m_current_setup.use_color ? m_current_setup.color : new Color(1, 1, 1, 1);
            local_setup.duration = m_current_setup.use_duration ? m_current_setup.duration : 0.0f;
            local_setup.depth_test = m_current_setup.use_depth_test ? m_current_setup.depth_test : true;
            local_setup.matrix = m_current_setup.use_matrix ? m_current_setup.matrix : Matrix4x4.identity;

            if (custom_setup != null)
            {
                if (custom_setup.Value.use_color)
                {
                    local_setup.color = custom_setup.Value.color;
                }

                if (custom_setup.Value.use_duration)
                {
                    local_setup.duration = custom_setup.Value.duration;
                }

                if (custom_setup.Value.use_depth_test)
                {
                    local_setup.depth_test = custom_setup.Value.depth_test;
                }

                if (custom_setup.Value.use_matrix)
                {
                    local_setup.matrix = custom_setup.Value.matrix;
                }
            }

            //Delay screen-space lines to end of frame 
            if (local_setup.space != SpaceType.World)
            {
                m_screen_space_lines.Add(new LineSetup() { start = start, end = end, setup = local_setup });
            }
            else
            {
                if (FrameRecorder.playback_active)
                    return;

                FrameRecorder.AddLine(new LineSetup() { start = start, end = end, setup = local_setup });

                if (local_setup.use_matrix)
                {
                    start = local_setup.matrix.MultiplyPoint(start);
                    end = local_setup.matrix.MultiplyPoint(end);
                }

                var line = new LineSetup() { start = start, end = end, setup = local_setup };
                ImmediateLine(ref line);
            }
        }
#endregion //Base primitives
    }
}
#endif //PRATEEK_DEBUG
