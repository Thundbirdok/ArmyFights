using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using System;

    [Serializable]
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsFighterComponent
    {
        public float attackDistance;
        
        public int damage;

        public float attackCooldown;

        [NonSerialized]
        public float TimeAfterAttackPassed;
    }
}