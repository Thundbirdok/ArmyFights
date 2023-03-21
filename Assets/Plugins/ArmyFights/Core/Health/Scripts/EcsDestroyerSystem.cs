using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Health.Scripts
{
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;
    using Plugins.ArmyFights.Core.GameObject.Scripts;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    //[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsDestroyerSystem))]
    public sealed class EcsDestroyerSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EcsHealthComponent, EcsDeathMark>> destroyedEntities;

        private EcsPoolInject<EcsGameObjectComponent> gameObjectPool;
        private EcsPoolInject<Inc<EcsDeathMark>> deathMarkPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in destroyedEntities.Value) 
            {
                ref var gameObjectComponent = ref gameObjectPool.Value.Get(entity);
                
                deathMarkPool.Value.Del(entity);
                
                gameObjectComponent.gameObject.SetActive(false);
                
                //World.RemoveEntity(entity);
            }
        }
    }
}