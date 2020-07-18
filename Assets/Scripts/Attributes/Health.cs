using System;
using GameDevTV.Utils;
using RPG.Core;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        LazyValue<float> healthPoints;

        bool isDead = false;

        BaseStats baseStats;

        private void Awake()
        {
            healthPoints = new LazyValue<float>(GetInitHealth);
            baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private float GetInitHealth()
        {
            return baseStats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            baseStats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            baseStats.onLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(0, healthPoints.value - damage);
            if (healthPoints.value == 0)
            {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * regenerationPercentage / 100;
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
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

        public float GetHealthPoints()
        {
            return healthPoints.value;
        }
        public void Heal(float healthtoRestore)
        {
            healthPoints.value = Mathf.Min(GetMaxHealthPoints(), healthPoints.value + healthtoRestore);
        }

        public float GetMaxHealthPoints()
        {
            return baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * GetFraction();
        }

        public float GetFraction()
        {
            return (GetHealthPoints() / GetMaxHealthPoints());
        }

        private void SetDeathState()
        {
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;
            if (healthPoints.value <= 0)
            {
                SetDeathState();
                GetComponent<Animator>().Play("Death", 0, 1);
            }
        }
    }
}