using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsFightTargetFinderSystem))]
    public sealed class EcsFightTargetFinderSystem : IEcsRunSystem
    {
        [SerializeField]
        private float updatesPerSecond = 10;
        
        private EcsFilterInject<Inc<EcsFighterComponent>> fighterFilter;
        private EcsFilterInject<Inc<EcsFightableComponent>> fightableFilter;

        private EcsPoolInject<EcsFighterComponent> fighterPool;
        private EcsPoolInject<EcsFightableComponent> fightablePool;
        private EcsPoolInject<EcsFightTargetDataComponent> targetDataPool;
        
        private float timer;

        public void Run(IEcsSystems systems)
        {
            if (IsCanUpdate(Time.deltaTime) == false)
            {
                return;
            }

            CleanTargets();
            
            FindTargets();
        }

        private bool IsCanUpdate(float deltaTime)
        {
            timer += deltaTime;

            if (timer < 1 / updatesPerSecond)
            {
                return false;
            }

            timer = 0;

            return true;
        }

        private void CleanTargets()
        {
            foreach (var entity in fighterFilter.Value)
            {
                fighterPool.Value.Del(entity);
            }
        }

        private void FindTargets()
        {
            foreach (var entity in fighterFilter.Value)
            {
                FindClosestFightableEntity(entity);
            }
        }

        private void FindClosestFightableEntity(int entity)
        {
            var fightableComponent = fightablePool.Value.Get(entity);
        
            var directionToClosest = Vector3.forward;
            var minSqrDistance = float.MaxValue;
            var closestEntityId = -1;
            
            foreach (var possibleTarget in fightableFilter.Value)
            {
                if (entity == possibleTarget)
                {
                    continue;
                }
        
                var possibleTargetComponent = fightablePool.Value.Get(possibleTarget);
        
                if (possibleTargetComponent.Side == fightableComponent.Side)
                {
                    continue;
                }
        
                var direction = possibleTargetComponent.Position - fightableComponent.Position;
        
                var sqrDistance = direction.sqrMagnitude;
        
                if (sqrDistance >= minSqrDistance)
                {
                    continue;
                }
        
                closestEntityId = possibleTarget;
                minSqrDistance = sqrDistance;
                directionToClosest = direction;
            }
        
            if (closestEntityId == -1)
            {
                return;
            }

            ref var fightTargetDataComponent = ref targetDataPool.Value.Add(entity);

            fightTargetDataComponent.TargetId = closestEntityId;
            fightTargetDataComponent.DirectionToTarget = directionToClosest;
        }
    }
}