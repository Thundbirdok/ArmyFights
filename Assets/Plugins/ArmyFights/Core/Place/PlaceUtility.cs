using UnityEngine;

namespace Plugins.ArmyFights.Core.Place
{
    using System.Collections.Generic;
    using Transform = UnityEngine.Transform;

    public static class PlaceUtility
    {
        public static List<Vector3> GetSpaceLocalPositionsInPlace(SquarePlace place, Vector2 space)
        {
            var positions = new List<Vector3>();

            var rightFront = place.RightFrontLocalPosition;
            var leftBack = place.LeftBackLocalPosition;

            var xDistance = rightFront.x - leftBack.x;
            var xOffset = (xDistance % space.x) / 2;

            var zDistance = rightFront.z - leftBack.z;
            var zOffset = (zDistance % space.y) / 2;

            var verticalStart = leftBack.z + zOffset + space.y / 2;
            var verticalEnd = rightFront.z - space.y / 2;

            for
            (
                var i = verticalStart;
                i <= verticalEnd;
                i += space.y
            )
            {
                var horizontalStart = leftBack.x + xOffset + space.x / 2;
                var horizontalEnd = rightFront.x - space.x / 2;

                for
                (
                    var j = horizontalStart;
                    j <= horizontalEnd;
                    j += space.x
                )
                {
                    var localPosition = new Vector3(j, 0, i);

                    positions.Add(localPosition);
                }
            }

            return positions;
        }

        public static void TransformToWorldPosition(Transform transform, List<Vector3> list)
        {
            for (var i = 0; i < list.Count; ++i)
            {
                list[i] = transform.TransformPoint(list[i]);
            }
        }
    }
}
