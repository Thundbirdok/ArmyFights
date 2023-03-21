using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Core.Team
{
    using Plugins.ArmyFights.Core.Place;
    using UnityEngine;

    [System.Serializable]
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsTeamSpawnSpotComponent
    {
        public Team team;
        
        public Vector2 soldierSpawnSpace;

        public Transform transform;

        public SquarePlace place;
        
        public Vector3 LeftBackLocalPosition => place.LeftBackLocalPosition;
        public Vector3 RightFrontLocalPosition => place.RightFrontLocalPosition;
    }
}