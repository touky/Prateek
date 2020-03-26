namespace Assets.Prateek.ToConvert.DebugMenu.Pages
{
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.DebugMenu.Content;
    using Assets.Prateek.ToConvert.DebugMenu.Fields;

    /// <summary>
    ///     Main class for debug menu pages, use this one for high-level debug menu functions.
    ///     For debug from inside of a system, use the generic one instead
    /// </summary>
    public abstract class DebugMenuPage : DebugMenuContent
    {
        #region Fields
        private Dictionary<Type, Field> fields;
        private DebugMenuPage parent;
        private int parentCount = Consts.RESET;
        private bool enabled = true;
        #endregion

        #region Properties
        public bool Enabled
        {
            get { return enabled; }
        }

        public bool HasParent
        {
            get { return parent != null; }
        }

        public int ParentCount
        {
            get { return parentCount; }
        }

        public virtual bool IgnoreIndent
        {
            get { return false; }
        }

        public bool IsActiveInHierachy
        {
            get
            {
                if (!enabled)
                {
                    return false;
                }

                if (this.parent != null)
                {
                    var parent = this.parent;
                    while (parent != null)
                    {
                        if (!parent.Enabled || !parent.ShowContent)
                        {
                            return false;
                        }

                        parent = parent.parent;
                    }
                }

                return enabled;
            }
        }

        public CategoryField NewCategory
        {
            get { return GetField<CategoryField>(); }
        }

        public LabelField NewLabel
        {
            get { return GetField<LabelField>(); }
        }

        public ButtonField NewButton
        {
            get { return GetField<ButtonField>(); }
        }
        #endregion

        #region Constructors
        protected DebugMenuPage(string title) : base(title) { }
        #endregion

        #region Class Methods
        public override void Draw(DebugMenuContext context)
        {
            ClearLastDebugFrame();
        }

        public void ClearLastDebugFrame()
        {
            if (fields != null)
            {
                var keys = new List<Type>(fields.Keys);
                for (var k = 0; k < keys.Count; k++)
                {
                    var   key   = keys[k];
                    Field value = default;
                    if (fields.TryGetValue(key, out value))
                    {
                        value.count = 0;
                        fields[key] = value;
                    }
                }
            }
        }

        public void SetParent(DebugMenuPage parent)
        {
            var parents = new HashSet<DebugMenuPage>();
            if (this == parent)
            {
                throw new Exception("Wrong parentage this == parent");
            }

            this.parent = parent;

            parentCount = Consts.RESET;
            while (parent != null)
            {
                if (parents.Contains(parent))
                {
                    throw new Exception($"Circular parentage between {TitleField} and parent {parent.TitleField}");
                }

                parentCount += parent.IgnoreIndent ? 0 : 1;
                parents.Add(parent);
                parent = parent.parent;
            }
        }

        /// <summary>
        ///     GetField uses a pool to provide you with a proper DebugField, recycled each frame.
        ///     If you need long term usage DebugField, prefer declaring a field in your MenuPage
        /// </summary>
        /// <typeparam name="TField"></typeparam>
        /// <returns></returns>
        public TField GetField<TField>() where TField : DebugField, new()
        {
            if (fields == null)
            {
                fields = new Dictionary<Type, Field>();
            }

            var type = typeof(TField);
            if (!fields.ContainsKey(type))
            {
                fields.Add(type, new Field {count = 0, fields = new List<DebugField>()});
            }

            Field value = default;
            if (fields.TryGetValue(type, out value))
            {
                if (value.count >= value.fields.Count)
                {
                    value.fields.Add(new TField());
                }

                var result = value.fields[value.count++] as TField;

                fields[type] = value;

                return result;
            }

            return null;
        }
        #endregion

        #region Nested type: Field
        private struct Field
        {
            public int count;
            public List<DebugField> fields;
        }
        #endregion
    }
}
