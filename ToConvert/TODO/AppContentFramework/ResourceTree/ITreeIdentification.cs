namespace Mayfair.Core.Code.Resources.ResourceTree
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public interface ITreeIdentification
    {
        #region Properties
        List<string[]> TreeTags { get; }
        #endregion
    }
}
