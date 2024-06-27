using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _followObject;

    private Vector3 _defaultPosition;

    private void Awake()
    {
        _defaultPosition = gameObject.transform.position;
    }

    public void Init(Transform followObject)
    {
        _followObject = followObject;
    }

    private void Update()
    {
        gameObject.transform.position = _defaultPosition + _followObject.localPosition;
    }
}