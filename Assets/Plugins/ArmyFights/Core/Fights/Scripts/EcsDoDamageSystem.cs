namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Plugins.ArmyFights.Core.Health.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Scellecs.Morpeh.Systems;
    using Unity.IL2CPP.CompilerServices;
    using Unity.Jobs;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsDoDamageSystem))]
    public sealed class EcsDoDamageSystem : UpdateSystem
    {
        private Filter filter;

        private Stash<EcsFighterComponent> fighterStash;
        private Stash<EcsHealthComponent> healthStash;
        private Stash<EcsFightTargetDataComponent> targetDataStash;
        
        public override void OnAwake()
        {
            filter = World.Filter.With<EcsFightTargetDataComponent>();

            fighterStash = World.GetStash<EcsFighterComponent>();
            healthStash = World.GetStash<EcsHealthComponent>();
            targetDataStash = World.GetStash<EcsFightTargetDataComponent>();
        }

        public override void OnUpdate(float deltaTime) 
        {
            using (var nativeFilter = filter.AsNative())
            {
                var parallelJob = new DoDamageToTargetJob
                {
                    Entities = nativeFilter,
                    FighterStash = fighterStash.AsNative(),
                    HealthStash = healthStash.AsNative(),
                    TargetDataStash = targetDataStash.AsNative(),
                    DeltaTime = deltaTime
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