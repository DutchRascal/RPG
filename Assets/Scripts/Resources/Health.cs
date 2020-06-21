using System;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;

        float healthPoints = -1f;

        bool isDead = false;

        BaseStats baseStats;
        // float currentPercentage;

        private void Start()
        {
            if (healthPoints < 0)
            {
                baseStats = GetComponent<BaseStats>();
                healthPoints = baseStats.GetStat(Stat.Health);
                baseStats.onLevelUp += RegenerateHealth;
                // currentPercentage = GetPercentage();
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(0, healthPoints - damage);
            // currentPercentage = GetPercentage();
            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
            // float newMaxHealth = baseStats.GetStat(Stat.Health);
            // healthPoints = currentPercentage * newMaxHealth / 100;
            // currentPercentage = GetPercentage();
        }

        private void Die()
        {
            if (!isDead)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
                SetDeathState();
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (!experience) { return; }
            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
        }

        public float GetPercentage()
        {
            return healthPoints * 100 / baseStats.GetStat(Stat.Health);
        }

        private void SetDeathState()
        {
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if (healthPoints <= 0)
            {
                SetDeathState();
                GetComponent<Animator>().Play("Death", 0, 1);
            }
        }
    }
}