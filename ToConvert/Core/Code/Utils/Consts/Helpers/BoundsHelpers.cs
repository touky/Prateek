namespace Mayfair.Core.Code.Utils.Helpers
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class BoundsHelpers
    {
        #region Class Methods
        public static bool MatchAxis(Bounds bound0, Bounds bound1, int axis)
        {
            return Mathf.Approximately(bound0.min[axis], bound1.min[axis]) && Mathf.Approximately(bound0.max[axis], bound1.max[axis]);
        }

        public static bool IsTouching(Bounds bound0, Bounds bound1, int axis)
        {
            return Mathf.Approximately(bound0.min[axis], bound1.max[axis]) || Mathf.Approximately(bound0.max[axis], bound1.min[axis]);
        }

        public static bool Contains(this Bounds bound0, Bounds bound1)
        {
            return bound0.Contains(bound1.min) && bound0.Contains(bound1.max);
        }

        /// <summary>
        /// Simplify bounds goes through the given list of bounds and:
        /// - Merges bounds fully contained in each other
        /// - Merges Intersecting bound if the new bounds size increase is below encapsulateSkin
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="encapsulateSkin"></param>
        public static void TrySimplifyBounds(List<Bounds> bounds, float encapsulateSkin = 0.05f)
        {
            //Simplify all bounds by merging those encapsulated fully by others
            for (int b0 = 0; b0 < bounds.Count - 1; b0++)
            {
                Bounds bound0 = bounds[b0];
                for (int b1 = b0 + 1; b1 < bounds.Count; b1++)
                {
                    Bounds bound1 = bounds[b1];

                    //No intersection, don't event test
                    if (!bound0.Intersects(bound1))
                    {
                        continue;
                    }

                    Bounds skinBounds = bound0;
                    skinBounds.Encapsulate(bound1);
                    if (!bound0.Contains(bound1) && !bound1.Contains(bound0))
                    {
                        bool skinTestFailed = (skinBounds.extents - bound0.extents).magnitude > encapsulateSkin
                                           && (skinBounds.extents - bound1.extents).magnitude > encapsulateSkin;
                        if (skinTestFailed)
                        {
                            continue;
                        }
                    }

                    Bounds tempBound = bound0;
                    tempBound.Encapsulate(bound1);

                    //Set the new bound0 and restart the logic
                    bounds[b0] = tempBound;
                    bounds.RemoveAt(b1);
                    b0 = Consts.INDEX_NONE;
                    break;
                }
            }
        }

        /// <summary>
        /// Merge bounds goes through the given list of bounds and checks if
        /// - Bounds are the same size on 2 axis
        /// - are adjacent on the third axis
        /// Then merge them
        /// </summary>
        /// <param name="bounds"></param>
        public static void TryMergingBounds(List<Bounds> bounds)
        {
            //Merge all bounds
            for (int b0 = 0; b0 < bounds.Count - 1; b0++)
            {
                Bounds bound0 = bounds[b0];
                for (int b1 = b0 + 1; b1 < bounds.Count; b1++)
                {
                    Bounds bound1 = bounds[b1];

                    //Not on the same line

                    if (!(MatchAxis(bound0, bound1, Consts.X) && MatchAxis(bound0, bound1, Consts.Y) && IsTouching(bound0, bound1, Consts.Z))
                     && !(MatchAxis(bound0, bound1, Consts.Z) && MatchAxis(bound0, bound1, Consts.Y) && IsTouching(bound0, bound1, Consts.X))
                     && !(MatchAxis(bound0, bound1, Consts.X) && MatchAxis(bound0, bound1, Consts.Z) && IsTouching(bound0, bound1, Consts.Y)))
                    {
                        continue;
                    }

                    Bounds tempBound = bound0;
                    tempBound.Encapsulate(bound1);

                    //Set the new bound0 and restart the logic
                    bounds[b0] = tempBound;
                    bounds.RemoveAt(b1);
                    b0 = Consts.INDEX_NONE;
                    break;
                }
            }
        }
        #endregion
    }
}
