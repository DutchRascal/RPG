using RPG.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        // [SerializeField] Transform target;

        NavMeshAgent navMeshAgent;
        Health health;

        public NavMeshAgent NavMeshAgent { get => navMeshAgent; }

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            NavMeshAgent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void Cancel()
        {
            NavMeshAgent.isStopped = true; ;
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            NavMeshAgent.destination = destination;
            NavMeshAgent.isStopped = false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = NavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}
