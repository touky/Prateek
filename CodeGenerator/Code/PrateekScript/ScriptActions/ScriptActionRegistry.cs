namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System;
    using System.Collections.Generic;
    using global::Prateek.CodeGenerator;
    using UnityEngine;

    public class ScriptActionRegistry
    {
        #region Code rules
        private static List<ScriptAction> actions = new List<ScriptAction>();

        public static TemplateGroup<ScriptAction> Actions
        {
            get { return new TemplateGroup<ScriptAction>(actions); }
        }

        public static void Add(ScriptAction data)
        {
            actions.Add(data);
        }
        #endregion Code rules
    }
}

