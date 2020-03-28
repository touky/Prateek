// -BEGIN_PRATEEK_COPYRIGHT-
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------

namespace Prateek.DaemonCore.Code
{
    using Prateek.Core.Code.Behaviours;

    internal abstract class DaemonRegistryTicker : NamedBehaviour
    {
        //---------------------------------------------------------------------
        #region Fields
        internal DaemonRegistry registry;
        #endregion
        
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
