using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Core.SoldiersSpawner.Scripts
{
    using System;
    using Leopotam.EcsLite.Di;
    using UnityEngine;

    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EcsSoldierContainerProvider : MonoBehaviour
    {
        [SerializeField]
        private EcsSoldierContainerComponent soldierContainerComponent;

        private EcsWorldInject world;

        private EcsPoolInject<EcsSoldierContainerComponent> soldierContainerPool;

        private void Awake()
        {
            var entity = world.Value.NewEntity();

            soldierContainerPool.Value.Add(entity);
        }
    }
}