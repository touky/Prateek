namespace Assets.Prateek.ToConvert.Helpers
{
    using System.Diagnostics;
    using UnityEngine;
    using UnityEngine.Profiling;

    public static class NamingHelper
    {
        #region Class Methods
        [Conditional("UNITY_EDITOR")]
        public static void SetSingleton(MonoBehaviour target)
        {
            Profiler.BeginSample("NamingHelper.SetSingleton");
            {
                target.name = $"{target.GetType().Name}(Singleton)";
            }
            Profiler.EndSample();
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetBuildingStatus(GameObject instance, string name, bool isPlaced)
        {
            SetBuildingStatus(instance, name, Vector3Int.zero, isPlaced);
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetBuildingStatus(GameObject instance, string name, Vector3Int cellPos, bool isPlaced)
        {
            instance.name = string.Format("{0}: {1}", isPlaced ? string.Format("PLACED at {0:D2}x{1:D2}", cellPos.x, cellPos.z) : "EDIT", name);
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetTile(MonoBehaviour target, string tileName, int tileX, int tileY, bool editorStyle = false)
        {
            SetTile(target.gameObject, tileName, tileX, tileY, editorStyle);
        }

        [Conditional("UNITY_EDITOR")]
        public static void SetTile(GameObject target, string tileName, int tileX, int tileY, bool editorStyle = false)
        {
            var resultName = target.name;
            SetTile(ref resultName, tileName, tileX, tileY, editorStyle);
            target.name = resultName;
        }

        //Note: Conditional can't have 'out'
        [Conditional("UNITY_EDITOR")]
        public static void SetTile(ref string resultName, string tileName, int tileX, int tileY, bool editorStyle = false)
        {
            var result = string.Empty;
            Profiler.BeginSample("NamingHelper.SetTile");
            {
                if (editorStyle)
                {
                    result = string.Format("{0}[{1:D2}x{2:D2}]", tileName, tileX, tileY);
                }
                else
                {
                    var i0 = tileName.IndexOf('[');
                    var i1 = tileName.IndexOf('x');
                    var i2 = tileName.IndexOf(']');
                    if (i0 >= 0 && i1 >= 0 && i2 >= 0 && i1 - i0 == i2 - i1)
                    {
                        tileName = tileName.Substring(i2 + 3);
                    }

                    result = string.Format("[{1:D2}x{2:D2}]: {0}_Cell", tileName, tileX, tileY);
                }
            }
            Profiler.EndSample();

            resultName = result;
        }
        #endregion
    }
}
