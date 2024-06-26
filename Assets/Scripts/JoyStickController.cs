using System;
using UnityEngine;

public class JoyStickController : MonoBehaviour, IMoveController
{
    [SerializeField] private Transform _background;
    [SerializeField] private Transform _handle;
    [SerializeField] private int _maxMouseShift;
    [SerializeField] private int _maxStickShift;
    [SerializeField] private float _speedCoefficient;

    public event Action<Vector3> Move;
    public event Action Stop;

    private bool _isMoving;
    private float _shiftDelta;

    private void Awake()
    {
        _shiftDelta = (float)_maxStickShift / _maxMouseShift;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var hitCollider = Physics2D.OverlapPoint(Input.mousePosition);
            if (hitCollider?.gameObject == gameObject)
                _isMoving = true;
        }

        if (_isMoving)
        {
            var direction = Input.mousePosition - gameObject.transform.position;
            var magnitude = direction.magnitude;
            var normalized = direction.normalized;

            _handle.localPosition = magnitude * _shiftDelta * normalized;
            _background.localPosition = magnitude <= _maxMouseShift
                ? Vector3.zero
                : (magnitude - _maxMouseShift) * _shiftDelta * normalized;

            direction *= _speedCoefficient;
            Move?.Invoke(new Vector3(direction.x, 0, direction.y));
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isMoving = false;
            _background.localPosition = _handle.localPosition = Vector3.zero;

            Stop?.Invoke();
        }
    }
}