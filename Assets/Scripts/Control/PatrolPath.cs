using UnityEngine;

namespace RPG.Control
{
    using UnityEngine;

    public class PatrolPath : MonoBehaviour
    {
        const float waypointGizmoRadius = 0.3f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawSphere(transform.GetChild(i).position, waypointGizmoRadius);
            }
        }
    }
}