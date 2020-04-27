using UnityEngine;

namespace RPG.Control
{
    public class AIControler : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        private void Update()
        {
            if (DistanceToPlayer() < chaseDistance)
            {
                print(gameObject.name + ": Should chase: ");
            }
        }

        private float DistanceToPlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return Vector3.Distance(transform.position, player.transform.transform.position);
        }
    }
}