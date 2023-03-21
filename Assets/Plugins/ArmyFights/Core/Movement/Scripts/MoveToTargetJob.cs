namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Unity.Burst;
    using Unity.Collections;
    using Unity.Jobs;
    
    [BurstCompile]
    public struct MoveToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeFilter Entities;

        [ReadOnly]
        public NativeStash<EcsFighterComponent> FighterStash;

        [ReadOnly]
        public NativeStash<EcsFightTargetDataComponent> TargetDataStash;

        [ReadOnly]
        public NativeStash<EcsMovementComponent> MovementStash;

        public void Execute(int index) => MoveToTarget(Entities[index]);

        private void MoveToTarget(int entity)
        {
            var fighterComponent = FighterStash.Get(entity);
            var targetDataComponent = TargetDataStash.Get(entity);

            ref var movementComponent = ref MovementStash.Get(entity);

            var directionToTarget = targetDataComponent.DirectionToTarget;
            var distance = directionToTarget.magnitude;

            if (distance < fighterComponent.attackDistance)
            {
                return;
            }

            var force = directionToTarget.normalized
                        * movementComponent.movementForce;

            movementComponent.CurrentForce = force;
        }
    }
}
