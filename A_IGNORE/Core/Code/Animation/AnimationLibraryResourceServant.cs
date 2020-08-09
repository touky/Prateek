namespace Mayfair.Core.Code.Animation
{
    using Mayfair.Core.Code.Animation.Messages;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Code.VisualAsset.Providers;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Commands;

    public class AnimationLibraryResourceServant
        : ScriptableObjectResourceServant<AnimationLibrary>
        , IDebugMenuNotebookOwner
    {
        #region Static and Constants
        private const string COMMON_LIBRARY_KEYWORD = "AnimationLibrary/";
        private const string DEFAULT_PREFAB_NAME = "DefaultAnimationLibrary";

        public static readonly string[] KEYWORDS = {COMMON_LIBRARY_KEYWORD};
        #endregion

        #region Properties
        public override string[] ResourceKeywords
        {
            get { return KEYWORDS; }
        }

        protected override ContentAccessRequest CreateContentAccessRequest()
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Class Methods
        protected override void Init()
        {
            AddSupportedAssignable(PathHelper.RemoveLeadingAndTrailingSlashes(COMMON_LIBRARY_KEYWORD), DEFAULT_PREFAB_NAME);
        }

        //todo protected override RequestAccessToContent CreateResourceChangeRequest()
        //todo {
        //todo     return Command.Create<RequestCallbackOnScriptableResourceChange<AnimationLibraryResourceHasChanged, AnimationLibrary>>();
        //todo }

        protected override bool IsResponseAccepted(ScriptableContentAccessChangedResponse<AnimationLibrary> response)
        {
            return response is AnimationLibraryResourceHasChangedResponse;
        }

        protected override void OnContentAccessChangedResponse(ContentAccessChangedResponse response)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
