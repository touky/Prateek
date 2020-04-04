namespace Mayfair.Core.Code.DebugMenu
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    //todo using Mayfair.Core.Code.Input;
    //todo using Mayfair.Core.Code.Input.InputLayers;
    //todo using Mayfair.Core.Code.Service;
    //todo using Mayfair.Core.Code.Utils.Types;
    using Prateek.DaemonCore.Code;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

#endif

    public sealed class DebugMenuDaemonCore : DaemonCore<DebugMenuDaemonCore, DebugMenuDaemonBranch>
    {
        #region Static and Constants
        private static readonly float[] MAGIC_CONTENT_SIZE = {.5f, 0.5f};
        private static readonly string PREFS_MENU_TOGGLE = $"{typeof(DebugMenuDaemonCore).Name}.MenuStatus";
        private static readonly string PREFS_SHORT_NAMES = $"{typeof(DebugMenuDaemonCore).Name}.shortNamesStatus";
        private static readonly string PREFS_SCREEN_RATIO = $"{typeof(DebugMenuDaemonCore).Name}.ScreenRatio";
        private static readonly string PREFS_SCREEN_SIDE = $"{typeof(DebugMenuDaemonCore).Name}.ScreenSide";

        private static readonly UiSettings EditorSettings = new UiSettings
        {
            titleMenuRatio = 0.3f,
            subCategoryInitialRatio = 0.4f,
            subCategoryMinimumRatio = 0.05f,
            subCategoryIncrementRatio = 0.1f,
            subCategoryMaxScreenRatio = 0.9f,
            buttonSize = new Vector2(40, 35),
            buttonFontSize = 15
        };

        private static readonly UiSettings MobileSettings = new UiSettings
        {
            titleMenuRatio = 0.3f,
            subCategoryInitialRatio = 0.4f,
            subCategoryMinimumRatio = 0.05f,
            subCategoryIncrementRatio = 0.1f,
            subCategoryMaxScreenRatio = 0.9f,
            buttonSize = new Vector2(80, 70),
            buttonFontSize = 30
        };

        private static UiSettings Settings = new UiSettings();
        #endregion

        #region Fields
        private DebugToggleStatus menuStatus = DebugToggleStatus.None;
        private DebugToggleStatus shortNamesStatus = DebugToggleStatus.None;
        private DebugToggleStatus rightSideMenu = DebugToggleStatus.None;

        private DebugMenuContext context;
        private float screenRatio = Settings.subCategoryInitialRatio;
        private Vector2 titleScrollPosition;
        private Vector2 fieldScrollPosition;

        private DebugMenuNotebook localNotebook;
        private EmptyMenuPage rootPage;
        private List<AbstractDebugNotebook> notebooks = new List<AbstractDebugNotebook>();

        private Rect titlesRect = new Rect();
        private Rect screenRect = new Rect();
        private List<EventType> eventsThisFrame = new List<EventType>();
        private int eventCount = 0;
        #endregion

        #region Properties
        private bool IsOpen
        {
            get { return Get(menuStatus, PREFS_MENU_TOGGLE, false); }
            set { Set(ref menuStatus, PREFS_MENU_TOGGLE, IsOpen, value); }
        }

        private bool UseShortNames
        {
            get { return Get(shortNamesStatus, PREFS_SHORT_NAMES, false); }
            set { Set(ref shortNamesStatus, PREFS_SHORT_NAMES, UseShortNames, value); }
        }

        private bool RightSideMenu
        {
            get { return Get(rightSideMenu, PREFS_SCREEN_SIDE, false); }
            set { Set(ref rightSideMenu, PREFS_SCREEN_SIDE, RightSideMenu, value); }
        }
        #endregion

        #region Unity Methods
        private void OnDestroy()
        {
            List<AbstractDebugNotebook> tempNotebooks = new List<AbstractDebugNotebook>(notebooks);
            foreach (AbstractDebugNotebook notebook in tempNotebooks)
            {
                notebook.Unregister();
            }

            notebooks.Clear();
        }

        private void Update()
        {
            eventsThisFrame.Clear();
            eventCount = 0;

            DrawFrame();
        }
        #endregion

        #region Unity EditorOnly Methods
        private void OnGUI()
        {
            DrawGUI();
        }
        #endregion

        protected override void OnBranchRegistered(DebugMenuDaemonBranch branch)
        {
            base.OnBranchRegistered(branch);

            branch.AddDebugContent(localNotebook, rootPage);
        }

        #region Service
        protected override void OnAwake()
        {
            //todo InputDaemonCore.RegisterLayer<DebugInputLayer>(() =>
            //todo {
            //todo     return new DebugInputLayer(this);
            //todo });

            SetupDebugContent();

            if (Application.isEditor)
            {
                Settings = EditorSettings;
            }
            else
            {
                Settings = MobileSettings;
            }

#if UNITY_EDITOR
            screenRatio = EditorPrefs.GetFloat(PREFS_SCREEN_RATIO, Settings.subCategoryInitialRatio);
#else
            screenRatio = Settings.subCategoryInitialRatio;
#endif
        }

        protected void Register(AbstractDebugNotebook notebook)
        {
            if (notebook == null)
            {
                return;
            }

            if (notebooks.Contains(notebook))
            {
                return;
            }

            notebooks.Add(notebook);

            notebooks.Sort((a, b) =>
            {
                if (a is MainDebugMenuNotebook || b is MainDebugMenuNotebook)
                {
                    return a is MainDebugMenuNotebook ? -1 : 1;
                }

                return string.Compare(a.Title, b.Title);
            });
        }
        #endregion

        #region Class Methods
        internal static bool Get(DebugToggleStatus status, string prefsName, bool defaultValue)
        {
            if (status == DebugToggleStatus.None)
            {
#if UNITY_EDITOR
                status = EditorPrefs.GetBool(prefsName, defaultValue) ? DebugToggleStatus.On : DebugToggleStatus.Off;
#else
                status = defaultValue ? DebugToggleStatus.On : DebugToggleStatus.Off;
#endif
            }

            return status == DebugToggleStatus.On;
        }

        internal static void Set(ref DebugToggleStatus status, string prefsName, bool currentValue, bool value)
        {
            if (currentValue != value)
            {
                status = value ? DebugToggleStatus.On : DebugToggleStatus.Off;
#if UNITY_EDITOR
                EditorPrefs.SetBool(prefsName, value);
#endif
            }
        }

        [Conditional("NVIZZIO_DEV")]
        public static void Toggle()
        {
            if (!Exists())
            {
                return;
            }

            Instance.IsOpen = !Instance.IsOpen;
        }

        private void SetupDebugContent()
        {
            localNotebook = new MainDebugMenuNotebook("DBUG", "Debug Menu Service");
            rootPage = new EmptyMenuPage("MAIN");
#if DEBUG_MENU_TEST_NOTEBOOK
            notebook.AddPagesWithParent(rootPage, new DebugMenuTestPage("Debug test pager"));
#endif
            localNotebook.AddPagesWithParent(rootPage, new LogLevelPage("DebugTools Log Level"));
            localNotebook.Register();
        }

        protected void Unregister(AbstractDebugNotebook binder)
        {
            if (binder == null)
            {
                return;
            }

            notebooks.Remove(binder);
        }

        private void DrawFrame()
        {
            if (context == null)
            {
                return;
            }

            //Draw all the available notebooks
            context.Reset(UseShortNames);
            for (int c = 0; c < notebooks.Count; c++)
            {
                AbstractDebugNotebook notebook = notebooks[c];
                notebook.Draw(context);
            }
        }

        private void DrawGUI()
        {
            titlesRect = new Rect();
            screenRect = new Rect();

            if (!IsOpen)
            {
                return;
            }

            if (context == null)
            {
                context = new DebugMenuContext();
            }

            context.CheckProperInit();

#if NVIZZIO_DEV
            using (new ProfilerScope("DrawGUI()"))
#endif
            {
                for (int c = 0; c < notebooks.Count; c++)
                {
                    AbstractDebugNotebook notebook = notebooks[c];
                    notebook.OnGUI();
                }

                Vector2 buttonSize = Vector2.one * 2f * context.LineHeight;
                float titlesWidth = context.GetTitlesSize() * MAGIC_CONTENT_SIZE[UseShortNames ? 0 : 1];
                titlesWidth = Mathf.Min(titlesWidth, Screen.width * Settings.titleMenuRatio);

                titlesRect = new Rect(0, 0, Mathf.Min(titlesWidth, Screen.width - buttonSize.x), Screen.height);
                SetCorrectSide(ref titlesRect);

                GUI.Box(titlesRect, GUIContent.none, context.Background);
                GUILayout.BeginArea(titlesRect);
                {
                    titleScrollPosition = GUILayout.BeginScrollView(titleScrollPosition);
                    {
                        context.DrawTitles();
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndArea();

                if (context.HasFields)
                {
#if NVIZZIO_DEV
                    using (new ProfilerScope("if (context.HasFields)"))
#endif
                    {
                        screenRect = new Rect(titlesRect.width, 0, Mathf.Min(Screen.width * screenRatio, Screen.width - buttonSize.x), Screen.height);
                        SetCorrectSide(ref screenRect);

                        GUI.Box(screenRect, GUIContent.none, context.Background);
                        GUILayout.BeginArea(screenRect);
                        {
                            fieldScrollPosition = GUILayout.BeginScrollView(
                                fieldScrollPosition,
                                false,
                                false,
                                context.HorizontalScrollBar, context.VerticalScrollBar);
                            {
                                context.DrawFields();
                            }
                            GUILayout.EndScrollView();
                        }
                        GUILayout.EndArea();

                        Rect buttonRect = GetButtonRect(screenRect);

                        DrawMenuControlButtons(buttonRect);
                    }
                }
                else
                {
#if NVIZZIO_DEV
                    using (new ProfilerScope("DrawCloseButton"))
#endif
                    {
                        Rect buttonRect = GetButtonRect(titlesRect);

                        DrawCloseButton(ref buttonRect);
                    }
                }
            }
        }

        private void SetCorrectSide(ref Rect screenRect)
        {
            if (RightSideMenu)
            {
                screenRect.x = Screen.width - (screenRect.x + screenRect.width);
            }
        }

        private Rect GetButtonRect(Rect screenRect)
        {
            Vector2 buttonSize = Settings.buttonSize;
            Rect result = new Rect(screenRect.position + (RightSideMenu ? 0 : screenRect.width) * Vector2.right, buttonSize);
            if (RightSideMenu)
            {
                result.x -= result.width;
            }

            return result;
        }

        private void DrawButton(ref Rect buttonRect, string text, GUIStyle style, Action onClick)
        {
            DrawButton(ref buttonRect, 0, text, style, onClick);
        }

        private void DrawButton(ref Rect buttonRect, float yOffset, string text, GUIStyle style, Action onClick)
        {
            buttonRect.y += buttonRect.height * yOffset;
            if (GUI.Button(buttonRect, text, style))
            {
                onClick.Invoke();
            }
        }

        private void DrawCloseButton(ref Rect buttonRect, float yOffset = 0f)
        {
            DrawButton(ref buttonRect, "X", context.Close, () => { IsOpen = false; });

            DrawButton(ref buttonRect, 1.1f, UseShortNames ? "T+" : "T-", context.Extend, () => { UseShortNames = !UseShortNames; });

            DrawButton(ref buttonRect, 1f, RightSideMenu ? "<<" : ">>", context.Extend, () => { RightSideMenu = !RightSideMenu; });
        }

        private void DrawMenuControlButtons(Rect buttonRect)
        {
            Vector2 buttonSize = Settings.buttonSize;
            int fontSize = Settings.buttonFontSize;

            int extendFontSizeReference = context.Extend.fontSize;
            int closeFontSizeReference = context.Close.fontSize;

            context.Extend.fontSize = fontSize;
            context.Close.fontSize = fontSize;

            DrawCloseButton(ref buttonRect);

            int ratioChangeValue = 0;

            DrawButton(ref buttonRect, 1.5f, ">", context.Extend, () => { ratioChangeValue = 1; });

            DrawButton(ref buttonRect, 1f, "<", context.Extend, () => { ratioChangeValue = -1; });

            if (ratioChangeValue != 0)
            {
                screenRatio = Mathf.Clamp(screenRatio + ratioChangeValue * Settings.subCategoryIncrementRatio,
                                          Settings.subCategoryMinimumRatio,
                                          Settings.subCategoryMaxScreenRatio - Settings.titleMenuRatio);
#if UNITY_EDITOR
                EditorPrefs.SetFloat(PREFS_SCREEN_RATIO, screenRatio);
#endif
            }

            buttonRect.y = screenRect.height - buttonRect.height;

            DrawButton(ref buttonRect, "A+", context.Extend, () => { context.MaxLineCount += 1; });

            DrawButton(ref buttonRect, -1f, "A-", context.Extend, () => { context.MaxLineCount -= 1; });

            context.Extend.fontSize = extendFontSizeReference;
            context.Close.fontSize = closeFontSizeReference;
        }
        #endregion

        #region Nested type: AbstractDebugNotebook
        //This class is inside DebugMenuService because I want to keep Register/Unregister protected
        public abstract class AbstractDebugNotebook : DebugMenuContent
        {
            #region Properties
            public string Title
            {
                get { return TitleField.Text; }
            }
            #endregion

            #region Constructors
            protected AbstractDebugNotebook(string title) : base(title) { }
            #endregion

            #region Service
            public void Register()
            {
                Instance.Register(this);
            }
            #endregion

            #region Class Methods
            public void Unregister()
            {
                DebugMenuDaemonCore instance = Instance;
                if (instance == null)
                {
                    return;
                }

                Instance.Unregister(this);
            }
            #endregion

            ~AbstractDebugNotebook()
            {
                Unregister();
            }
        }
        #endregion

        #region Nested type: DebugInputLayer
        //todo private class DebugInputLayer : InputLayer
        //todo {
        //todo     #region Fields
        //todo     private DebugMenuDaemonCore daemonCore;
        //todo     #endregion
        //todo 
        //todo     #region Properties
        //todo     public override bool IsActive
        //todo     {
        //todo         get { return daemonCore.IsOpen; }
        //todo     }
        //todo 
        //todo     public override bool SupportsMultiInput
        //todo     {
        //todo         get { return true; }
        //todo     }
        //todo 
        //todo     public override bool NeedActiveReceiverToLock
        //todo     {
        //todo         get { return false; }
        //todo     }
        //todo 
        //todo     public override bool CanUnlockIfActive
        //todo     {
        //todo         get { return false; }
        //todo     }
        //todo 
        //todo     public override int Priority
        //todo     {
        //todo         get { return 100000; }
        //todo     }
        //todo     #endregion
        //todo 
        //todo     #region Constructors
        //todo     public DebugInputLayer(DebugMenuDaemonCore daemonCore)
        //todo     {
        //todo         this.daemonCore = daemonCore;
        //todo     }
        //todo     #endregion
        //todo 
        //todo     #region Class Methods
        //todo     public override bool RefreshLayerLocking(Vector2 cursorPosition, RaycastHit[] hits)
        //todo     {
        //todo         return daemonCore.titlesRect.Contains(cursorPosition)
        //todo             || daemonCore.screenRect.Contains(cursorPosition);
        //todo     }
        //todo 
        //todo     public override bool RefreshLayerLocking(Vector2 cursorPosition0, RaycastHit[] hits0, Vector2 cursorPosition1, RaycastHit[] hits1)
        //todo     {
        //todo         if (!RefreshLayerLocking(cursorPosition0, hits0))
        //todo         {
        //todo             return RefreshLayerLocking(cursorPosition1, hits1);
        //todo         }
        //todo 
        //todo         return false;
        //todo     }
        //todo     #endregion
        //todo }
        #endregion

        #region Nested type: MainDebugMenuNotebook
        private class MainDebugMenuNotebook : DebugMenuNotebook
        {
            #region Constructors
            public MainDebugMenuNotebook(string title) : base(title) { }
            public MainDebugMenuNotebook(string shortTitle, string title) : base(shortTitle, title) { }
            #endregion
        }
        #endregion

        #region Nested type: UiSettings
        private class UiSettings
        {
            #region Fields
            public float titleMenuRatio = 0.3f;
            public float subCategoryMinimumRatio = 0.05f;
            public float subCategoryInitialRatio = 0.2f;
            public float subCategoryIncrementRatio = 0.1f;
            public float subCategoryMaxScreenRatio = 0.9f;
            public Vector2 buttonSize = new Vector2(40, 30);
            public int buttonFontSize = 15;
            #endregion
        }
        #endregion
    }
}
