namespace Plugins.ArmyFights.Core.Rigidbody.Scripts
{
    using Scellecs.Morpeh.Providers;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EcsRigidbodyProvider : MonoProvider<EcsRigidbodyComponent> { }
}