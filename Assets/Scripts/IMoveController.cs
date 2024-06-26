using System;
using UnityEngine;

public interface IMoveController
{
    event Action<Vector3> Move;
    event Action Stop;
}