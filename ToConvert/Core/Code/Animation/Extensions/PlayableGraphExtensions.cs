namespace Mayfair.Core.Code.Animation.Extensions
{
    using UnityEngine.Playables;

    public static class PlayableGraphExtensions
    {
        #region Class Methods
        public static void DisconnectAll<U>(ref this PlayableGraph graph, U input) where U : struct, IPlayable
        {
            int count = input.GetInputCount();
            for (int c = 0; c < count; c++)
            {
                graph.Disconnect(input, c);
            }
        }
        #endregion
    }
}
