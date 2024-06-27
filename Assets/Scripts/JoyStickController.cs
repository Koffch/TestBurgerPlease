using System;
using UnityEngine;

public class JoyStickController : MonoBehaviour, IMoveController
{
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
            var div = Input.mousePosition - gameObject.transform.position;
            var magnitude = div.magnitude;
            if (magnitude > _maxMouseShift)
                div *= _maxMouseShift / magnitude;

            _handle.localPosition = div * _shiftDelta;

            div *= _speedCoefficient;
            Move?.Invoke(new Vector3(div.x, 0, div.y));
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isMoving = false;
            _handle.localPosition = Vector3.zero;

            Stop?.Invoke();
        }
    }
}