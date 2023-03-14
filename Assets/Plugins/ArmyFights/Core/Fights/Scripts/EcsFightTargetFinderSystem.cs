using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Scellecs.Morpeh;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsFightTargetFinderSystem))]
    public sealed class EcsFightTargetFinderSystem : LateUpdateSystem
    {
        [SerializeField]
        private float updatesPerSecond = 10;
        
        private Filter fighterFilter;
        private Filter fightableFilter;

        private Stash<EcsFightableComponent> fightableStash;
        private Stash<EcsFightTargetDataComponent> targetDataStash;
        
        private float _timer;
        
        public override void OnAwake()
        {
            fighterFilter = World.Filter.With<EcsFighterComponent>();
            fightableFilter = World.Filter.With<EcsFightableComponent>();

            fightableStash = World.GetStash<EcsFightableComponent>();
            targetDataStash = World.GetStash<EcsFightTargetDataComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (IsCanUpdate(deltaTime) == false)
            {
                return;
            }

            CleanTargets();
            
            FindTargets();
        }

        private bool IsCanUpdate(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer < 1 / updatesPerSecond)
            {
                return false;
            }

            _timer = 0;

            return true;
        }

        private void CleanTargets()
        {
            foreach (var entity in fighterFilter)
            {
                entity.RemoveComponent<EcsFightTargetDataComponent>();
            }
        }

        private void FindTargets()
        {
            foreach (var entity in fighterFilter)
            {
                FindClosestFightableEntity(entity);
            }
        }

        private void FindClosestFightableEntity(Entity entity)
        {
            var fightableComponent = fightableStash.Get(entity);
        
            var directionToClosest = Vector3.forward;
            var minSqrDistance = float.MaxValue;
            var closestEntityId = EntityId.Invalid;
            
            foreach (var possibleTarget in fightableFilter)
            {
                if (entity == possibleTarget)
                {
                    continue;
                }
        
                var possibleTargetComponent = fightableStash.Get(possibleTarget);
        
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
        
                closestEntityId = possibleTarget.ID;
                minSqrDistance = sqrDistance;
                directionToClosest = direction;
            }
        
            if (closestEntityId == EntityId.Invalid)
            {
                return;
            }

            targetDataStash.Set(entity, new EcsFightTargetDataComponent
            {
                TargetId = closestEntityId,
                DirectionToTarget = directionToClosest
            });
        }
    }
}