namespace Prateek.Editor.CodeGeneration.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using Prateek.Editor.EditorJobSystem;
    using Prateek.Runtime.JobFramework;

    public class ScriptActionRegistry
    {
        #region Code rules
        private static ConcurrentList<ScriptAction> actions = new ConcurrentList<ScriptAction>();

        public static IReadOnlyList<ScriptAction> Actions
        {
            get
            {
                EditorJobSystem.JoinWork();

                return actions.Copy;
            }
        }

        public static void Add(ScriptAction data)
        {
            actions.Add(data);
        }
        #endregion Code rules
    }
}

