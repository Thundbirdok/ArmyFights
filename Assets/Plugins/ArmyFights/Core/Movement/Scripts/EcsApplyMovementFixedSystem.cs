using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Rigidbody.Scripts;
    using Plugins.ArmyFights.Core.Transform.Scripts;
    using Scellecs.Morpeh;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsApplyMovementFixedSystem))]
    public sealed class EcsApplyMovementFixedSystem : FixedUpdateSystem
    {
        private Filter filter;

        private Stash<EcsTransformComponent> transformStash;
        private Stash<EcsRigidbodyComponent> rigidbodyStash;
        private Stash<EcsMovementComponent> movementStash;
        
        public override void OnAwake()
        {
            filter = World.Filter
                .With<EcsTransformComponent>()
                .With<EcsRigidbodyComponent>()
                .With<EcsMovementComponent>();

            transformStash = World.GetStash<EcsTransformComponent>();
            rigidbodyStash = World.GetStash<EcsRigidbodyComponent>();
            movementStash = World.GetStash<EcsMovementComponent>();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in filter)
            {
                var transformComponent = transformStash.Get(entity);
                var rigidbodyComponent = rigidbodyStash.Get(entity);
                var movementComponent = movementStash.Get(entity);

                transformComponent.transform.rotation = movementComponent.Rotation;
                rigidbodyComponent.rigidbody.AddForce(movementComponent.CurrentForce);
            }
        }
    }
}