using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Fighter fighter;
        Mover mover;

        void Update()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            if (InteractWithCombat()) { return; }
            if (InteractWithMovement()) { return; }
            print("Nothing to do!");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (!target) { continue; }
                GameObject targetGameObject = target.gameObject;
                if (!fighter.CanAttack(targetGameObject)) { continue; }
                if (Input.GetMouseButton(0))
                {
                    fighter.Attack(targetGameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            if (GetComponent<Health>().IsDead()) { return false; }
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point);
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
