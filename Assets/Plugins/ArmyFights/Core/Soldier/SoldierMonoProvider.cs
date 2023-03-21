using UnityEngine;

namespace Plugins.ArmyFights.Core.Soldier
{
    using Leopotam.EcsLite;
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Plugins.ArmyFights.Core.GameObject.Scripts;
    using Plugins.ArmyFights.Core.Health.Scripts;
    using Plugins.ArmyFights.Core.Movement.Scripts;
    using Plugins.ArmyFights.Core.Rigidbody.Scripts;
    using Plugins.ArmyFights.Core.Transform.Scripts;
    using Plugins.ArmyFights.Example.Scripts;
    using UnityEngine.Serialization;

    public class SoldierMonoProvider : MonoBehaviour
    {
        [SerializeField]
        private EcsGameObjectComponent ecsGameObjectComponent;

        [SerializeField]
        private EcsTransformComponent ecsTransformComponent;

        [SerializeField]
        private EcsRigidbodyComponent ecsRigidbodyComponent;

        [SerializeField]
        private EcsHealthComponent ecsHealthComponent;

        [SerializeField]
        private EcsMovementComponent ecsMovementComponent;

        [SerializeField]
        private EcsFightableComponent ecsFightableComponent;

        [SerializeField]
        private EcsFighterComponent ecsFighterComponent;

        [SerializeField]
        private EcsColorControllerComponent ecsColorControllerComponent;

        public void AddToEntity
        (
            int entity,
            EcsPool<EcsGameObjectComponent> gameObjectPool,
            EcsPool<EcsTransformComponent> transformPool,
            EcsPool<EcsRigidbodyComponent> rigidbodyPool,
            EcsPool<EcsHealthComponent> healthPool,
            EcsPool<EcsMovementComponent> movementPool,
            EcsPool<EcsFightableComponent> fightablePool,
            EcsPool<EcsFighterComponent> fighterPool,
            EcsPool<EcsColorControllerComponent> colorControllerPool
        )
        {
            ref var gameObjectComponent = ref gameObjectPool.Add(entity);
            gameObjectComponent = ecsGameObjectComponent;

            ref var transformComponent = ref transformPool.Add(entity);
            transformComponent = ecsTransformComponent;
            
            ref var rigidbodyComponent = ref rigidbodyPool.Add(entity);
            rigidbodyComponent = ecsRigidbodyComponent;
            
            ref var healthComponent = ref healthPool.Add(entity);
            healthComponent = ecsHealthComponent;
            
            ref var movementComponent = ref movementPool.Add(entity);
            movementComponent = ecsMovementComponent;
            
            ref var fightableComponent = ref fightablePool.Add(entity);
            fightableComponent = ecsFightableComponent;
            
            ref var fighterComponent = ref fighterPool.Add(entity);
            fighterComponent = ecsFighterComponent;
            
            ref var colorControllerComponent = ref colorControllerPool.Add(entity);
            colorControllerComponent = ecsColorControllerComponent;
            
            Destroy(this);
        }
    }
}
