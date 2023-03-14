using UnityEngine;

namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Unity.Burst;
    using Unity.Collections;
    using UnityEngine.Jobs;

    [BurstCompile]
    public struct RotateToTargetJob : IJobParallelForTransform
    {
        [ReadOnly]
        public NativeFilter Entities;

        [ReadOnly]
        public NativeStash<EcsFightTargetDataComponent> TargetDataStash;

        public void Execute(int index, TransformAccess transform) => RotateToTarget(Entities[index], transform);

        private void RotateToTarget(EntityId entity, TransformAccess transform)
        {
            var targetDataComponent = TargetDataStash.Get(entity);

            transform.rotation = Quaternion.LookRotation
            (
                targetDataComponent.DirectionToTarget
            );
        }
    }
}
