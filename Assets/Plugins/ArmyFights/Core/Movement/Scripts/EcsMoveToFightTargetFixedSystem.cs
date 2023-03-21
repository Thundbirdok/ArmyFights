namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Plugins.ArmyFights.Core.Transform.Scripts;
    using Unity.IL2CPP.CompilerServices;
    using Unity.Jobs;
    using UnityEngine;
    using UnityEngine.Jobs;
    using System.Linq;
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    //[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsMoveToFightTargetFixedSystem))]
    public sealed class EcsMoveToFightTargetFixedSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EcsMovementComponent, EcsFightTargetDataComponent>> filter;
        
        private EcsPoolInject<EcsFighterComponent> fighterStash;
        private EcsPoolInject<EcsMovementComponent> movementStash;
        private EcsPoolInject<EcsTransformComponent> transformStash;
        private EcsPoolInject<EcsFightTargetDataComponent> targetDataStash;
        
        private const int BATCH_COUNT = 64;

        public void Run(IEcsSystems systems) => CalcMovement();

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