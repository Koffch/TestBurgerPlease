using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using Systems;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private JoyStickController _joyStickController;
    [SerializeField] private AvatarController _avatarController;

    private EcsWorld _world;
    private IEcsSystems _systems;

    private void Start()
    {
        _avatarController.Init(_joyStickController);

        var sharedData = new SharedData { GameTransform = transform };

        _world = new EcsWorld();
        _systems = new EcsSystems(_world, sharedData);
        _systems.Add(new GeneratorEcsSystem()).Add(new CollectorEcsSystem())
            .Add(new ConveyorEcsSystem()).Add(new JumpEcsSystem())
            .Init();

        var generators = _world.GetPool<GeneratorComponent>();
        var conveyors = _world.GetPool<ConveyorComponent>();
        var transforms = _world.GetPool<TransformComponent>();

        //add tree
        var entity = _world.NewEntity();
        generators.Add(entity);
        ref var generatorTransform = ref transforms.Add(entity);
        var prefab = Resources.Load("Prefabs/tree");
        var go = (GameObject)Instantiate(prefab, transform);
        generatorTransform.Transform = go.transform;
        generatorTransform.Transform.position = new Vector3(-5, 0, 15);

        //add playerCollector
        var collectors = _world.GetPool<CollectorComponent>();
        entity = _world.NewEntity();
        ref var collector = ref collectors.Add(entity);
        collector.Stack = new List<int>();
        collector.Max = 50;
        ref var collectorTransform = ref transforms.Add(entity);
        collectorTransform.Transform = _avatarController.transform;

        //add conveyor
        entity = _world.NewEntity();
        conveyors.Add(entity);
        ref var conveyorTransform = ref transforms.Add(entity);
        prefab = Resources.Load("Prefabs/conveyor");
        go = (GameObject)Instantiate(prefab, transform);
        conveyorTransform.Transform = go.transform;
        conveyorTransform.Transform.position = new Vector3(7, 0, 0);
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
        _avatarController.Dispose();

        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}