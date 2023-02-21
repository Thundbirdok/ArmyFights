using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Plugins.ArmyFights.Example.Scripts
{
    using UnityEngine;

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EcsColorControllerComponent : IComponent
    {
        public MeshRenderer meshRenderer;
    }
}