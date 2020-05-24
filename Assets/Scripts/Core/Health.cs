using System;
using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(0, healthPoints - damage);
            if (healthPoints == 0)
            {
                Die();
            }
        }

        public bool IsDead()
        {
            return isDead;
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