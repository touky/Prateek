namespace Prateek.CodeGenerator {
    using System.Collections.Generic;
    using Prateek.CodeGenerator.ScriptTemplates;

    public struct TemplateGroup<T> where T: BaseTemplate
    {
        //-----------------------------------------------------------------
        private List<T> list;

        //-----------------------------------------------------------------
        public TemplateGroup(List<T> list)
        {
            this.list = list;
        }

        //-----------------------------------------------------------------
        public int Count { get { return list != null ? list.Count : 0; } }
        public T this[int index] { get { return list != null ? list[index] : default(T); } }
        public List<T> List { get { return list != null ? list : null; } }
    }
}