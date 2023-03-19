namespace Plugins.ArmyFights.Core.SoldiersSpawner.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Plugins.ArmyFights.Core.GameObject.Scripts;
    using Plugins.ArmyFights.Core.Health.Scripts;
    using Plugins.ArmyFights.Core.Place;
    using Plugins.ArmyFights.Core.Rigidbody.Scripts;
    using Plugins.ArmyFights.Core.Team;
    using Plugins.ArmyFights.Example.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(EcsSoldiersSpawner))]
    public sealed class EcsSoldiersSpawner : Initializer
    {
        [SerializeField]
        private GameObject soldierPrefab;

        public override void OnAwake()
        {
            var containerFilter = World.Filter.With<EcsSoldierContainerComponent>();

            var container = containerFilter.First().GetComponent<EcsSoldierContainerComponent>();
            
            var teamsFilter = World.Filter.With<EcsTeamSpawnSpotComponent>();
            
            Spawn(teamsFilter, container);
        }

        private void Spawn(Filter teamsFilter, EcsSoldierContainerComponent container)
        {
            foreach (var entity in teamsFilter)
            {
                var team = entity.GetComponent<EcsTeamSpawnSpotComponent>();

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

        public override void Dispose() 
        {
            
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
            var soldier = Instantiate(soldierPrefab, position, rotation, container);

            if (soldier.TryGetComponent(out EcsHealthProvider healthProvider))
            {
                ref var healthComponent = ref healthProvider.Stash.Get(healthProvider.Entity);
                healthComponent.HealthPoints = healthComponent.maxHealthPoints;
            }
            
            if (soldier.TryGetComponent(out EcsFightableProvider fightableProvider))
            {
                ref var fightableComponent = ref fightableProvider.Stash.Get(fightableProvider.Entity);
                fightableComponent.Side = side;
            }

            if (soldier.TryGetComponent(out EcsColorControllerProvider colorControllerProvider))
            {
                var colorControllerComponent = colorControllerProvider.Stash.Get(colorControllerProvider.Entity);

                colorControllerComponent.meshRenderer.material.color = color;
            }
            
            if (soldier.TryGetComponent(out EcsRigidbodyProvider rigidbodyProvider))
            {
                var rigidbodyComponent = rigidbodyProvider.Stash.Get(rigidbodyProvider.Entity);

                rigidbodyComponent.rigidbody.solverIterations = 1;
            }
            
            if (soldier.TryGetComponent(out EcsGameObjectProvider gameObjectProvider))
            {
                var gameObjectComponent = gameObjectProvider.Stash.Get(gameObjectProvider.Entity);

                gameObjectComponent.gameObject.SetActive(true);
            }
        }
    }
}