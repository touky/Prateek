namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.EditorJobSystem;
    using global::Prateek.CodeGenerator;
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

