namespace Assets.Prateek.ToConvert.DebugMenu.Pages
{
    using System;
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using UnityEditor;

#if UNITY_EDITOR

#endif

    public class LogLevelPage : DebugMenuPage
    {
        #region Constructors
        public LogLevelPage(string title) : base(title)
        {
#if UNITY_EDITOR
            //DebugTools.logLevel = (DebugTools.LogLevel) EditorPrefs.GetInt(DebugTools.LOG_LEVEL_PREFS, (int) DebugTools.logLevel);
#endif
        }
        #endregion

        #region Class Methods
        public override void Draw(DebugMenuContext context)
        {
            base.Draw(context);

//            var label = GetField<LabelField>();
//            label.Draw(context, DebugTools.logLevel.ToString());

//            foreach (DebugTools.LogLevel value in Enum.GetValues(typeof(DebugTools.LogLevel)))
//            {
//                var button = GetField<ButtonField>();
//                if (button.Draw(context, value.ToString()))
//                {
//                    DebugTools.logLevel = value;
//#if UNITY_EDITOR
//                    EditorPrefs.SetInt(DebugTools.LOG_LEVEL_PREFS, (int) DebugTools.logLevel);
//#endif
//                }
//            }
        }
        #endregion
    }
}
