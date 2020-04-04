namespace Mayfair.Core.Code.DebugMenu.Pages
{
    using System;
    using Mayfair.Core.Code.DebugMenu.Fields;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class LogLevelPage : DebugMenuPage
    {
        #region Constructors
        public LogLevelPage(string title) : base(title)
        {
#if UNITY_EDITOR
            //todo DebugTools.logLevel = (DebugTools.LogLevel)EditorPrefs.GetInt(DebugTools.LOG_LEVEL_PREFS, (int)DebugTools.logLevel);
#endif
        }
        #endregion

        #region Class Methods
        public override void Draw(DebugMenuContext context)
        {
            base.Draw(context);

//todo             LabelField label = GetField<LabelField>();
//todo             label.Draw(context, DebugTools.logLevel.ToString());
//todo 
//todo             foreach (DebugTools.LogLevel value in Enum.GetValues(typeof(DebugTools.LogLevel)))
//todo             {
//todo                 ButtonField button = GetField<ButtonField>();
//todo                 if (button.Draw(context, value.ToString()))
//todo                 {
//todo                     DebugTools.logLevel = value;
//todo #if UNITY_EDITOR
//todo                     EditorPrefs.SetInt(DebugTools.LOG_LEVEL_PREFS, (int)DebugTools.logLevel);
//todo #endif
//todo                 }
//todo             }
        }
        #endregion
    }
}
