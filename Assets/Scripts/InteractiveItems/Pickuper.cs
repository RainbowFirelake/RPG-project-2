using UnityEngine;

using RPG.Core;
using RPG.Combat;
using RPG.Movement;

namespace RPG.InteractiveItems
{
    public class Pickuper : MonoBehaviour, IAction
    {
        [SerializeField] float distanceOfPickuping;

        public PickupItems weaponToPickup = null;
        private Mover mover;
        private Fighter fighter;
        private ActionScheduler actionScheduler;

        private void Start() 
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update() {
            if (weaponToPickup == null) return;
            if (IsInDistanceOfPickuping())
            {
                mover.Cancel();
                fighter.EquipWeapon(weaponToPickup.GetWeapon());
                Destroy(weaponToPickup.gameObject);
                weaponToPickup = null;
            }
            else 
            {
                mover.MoveTo(weaponToPickup.transform.position, 1f);
            }
        }

        public void StartPickupAction(GameObject weapon, float speedFraction)
        {
            actionScheduler.ActionStart(this);
            weaponToPickup = weapon.GetComponent<PickupItems>();
        }

        public void Cancel()
        {
            weaponToPickup = null;
            mover.Cancel();
        }

        public bool IsInDistanceOfPickuping()
        {
            return Vector3.Distance(transform.position, weaponToPickup.transform.position) < distanceOfPickuping;
        }

        // public bool CanPickup()
        // {
        //     return weaponToPickup != null;
        // }
    }
}
