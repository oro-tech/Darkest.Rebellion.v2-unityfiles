using UnityEngine;
using Player;
using System;
using System.Collections;

namespace Enemy
{
    public class EnemySkills : MonoBehaviour
    {
        public int skill1Damage = 20;
        public int skill2Damage = 10;

        public playerHealth playerHealth;

        public event Action OnSkillUsed;

        private bool skill1Used;
        private bool skill2Used;

        public IEnumerator ExecuteAITurn()
        {
            yield return new WaitForSeconds(0.5f);

            if (!skill1Used)
            {
                playerHealth.TakeDamage(skill1Damage);
                skill1Used = true;
                DamageFlash.Instance.FlashPlayer();
                OnSkillUsed?.Invoke();
            }
            else if (!skill2Used)
            {
                playerHealth.TakeDamage(skill2Damage);
                skill2Used = true;
                DamageFlash.Instance.FlashPlayer();
                OnSkillUsed?.Invoke();
            }

            yield return new WaitForSeconds(0.5f);
        }

        public void ResetSkillUsage()
        {
            skill1Used = false;
            skill2Used = false;
        }
    }
}
