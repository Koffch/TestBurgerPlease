namespace Components
{
    public struct LootComponent
    {
        public LootStatus Status;
        public const float Height = 0.1f;
    }

    public enum LootStatus
    {
        Ground,
        Looted
    }
}