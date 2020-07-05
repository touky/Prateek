namespace Mayfair.Core.Editor.EditorWindows
{
    using System;
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;
#if UNITY_EDITOR

#endif

    public class BaseEditorWindow : EditorWindow
    {
        #region Static and Constants
        protected const string LOG_OPTIONS = "LOG_OPTIONS";
        #endregion

        #region Fields
        protected GUILogger logger;

        protected EditorPrefsWrapper prefsWrapper;
        #endregion

        #region Properties
        protected GUILogger.TintPrefix LOG_ERROR { get; private set; }
        protected GUILogger.TintPrefix LOG_WARNING { get; private set; }
        protected GUILogger.TintPrefix LOG_IGNORE { get; private set; }
        protected GUILogger.TintPrefix LOG_TASK { get; private set; }
        protected GUILogger.TintPrefix LOG_STORY { get; private set; }
        protected GUILogger.TintPrefix LOG_TITLE { get; private set; }
        #endregion

        #region Class Methods
        protected void DrawLogger()
        {
            InitLogger();

            bool foldout = false;
            this.prefsWrapper.Get(LOG_OPTIONS, ref foldout);
            using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
            {
                foldout = EditorGUILayout.Foldout(foldout, "Logging options", true);
                if (foldout)
                {
                    using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
                    {
                        DrawLogOptions();
                    }
                }

                if (scope.changed)
                {
                    this.prefsWrapper.Set(LOG_OPTIONS, foldout);
                }
            }

            this.logger.OnGUI(EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true)));
        }
        #endregion

        #region Unity defaults
        protected static void DoOpen<T>(string title) where T : BaseEditorWindow
        {
            T window = GetWindow<T>();
            window.Focus();
            window.titleContent = new GUIContent(title);
        }

        protected virtual void OnEnable()
        {
            InitStyles();

            InitPrefHelper();

            InitLogger();
        }

        protected virtual void OnGUI()
        {
            DrawLogger();
        }
        #endregion Unity defaults

        #region Virtual Behaviour
        protected virtual void InitStyles() { }

        protected virtual void InitPrefHelper()
        {
            if (this.prefsWrapper != null)
            {
                return;
            }

            this.prefsWrapper = new EditorPrefsWrapper(GetType().ToString());
        }

        protected virtual void InitLogger()
        {
            if (SetProperLogger())
            {
                return;
            }

            LOG_ERROR = this.logger.AddTintPrefix("-=X", Color.red);
            LOG_WARNING = this.logger.AddTintPrefix("-=!", ColorHelper.yellow.light75);
            LOG_IGNORE = this.logger.AddTintPrefix("-=-", ColorHelper.grey.dark50);
            LOG_STORY = this.logger.AddTintPrefix("-=S", new Color(0, 0.8f, 1));
            LOG_TASK = this.logger.AddTintPrefix("-=T", ColorHelper.magenta.light75);
            LOG_TITLE = this.logger.AddTintPrefix("-=]", Color.white);
        }

        protected virtual bool SetProperLogger()
        {
            if (this.logger != null)
            {
                return false;
            }

            this.logger = new GUILogger();
            return true;
        }

        protected virtual void DrawLogOptions() { }

        protected void DrawLogOptions<T>(ref T optionFlag, Func<T, T, T> setAction) where T : struct, IConvertible
        {
            optionFlag = default;

            Array values = Enum.GetValues(typeof(T));
            for (int v = 1; v < values.Length; v++)
            {
                bool toggle = false;
                T value = (T) values.GetValue(v);
                this.prefsWrapper.Get(value, ref toggle);
                using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
                {
                    toggle = EditorGUILayout.ToggleLeft(ObjectNames.NicifyVariableName(value.ToString()), toggle);
                    if (toggle)
                    {
                        optionFlag = setAction(optionFlag, value);
                    }

                    if (scope.changed)
                    {
                        this.prefsWrapper.Set(value, toggle);
                    }
                }
            }
        }
        #endregion Virtual Behaviour
    }
}
