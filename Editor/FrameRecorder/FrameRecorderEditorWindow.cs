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
namespace Prateek.Editor.FrameRecorder
{
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.FrameRecorder;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
    using static Runtime.Core.Statics.Statics;

    ///-------------------------------------------------------------------------
    public class FrameRecorderEditorWindow : EditorWindow
    {
        #region Static and Constants
        private const string recordedFramesFormat = "Recorded Frames {0:00}/{1:00}";
        private const string previewingSingleFormat = "Previewing Frame: {0:00}";
        private const string previewingMultipleFormat = "Previewing Frames [{0:00} -> {1:00}]";
        #endregion

        #region Fields
        ///---------------------------------------------------------------------
        private VisualElement rootElement;

        /// <summary>
        /// </summary>
        private VisualElement controlParent;

        private VisualElement buttonParent;
        private Button toggleRecording;
        private Button togglePlayback;
        private Toggle playbackPauseGame;

        /// <summary>
        /// </summary>
        private VisualElement framesParent;

        private Label recordedFrames;
        private Label previewedFrames;
        private IntegerField frameCapacity;
        private Toggle showMultipleFrames;

        /// <summary>
        /// </summary>
        private VisualElement playbackParent;

        private SliderInt playbackSingle;
        private MinMaxSlider playbackMultiple;

        private RecorderState lastState = RecorderState.Nothing;
        private ToggleStatus lastShowMultipleFrames = ToggleStatus.Nothing;
        private double timer;
        #endregion

        #region Unity Methods
        protected virtual void OnEnable()
        {
            // Reference to the root of the window.
            rootElement = rootVisualElement;

            controlParent = new VisualElement();
            controlParent.Add(toggleRecording = new Button(ToggleRecording) {text = "--"});
            buttonParent = new VisualElement();
            buttonParent.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            buttonParent.Add(togglePlayback = new Button(TogglePlayback) {text = "--"});
            buttonParent.Add(playbackPauseGame = new Toggle("Pause game on playback ?") {viewDataKey = "playbackPauseGame"});
            controlParent.Add(new Label(" "));
            controlParent.Add(buttonParent);
            controlParent.Add(playbackPauseGame);
            controlParent.Add(new Label(" "));
            rootElement.Add(controlParent);

            framesParent = new VisualElement();
            var textParent = new VisualElement();
            textParent.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            textParent.style.alignContent = new StyleEnum<Align>(Align.Center);
            textParent.Add(recordedFrames = new Label("Recordered --/--"));
            textParent.Add(previewedFrames = new Label("Previewing --/--"));
            framesParent.Add(textParent);
            var recordParent = new VisualElement();
            recordParent.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            recordParent.style.alignContent = new StyleEnum<Align>(Align.Center);
            recordParent.Add(frameCapacity = new IntegerField("Frame capacity: ") {value = 50, viewDataKey = "frameCapacity"});
            recordParent.Add(showMultipleFrames = new Toggle("Show multiple frames ?") {viewDataKey = "showMultipleFrames"});
            framesParent.Add(recordParent);
            rootElement.Add(framesParent);

            playbackParent = new VisualElement();
            playbackParent.Add(playbackSingle = new SliderInt(0, 100) {viewDataKey = "playbackSingle"});
            playbackParent.Add(playbackMultiple = new MinMaxSlider(0, 100, 0, 100) {viewDataKey = "playbackMultiple"});
            rootElement.Add(playbackParent);
        }

        private void Update()
        {
            if (!EditorApplication.isPlaying)
            {
                ChangeState(RecorderState.Inactive);
                lastState = RecorderState.Nothing;

                rootElement.SetEnabled(false);
                return;
            }

            rootElement.SetEnabled(true);
            if (lastState != FrameRecorderEditorProxy.RecorderState)
            {
                ChangeState(FrameRecorderEditorProxy.RecorderState);
            }

            if (lastState == RecorderState.Recording)
            {
                recordedFrames.text = string.Format(recordedFramesFormat, FrameRecorderEditorProxy.FrameCapacity, FrameRecorderEditorProxy.FrameCount);
                FrameRecorderEditorProxy.FrameCapacity = frameCapacity.value;
            }

            if (lastState == RecorderState.Playback)
            {
                var newShowMultipleFrames = showMultipleFrames.value ? ToggleStatus.ON : ToggleStatus.OFF;
                if (lastShowMultipleFrames != newShowMultipleFrames)
                {
                    switch (newShowMultipleFrames)
                    {
                        case ToggleStatus.OFF:
                        {
                            playbackSingle.visible = true;
                            playbackMultiple.visible = false;

                            playbackSingle.value = (int) playbackMultiple.value.x;
                            break;
                        }
                        case ToggleStatus.ON:
                        {
                            playbackSingle.visible = false;
                            playbackMultiple.visible = true;

                            playbackMultiple.value = vec2i(playbackSingle.value);
                            break;
                        }
                    }

                    lastShowMultipleFrames = newShowMultipleFrames;
                }

                playbackMultiple.highLimit = FrameRecorderEditorProxy.FrameCount;
                playbackSingle.highValue = FrameRecorderEditorProxy.FrameCount;

                if (showMultipleFrames.value)
                {
                    previewedFrames.text = string.Format(previewingMultipleFormat, playbackMultiple.value.x, playbackMultiple.value.y);
                    FrameRecorderEditorProxy.PlaybackRange = Int(playbackMultiple.value);
                }
                else
                {
                    previewedFrames.text = string.Format(previewingSingleFormat, playbackMultiple.value.x);
                    FrameRecorderEditorProxy.PlaybackRange = vec2i(playbackSingle.value);
                }
            }
        }
        #endregion

        #region Class Methods
        ///---------------------------------------------------------------------
        [MenuItem("Prateek/Window/Frame Recorder Window")]
        private static void CreateWindow()
        {
            var window = GetWindow<FrameRecorderEditorWindow>();
            window.Show();
        }

        private void ToggleRecording()
        {
            var nextState = FrameRecorderEditorProxy.RecorderState == RecorderState.Inactive
                ? RecorderState.Recording
                : RecorderState.Inactive;

            FrameRecorderEditorProxy.RecorderState = nextState;
        }

        private void TogglePlayback()
        {
            var nextState = FrameRecorderEditorProxy.RecorderState == RecorderState.Recording
                ? RecorderState.Playback
                : RecorderState.Recording;

            FrameRecorderEditorProxy.RecorderState = nextState;
        }

        private void ChangeState(RecorderState nextState)
        {
            switch (nextState)
            {
                case RecorderState.Inactive:
                {
                    togglePlayback.SetEnabled(false);
                    framesParent.SetEnabled(false);
                    playbackParent.SetEnabled(false);

                    toggleRecording.text = "Enable Frame Recorder";
                    break;
                }
                case RecorderState.Recording:
                {
                    toggleRecording.text = "Disable Frame Recorder";
                    togglePlayback.text = "Start Playback";

                    if (playbackPauseGame.value && EditorApplication.isPaused)
                    {
                        EditorApplication.isPaused = false;
                    }

                    FrameRecorderEditorProxy.PlaybackRange = vec2i(0);
                    frameCapacity.value = FrameRecorderEditorProxy.FrameCapacity;

                    togglePlayback.SetEnabled(true);
                    framesParent.SetEnabled(false);
                    playbackParent.SetEnabled(false);
                    break;
                }
                case RecorderState.Playback:
                {
                    togglePlayback.text = "Stop Playback";

                    if (playbackPauseGame.value && !EditorApplication.isPaused)
                    {
                        EditorApplication.isPaused = true;
                    }

                    frameCapacity.value = FrameRecorderEditorProxy.FrameCapacity;

                    togglePlayback.SetEnabled(true);
                    framesParent.SetEnabled(true);
                    playbackParent.SetEnabled(true);
                    break;
                }
            }

            if (EditorApplication.isPlaying)
            {
                playbackSingle.value = FrameRecorderEditorProxy.PlaybackRange.x;
                playbackMultiple.value = FrameRecorderEditorProxy.PlaybackRange;

                lastState = nextState;
            }
        }
        #endregion

        /////---------------------------------------------------------------------
        //private void Update()
        //{
        //    var isRecord = FrameRecorder__.RecorderState == RecorderState.Recording;
        //    var isPlayback = FrameRecorder__.RecorderState == RecorderState.Playback;
        //    if (isRecord || isPlayback)
        //        Repaint();

        //    //todo if (EditorApplication.isPaused)
        //    //todo     TickableRegistry.GetManager<FrameRecorderManager>().OnFakeUpdate();
        //}
    }
}
