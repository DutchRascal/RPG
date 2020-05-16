using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using System.Collections.Generic;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;

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

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            NavMeshAgent.destination = destination;
            NavMeshAgent.speed = maxSpeed * (Mathf.Clamp01(speedFraction));
            NavMeshAgent.isStopped = false;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = NavMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }


        /*
    public object CaptureState()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["position"] = new SerializableVector3(transform.position);
        data["rotation"] = new SerializableVector3(transform.eulerAngles);
        return data;
        
    }

        public void RestoreState(object state)
        {
            /*
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            
    }
    */
        /*   [System.Serializable]
           struct MoverSaveData
           {
               public SerializableVector3 position;
               public SerializableVector3 rotation;
           }

           public object CaptureState()
           {
               MoverSaveData data = new MoverSaveData();
               data.position = new SerializableVector3(transform.position);
               data.rotation = new SerializableVector3(transform.eulerAngles);
               return data;
           }

           public void RestoreState(object state)
           {
               MoverSaveData data = (MoverSaveData)state;
               transform.position = ((SerializableVector3)data.position).ToVector();
               transform.eulerAngles = ((SerializableVector3)data.rotation).ToVector();
           }
           */


    }

}
