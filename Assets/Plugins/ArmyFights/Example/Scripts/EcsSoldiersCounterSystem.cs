using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Plugins.ArmyFights.Example.Scripts
{
    using System;
    using System.Linq;
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Scellecs.Morpeh;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsSoldiersCounterSystem))]
    public sealed class EcsSoldiersCounterSystem : UpdateSystem
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

        private Filter filter;
        
        public override void OnAwake()
        {
            filter = World.Filter.With<EcsFighterComponent>();

            Count = 0;
        }

        public override void OnUpdate(float deltaTime)
        {
            Count = filter.Count();
        }
    }
}