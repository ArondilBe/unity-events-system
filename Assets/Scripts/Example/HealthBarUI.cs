using Events;
using UnityEngine;
using UnityEngine.UI;

namespace Example
{
    public class HealthBarUI : MonoBehaviour
    {
        [SerializeField]
        private Slider healthSlider;

        private void OnEnable()
        {
            EventBus.Subscribe<PlayerDamageEvent>(OnPlayerDamaged);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<PlayerDamageEvent>(OnPlayerDamaged);
        }

        private void OnPlayerDamaged(PlayerDamageEvent playerDamageEvent)
        {
            healthSlider.value =
                (float)playerDamageEvent.CurrentHealth / playerDamageEvent.MaxHealth;
        }
    }
}
