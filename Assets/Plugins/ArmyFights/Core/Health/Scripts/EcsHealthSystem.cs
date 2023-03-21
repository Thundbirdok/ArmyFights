namespace Plugins.ArmyFights.Core.Health.Scripts
{
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsHealthSystem))]
    public sealed class EcsHealthSystem : IEcsRunSystem 
    {
        private EcsFilterInject<Inc<EcsHealthComponent>> filter;
        private EcsPoolInject<EcsHealthComponent> healthPool;
        private EcsPoolInject<EcsDeathMark> deathMarkPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var healthComponent = ref healthPool.Value.Get(entity);

                if (healthComponent.HealthPoints > 0)
                {
                    continue;
                }

                deathMarkPool.Value.Add(entity);
            }
        }
    }
}