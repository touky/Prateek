namespace Prateek.CodeGenerator.PrateekScriptBuilder {
    using System.Collections.Generic;

    public class ScriptActionDatabase : ScriptTemplate
    {
        #region Code rules
        private static List<ScriptAction> actions = new List<ScriptAction>();

        public static Group<ScriptAction> Actions
        {
            get { return new Group<ScriptAction>(actions); }
        }

        public static void Add(ScriptAction data)
        {
            actions.Add(data);
        }
        #endregion Code rules
    }
}