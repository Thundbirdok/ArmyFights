namespace Plugins.ArmyFights.Core.Movement.Scripts
{
    using Plugins.ArmyFights.Core.Fights.Scripts;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Native;
    using Scellecs.Morpeh.Systems;
    using Unity.IL2CPP.CompilerServices;
    using Unity.Jobs;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(EcsMoveToFightTargetFixedSystem))]
    public sealed class EcsMoveToFightTargetFixedSystem : FixedUpdateSystem 
    {
        private Filter filter;
        
        private Stash<EcsFighterComponent> fighterStash;
        private Stash<EcsFightableComponent> fightableStash;
        private Stash<EcsMovementComponent> movementStash;

        public override void OnAwake()
        {
            filter = World.Filter
                .With<EcsFighterComponent>()
                .With<EcsFightableComponent>()
                .With<EcsMovementComponent>();

            fighterStash = World.GetStash<EcsFighterComponent>();
            fightableStash = World.GetStash<EcsFightableComponent>();
            movementStash = World.GetStash<EcsMovementComponent>();
        }

        public override void OnUpdate(float deltaTime)
        {
            CalcMovement();
        }

        private void CalcMovement()
        {
            using (var nativeFilter = filter.AsNative())
            {
                var parallelJob = new MoveToTargetJob
                {
                    Entities = nativeFilter,
                    FighterStash = fighterStash.AsNative(),
                    FightableStash = fightableStash.AsNative(),
                    MovementStash = movementStash.AsNative()
                };

                var parallelJobHandle = parallelJob.Schedule(nativeFilter.length, 64);
                
                parallelJobHandle.Complete();
            }
        }
    }
}