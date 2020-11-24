using UnityEngine;

using RPG.Combat;
using RPG.Movement;
using RPG.InteractiveItems;
using RPG.Resources;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float distanceOfPickuping = 1f;

        private Health health;
        private Fighter fighter;
        private Mover mover;
        private Pickuper pickuper;

        private void Start() 
        {
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            pickuper = GetComponent<Pickuper>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (PickupInteract()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!fighter.CanAttack(target.gameObject)) 
                {
                    continue;
                }
                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(target.gameObject, 1f);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoving(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private bool PickupInteract()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                PickupItems item = hit.transform.GetComponent<PickupItems>();
                if (item == null) continue;

                if (Input.GetMouseButton(0))
                {
                    // if (!GetComponent<Pickuper>().CanPickup())
                    // {
                    //     print("can't pickup");
                    //     continue;
                    // }
                    if (Input.GetMouseButton(0))
                    {
                        pickuper.StartPickupAction(item.gameObject, 1.0f);
                    }
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
