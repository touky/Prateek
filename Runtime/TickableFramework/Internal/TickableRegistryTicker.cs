// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.Runtime.TickableFramework.Internal
{
    using Prateek.Runtime.TickableFramework.Enums;

    internal abstract class TickableRegistryTicker : TickableRegistryHelper
    {
        ///---------------------------------------------------------------------
        #region Properties
        internal abstract TickableSetup TickerOffset { get; }
        #endregion

        ///---------------------------------------------------------------------
        #region Unity Methods
        private void FixedUpdate()
        {
            registry.Execute((TickableSetup) ((int) TickableSetup.UpdateBeginFixed << (int) TickerOffset));
        }

        ///---------------------------------------------------------------------
        private void Update()
        {
            registry.Execute((TickableSetup) ((int) TickableSetup.UpdateBegin << (int) TickerOffset));
        }

        ///---------------------------------------------------------------------
        private void LateUpdate()
        {
            registry.Execute((TickableSetup) ((int) TickableSetup.UpdateEndLate << (int) TickerOffset));
        }
        #endregion
    }
}
