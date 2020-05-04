namespace Prateek.CodeGeneration.Code.PrateekScript.ScriptActions
{
    using System;
    using System.Collections.Generic;
    using Prateek.CodeGeneration;
    using Prateek.EditorJobSystem.Code;
    using UnityEngine;

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

