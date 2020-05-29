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
        [SerializeField] bool isRightHanded = true;

        public float WeaponDamage { get => weaponDamage; }
        public float WeaponRange { get => weaponRange; }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab)
            {
                Transform handTransfrom;
                if (isRightHanded) { handTransfrom = rightHand; }
                else { handTransfrom = leftHand; }
                // isRightHanded = true ? handTransfrom = rightHand : handTransfrom = leftHand;
                Instantiate(equippedPrefab, handTransfrom);
            }
            if (animatorOverride)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}