namespace Assets.Prateek.CodeGenerator.Code.PrateekScript.ScriptActions
{
    using System.Collections.Generic;
    using global::Prateek.CodeGenerator;

    public class ScriptActionRegistry : TemplateRegistry
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
