namespace Plugins.ArmyFights.Core.GameObject.Scripts
{
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsGameObjectComponent : IComponent
    {
        public GameObject gameObject;
    }
}