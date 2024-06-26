using System;
using UnityEngine;

public class AvatarController : MonoBehaviour, IDisposable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _animatorMoveSpeed;
    [SerializeField] private float _animatorRunSpeed;

    private readonly int _speedHashName = Animator.StringToHash("speed");
    private IMoveController _moveController;

    public void Init(IMoveController moveController)
    {
        _moveController = moveController;
        _moveController.Move += OnMove;
        _moveController.Stop += OnStop;
    }

    private void OnMove(Vector3 value)
    {
        var speed = value.magnitude;
        _animator.SetFloat(_speedHashName, speed);
        _animator.speed = speed * (speed > 0.2 ? _animatorRunSpeed : _animatorMoveSpeed);
        transform.localPosition += value;
        transform.forward = value;
    }

    private void OnStop()
    {
        _animator.speed = 1;
        _animator.SetFloat(_speedHashName, 0);
    }

    public void Dispose()
    {
        _moveController.Move -= OnMove;
        _moveController.Stop -= OnStop;
    }
}