//#PRATEEK:#PRATEEK_DEFINE_SECTION#

namespace Prateek.Runtime.DebugFramework.DebugDraw
{
    //#PRATEEK:#PRATEEK_CSHARP_NAMESPACE_CODE#

    //#PRATEEK:#PRATEEK_USING_NAMESPACE#
    using UnityEngine;

    ///---------------------------------------------------------------------
#if ACTIVE_CODE
    public class DebugScope : GUI.Scope
#else
    internal class DebugScopeCode : GUI.Scope
#endif
    {
        ///-----------------------------------------------------------------
        #region Fields
        private DebugStyle setup;
        #endregion Fields

        ///-----------------------------------------------------------------
        #region Properties
        public DebugStyle Setup { get { return setup; } }
        #endregion Properties

        ///-----------------------------------------------------------------
        #region Scope
#if ACTIVE_CODE
        private DebugScope(DebugStyle setup) : base()
#else
        private DebugScopeCode(DebugStyle setup) : base()
#endif
        {
            this.setup = setup;
#if ACTIVE_CODE
            ScopeRegistry.Add(this);
#endif
        }

#if PRATEEK_DEBUG
        public static DebugScope Open(DebugStyle setup)
#else
        public static DebugScopeCode Open(DebugStyle setup)
#endif
        {
#if PRATEEK_DEBUG
            return new DebugScope(setup);
#else
            return null;
#endif
        }

        ///-----------------------------------------------------------------
        protected override void CloseScope()
        {
#if ACTIVE_CODE
            ScopeRegistry.Remove(this);
#endif
        }
        #endregion Scope

        //#PRATEEK:#PRATEEK_CODEGEN_DATA#
    }
}
