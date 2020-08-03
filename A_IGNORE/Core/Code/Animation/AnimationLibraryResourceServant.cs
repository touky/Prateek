namespace Mayfair.Core.Code.Animation
{
    using Mayfair.Core.Code.Animation.Messages;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Code.VisualAsset.Providers;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class AnimationLibraryResourceServant
        : ScriptableObjectResourceServant<AnimationLibrary>, IDebugMenuNotebookOwner
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
        #endregion

        #region Class Methods
        protected override void Init()
        {
            AddSupportedAssignable(PathHelper.RemoveLeadingAndTrailingSlashes(COMMON_LIBRARY_KEYWORD), DEFAULT_PREFAB_NAME);
        }

        protected override RequestAccessToContent CreateResourceChangeRequest()
        {
            return Command.Create<RequestCallbackOnScriptableResourceChange<AnimationLibraryResourceHasChanged, AnimationLibrary>>();
        }

        protected override bool IsResponseAccepted(ScriptableResourcesHaveChanged<AnimationLibrary> response)
        {
            return response is AnimationLibraryResourceHasChanged;
        }
        #endregion
    }
}