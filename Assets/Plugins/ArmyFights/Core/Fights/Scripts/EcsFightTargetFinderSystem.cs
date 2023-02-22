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
    public sealed class EcsFightTargetFinderSystem : UpdateSystem
    {
        [SerializeField]
        private float updatesPerSecond = 10;
        
        private Filter fighterFilter;
        private Filter fightableFilter;
        private Stash<EcsFighterComponent> fighterStash;
        private Stash<EcsFightableComponent> fightableStash;

        private float _timer;
        
        public override void OnAwake()
        {
            fighterFilter = World.Filter.With<EcsFighterComponent>();
            fightableFilter = World.Filter.With<EcsFightableComponent>();
            
            fighterStash = World.GetStash<EcsFighterComponent>();
            fightableStash = World.GetStash<EcsFightableComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            _timer += deltaTime;

            if (_timer < 1 / updatesPerSecond)
            {
                return;
            }

            _timer = 0;
                
            FindTargets();
        }

        private void FindTargets()
        {
            foreach (var entity in fighterFilter)
            {
                ref var fighterComponent = ref fighterStash.Get(entity);

                var closestEntityId = FindClosestFightableEntity(entity);

                fighterComponent.TargetId = closestEntityId;
            }
        }

        private EntityId FindClosestFightableEntity(Entity entity)
        {
            var fightableComponent = fightableStash.Get(entity);
            
            var minDistance = float.MaxValue;
            var closestEntity = EntityId.Invalid;
            
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

                var distance = Vector3.Distance
                (
                    fightableComponent.Position,
                    possibleTargetComponent.Position
                );

                if (distance >= minDistance)
                {
                    continue;
                }

                closestEntity = possibleTarget.ID;
                minDistance = distance;
            }

            return closestEntity;
        }
    }
}