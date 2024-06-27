using Components;
using Leopotam.EcsLite;
using Systems;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private JoyStickController _joyStickController;
    [SerializeField] private TextMeshProUGUI _counter;

    private EcsWorld _world;
    private IEcsSystems _systems;

    private void Start()
    {
        var sharedData = new SharedData { GameTransform = transform, Counter = _counter };

        _world = new EcsWorld();
        _systems = new EcsSystems(_world, sharedData);
        _systems.Add(new GeneratorEcsSystem()).Add(new CollectorEcsSystem())
            .Add(new ConveyorEcsSystem()).Add(new JumpEcsSystem())
            .Init();

        var transforms = _world.GetPool<TransformComponent>();
        Add<GeneratorComponent>(transforms, new Vector3(-8, 0, 15), "Prefabs/tree");
        Add<GeneratorComponent>(transforms, new Vector3(0.7f, 0, 13), "Prefabs/tree");
        Add<GeneratorComponent>(transforms, new Vector3(5f, 0, -15), "Prefabs/tree");
        Add<ConveyorComponent>(transforms, new Vector3(7, 0, 0), "Prefabs/conveyor");

        var player = Add<CollectorComponent>(transforms, new Vector3(0, 0, 0), "Prefabs/man");
        player.GetComponent<AvatarController>().Init(_joyStickController);
        _cameraController.Init(player.transform);
    }

    private GameObject Add<T>(EcsPool<TransformComponent> transforms, Vector3 position, string prefabPath)
        where T : struct
    {
        var entity = _world.NewEntity();
        _world.GetPool<T>().Add(entity);

        ref var componentTransform = ref transforms.Add(entity);
        var go = (GameObject)Instantiate(Resources.Load(prefabPath), transform);
        componentTransform.Transform = go.transform;
        componentTransform.Transform.position = position;

        return go;
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
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