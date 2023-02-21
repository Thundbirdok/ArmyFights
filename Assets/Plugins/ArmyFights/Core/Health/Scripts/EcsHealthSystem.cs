namespace Plugins.ArmyFights.Core.Health.Scripts
{
    using Plugins.ArmyFights.Core.GameObjectProvider.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsHealthSystem))]
    public sealed class EcsHealthSystem : UpdateSystem 
    {
        private Filter filter;
        private Stash<EcsHealthComponent> healthStash;

        public override void OnAwake()
        {
            filter = World.Filter.With<EcsHealthComponent>();
        }

        public override void OnUpdate(float deltaTime) 
        {
            healthStash = World.GetStash<EcsHealthComponent>();
            
            foreach (var entity in filter)
            {
                ref var healthComponent = ref healthStash.Get(entity);

                if (healthComponent.healthPoints > 0)
                {
                    continue;
                }

                entity.SetComponent(new EcsDeathMark());
            }
        }
    }
}