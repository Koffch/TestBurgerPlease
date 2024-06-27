using System.Collections.Generic;

namespace Components
{
    public struct CollectorComponent
    {
        public const float Radius = 4f;
        public const int Max = 10;
        
        private Stack<int> _stack;
        public Stack<int> Stack => _stack ??= new Stack<int>();
    }
}