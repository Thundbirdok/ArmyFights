using UnityEngine;

namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Sirenix.OdinInspector;
    using Unity.Burst;
    using Unity.Jobs;

    [BurstCompile]
    public struct MoveToTargetJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeFilter Entities;
        [ReadOnly]
        public NativeStash<EcsFighterComponent> FighterStash;
        [ReadOnly]
        public NativeStash<EcsFightableComponent> FightableStash;
        [ReadOnly]
        public NativeStash<EcsMovementComponent> MovementStash;

        public void Execute(int index)
        {
            MoveToTarget(Entities[index]);
        }

        [BurstCompile]
        private void MoveToTarget(EntityId entity)
        {
            var fighterComponent = FighterStash.Get(entity);

            ref var movementComponent = ref MovementStash.Get(entity);
                
            if (fighterComponent.TargetId == EntityId.Invalid)
            {
                movementComponent.CurrentForce = Vector3.zero;
                    
                return;
            }

            var target = FightableStash.Get(fighterComponent.TargetId);

            var direction = target.Position - movementComponent.Position;

            if (direction == Vector3.zero)
            {
                return;
            }
                
            movementComponent.Rotation = Quaternion.LookRotation(direction);
            
            var distance = direction.magnitude;

            if (distance < fighterComponent.attackDistance)
            {
                movementComponent.CurrentForce = Vector3.zero;
                    
                return;
            }

            var force = direction.normalized * movementComponent.movementForce;
            
            movementComponent.CurrentForce = force;
        }
    }
}
