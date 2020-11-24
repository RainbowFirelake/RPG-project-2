using UnityEngine;

using RPG.Combat;

namespace RPG.InteractiveItems
{
    public class PickupItems : MonoBehaviour
    {
        [SerializeField] Weapons weapon;

        public Weapons GetWeapon()
        {
            return weapon;
        }
    }
}
// Это код с курса, может потом пригодиться
// private void OnTriggerEnter(Collider other) {
//     if (other.gameObject.tag == "Player")
//     {
//         other.GetComponent<Fighter>().EquipWeapon(weapon);
//         Destroy(gameObject);
//     }
// }
