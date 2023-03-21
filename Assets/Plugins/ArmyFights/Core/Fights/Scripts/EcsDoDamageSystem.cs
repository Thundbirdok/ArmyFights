namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;
    using Plugins.ArmyFights.Core.Health.Scripts;
    using Unity.IL2CPP.CompilerServices;
    using Unity.Jobs;
    using UnityEngine;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsDoDamageSystem))]
    public sealed class EcsDoDamageSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EcsFightTargetDataComponent>> filter;

        private EcsPoolInject<EcsFighterComponent> fighterPool;
        private EcsPoolInject<EcsHealthComponent> healthPool;
        private EcsPoolInject<EcsFightTargetDataComponent> targetDataPool;

        public void Run(IEcsSystems systems)
        {
            using (var nativeFilter = filter.AsNative())
            {
                var parallelJob = new DoDamageToTargetJob
                {
                    Entities = nativeFilter,
                    FighterStash = fighterPool.AsNative(),
                    HealthStash = healthPool.AsNative(),
                    TargetDataStash = targetDataPool.AsNative(),
                    DeltaTime = Time.deltaTime
                };
            
                var parallelJobHandle = parallelJob.Schedule
                (
                    nativeFilter.length, 
                    64
                );
                
                parallelJobHandle.Complete();
            }
        }
    }
}