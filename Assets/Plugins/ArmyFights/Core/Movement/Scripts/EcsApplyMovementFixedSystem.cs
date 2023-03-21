using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;
    using Plugins.ArmyFights.Core.Rigidbody.Scripts;
    using Plugins.ArmyFights.Core.Transform.Scripts;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsApplyMovementFixedSystem))]
    public sealed class EcsApplyMovementFixedSystem : IEcsRunSystem
    {
        private EcsFilterInject
        <
            Inc
            <
                EcsTransformComponent,
                EcsRigidbodyComponent, 
                EcsMovementComponent
            >
        > filter;
        
        private EcsPoolInject<EcsRigidbodyComponent> rigidbodyPool;
        private EcsPoolInject<EcsMovementComponent> movementPool;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var rigidbodyComponent = rigidbodyPool.Value.Get(entity);
                var movementComponent = movementPool.Value.Get(entity);
                
                rigidbodyComponent.rigidbody.AddForce(movementComponent.CurrentForce);
            }
        }
    }
}