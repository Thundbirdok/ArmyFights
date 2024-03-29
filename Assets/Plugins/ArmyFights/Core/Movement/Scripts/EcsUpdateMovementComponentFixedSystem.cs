using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Transform.Scripts;
    using Scellecs.Morpeh;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsUpdateMovementComponentFixedSystem))]
    public sealed class EcsUpdateMovementComponentFixedSystem : FixedUpdateSystem 
    {
        private Filter filter;
        
        private Stash<EcsMovementComponent> movementStash;
    
        public override void OnAwake()
        {
            filter = World.Filter
                .With<EcsMovementComponent>()
                .With<EcsTransformComponent>();
                
            movementStash = World.GetStash<EcsMovementComponent>();
        }
    
        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in filter)
            {
                ref var component = ref movementStash.Get(entity);
                
                component.CurrentForce = Vector3.zero;
            }
        }
    }
}