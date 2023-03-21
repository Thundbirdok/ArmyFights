using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Core.Team
{
    using Leopotam.EcsLite.Di;
    using UnityEngine;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EcsTeamSpawnSpotProvider : MonoBehaviour
    {
        [field: SerializeField]
        public EcsTeamSpawnSpotComponent TeamSpawnSpotComponent { get; private set; }

        private EcsWorldInject world;

        private EcsPoolInject<EcsTeamSpawnSpotComponent> teamSpawnSpotPool;

        private void Awake()
        {
            var entity = world.Value.NewEntity();

            teamSpawnSpotPool.Value.Add(entity);
        }
    }
}