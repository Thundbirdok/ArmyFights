namespace Plugins.ArmyFights.Core.Health.Scripts 
{
    using System;
    using Unity.IL2CPP.CompilerServices;

    [Serializable]
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsHealthComponent
    {
        public int maxHealthPoints;
        
        [NonSerialized]
        public int HealthPoints;
    }
}