namespace Plugins.ArmyFights.Core.Team
{
    using Plugins.ArmyFights.Core.Place;
    using UnityEditor;
    using UnityEngine;

    public class EcsTeamSpawnSpotProviderEditor : Editor
    {
        [DrawGizmo(GizmoType.InSelectionHierarchy)]
        private static void DrawGizmos(EcsTeamSpawnSpotProvider spot, GizmoType gizmoType)
        {
            var component = spot.GetSerializedData();
            
            var leftBackPosition = component.LeftBackLocalPosition;
            var rightFrontPosition = component.RightFrontLocalPosition;

            var spotSize = rightFrontPosition - leftBackPosition;
            var spotSize2d = new Vector3(spotSize.x, 0.01f, spotSize.z);
            
            var componentTransform = component.transform;

            var localCenter = new Vector3
            (
                leftBackPosition.x + spotSize.x / 2,
                componentTransform.position.y,
                leftBackPosition.z + spotSize.z / 2
            );

            var center = componentTransform.TransformPoint(localCenter);
            
            var prevColor = Gizmos.color;

            Gizmos.color = component.team.Color * 0.8f;

            Gizmos.DrawCube(center, spotSize2d);

            var positions = PlaceUtility.GetSpaceLocalPositionsInPlace
            (
                component.place,
                component.soldierSpawnSpace
            );
            
            PlaceUtility.TransformToWorldPosition(componentTransform, positions);
            
            var soldierSize = component.soldierSpawnSpace;
            var soldierSize2d = new Vector3(soldierSize.x, 0.02f, soldierSize.y);

            foreach (var position in positions)
            {
                Gizmos.color = component.team.Color * 0.9f;
                
                Gizmos.DrawCube(position, soldierSize2d);
                
                Gizmos.color = component.team.Color * 0.7f;
                
                Gizmos.DrawWireCube(position, soldierSize2d);
            }
            
            Gizmos.color = prevColor;
        }
    }
}
