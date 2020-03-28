namespace Mayfair.Core.Code.Utils
{
    public static class Consts
    {
        #region Static and Constants
        //Imported from SceneAutoLoader
        //TODO benjaminh this is VERY temporary and should be fixed as soon as Tag&UniqueId second pass is done
        public static readonly string[] BUILDING_FILTERS = {"Business", "Landmark", "Decoration", "Railroad", "Residential", "Service", "Transport", "Property"};
        public const string PACK_FILTER = "Pack";
        public const string RESOURCE_FILTER = "Resource";
        public const string PROPERTY_FILTER = "Property";

        public const string UNDEFINED = "#UNDEFINED#";
        public const string UNDERSCORE_DOUBLE = "__";
        public const string UNDERSCORE_SINGLE = "_";

        public const int MIN_INIT_INT = int.MaxValue;
        public const int MAX_INIT_INT = int.MinValue;

        public const float MIN_INIT_FLOAT = float.MaxValue;
        public const float MAX_INIT_FLOAT = float.MinValue;

        public const int FAIL_VALUE = -10000;

        public const int NOT_INIT = -1;
        public const int BEFORE_ZERO = -1;
        public const int INDEX_NONE = -1;
        public const int RESET = 0;

        public const int SORT_ORDERED = -1;
        public const int SORT_EQUAL = 0;
        public const int SORT_INVERTED = 1;

        public const int ONE_ITEM = 1;

        public const int X = 0;
        public const int Y = 1;
        public const int Z = 2;

        public const int FIRST_ITEM = 0;
        public const int SECOND_ITEM = 1;
        public const int THIRD_ITEM = 2;
        public const int PREVIOUS_ITEM = -1;
        public const int NEXT_ITEM = 1;
        public const int TWO_ITEMS = 2;
        public const int COUNT_TO_INDEX = -1;

        public const int WAIT_NEXT_FRAMES = 1;
        public const int WAIT_5_FRAMES = 5;
        public const int WAIT_10_FRAMES = 10;

        public const int FIRST_PASS = 0;
        public const int SECOND_PASS = 1;
        public const int THIRD_PASS = 2;
        public const int FOURTH_PASS = 3;

        public const int TWO_PASS = 2;
        public const int THREE_PASS = 3;
        public const int FOUR_PASS = 4;
        
        public const float DEGREE_00 = 0f;
        public const float DEGREE_45 = 45f;
        public const float DEGREE_90 = 90f;

        public const float SEA_LEVEL = 0f;
        public const float ZERO_ALPHA = 0f;
        public const float MAX_ALPHA = 1f;
        #endregion
    }
}
