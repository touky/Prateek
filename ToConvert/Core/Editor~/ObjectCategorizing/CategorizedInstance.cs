namespace Mayfair.Core.Editor.ObjectCategorizing
{
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    public class CategorizedInstance
    {
        #region Fields
        private CategorizedInstance parent;
        public Transform originalTransform;
        public Transform matchedTransform;
        public CategoryContentType category;
        private string name = string.Empty;
        private string fullName = string.Empty;
        private int indent = Consts.NOT_INIT;
        #endregion

        #region Properties
        public string Name
        {
            get
            {
                if (this.name == string.Empty && this.originalTransform != null)
                {
                    this.name = this.originalTransform.name;
                }

                return this.name;
            }
            set
            {
                this.name = value;
                this.originalTransform.name = value;
            }
        }

        public CategorizedInstance Parent
        {
            get { return this.parent; }
            set
            {
                this.parent = value;
                ClearBuiltDatas();
            }
        }

        public string FullName
        {
            get
            {
                if (this.fullName == string.Empty)
                {
                    this.fullName = this.parent != null ? this.parent.Name : string.Empty;
                    this.fullName += "/" + Name;
                }

                return this.fullName;
            }
        }

        public int Indent
        {
            get
            {
                if (this.parent != null)
                {
                    if (this.indent <= this.parent.Indent
                     || this.indent > this.parent.Indent + 1)
                    {
                        this.indent = this.parent.Indent + 1;
                    }
                }

                if (this.indent < Consts.RESET)
                {
                    this.indent = 0;
                    CategorizedInstance current = this.parent;
                    while (current != null)
                    {
                        this.indent++;
                        current = current.parent;
                    }
                }

                return this.indent;
            }
        }

        public CategorizedInstance HigherParent
        {
            get
            {
                CategorizedInstance current = this.parent;
                while (current != null)
                {
                    if (current.parent == null)
                    {
                        return current;
                    }

                    current = current.parent;
                }

                return null;
            }
        }
        #endregion

        #region Class Methods
        public void ClearBuiltDatas()
        {
            this.indent = Consts.NOT_INIT;
            this.name = this.originalTransform != null ? string.Empty : this.name;
            this.fullName = string.Empty;
        }

        public static implicit operator CategorizedInstance(Transform transform)
        {
            return new CategorizedInstance {originalTransform = transform};
        }

        public static implicit operator CategorizedInstance(string name)
        {
            return new CategorizedInstance {name = name};
        }

        public bool MatchNames(CategorizedInstance other)
        {
            return FullName == other.FullName;
        }
        #endregion
    }
}
