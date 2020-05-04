namespace Mayfair.Core.Code.Types.Extensions
{
    using System.Collections;
    using UnityEngine.UI;

    public static class LayoutGroupExtensions
    {
        public static void DisableNextFrame(this LayoutGroup layoutGroup)
        {
            layoutGroup.StartCoroutine(DisableLayoutGroupNextFrame(layoutGroup));
        }

        private static IEnumerator DisableLayoutGroupNextFrame(LayoutGroup layoutGroup)
        {
            yield return null;

            if (layoutGroup != null)
            {
                layoutGroup.enabled = false;
            }
        }
    }
}