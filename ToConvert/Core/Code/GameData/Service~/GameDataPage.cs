namespace Mayfair.Core.Code.GameData
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Pages;

    public class GameDataPage : DebugMenuPage<GameDataService>
    {
        #region Constructors
        public GameDataPage(GameDataService owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void Draw(GameDataService owner, DebugMenuContext context)
        {
            IEnumerable<GameDataServiceProvider> providers = owner.GetAllValidProviders();
            foreach (GameDataServiceProvider provider in providers)
            {
                provider.OnDebugDraw(this, context);
            }
        }
        #endregion
    }
}
