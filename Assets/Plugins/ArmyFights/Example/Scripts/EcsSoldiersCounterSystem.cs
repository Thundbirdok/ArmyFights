using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Example.Scripts
{
    using System;
    using System.Linq;
    using Leopotam.EcsLite;
    using Leopotam.EcsLite.Di;
    using Plugins.ArmyFights.Core.Fights.Scripts;
    
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    //[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsSoldiersCounterSystem))]
    public sealed class EcsSoldiersCounterSystem : IEcsInitSystem, IEcsRunSystem
    {
        public event Action OnChangedValue; 
        
        [NonSerialized]
        private int count;
        public int Count
        {
            get
            {
                return count;
            }
            
            private set
            {
                if (count == value)
                {
                    return;
                }

                count = value;
                
                OnChangedValue?.Invoke();
            }
        }

        private EcsFilterInject<Inc<EcsFighterComponent>> filter;
        
        public void Init(IEcsSystems systems)
        {
            Count = 0;
        }

        public void Run(IEcsSystems systems)
        {
            Count = filter.Value.GetEntitiesCount();
        }
    }
}