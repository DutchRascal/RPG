using RPG.Movement;
using RPG.Saving;
using UnityEngine;
using RPG.Attributes;
using RPG.Core;
using RPG.Stats;
using System.Collections.Generic;
using GameDevTV.Utils;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health target;
        Mover mover;
        Animator animator;
        ActionScheduler actionScheduler;
        WeaponConfig currentWeaponConfig;
        LazyValue<Weapon> currentWeapon;

        float timeSinceLastAttack = 0;

        private void Awake()
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            return AttachWeapon(defaultWeapon);
        }

        private void Start()
        {
            currentWeapon.ForceInit();
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
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (currentWeapon.value)
            {
                currentWeapon.value.OnHit();
            }
            if (!currentWeaponConfig.HasProjectile())
            {
                target.TakeDamage(gameObject, damage);
            }
            else
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
        }

        //Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeaponConfig.WeaponRange;
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

        public void EquipWeapon(WeaponConfig weapon)
        {
            if (!weapon) { return; }
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon(weapon);
        }

        private Weapon AttachWeapon(WeaponConfig weapon)
        {
            Animator animator = GetComponent<Animator>();
            return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
        }

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {
            if (state == null) { return; }
            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            if (weapon)
            {
                EquipWeapon(weapon);
            }
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetpercentageBonus();
            }
        }
    }
}