// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.TickableFramework.Code.Internal
{
    using Prateek.TickableFramework.Code.Enums;

    internal abstract class TickableRegistryTicker : TickableRegistryHelper
    {
        //---------------------------------------------------------------------
        #region Properties
        internal abstract TickType TickerOffset { get; }
        #endregion

        //---------------------------------------------------------------------
        #region Unity Methods
        private void FixedUpdate()
        {
            registry.Execute((TickType) ((int) TickType.BeginUpdateFixed << (int) TickerOffset));
        }

        //---------------------------------------------------------------------
        private void Update()
        {
            registry.Execute((TickType) ((int) TickType.BeginUpdate << (int) TickerOffset));
        }

        //---------------------------------------------------------------------
        private void LateUpdate()
        {
            registry.Execute((TickType) ((int) TickType.EndUpdateLate << (int) TickerOffset));
        }
        #endregion
    }
}
