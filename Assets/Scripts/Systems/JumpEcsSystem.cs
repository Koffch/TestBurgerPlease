using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class JumpEcsSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<JumpComponent> _jumps;
        private EcsPool<TransformComponent> _transforms;
        private EcsPool<LootComponent> _loots;

        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _filter = world.Filter<JumpComponent>().End();
            
            _jumps = world.GetPool<JumpComponent>();
            _transforms = world.GetPool<TransformComponent>();
            _loots = world.GetPool<LootComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var jump = ref _jumps.Get(entity);
                ref var transform = ref _transforms.Get(entity);

                jump.Time += Time.deltaTime;
                var t = jump.Time / JumpComponent.Duration;
                if (t < 1)
                {
                    var up = Vector3.up * JumpComponent.Height * 4 * t * (1 - t);
                    var endPosition = jump.Parent.position + jump.FinishLocalPosition + up;
                    transform.Transform.position = Vector3.Lerp(jump.StartPosition, endPosition, t);
                }
                else
                {
                    transform.Transform.SetParent(jump.Parent);
                    transform.Transform.localPosition = jump.FinishLocalPosition;
                    ref var loot = ref _loots.Get(entity);
                    loot.Status = LootStatus.Looted;
                    _jumps.Del(entity);
                }
            }
        }
    }
}