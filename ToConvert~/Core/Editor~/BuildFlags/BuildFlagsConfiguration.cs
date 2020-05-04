namespace Mayfair.Core.Editor.BuildFlags
{
    using System.Collections.Generic;
    using Relationship = BuildFlagsEditorWindow.SymbolRelationship;
    using Symbol = BuildFlagsConfiguration.BuildSymbolsType;

    public class BuildFlagsConfiguration
    {
        public enum BuildSymbolsType
        {
            NOTHING,

            FINAL_BUILD,

            GOOGLE_PROTOCOL,
			ADDRESSABLES_LOG_ALL,

            SKU_US,
            SKU_EUR,
            SKU_JA,

            PROFILER_LEVEL_0,
            PROFILER_LEVEL_1,
            PROFILER_LEVEL_2,

            MAYFAIR_DEBUG,
            MAYFAIR_DEBUG_THING0,
            MAYFAIR_DEBUG_THING1,

            MARKETING_VERSION,

            DEBUG_MENU_TEST_NOTEBOOK,
            NVIZZIO_DEV,

            MAX
        }

        #region Fields
        public Relationship[] relationships =
        {
            new Relationship { symbol = Symbol.SKU_US, exclude = new List<Symbol> { Symbol.SKU_EUR, Symbol.SKU_JA } },
            new Relationship { symbol = Symbol.SKU_EUR, exclude = new List<Symbol> { Symbol.SKU_JA, Symbol.SKU_US } },
            new Relationship { symbol = Symbol.SKU_JA, exclude = new List<Symbol> { Symbol.SKU_US, Symbol.SKU_EUR } },
            new Relationship { symbol = Symbol.PROFILER_LEVEL_0, exclude = new List<Symbol> { Symbol.PROFILER_LEVEL_1, Symbol.PROFILER_LEVEL_2 } },
            new Relationship { symbol = Symbol.PROFILER_LEVEL_1, exclude = new List<Symbol> { Symbol.PROFILER_LEVEL_0, Symbol.PROFILER_LEVEL_2 } },
            new Relationship { symbol = Symbol.PROFILER_LEVEL_2, exclude = new List<Symbol> { Symbol.PROFILER_LEVEL_0, Symbol.PROFILER_LEVEL_1 } },
            new Relationship { symbol = Symbol.MAYFAIR_DEBUG_THING0, include = new List<Symbol> { Symbol.MAYFAIR_DEBUG } },
            new Relationship { symbol = Symbol.MAYFAIR_DEBUG_THING1, include = new List<Symbol> { Symbol.MAYFAIR_DEBUG } }
        };
        #endregion
    }
}
