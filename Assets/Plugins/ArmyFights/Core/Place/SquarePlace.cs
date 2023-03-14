namespace Plugins.ArmyFights.Core.Place
{
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SquarePlace
    {
        public Transform leftBackPoint;
        public Vector3 LeftBackLocalPosition => leftBackPoint.localPosition;
        
        public Transform rightFrontPoint;
        public Vector3 RightFrontLocalPosition => rightFrontPoint.localPosition;
    }
}
