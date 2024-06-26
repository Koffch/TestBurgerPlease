using UnityEngine;

namespace Components
{
    public struct JumpComponent
    {
        public Vector3 StartPosition;
        public Transform Parent;
        public Vector3 FinishLocalPosition;
        public const float Duration = 0.3f;
        public float Time;
        public const float Height = 8f;
    }
}