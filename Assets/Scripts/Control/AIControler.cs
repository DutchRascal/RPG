using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIControler : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        GameObject player;
        Fighter fighter;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            if (!GetComponent<NavMeshAgent>().isStopped)
            {
                fighter.Cancel();
            }

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
            }
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }
}