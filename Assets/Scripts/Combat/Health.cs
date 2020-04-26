using System;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        bool isDead = false;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(0, health - damage);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (!isDead)
            {
                GetComponent<Animator>().SetTrigger("die");
                isDead = true;
            }
        }
    }
}