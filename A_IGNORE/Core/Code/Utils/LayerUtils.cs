namespace Mayfair.Core.Code.Utils
{
    using UnityEngine;

    public static class LayerUtils
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static readonly LayerMask LAYER_GRID           = LayerMask.NameToLayer("Grid");
        public static readonly LayerMask LAYER_GRID_FAILSAFE  = LayerMask.NameToLayer("GridFailSafe");
        public static readonly LayerMask LAYER_GRID_DETECTION = LayerMask.NameToLayer("GridDetection");
        public static readonly LayerMask LAYER_BUILDING       = LayerMask.NameToLayer("Building");
        public static readonly LayerMask LAYER_BUILDING_GHOST = LayerMask.NameToLayer("BuildingGhost");
        public static readonly LayerMask LAYER_CURRENCY       = LayerMask.NameToLayer("Currency");

        public static readonly int MASK_GRID           = 1 << LAYER_GRID.value;
        public static readonly int MASK_GRID_FAILSAFE  = 1 << LAYER_GRID_FAILSAFE.value;
        public static readonly int MASK_GRID_DETECTION = 1 << LAYER_GRID_DETECTION.value;
        public static readonly int MASK_BUILDING       = 1 << LAYER_BUILDING.value;
        public static readonly int MASK_BUILDING_GHOST = 1 << LAYER_BUILDING_GHOST.value;
        public static readonly int MASK_CURRENCY       = 1 << LAYER_CURRENCY.value;

        public static readonly int MASK_ALL = MASK_GRID
                                                | MASK_GRID_FAILSAFE
                                                | MASK_GRID_DETECTION
                                                | MASK_BUILDING
                                                | MASK_BUILDING_GHOST; // always add here the layer you added

        public static readonly int MASK_GRID_ALL     = MASK_GRID | MASK_GRID_FAILSAFE | MASK_GRID_DETECTION;
        public static readonly int MASK_BUILDING_ALL = MASK_BUILDING | MASK_BUILDING_GHOST;
    }
}
