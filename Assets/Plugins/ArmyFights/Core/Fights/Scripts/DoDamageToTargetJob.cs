namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Plugins.ArmyFights.Core.Health.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;

    [BurstCompile]
    public struct DoDamageToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeFilter Entities;
        
        [ReadOnly]
        public NativeStash<EcsFighterComponent> FighterStash;
        
        [ReadOnly]
        public NativeStash<EcsFightTargetDataComponent> TargetDataStash;
        
        [ReadOnly]
        public NativeStash<EcsHealthComponent> HealthStash;
        
        [ReadOnly]
        public float DeltaTime;
        
        public void Execute(int index) => DoDamage(Entities[index]);

        private void DoDamage(EntityId entity)
        {
            ref var fighterComponent = ref FighterStash.Get(entity);

            ref var targetDataComponent = ref TargetDataStash.Get(entity);

            fighterComponent.TimeAfterAttackPassed += DeltaTime;

            if (fighterComponent.TimeAfterAttackPassed < fighterComponent.attackCooldown)
            {
                return;
            }

            if (targetDataComponent.DirectionToTarget.magnitude > fighterComponent.attackDistance)
            {
                return;
            }
            
            fighterComponent.TimeAfterAttackPassed = 0;
            
            ref var healthComponent = ref HealthStash.Get(targetDataComponent.TargetId);

            healthComponent.HealthPoints -= fighterComponent.damage;
        }
    }
}
