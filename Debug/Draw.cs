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
    //-------------------------------------------------------------------------
    public static partial class Draw
    {
        //---------------------------------------------------------------------
        #region Declarations
        public class Scope : GUI.Scope
        {
            //---------------------------------------------------------------------
            #region Declarations
            #endregion Declarations

            //---------------------------------------------------------------------
            #region Fields
            #endregion Fields

            //---------------------------------------------------------------------
            protected Scope() : base() { }

            //---------------------------------------------------------------------
            protected override void CloseScope() { }
        }

        //---------------------------------------------------------------------
        public enum Space
        {
            World,
            CameraLocal,
            CameraRef,
            CameraViewRatio,
            CameraViewPixel,
        }

        //---------------------------------------------------------------------
        public partial struct Setup
        {
            //---------------------------------------------------------------------
            public enum InitMode
            {
                Reset,
                UseLast,
            }

            //---------------------------------------------------------------------
            private Space space;
            private Matrix4x4 matrix;
            private Color color;
            private float duration;
            private bool depthTest;

            //---------------------------------------------------------------------
            public Space Space { get { return space; } set { space = value; } }
            public Matrix4x4 Matrix { get { return matrix; } set { matrix = value; } }
            public Color Color { get { return color; } set { color = value; } }
            public float Duration { get { return duration; } set { duration = value; } }
            public bool DepthTest { get { return depthTest; } set { depthTest = value; } }

            //---------------------------------------------------------------------
            public Setup(InitMode initMode)
            {
                switch(initMode)
                {
                    case InitMode.UseLast:
                    //{
                    //    break;
                    //}
                    default:
                    {
                        space = Space.World;
                        matrix = Matrix4x4.identity;
                        color = Color.white;
                        duration = -1;
                        depthTest = false;
                        break;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------
        public enum SpaceType
        {
            World,
            CameraLocal,
            CameraRef,
            CameraViewRatio,
            CameraViewPixel,
        }

        //using static Prateek.ShaderTo.CSharp;
        //public static Vector4 vec4(float n_0, float n_1, float n_2, float n_3) { return new Vector4(n_0, n_1, n_2, n_3); }



        //---------------------------------------------------------------------
        public struct RenderSetup
        {
            private SpaceType space;
            private bool useMatrix;
            private Matrix4x4 matrix;
            private bool useColor;
            private Color color;
            private bool useDuration;
            private float duration;
            private bool useDepthTest;
            private bool depthTest;

            public SpaceType Space { get { return space; } set { space = value; } }
            public bool UseMatrix { get { return useMatrix; } set { useMatrix = value; if (!useMatrix) matrix = Matrix4x4.identity; } }
            public Matrix4x4 Matrix { get { return useMatrix ? matrix : Matrix4x4.identity; } set { useMatrix = true; matrix = value; } }
            public bool UseColor { get { return useColor; } set { useColor = value; if (!useColor) color = Color.white; } }
            public Color Color { get { return useColor ? color : Color.white; } set { useColor = true; color = value; } }
            public bool UseDuration { get { return useDuration; } set { useDuration = value; if (!useDuration) duration = 0.0f; } }
            public float Duration { get { return useDuration ? duration : 0.0f; } set { useDuration = true; duration = value; } }
            public bool UseDepthTest { get { return useDepthTest; } set { useDepthTest = value; if (!useDepthTest) depthTest = true; } }
            public bool DepthTest { get { return useDepthTest ? depthTest : true; } set { useDepthTest = true; depthTest = value; } }

            public RenderSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test, Matrix4x4 new_matrix)
                : this(new_space, new_color, new_duration, new_depth_test)
            {
                useMatrix = true;
                matrix = new_matrix;
            }

            public RenderSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test)
                : this(new_space, new_color, new_duration)
            {
                useDepthTest = true;
                depthTest = new_depth_test;
            }

            public RenderSetup(SpaceType new_space, Color new_color, float new_duration)
                : this(new_space, new_color)
            {
                useDuration = true;
                duration = new_duration;
            }

            public RenderSetup(SpaceType new_space, Color new_color) : this(new_space)
            {
                useColor = true;
                color = new_color;
            }

            public RenderSetup(SpaceType new_space)
            {
                space = new_space;
                useColor = false;
                color = Color.white;
                useDuration = false;
                duration = 0.0f;
                useDepthTest = false;
                depthTest = false;
                useMatrix = false;
                matrix = Matrix4x4.identity;
            }

            public RenderSetup(RenderSetup other) : this(other.space, other.color, other.duration, other.depthTest, other.matrix) { }
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

            public SpaceType space { get { return m_current_setup.Space; } set { m_current_setup.Space = value; } }

            public bool use_matrix { get { return m_current_setup.UseMatrix; } set { m_current_setup.UseMatrix = value; } }
            public Matrix4x4 matrix { get { return m_current_setup.Matrix; } set { m_current_setup.Matrix = value; } }
            public bool use_color { get { return m_current_setup.UseColor; } set { m_current_setup.UseColor = value; } }
            public Color color { get { return m_current_setup.Color; } set { m_current_setup.Color = value; } }
            public bool use_duration { get { return m_current_setup.UseDuration; } set { m_current_setup.UseDuration = value; } }
            public float duration { get { return m_current_setup.Duration; } set { m_current_setup.Duration = value; } }
            public bool use_depth_test { get { return m_current_setup.UseDepthTest; } set { m_current_setup.UseDepthTest = value; } }
            public bool depth_test { get { return m_current_setup.DepthTest; } set { m_current_setup.DepthTest = value; } }

            public ContextSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test, Matrix4x4 new_matrix)
                : this(new_space, new_color, new_duration, new_depth_test)
            {
                m_current_setup.Matrix = new_matrix;
            }

            public ContextSetup(SpaceType new_space, Color new_color, float new_duration, bool new_depth_test)
                : this(new_space, new_color, new_duration)
            {
                m_current_setup.DepthTest = new_depth_test;
            }

            public ContextSetup(SpaceType new_space, Color new_color, float new_duration)
                : this(new_space, new_color)
            {
                m_current_setup.Duration = new_duration;
            }

            public ContextSetup(SpaceType new_space, Color new_color) : this(new_space)
            {
                m_current_setup.Color = new_color;
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
            Matrix4x4 mx = (data.setup.UseMatrix ? data.setup.Matrix : Matrix4x4.identity);
            switch (data.setup.Space)
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

            data.setup.Space = SpaceType.World;
            data.setup.UseMatrix = false;

            FrameRecorder.AddLine(data);

            data.setup.Duration = useDuration ? data.setup.Duration : -1f;
            ImmediateLine(ref data);

        }

        //---------------------------------------------------------------------
#if UNITY_EDITOR
        private static void ImmediateLine(ref LineSetup data)
        {
            UnityEngine.Debug.DrawLine(data.start, data.end, data.setup.Color, data.setup.Duration, data.setup.DepthTest);
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
            RenderSetup local_setup = new RenderSetup(custom_setup != null ? custom_setup.Value.Space : m_current_setup.Space);
            local_setup.Color = m_current_setup.UseColor ? m_current_setup.Color : new Color(1, 1, 1, 1);
            local_setup.Duration = m_current_setup.UseDuration ? m_current_setup.Duration : 0.0f;
            local_setup.DepthTest = m_current_setup.UseDepthTest ? m_current_setup.DepthTest : true;
            local_setup.Matrix = m_current_setup.UseMatrix ? m_current_setup.Matrix : Matrix4x4.identity;

            if (custom_setup != null)
            {
                if (custom_setup.Value.UseColor)
                {
                    local_setup.Color = custom_setup.Value.Color;
                }

                if (custom_setup.Value.UseDuration)
                {
                    local_setup.Duration = custom_setup.Value.Duration;
                }

                if (custom_setup.Value.UseDepthTest)
                {
                    local_setup.DepthTest = custom_setup.Value.DepthTest;
                }

                if (custom_setup.Value.UseMatrix)
                {
                    local_setup.Matrix = custom_setup.Value.Matrix;
                }
            }

            //Delay screen-space lines to end of frame 
            if (local_setup.Space != SpaceType.World)
            {
                m_screen_space_lines.Add(new LineSetup() { start = start, end = end, setup = local_setup });
            }
            else
            {
                if (FrameRecorder.playback_active)
                    return;

                FrameRecorder.AddLine(new LineSetup() { start = start, end = end, setup = local_setup });

                if (local_setup.UseMatrix)
                {
                    start = local_setup.Matrix.MultiplyPoint(start);
                    end = local_setup.Matrix.MultiplyPoint(end);
                }

                var line = new LineSetup() { start = start, end = end, setup = local_setup };
                ImmediateLine(ref line);
            }
        }
        #endregion //Base primitives
    }
}
#endif //PRATEEK_DEBUG
