using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Plugins.ArmyFights.Core.Transform.Scripts;
    using Scellecs.Morpeh;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsUpdateFightableComponentSystem))]
    public sealed class EcsUpdateFightableComponentSystem : UpdateSystem 
    {
        private Filter filter;

        private Stash<EcsTransformComponent> transformStash;
        private Stash<EcsFightableComponent> fightableStash;

        public override void OnAwake()
        {
            filter = World.Filter
                .With<EcsFightableComponent>()
                .With<EcsTransformComponent>();
            
            fightableStash = World.GetStash<EcsFightableComponent>();
            transformStash = World.GetStash<EcsTransformComponent>();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in filter)
            {
                ref var component = ref fightableStash.Get(entity);

                component.Position = transformStash.Get(entity).transform.position;
            }
        }
    }
}