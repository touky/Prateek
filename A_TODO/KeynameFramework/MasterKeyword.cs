namespace Mayfair.Core.Code.TagSystem
{
    /// <summary>
    /// MasterKeyword should be the base class for any tag
    /// </summary>
    public abstract class MasterKeyword
    {
        public const string TYPE_USE_PARENT_SPOOF = "USE_PARENT_TYPE";
    }

    /// <summary>
    /// IMasterKeywordTag should be used to set alternative tags to keywords
    /// </summary>
    public interface IMasterKeywordTag
    {

    }

    /// <summary>
    /// Marking a keyword with this tag will make it parsed properly, but its parent type will be the keyword actually used in the keyname
    /// </summary>
    public interface IUseParentKeywordTag : IMasterKeywordTag
    {
    }
}
