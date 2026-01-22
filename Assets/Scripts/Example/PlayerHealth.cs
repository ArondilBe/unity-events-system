using Events;
using UnityEngine;

namespace Example
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private int maxHealth = 100;
        private int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
            EventBus.Publish(
                new PlayerDamageEvent
                {
                    Damage = 0,
                    CurrentHealth = currentHealth,
                    MaxHealth = maxHealth,
                }
            );
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
            {
                return;
            }

            currentHealth = Mathf.Max(currentHealth - damage, 0);

            EventBus.Publish(
                new PlayerDamageEvent
                {
                    Damage = damage,
                    CurrentHealth = currentHealth,
                    MaxHealth = maxHealth,
                }
            );
        }
    }
}
