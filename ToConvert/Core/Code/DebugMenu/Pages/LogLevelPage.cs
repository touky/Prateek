namespace Mayfair.Core.Code.DebugMenu.Pages
{
    using System;
    using Mayfair.Core.Code.DebugMenu.Fields;
    using Mayfair.Core.Code.Utils.Debug;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class LogLevelPage : DebugMenuPage
    {
        #region Constructors
        public LogLevelPage(string title) : base(title)
        {
#if UNITY_EDITOR
            DebugTools.logLevel = (DebugTools.LogLevel)EditorPrefs.GetInt(DebugTools.LOG_LEVEL_PREFS, (int)DebugTools.logLevel);
#endif
        }
        #endregion

        #region Class Methods
        public override void Draw(DebugMenuContext context)
        {
            base.Draw(context);

            LabelField label = GetField<LabelField>();
            label.Draw(context, DebugTools.logLevel.ToString());

            foreach (DebugTools.LogLevel value in Enum.GetValues(typeof(DebugTools.LogLevel)))
            {
                ButtonField button = GetField<ButtonField>();
                if (button.Draw(context, value.ToString()))
                {
                    DebugTools.logLevel = value;
#if UNITY_EDITOR
                    EditorPrefs.SetInt(DebugTools.LOG_LEVEL_PREFS, (int)DebugTools.logLevel);
#endif
                }
            }
        }
        #endregion
    }
}
