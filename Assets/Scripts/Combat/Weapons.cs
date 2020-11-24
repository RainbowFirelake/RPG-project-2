using UnityEngine;

using RPG.Resources;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapons : ScriptableObject
    {
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float attackDistance;
        [SerializeField] float damage;
        [SerializeField] float timeBetweenAttacks;
        [SerializeField] bool leftHanded;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public void Equip(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(weaponPrefab, handTransform);
                weapon.name = weaponName;
            }
            
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find("Weapon");
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find("Weapon");
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (leftHanded)
            {
                handTransform = leftHand;
            }
            else
            {
                handTransform = rightHand;
            }

            return handTransform;
        }

        private bool IsWeaponLeftHanded()
        {
            return leftHanded;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator)
        {
            Projectile projectileInstance = Instantiate(projectile, 
                GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, damage);
        }

        public float GetAttackDistance() 
        {
            return attackDistance;
        }

        public float GetDamage()
        {
            return damage;
        }

        public float GetTimeBetweenAttacks()
        {
            return timeBetweenAttacks;
        }
    }
}
