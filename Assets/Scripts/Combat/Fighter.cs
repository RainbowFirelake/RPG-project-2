using UnityEngine;

using RPG.Movement;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapons defaultWeapon = null;

        private float timeSinceLastAttack = Mathf.Infinity;
        private float moveSpeedFractionWhenAttacking = 1f;
        private Health target;
        private Animator animator;
        private Mover mover;
        private ActionScheduler actionScheduler;
        private Weapons currentWeapon;

        private void Start() 
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead()) return;
            if (target.gameObject.tag == "Player" && gameObject.tag == "Player") return;
            if (!CheckingAttackDistance())
            {
                mover.MoveTo(target.transform.position, moveSpeedFractionWhenAttacking);
            }
            else
            {
                mover.Cancel();
                AttackHit();
            }
        }

        private bool CheckingAttackDistance()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetAttackDistance();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health testTarget = combatTarget.GetComponent<Health>();
            return !testTarget.IsDead() && testTarget != null;
        }

        public void Attack(GameObject combatTarget, float speedFraction)
        {
            moveSpeedFractionWhenAttacking = speedFraction;
            actionScheduler.ActionStart(this);
            target = combatTarget.GetComponent<Health>();
        }

        void Hit()
        {
            if (target == null) return;
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, 
                    leftHandTransform, target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject, currentWeapon.GetDamage());
            }
            
        }

        void Shoot()
        {
            Hit();
        }

        public void AttackHit()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > currentWeapon.GetTimeBetweenAttacks())
            {
                attackAnimationReset();
                timeSinceLastAttack = 0;
            }
        }

        private void attackAnimationReset()
        {
            animator.ResetTrigger("stopAttacking");
            animator.SetTrigger("attack");
        }

        public void Cancel()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttacking");
            target = null;
            mover.Cancel();
        }  

        public void EquipWeapon(Weapons weapon)
        {
            currentWeapon = weapon;
            currentWeapon.Equip(rightHandTransform, leftHandTransform, animator);
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
            string weaponName = (string)state;
            Weapons weapon = UnityEngine.Resources.Load<Weapons>(weaponName); 
            EquipWeapon(weapon);
        }
    }
}