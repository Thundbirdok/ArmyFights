namespace Plugins.ArmyFights.Core.Rigidbody.Scripts
{
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [System.Serializable]
    // [Il2CppSetOption(Option.NullChecks, false)]
    // [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    // [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsRigidbodyComponent
    {
        public Rigidbody rigidbody;
    }
}