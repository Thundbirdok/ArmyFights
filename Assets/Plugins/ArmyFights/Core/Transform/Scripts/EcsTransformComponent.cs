namespace Plugins.ArmyFights.Core.Transform.Scripts
{
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [System.Serializable]
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsTransformComponent
    {
        public Transform transform;
    }
}