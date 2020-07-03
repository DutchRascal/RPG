using System;
using RPG.Combat;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Fighter fighter;
        Mover mover;
        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavNeshProjectionDistance = 1f;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
        }

        private void Start()
        {
            SetCursor(CursorType.None);
        }

        void Update()
        {
            if (InteractWithUI()) { return; }
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) { return; }
            if (InteractWithMovement()) { return; }
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            print(hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            if (GetComponent<Health>().IsDead()) { return false; }
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            print("hasHit: " + hasHit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) { return false; }
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavNeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) { return false; }
            target = navMeshHit.position;
            return true;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }
    }
}
