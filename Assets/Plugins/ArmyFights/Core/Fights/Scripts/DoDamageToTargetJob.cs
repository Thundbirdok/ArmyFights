namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Plugins.ArmyFights.Core.Health.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Sirenix.OdinInspector;
    using Unity.Burst;
    using Unity.Jobs;
    using UnityEngine;

    [BurstCompile]
    public struct DoDamageToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeFilter Entities;
        [ReadOnly]
        public NativeStash<EcsFighterComponent> FighterStash;
        [ReadOnly]
        public NativeStash<EcsHealthComponent> HealthStash;
        [ReadOnly]
        public float DeltaTime;
        
        public void Execute(int index)
        {
            DoDamage(Entities[index]);
        }
            
        [BurstCompile]
        private void DoDamage(EntityId entity)
        {
            ref var fighterComponent = ref FighterStash.Get(entity);

            fighterComponent.TimeAfterAttackPassed += DeltaTime;

            if (fighterComponent.TimeAfterAttackPassed < fighterComponent.attackCooldown)
            {
                return;
            }
            
            if (fighterComponent.TargetId == EntityId.Invalid)
            {
                return;
            }

            fighterComponent.TimeAfterAttackPassed = 0;
            
            ref var healthComponent = ref HealthStash.Get(fighterComponent.TargetId);

            healthComponent.HealthPoints -= fighterComponent.damage;
        }
    }
}
