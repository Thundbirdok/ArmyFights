using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Health.Scripts
{
    using Scellecs.Morpeh;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsDestroyerSystem))]
    public sealed class EcsDestroyerSystem : UpdateSystem 
    {
        private Filter destroyedEntities;
        private Stash<EcsHealthComponent> gameObjectStash;
        
        public override void OnAwake() 
        {
            destroyedEntities = World.Filter
                .With<EcsHealthComponent>()
                .With<EcsDeathMark>();
            
            gameObjectStash = World.GetStash<EcsHealthComponent>();
        }

        public override void OnUpdate(float deltaTime) 
        {
            foreach (var entity in destroyedEntities) 
            {
                ref var gameObjectComponent = ref gameObjectStash.Get(entity);

                entity.RemoveComponent<EcsDeathMark>();
                
                gameObjectComponent.gameObject.SetActive(false);
                
                //World.RemoveEntity(entity);
            }
        }
    }
}