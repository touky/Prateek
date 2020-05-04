namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter
{
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;

    public class ResultNameParameter : LabelParameter
    {
        #region Fields
        private readonly ResultNameLabelType labelType;
        #endregion

        #region Properties
        protected override string Title
        {
            get
            {
                switch (labelType)
                {
                    case ResultNameLabelType.ShaderName:   { return "Shader name"; }
                    case ResultNameLabelType.MaterialName: { return "Material name"; }
                    case ResultNameLabelType.Location:     { return "Location"; }
                }

                return base.Title;
            }
        }

        public override SortBy Sort
        {
            get
            {
                switch (labelType)
                {
                    case ResultNameLabelType.ShaderName:   { return SortBy.Shader; }
                    case ResultNameLabelType.MaterialName: { return SortBy.Material; }
                    case ResultNameLabelType.Location:     { return SortBy.Location; }
                }

                return base.Sort;
            }
        }
        #endregion

        #region Constructors
        public ResultNameParameter(ResultNameLabelType labelType)
        {
            this.labelType = labelType;
        }
        #endregion

        #region Class Methods
        public override string GetLabel(SearchResult result)
        {
            switch (labelType)
            {
                case ResultNameLabelType.ShaderName:   { return result.shader; }
                case ResultNameLabelType.MaterialName: { return result.material; }
                case ResultNameLabelType.Location:     { return result.location; }
            }

            return base.GetLabel(result);
        }
        #endregion
    }
}
