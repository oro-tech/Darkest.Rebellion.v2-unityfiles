using UnityEngine;
using UnityEngine.UI;
using Enemy;
using System;

namespace Player
{
    public class playerSkills : MonoBehaviour
    {
        public Button skill1Button;
        public Button skill2Button;

        public int skill1Damage = 20;
        public int skill2Damage = 10;

        public EnemyHealth enemyHealth;

        public event Action OnSkillUsed;

        public bool skill1Used { get; private set; }
        public bool skill2Used { get; private set; }

        void Start()
        {
            skill1Button.onClick.AddListener(UseSkill1);
            skill2Button.onClick.AddListener(UseSkill2);
        }

        void UseSkill1()
        {
            if (skill1Used) return;
            enemyHealth.TakeDamage(skill1Damage);
            skill1Used = true;

            DamageFlash.Instance.FlashEnemy();
            OnSkillUsed?.Invoke();
        }

        void UseSkill2()
        {
            if (skill2Used) return;
            enemyHealth.TakeDamage(skill2Damage);
            skill2Used = true;

            DamageFlash.Instance.FlashEnemy();
            OnSkillUsed?.Invoke();
        }

        public void ResetSkillUsage()
        {
            skill1Used = false;
            skill2Used = false;
        }
    }
}
