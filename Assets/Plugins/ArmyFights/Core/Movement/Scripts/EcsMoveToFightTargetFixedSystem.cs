namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Scellecs.Morpeh.Systems;
    using Sirenix.OdinInspector;
    using Unity.Burst;
    using Unity.IL2CPP.CompilerServices;
    using Unity.Jobs;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsMoveToFightTargetFixedSystem))]
    public sealed class EcsMoveToFightTargetFixedSystem : FixedUpdateSystem 
    {
        private Filter filter;
        
        private Stash<EcsFighterComponent> fighterStash;
        private Stash<EcsFightableComponent> fightableStash;
        private Stash<EcsMovementComponent> movementStash;

        public override void OnAwake()
        {
            filter = World.Filter
                .With<EcsFighterComponent>()
                .With<EcsFightableComponent>()
                .With<EcsMovementComponent>();

            fighterStash = World.GetStash<EcsFighterComponent>();
            fightableStash = World.GetStash<EcsFightableComponent>();
            movementStash = World.GetStash<EcsMovementComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            CalcMovement();
        }

        private void CalcMovement()
        {
            using (var nativeFilter = filter.AsNative())
            {
                var parallelJob = new MovementJob
                {
                    Entities = nativeFilter,
                    FighterStash = fighterStash.AsNative(),
                    FightableStash = fightableStash.AsNative(),
                    MovementStash = movementStash.AsNative()
                };

                var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
                
                parallelJobHandle.Complete();
            }
        }
        
        [BurstCompile]
        private struct MovementJob : IJobParallelFor
        {
            [ReadOnly]
            public NativeFilter Entities;
            public NativeStash<EcsFighterComponent> FighterStash;
            public NativeStash<EcsFightableComponent> FightableStash;
            public NativeStash<EcsMovementComponent> MovementStash;
            
            public void Execute(int index)
            {
                MoveToTarget(Entities[index]);
            }
            
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
}