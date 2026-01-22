using Events;

namespace Example
{
    public struct PlayerDamageEvent : IGameEvent
    {
        public int Damage;
        public int CurrentHealth;
        public int MaxHealth;
    }
}
