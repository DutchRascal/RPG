using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;

        public float WeaponDamage { get => weaponDamage; }
        public float WeaponRange { get => weaponRange; }

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (equippedPrefab)
            {
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOverride)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}