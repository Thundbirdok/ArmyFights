using UnityEngine;

namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Leopotam.EcsLite;
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Unity.Burst;
    using Unity.Collections;
    using UnityEngine.Jobs;

    [BurstCompile]
    public struct RotateToTargetJob : IEcsUnityJob<EcsFightTargetDataComponent>
    {
        [ReadOnly]
        public NativeArray<int> Entities;
        
        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<EcsFightTargetDataComponent> TargetDataPool;
        
        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<int> Indices;
        
        public void Execute(int index, TransformAccess transform) 
            => RotateToTarget(Entities[index], transform);

        private void RotateToTarget(int entity, TransformAccess transform)
        {
            var poolIdx = Indices[entity];

            var targetDataComponent = TargetDataPool[poolIdx];

            transform.rotation = Quaternion.LookRotation
            (
                targetDataComponent.DirectionToTarget
            );
        }
    }
}
