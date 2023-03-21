namespace Plugins.ArmyFights.Core.SoldiersSpawner.Scripts
{
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Plugins.ArmyFights.Core.GameObject.Scripts;
    using Plugins.ArmyFights.Core.Health.Scripts;
    using Plugins.ArmyFights.Core.Movement.Scripts;
    using Plugins.ArmyFights.Core.Place;
    using Plugins.ArmyFights.Core.Rigidbody.Scripts;
    using Plugins.ArmyFights.Core.Soldier;
    using Plugins.ArmyFights.Core.Team;
    using Plugins.ArmyFights.Core.Transform.Scripts;
    using Plugins.ArmyFights.Example.Scripts;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(EcsSoldiersSpawner))]
    public sealed class EcsSoldiersSpawner : IEcsInitSystem
    {
        [SerializeField]
        private GameObject soldierPrefab;

        private EcsWorldInject world;
        
        private EcsFilterInject<Inc<EcsTeamSpawnSpotComponent>> teamsFilter;
        private EcsFilterInject<Inc<EcsSoldierContainerComponent>> containerFilter;

        private EcsPoolInject<EcsSoldierContainerComponent> containerPool;
        private EcsPoolInject<EcsTeamSpawnSpotComponent> spawnSpotPool;

        private EcsPoolInject<EcsGameObjectComponent> gameObjectPool;
        private EcsPoolInject<EcsTransformComponent> transformPool;
        private EcsPoolInject<EcsRigidbodyComponent> rigidbodyPool;
        private EcsPoolInject<EcsHealthComponent> healthPool;
        private EcsPoolInject<EcsMovementComponent> movementPool;
        private EcsPoolInject<EcsFightableComponent> fightablePool;
        private EcsPoolInject<EcsFighterComponent> fighterPool;
        private EcsPoolInject<EcsColorControllerComponent> colorControllerPool;

        public void Init(IEcsSystems systems)
        {
            var container = GetContainer();

            Spawn(container);
        }

        private EcsSoldierContainerComponent GetContainer()
        {
            var containerId = containerFilter.Value.GetEnumerator().Current;

            var container = containerPool.Value.Get(containerId);

            return container;
        }

        private void Spawn(EcsSoldierContainerComponent container)
        {
            foreach (var entity in teamsFilter.Value)
            {
                var team = spawnSpotPool.Value.Get(entity);

                var positions = PlaceUtility.GetSpaceLocalPositionsInPlace
                (
                    team.place,
                    team.soldierSpawnSpace
                );
                
                PlaceUtility.TransformToWorldPosition(team.transform, positions);

                foreach (var position in positions)
                {
                    CreateSoldier
                    (
                        container.transform, 
                        position,
                        team.transform.rotation,
                        team.team.Side,
                        team.team.Color
                    );
                }
            }
        }

        private void CreateSoldier
        (
            Transform container, 
            Vector3 position, 
            Quaternion rotation,
            bool side,
            Color color
        )
        {
            var soldier = Object.Instantiate
            (
                soldierPrefab,
                position, 
                rotation,
                container
            );

            var entity = world.Value.NewEntity();
            
            if (soldier.TryGetComponent(out SoldierMonoProvider soldierProvider))
            {
                soldierProvider.AddToEntity
                (
                    entity,
                    gameObjectPool.Value,
                    transformPool.Value,
                    rigidbodyPool.Value,
                    healthPool.Value,
                    movementPool.Value,
                    fightablePool.Value,
                    fighterPool.Value,
                    colorControllerPool.Value
                );
            }
            
            ref var healthComponent = ref healthPool.Value.Get(entity);
            healthComponent.HealthPoints = healthComponent.maxHealthPoints;
            
            ref var fightableComponent = ref fightablePool.Value.Get(entity);
            fightableComponent.Side = side;
            
            var colorControllerComponent = colorControllerPool.Value.Get(entity);
            colorControllerComponent.meshRenderer.material.color = color;
            
            var rigidbodyComponent = rigidbodyPool.Value.Get(entity);
            rigidbodyComponent.rigidbody.solverIterations = 1;
            
            var gameObjectComponent = gameObjectPool.Value.Get(entity);
            gameObjectComponent.gameObject.SetActive(true);
        }
    }
}