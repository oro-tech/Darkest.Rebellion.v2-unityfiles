using UnityEngine;

namespace Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(0, currentHealth);
        }
    }
}
