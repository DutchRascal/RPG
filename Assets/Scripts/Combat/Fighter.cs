using RPG.Movement;
using RPG.Saving;
using UnityEngine;
using RPG.Resources;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        Mover mover;
        Animator animator;
        ActionScheduler actionScheduler;
        Weapon currentWeapon = null;

        float timeSinceLastAttack = 0;

        private void Start()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            if (!currentWeapon)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target) return;
            // if (target.IsDead())

            if (!mover.NavMeshAgent.isStopped)
            {
                animator.ResetTrigger("attack");
                animator.SetTrigger("stopAttack");
            }

            if (target.IsDead())
            {
                animator.SetTrigger("stopAttack");
                return;
            }

            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
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
                // This will trigger the Hit() event.
                TriggerAttack();
                timeSinceLastAttack = Mathf.Infinity;
            }
        }

        //Animation Event
        void Hit()
        {
            if (!target) { return; }
            if (!currentWeapon.HasProjectile())
            {
                target.TakeDamage(currentWeapon.WeaponDamage);
            }
            else
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
        }

        //Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange;
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            mover.Cancel();
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger("stopAttack");
            animator.SetTrigger("attack");
        }

        private void StopAttack()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (!combatTarget) { return false; }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest && !targetToTest.IsDead());
        }

        public void EquipWeapon(Weapon weapon)
        {
            if (!weapon) { return; }
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            if (state == null) { return; }
            string weaponName = (string)state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            if (weapon)
            {
                EquipWeapon(weapon);
            }
        }
    }
}