using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class ConveyorEcsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _collectorFilter;

        private EcsPool<ConveyorComponent> _conveyors;
        private EcsPool<TransformComponent> _transforms;
        private EcsPool<CollectorComponent> _collectors;
        private EcsPool<JumpComponent> _jumps;

        private SharedData _shared;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<ConveyorComponent>().End();
            _collectorFilter = world.Filter<CollectorComponent>().End();

            _conveyors = world.GetPool<ConveyorComponent>();
            _transforms = world.GetPool<TransformComponent>();
            _collectors = world.GetPool<CollectorComponent>();
            _jumps = world.GetPool<JumpComponent>();

            _shared = systems.GetShared<SharedData>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                foreach (var collectorEntity in _collectorFilter)
                {
                    ref var collector = ref _collectors.Get(collectorEntity);
                    if (collector.Stack.Count == 0) continue;

                    ref var collectorTransform = ref _transforms.Get(collectorEntity);
                    ref var conveyorTransform = ref _transforms.Get(entity);
                    var magnitude = (collectorTransform.Transform.position - conveyorTransform.Transform.position).magnitude;
                    if (magnitude > ConveyorComponent.Radius) continue;

                    foreach (var lootEntity in collector.Stack)
                    {
                        ref var conveyor = ref _conveyors.Get(entity);
                        conveyor.Counter += 1;

                        var lootTransform = _transforms.Get(lootEntity);
                        lootTransform.Transform.SetParent(_shared.GameTransform);

                        ref var jump = ref GetOrAddComponent(lootEntity);
                        jump.Parent = conveyorTransform.Transform;
                        jump.StartPosition = lootTransform.Transform.position;
                        var y = 1.2f + conveyor.Counter * LootComponent.Height;
                        jump.FinishLocalPosition = new Vector3(-4, y, 0);
                        jump.Time = 0;
                    }

                    collector.Stack.Clear();
                }
            }
        }

        private ref JumpComponent GetOrAddComponent(int entity)
        {
            if (_jumps.Has(entity))
                return ref _jumps.Get(entity);

            return ref _jumps.Add(entity);
        }
    }
}