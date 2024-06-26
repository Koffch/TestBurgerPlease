using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _followObject;

    private Vector3 _defaultPosition;

    private void Awake()
    {
        _defaultPosition = gameObject.transform.position;
    }

    private void Update()
    {
        gameObject.transform.position = _defaultPosition + _followObject.localPosition;
    }
}