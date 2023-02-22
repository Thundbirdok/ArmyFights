using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using System;
    using UnityEngine;

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsFighterComponent : IComponent
    {
        public float attackDistance;
        
        public float damagePerSecond;
            
        [NonSerialized]
        public EntityId TargetId;
    }
}