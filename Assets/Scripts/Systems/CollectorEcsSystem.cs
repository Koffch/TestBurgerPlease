using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class CollectorEcsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsFilter _lootFilter;

        private EcsPool<CollectorComponent> _collectors;
        private EcsPool<TransformComponent> _transforms;
        private EcsPool<JumpComponent> _jumps;
        private EcsPool<LootComponent> _loots;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<CollectorComponent>().End();
            _lootFilter = world.Filter<LootComponent>().Exc<JumpComponent>().End();

            _collectors = world.GetPool<CollectorComponent>();
            _transforms = world.GetPool<TransformComponent>();
            _jumps = world.GetPool<JumpComponent>();
            _loots = world.GetPool<LootComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var collector = ref _collectors.Get(entity);
                if (collector.Stack.Count >= collector.Max) continue;

                ref var collectorTransform = ref _transforms.Get(entity);
                foreach (var lootEntity in _lootFilter)
                {
                    ref var loot = ref _loots.Get(lootEntity);
                    if (loot.Status != LootStatus.Ground) continue;

                    ref var lootTransform = ref _transforms.Get(lootEntity);
                    var magnitude = (collectorTransform.Transform.position - lootTransform.Transform.position).magnitude;
                    if (magnitude > CollectorComponent.Radius) continue;

                    collector.Stack.Add(lootEntity);
                    loot.Status = LootStatus.Looted;

                    ref var jump = ref _jumps.Add(lootEntity);
                    jump.Parent = collectorTransform.Transform;
                    jump.StartPosition = lootTransform.Transform.position;
                    var y = 2f + collector.Stack.Count * LootComponent.Height;
                    jump.FinishLocalPosition = new Vector3(0, y, -0.8f);
                    jump.Time = 0;
                }
            }
        }
    }
}