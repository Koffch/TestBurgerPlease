using Components;
using Leopotam.EcsLite;
using UnityEngine;

public class GeneratorEcsSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;

    private EcsPool<GeneratorComponent> _generators;
    private EcsPool<TransformComponent> _transforms;
    private EcsPool<LootComponent> _loots;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _filter = world.Filter<GeneratorComponent>().End();

        _generators = world.GetPool<GeneratorComponent>();
        _transforms = world.GetPool<TransformComponent>();
        _loots = world.GetPool<LootComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        foreach (var entity in _filter)
        {
            ref var generator = ref _generators.Get(entity);
            generator.TimeToNextGenerate -= Time.deltaTime;
            if (generator.TimeToNextGenerate > 0) continue;

            generator.TimeToNextGenerate = GeneratorComponent.Period;

            var prefab = Resources.Load(GeneratorComponent.ItemPath);
            var generatorTransform = _transforms.Get(entity);
            var randomOffset = new Vector3(Random.Range(-1f, 1), 0, Random.Range(-1f, 1)).normalized *
                               Random.Range(GeneratorComponent.MinRadius, GeneratorComponent.MaxRadius);
            var randomPosition = generatorTransform.Transform.position + randomOffset;
            var eulerAngles = generatorTransform.Transform.rotation.eulerAngles;
            var randomRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, Random.Range(0f, 360f));

            var go = (GameObject)Object.Instantiate(prefab, randomPosition, randomRotation);
            var newEntity = world.NewEntity();
            ref var newTransform = ref _transforms.Add(newEntity);
            newTransform.Transform = go.transform;
            ref var newLoot = ref _loots.Add(newEntity);
            newLoot.Status = LootStatus.Ground;
        }
    }
}