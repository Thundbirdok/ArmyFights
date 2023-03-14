namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Plugins.ArmyFights.Core.Transform.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Scellecs.Morpeh.Systems;
    using Unity.IL2CPP.CompilerServices;
    using Unity.Jobs;
    using UnityEngine;
    using UnityEngine.Jobs;
    using System.Linq;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsMoveToFightTargetFixedSystem))]
    public sealed class EcsMoveToFightTargetFixedSystem : FixedUpdateSystem 
    {
        private Filter filter;
        
        private Stash<EcsFighterComponent> fighterStash;
        private Stash<EcsMovementComponent> movementStash;
        private Stash<EcsTransformComponent> transformStash;
        private Stash<EcsFightTargetDataComponent> targetDataStash;
        
        private const int BATCH_COUNT = 64;
        
        public override void OnAwake()
        {
            filter = World.Filter
                .With<EcsMovementComponent>()
                .With<EcsFightTargetDataComponent>();

            fighterStash = World.GetStash<EcsFighterComponent>();
            movementStash = World.GetStash<EcsMovementComponent>();
            targetDataStash = World.GetStash<EcsFightTargetDataComponent>();
            transformStash = World.GetStash<EcsTransformComponent>();
        }

        public override void OnUpdate(float deltaTime) => CalcMovement();

        private void CalcMovement()
        {
            using (var nativeFilter = filter.AsNative())
            {
                var moveJob = new MoveToTargetJob
                {
                    Entities = nativeFilter,
                    FighterStash = fighterStash.AsNative(),
                    MovementStash = movementStash.AsNative(),
                    TargetDataStash = targetDataStash.AsNative()
                };

                var moveJobHandle = moveJob.Schedule(nativeFilter.length, BATCH_COUNT);
                
                var rotateJob = new RotateToTargetJob
                {
                    Entities = nativeFilter,
                    TargetDataStash = targetDataStash.AsNative()
                };

                var transformArray = new Transform[filter.Count()];

                var i = 0;
                foreach (var entity in filter)
                {
                    transformArray[i] = transformStash.Get(entity).transform;
                    ++i;
                }
                
                var accessArray = new TransformAccessArray(transformArray, BATCH_COUNT);
                
                var rotateJobHandle = rotateJob.Schedule(accessArray);
                
                rotateJobHandle.Complete();
                moveJobHandle.Complete();
                
                accessArray.Dispose();
            }
        }
    }
}