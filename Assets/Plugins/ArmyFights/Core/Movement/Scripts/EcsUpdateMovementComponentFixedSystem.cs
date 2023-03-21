using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Leopotam.EcsLite;
    using Plugins.ArmyFights.Core.Transform.Scripts;
    
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    // [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsUpdateMovementComponentFixedSystem))]
    public sealed class EcsUpdateMovementComponentFixedSystem : IEcsInitSystem, IEcsRunSystem 
    {
        private EcsFilter filter;
        
        private EcsPool<EcsMovementComponent> movementPool;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
        
            filter = world
                .Filter<EcsMovementComponent>()
                .Inc<EcsTransformComponent>()
                .End();
                
            movementPool = world.GetPool<EcsMovementComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter)
            {
                ref var component = ref movementPool.Get(entity);
                
                component.CurrentForce = Vector3.zero;
            }
        }
    }
}