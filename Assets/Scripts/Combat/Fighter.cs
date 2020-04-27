using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        Mover mover;
        Animator animator;
        ActionScheduler actionScheduler;

        float timeSinceLastAttack = 0;

        private void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target) return;
            if (target.IsDead())
            {
                animator.SetTrigger("stopAttack");
                return;
            }
            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger teh Hit() event.
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        //Animation Event
        void Hit()
        {
            if (target)
            {
                {
                    target.TakeDamage(weaponDamage);
                }
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.SetTrigger("stopAttack");
            target = null;
        }

        public bool CanAttack(CombatTarget combatTarget)
        {
            if (!combatTarget) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest && !targetToTest.IsDead());
        }
    }
}