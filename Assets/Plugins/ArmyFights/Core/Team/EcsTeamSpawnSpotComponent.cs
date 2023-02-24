using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Core.Team
{
    using UnityEngine;

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsTeamSpawnSpotComponent : IComponent
    {
        public Team team;
        
        public Vector2 soldierSpawnSpace;

        public Transform transform;
        
        public Transform leftBackPoint;
        public Vector3 LeftBackLocalPosition => leftBackPoint.localPosition;
        
        public Transform rightFrontPoint;
        public Vector3 RightFrontLocalPosition => rightFrontPoint.localPosition;
    }
}