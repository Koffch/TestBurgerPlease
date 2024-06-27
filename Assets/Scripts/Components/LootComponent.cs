namespace Components
{
    public struct LootComponent
    {
        public const float Height = 0.1f;
        public LootStatus Status;
    }

    public enum LootStatus
    {
        Ground,
        Looted
    }
}