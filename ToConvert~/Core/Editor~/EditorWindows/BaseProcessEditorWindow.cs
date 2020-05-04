namespace Mayfair.Core.Editor.EditorWindows
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.StateMachines;
    using Mayfair.Core.Code.StateMachines.Interfaces;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Editor.GUI;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    public class BaseProcessEditorWindow : BaseProcessEnumGuiContainer,
                                           ISequentialStateMachineOwner<EditorWorkState>
    {
        #region Settings
        [SerializeField]
        [HideInInspector]
        protected EditorWorkState saveState = EditorWorkState.Idle;

        [SerializeField]
        [HideInInspector]
        private bool requestedAutoExec = false;
        #endregion

        #region Fields
        protected SequentialStateMachine<EditorWorkState> stateMachine;
        protected ProcessingStatusType processingStatus = ProcessingStatusType.None;
        protected int processingPass = 0;

        private List<GUIDrawAction> guiDrawOrder = new List<GUIDrawAction>();

        private GUIStyle autoStartStyleBG;
        private GUIStyle autoStartStyle;

        private string[] fieldSlots = new string[(int) FieldSlot.MAX];
        private List<string> dataFiles = new List<string>();

        protected bool shouldStartWork;
        private bool forceOneRepaint;

        protected int processPassCount = 1;
        protected string cmdLineFileName = string.Empty;
        protected string cmdLineArguments = string.Empty;
        protected int exitCode = -1;

        private bool allowWorkStart = true;
        private string startButtonText = string.Empty;
        private string showCmdLineTag = "ShowCmdLineArguments";
        private bool showCmdLineArguments;

        private string runTestModeTag = "RunTestMode";
        private bool runTestMode = true;

        private int workRefresherPos;
        private int maxWorkRefresher = 10;

        private double autoExecOpenTimeMark = double.MaxValue;

        //private double autoExecFadeTimeMark = double.MaxValue;
        private double autoExecCloseTimeMark = double.MaxValue;
        private string showAutoExecTag = "ShowAutoStart";
        private bool showAutoExec;

        protected List<string> autoExecuteFiles = new List<string>();

        private bool waitForCompilingEnd;
        private bool waitForCompilingEndValidated;
        private double waitForCompilingTimeMark = double.MaxValue;
        #endregion

        #region Properties
        protected virtual double WaitForCompilingDuration
        {
            get { return 3f; }
        }

        protected virtual double AutoExecOpenDuration
        {
            get { return 3.5; }
        }

        protected virtual double AutoExecCloseDuration
        {
            get { return 2; }
        }

        private bool AutoExecStartDepleted
        {
            get { return EditorApplication.timeSinceStartup > autoExecOpenTimeMark + AutoExecOpenDuration; }
        }

        private bool AutoExecStopDepleted
        {
            get { return EditorApplication.timeSinceStartup > autoExecCloseTimeMark + AutoExecCloseDuration; }
        }

        protected bool IsInAutoStart
        {
            get
            {
                return stateMachine.CurrentState == EditorWorkState.AutoExecute
                    || stateMachine.CurrentState == EditorWorkState.WaitForAutoExecute;
            }
        }

        protected bool IsInAutoClose
        {
            get
            {
                return stateMachine.CurrentState == EditorWorkState.AutoStop
                    || stateMachine.CurrentState == EditorWorkState.WaitForAutoStop;
            }
        }

        private float AutoStartPercent
        {
            get { return Mathf.Clamp01((float) ((EditorApplication.timeSinceStartup - autoExecOpenTimeMark) / AutoExecOpenDuration)); }
        }

        private float AutoClosePercent
        {
            get { return 1f - Mathf.Clamp01((float) ((EditorApplication.timeSinceStartup - autoExecCloseTimeMark) / AutoExecCloseDuration)); }
        }

        protected bool AllowWorkStart
        {
            get { return !IsWorking && allowWorkStart; }
        }

        protected bool RunTestMode
        {
            get { return runTestMode; }
        }

        protected virtual bool IsWorking
        {
            get
            {
                return stateMachine.CurrentState != EditorWorkState.Init
                    && stateMachine.CurrentState != EditorWorkState.CheckForCompiling
                    && stateMachine.CurrentState != EditorWorkState.WaitForCompiling
                    && stateMachine.CurrentState != EditorWorkState.AutoExecute
                    && stateMachine.CurrentState != EditorWorkState.WaitForAutoExecute
                    && stateMachine.CurrentState != EditorWorkState.AutoStop
                    && stateMachine.CurrentState != EditorWorkState.WaitForAutoStop
                    && stateMachine.CurrentState != EditorWorkState.Idle;
            }
        }

        protected bool IsCompilingDone
        {
            get { return !waitForCompilingEnd; }
        }
        #endregion

        #region Class Methods
        protected override void RefreshStatusOnFileExist(bool valueExists)
        {
            allowWorkStart = allowWorkStart && valueExists;
        }

        protected override void SetFieldSlotTo(FieldSlot slot, string value)
        {
            fieldSlots[(int) slot] = value;
        }

        #region Virtual GUI Logic
        protected override bool DrawGUIAction(GUIDrawAction drawAction)
        {
            switch (drawAction)
            {
                case GUIDrawAction.DrawTags:
                {
                    DrawFieldSlots();
                    return true;
                }
                case GUIDrawAction.DrawAutoStart:
                {
                    DrawAutoStart();
                    return true;
                }
                case GUIDrawAction.DrawProcess:
                {
                    DrawProcess();
                    return true;
                }
                case GUIDrawAction.DrawLogger:
                {
                    DrawLogger();
                    return true;
                }
            }

            return false;
        }
        #endregion Virtual GUI Logic
        #endregion

        #region Unity defaults
        protected override void OnEnable()
        {
            base.OnEnable();

            if (stateMachine == null)
            {
                List<EditorWorkState> states = new List<EditorWorkState>();

                states.AddRange((EditorWorkState[])Enum.GetValues(typeof(EditorWorkState)));

                int index = states.FindIndex(state => state == EditorWorkState.WorkTaskCustom);

                states.InsertRange(index+1, InjectCustomStates());

                stateMachine = new SequentialStateMachine<EditorWorkState>(this, states.ToArray());
                stateMachine.Trigger(SequentialTriggerType.ForceNextState, saveState);
                saveState = EditorWorkState.Idle;
            }

            RefreshDatas();
        }

        protected virtual EditorWorkState[] InjectCustomStates()
        {
            return new EditorWorkState[] {};
        }

        protected virtual void OnInspectorUpdate()
        {
            if (IsWorking || forceOneRepaint)
            {
                forceOneRepaint = false;
                workRefresherPos = (workRefresherPos + 1) % maxWorkRefresher;
                Repaint();
            }

            stateMachine.Advance();
            if (!stateMachine.IsRunning && shouldStartWork)
            {
                shouldStartWork = false;
                stateMachine.Trigger(SequentialTriggerType.ForceNextState, EditorWorkState.ShouldStartWork);
            }

            UpdateCheckForCompiling();
        }
        
        public virtual void OnTrigger(SequentialTriggerType trigger, bool hasTriggered)
        {
        }

        public virtual void StateChange(EditorWorkState previousState, EditorWorkState nextState)
        {
            logger.Log(LOG_STORY, $"Changing state: {GetStateName(nextState)}");
        }

        public virtual string GetStateName(EditorWorkState state)
        {
            switch (state)
            {
                case EditorWorkState.WorkTaskProcessing:
                {
                    return "Starting process";
                }
                case EditorWorkState.WaitForWorkTaskProcessing:
                {
                    return "Waiting for processing";
                }
            }

            return state.ToString();
        }

        public virtual void ExecuteState(EditorWorkState state)
        {
            switch (state)
            {
                case EditorWorkState.CheckForCompiling:
                {
                    CheckForCompiling();

                    saveState = stateMachine.NextState;
                    break;
                }
                case EditorWorkState.WaitForCompiling:
                {
                    if (!IsCompilingDone)
                    {
                        stateMachine.Trigger(SequentialTriggerType.PreventStateChange);
                    }

                    break;
                }
                case EditorWorkState.AutoExecute:
                {
                    if (!requestedAutoExec)
                    {
                        stateMachine.Trigger(SequentialTriggerType.ForceNextState, EditorWorkState.ShouldStartWork);
                        break;
                    }

                    autoExecOpenTimeMark = EditorApplication.timeSinceStartup;
                    break;
                }
                case EditorWorkState.WaitForAutoExecute:
                {
                    if (autoExecuteFiles.Count == 0)
                    {
                        stateMachine.Trigger(SequentialTriggerType.ForceNextState, EditorWorkState.Idle);
                        break;
                    }

                    if (AutoExecStartDepleted)
                    {
                        if (!AllowWorkStart)
                        {
                            logger.Log(LOG_ERROR, "Auto-start FAILED, restarting timer");
                            stateMachine.Trigger(SequentialTriggerType.ForceNextState, EditorWorkState.AutoExecute);
                        }
                    }
                    else
                    {
                        stateMachine.Trigger(SequentialTriggerType.PreventStateChange);
                    }

                    break;
                }
                case EditorWorkState.ShouldStartWork:
                {
                    break;
                }
                case EditorWorkState.WorkTaskProcessing:
                {
                    StartProcessing();
                    break;
                }
                case EditorWorkState.WaitForWorkTaskProcessing:
                {
                    if (!UpdateProcessing())
                    {
                        stateMachine.Trigger(SequentialTriggerType.PreventStateChange);
                    }

                    break;
                }
                case EditorWorkState.AutoStop:
                {
                    if (!requestedAutoExec)
                    {
                        stateMachine.Trigger(SequentialTriggerType.JumpToEnd);
                        break;
                    }

                    autoExecCloseTimeMark = EditorApplication.timeSinceStartup;
                    break;
                }
                case EditorWorkState.WaitForAutoStop:
                {
                    if (AutoExecStopDepleted)
                    {
                        autoExecuteFiles.Clear();
                        requestedAutoExec = false;

                        Close();
                    }
                    else
                    {
                        stateMachine.Trigger(SequentialTriggerType.PreventStateChange);
                    }

                    break;
                }
            }
        }

        public bool Compare(EditorWorkState state0, EditorWorkState state1)
        {
            return state0 == state1;
        }

        public bool Compare(SequentialTriggerType trigger0, SequentialTriggerType trigger1)
        {
            return trigger0 == trigger1;
        }

        protected void CheckForCompiling()
        {
            waitForCompilingEnd = true;
            waitForCompilingEndValidated = false;
            waitForCompilingTimeMark = EditorApplication.timeSinceStartup;
        }

        private void UpdateCheckForCompiling()
        {
            if (waitForCompilingEnd)
            {
                if (!EditorApplication.isCompiling)
                {
                    if (waitForCompilingEndValidated
                     || EditorApplication.timeSinceStartup - waitForCompilingTimeMark > WaitForCompilingDuration)
                    {
                        waitForCompilingEnd = false;
                    }
                }
                else
                {
                    waitForCompilingEndValidated = true;
                }
            }
        }

        private void StartProcessing()
        {
            processingStatus = ProcessingStatusType.None;
            processingPass = 0;

            logger.ClearLog();

            logger.Log(LOG_TITLE, " Starting process execution");
        }

        private bool UpdateProcessing()
        {
            if (!UpdateProcessingStatus())
            {
                return false;
            }

            if (!runTestMode)
            {
                if (WaitForAdditionalProcessing())
                {
                    OnProcessEnded();
                    return true;
                }

                return false;
            }
            else
            {
                EditorUtility.DisplayDialog("TEST MODE", "Work should have started", "OK");

                OnProcessEnded();
            }

            return true;
        }

        private bool UpdateProcessingStatus()
        {
            switch (processingStatus)
            {
                case ProcessingStatusType.None:
                {
                    processingStatus = ProcessingStatusType.PreExecute;
                    break;
                }
                case ProcessingStatusType.PreExecute:
                {
                    logger.Log(LOG_STORY, " PreExecuteProcess()");
                    PreExecuteProcess();

                    logger.Log(LOG_STORY, " ExecuteProcess()");
                    processingStatus = ProcessingStatusType.Executing;
                    break;
                }
                case ProcessingStatusType.Executing:
                {
                    if (!runTestMode)
                    {
                        if (ExecuteProcess(processingPass))
                        {
                            processingPass++;
                        }
                    }

                    if (processingPass >= processPassCount)
                    {
                        processingStatus = ProcessingStatusType.PostExecute;
                    }

                    break;
                }
                case ProcessingStatusType.PostExecute:
                {
                    logger.Log(LOG_STORY, " PostExecuteProcess()");
                    PostExecuteProcess();

                    processingStatus = ProcessingStatusType.Done;
                    break;
                }
                default:
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnGUI()
        {
            //Reset tag draw order
            guiDrawOrder.Clear();

            InitGUIDrawOrder(guiDrawOrder);

            allowWorkStart = true;

            using (new EditorGUI.DisabledScope(IsWorking))
            {
                DrawWaitForCompile();

                int o = 0;
                foreach (GUIDrawAction guiOperation in guiDrawOrder)
                {
                    EditorGUILayout.Space();
                    if (!DrawGUIAction(guiOperation))
                    {
                        break;
                    }

                    o++;
                }
            }
        }
        #endregion Unity defaults

        #region Auto-start logic
        public static void DoOpenWithAutoExecute<T>(string file) where T : BaseProcessEditorWindow
        {
            T instance = GetWindow<T>();
            if (instance.autoExecuteFiles.Contains(file))
            {
                instance.Close();
                return;
            }

            if (!instance.AcceptAutoExecute(file))
            {
                instance.Close();
                return;
            }

            instance.AutoExecute();
            instance.autoExecuteFiles.Add(file);
        }

        protected virtual bool AcceptAutoExecute(string path)
        {
            return true;
        }

        protected void AutoExecute()
        {
            requestedAutoExec = true;
            stateMachine.Trigger(SequentialTriggerType.ForceNextState, EditorWorkState.CheckForCompiling);
        }

        protected void AutoPause()
        {
            autoExecOpenTimeMark = double.MaxValue;
            autoExecCloseTimeMark = double.MaxValue;
        }

        protected void AutoCancel()
        {
            AutoPause();
            requestedAutoExec = false;
        }
        #endregion Auto-start logic

        #region Main Behaviour
        protected string GetFieldSlot(FieldSlot slot)
        {
            return fieldSlots[(int) slot];
        }

        protected void BuildCommandLine()
        {
            BuildCommandFileName();

            BuildCommandArguments();
        }
        #endregion Main Behaviour

        #region Virtual Behaviour
        protected override void InitStyles()
        {
            base.InitStyles();

            if (autoStartStyleBG != null)
            {
                return;
            }

            autoStartStyleBG = GUIStyleHelper.CreateStyle("BaseProcessEditor_autoStartStyleBG", Color.black, ColorHelper.editor.background, Color.black);
            autoStartStyle = GUIStyleHelper.CreateStyle("BaseProcessEditor_autoStartStyle", ColorHelper.editor.text, ColorHelper.editor.text, ColorHelper.editor.text);
        }

        protected override void RefreshDatas()
        {
            dataFiles.Clear();
            for (int t = 0; t < (int) FieldSlot.MAX; t++)
            {
                FieldSlot slot = (FieldSlot) t;
                FieldSlotInfos infos = GetFieldSlotInfos(slot);
                if (!infos.HasEither(FieldSlotTag.Enabled))
                {
                    continue;
                }

                string value = fieldSlots[t];
                string def = infos.defaultValue;
                prefsWrapper.Get(slot, ref value, def);
                fieldSlots[t] = value;

                if (!infos.HasEither(FieldSlotTag.DataFile))
                {
                    continue;
                }

                dataFiles.Add(value);
            }
        }

        protected override FieldSlotInfos GetFieldSlotInfos(FieldSlot slot)
        {
            switch (slot)
            {
                case FieldSlot.ProcessFolder:
                {
                    return new FieldSlotInfos
                    {
                        title = "Process Directory",
                        status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.AddSpaceBefore | FieldSlotTag.RelativeToAssetPath,
                        defaultValue = "./"
                    };
                }
                case FieldSlot.ProcessFile:
                {
                    return new FieldSlotInfos
                    {
                        title = "Batch File",
                        status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsFile,
                        defaultValue = "nothing.exe"
                    };
                }
                case FieldSlot.ArgumentSrcTag:
                {
                    return new FieldSlotInfos
                    {
                        title = "Sources Tag",
                        status = FieldSlotTag.Enabled | FieldSlotTag.AddSpaceBefore,
                        defaultValue = "-SRC="
                    };
                }
                case FieldSlot.ArgumentSrcDir:
                {
                    return new FieldSlotInfos
                    {
                        title = "Sources Dir",
                        status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToAssetPath,
                        defaultValue = "./"
                    };
                }
                case FieldSlot.ArgumentDstTag:
                {
                    return new FieldSlotInfos
                    {
                        title = "Destination Tag",
                        status = FieldSlotTag.Enabled | FieldSlotTag.AddSpaceBefore,
                        defaultValue = "-DST="
                    };
                }
                case FieldSlot.ArgumentDstDir:
                {
                    return new FieldSlotInfos
                    {
                        title = "Destination Dir",
                        status = FieldSlotTag.Enabled | FieldSlotTag.NeedExistenceCheck | FieldSlotTag.IsDirectory | FieldSlotTag.RelativeToAssetPath,
                        defaultValue = "./"
                    };
                }
            }

            return new FieldSlotInfos {title = "#NO_TITLE#", status = FieldSlotTag.Nothing};
        }

        protected void ApplyDirectoryRules(FieldSlot slot, FieldSlotInfos infos, ref string value)
        {
            if (infos.HasEither(FieldSlotTag.RelativeToAssetPath))
            {
                value = Path.Combine(Application.dataPath, value);
            }
            else if (infos.HasEither(FieldSlotTag.RelativeToOtherPath))
            {
                string relPath = GetRelativePath(slot, infos);
                value = Path.Combine(relPath, value);
            }
        }

        protected void ApplyIORules(FieldSlotInfos infos, ref string value)
        {
            if (infos.HasEither(FieldSlotTag.IsFile | FieldSlotTag.IsDirectory))
            {
                value = Utils.PathHelper.Simplify(value);
            }
        }

        protected override void GetFieldSlotValues(FieldSlot slot, FieldSlotInfos infos, ref string currentValue, ref string validationValue)
        {
            currentValue = GetFieldSlot(slot);
            validationValue = currentValue;

            ApplyDirectoryRules(slot, infos, ref validationValue);

            switch (slot)
            {
                case FieldSlot.ProcessFile:
                {
                    string folder = string.Empty;
                    GetFieldSlotValues(FieldSlot.ProcessFolder, ref folder);
                    validationValue = Path.Combine(folder, currentValue);
                    break;
                }
            }

            ApplyIORules(infos, ref validationValue);
        }

        protected virtual bool DisplayWarning()
        {
            //if (!EditorUtility.DisplayDialog("Version deletion", "This will delete version:" + gameVersion + " from the store, Dave, are you sure you want to continue ?", "OK", "CANCEL"))
            //{
            //    return false;
            //}
            return true;
        }

        protected virtual void BuildCommandFileName()
        {
            GetFieldSlotValues(FieldSlot.ProcessFile, ref cmdLineFileName);
        }

        protected virtual void BuildCommandArguments()
        {
            string srcTag = string.Empty;
            string srcDir = string.Empty;
            GetFieldSlotValues(FieldSlot.ArgumentSrcTag, ref srcTag);
            GetFieldSlotValues(FieldSlot.ArgumentSrcDir, ref srcDir);

            string dstTag = string.Empty;
            string dstDir = string.Empty;
            GetFieldSlotValues(FieldSlot.ArgumentDstTag, ref dstTag);
            GetFieldSlotValues(FieldSlot.ArgumentDstDir, ref dstDir);

            cmdLineArguments = string.Format("\n{0}{1}\n{2}{3}", srcTag, srcDir, dstTag, dstDir);
        }
        #endregion Behaviour

        #region Main GUI Logic
        protected void DrawWaitForCompile()
        {
            if (waitForCompilingEnd)
            {
                GUIHelper.ShowStatusBox(Color.yellow, "Waiting for compiling to end");
            }
        }

        private void DrawAutoStart()
        {
            if (!requestedAutoExec)
            {
                return;
            }

            Repaint();

            float fade = Mathf.Clamp01(20f * (requestedAutoExec ? Mathf.Max(AutoStartPercent, AutoClosePercent) : Mathf.Min(AutoStartPercent, AutoClosePercent)));
            EditorGUILayout.BeginFadeGroup(fade);
            {
                EditorGUILayout.HelpBox("Auto build requested", MessageType.Warning);

                Rect rectButton = GUILayoutUtility.GetLastRect();
                RectHelper.TruncateX(ref rectButton, rectButton.width - Mathf.Min(rectButton.width / 2f, 360f));
                RectHelper.Inflate(ref rectButton, -4);

                Rect rectIcon = RectHelper.TruncateX(ref rectButton, rectButton.height);
                GUI.Box(rectIcon, EditorGUIUtility.IconContent("d_CollabConflict Icon"));

                Rect pauseRect = RectHelper.TruncateX(ref rectButton, rectButton.width * (2f / 3f));
                if (GUI.Button(pauseRect, "Pause auto build"))
                {
                    AutoPause();
                }

                if (GUI.Button(rectButton, "Cancel"))
                {
                    AutoCancel();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    if (IsWorking)
                    {
                        EditorGUILayout.LabelField("Waiting for process end");
                    }
                    else
                    {
                        string text = IsInAutoStart ? "Time until auto start: " : "Time until auto close: ";
                        EditorGUILayout.LabelField(text, GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
                        Rect rectStart = EditorGUILayout.GetControlRect();
                        Rect rectTime = RectHelper.TruncateX(ref rectStart, Mathf.Min(rectStart.width * 0.25f, 60f));
                        EditorGUI.LabelField(rectTime, string.Format("{0:F2}", IsInAutoStart ? AutoStartPercent * AutoExecOpenDuration : AutoClosePercent * AutoExecCloseDuration));

                        GUI.Box(rectStart, GUIContent.none, autoStartStyleBG);
                        RectHelper.Inflate(ref rectStart, -2);
                        RectHelper.Trim(ref rectStart, rectStart.width * (IsInAutoStart ? AutoStartPercent : AutoClosePercent), 0);
                        GUI.Box(rectStart, GUIContent.none, autoStartStyle);
                    }
                }

                using (EditorGUI.ChangeCheckScope change = new EditorGUI.ChangeCheckScope())
                {
                    prefsWrapper.Get(showAutoExecTag, ref showAutoExec, false);

                    showAutoExec = EditorGUILayout.Foldout(showAutoExec, "Auto start requester:", true);
                    if (showAutoExec)
                    {
                        using (new EditorGUI.IndentLevelScope(ConstsEditor.TWO_INDENT))
                        {
                            for (int f = 0; f < autoExecuteFiles.Count; f++)
                            {
                                EditorGUILayout.LabelField(autoExecuteFiles[f]);
                            }
                        }
                    }

                    if (change.changed)
                    {
                        prefsWrapper.Set(showAutoExecTag, showAutoExec);
                    }
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        protected void DrawProcess()
        {
            BuildCommandLine();

            float cmdLineWidth = 180f;
            float testModeWidth = -120f;
            float buttonMaxWidth = -140f * 2f;
            Rect cmdLineRect = EditorGUILayout.GetControlRect();
            int c = 0;
            Rect[] cmdLineRects = RectHelper.SplitX(ref cmdLineRect, cmdLineWidth, testModeWidth, buttonMaxWidth);

            //Show command line value
            using (EditorGUI.ChangeCheckScope change = new EditorGUI.ChangeCheckScope())
            {
                prefsWrapper.Get(showCmdLineTag, ref showCmdLineArguments, false);

                showCmdLineArguments = EditorGUI.Foldout(cmdLineRects[c++], showCmdLineArguments, "Show command line call", true);
                if (showCmdLineArguments)
                {
                    using (new EditorGUI.IndentLevelScope(ConstsEditor.TWO_INDENT))
                    {
                        //using (new EditorGUI.DisabledScope(true))
                        {
                            EditorGUILayout.TextArea((cmdLineFileName + cmdLineArguments).Replace("\n", "\n    "));
                        }
                    }
                }

                if (change.changed)
                {
                    prefsWrapper.Set(showCmdLineTag, showCmdLineArguments);
                }
            }

            //Show test mode button
            {
                prefsWrapper.Get(runTestModeTag, ref runTestMode, false);

                using (EditorGUI.ChangeCheckScope change = new EditorGUI.ChangeCheckScope())
                {
                    runTestMode = EditorGUI.ToggleLeft(cmdLineRects[c++], "Run in test mode", runTestMode);

                    if (change.changed)
                    {
                        prefsWrapper.Set(runTestModeTag, runTestMode);
                    }
                }
            }

            //Show Work button
            bool startWork = false;
            {
                startButtonText = "Start process";
                if (IsWorking)
                {
                    startButtonText = string.Empty;
                    for (int w = 0; w < maxWorkRefresher; w++)
                    {
                        startButtonText += w == workRefresherPos ? "X" : "x";
                    }

                    startButtonText = string.Format("Working: [{0}]", startButtonText);
                }

                using (new EditorGUI.DisabledScope(!AllowWorkStart))
                {
                    startWork = GUI.Button(cmdLineRects[c++], startButtonText);
                }

                if (startWork)
                {
                    if (!DisplayWarning())
                    {
                        startWork = false;
                    }
                }
            }

            shouldStartWork = shouldStartWork || startWork;
        }
        #endregion Main GUI Logic

        #region Process Execution
        protected virtual void PreExecuteProcess()
        {
            //Init refresher datas
            workRefresherPos = 0;
            processPassCount = 1;
        }

        protected virtual bool ExecuteProcess(int pass)
        {
            return true;
        }

        protected virtual void PostExecuteProcess() { }

        protected virtual void OnProcessEnded()
        {
            forceOneRepaint = true;
            processPassCount = 1;

            if (exitCode != 0) { }
        }

        protected virtual bool WaitForAdditionalProcessing()
        {
            return true;
        }
        #endregion Process Execution
    }
}
