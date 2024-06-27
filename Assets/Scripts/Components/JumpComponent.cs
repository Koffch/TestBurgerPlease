using UnityEngine;

namespace Components
{
    public struct JumpComponent
    {
        public const float Height = 8f;
        public const float Duration = 0.3f;
        
        public Vector3 StartPosition;
        public Transform Parent;
        public Vector3 FinishLocalPosition;
        public float Time;
    }
}