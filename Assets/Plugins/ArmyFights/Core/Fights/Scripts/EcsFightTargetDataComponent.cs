using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Core.Fights.Scripts
{
    using System;
    using UnityEngine;

    [System.Serializable]
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsFightTargetDataComponent 
    {
        [NonSerialized]
        public int TargetId;

        [NonSerialized]
        public Vector3 DirectionToTarget;
    }
}