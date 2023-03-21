using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;
    using Plugins.ArmyFights.Core.Transform.Scripts;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    //[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsUpdateFightableComponentSystem))]
    public sealed class EcsUpdateFightableComponentSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<EcsFightableComponent, EcsTransformComponent>> filter;

        private EcsPoolInject<EcsTransformComponent> transformPool;
        private EcsPoolInject<EcsFightableComponent> fightablePool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var component = ref fightablePool.Value.Get(entity);

                component.Position = transformPool.Value.Get(entity).transform.position;
            }
        }
    }
}