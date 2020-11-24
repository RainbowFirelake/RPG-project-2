using UnityEngine;
using UnityEngine.AI;

using RPG.Core;
using RPG.Saving;
using RPG.Resources;


namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 5.66f;

        private NavMeshAgent navMeshAgent;
        private Ray lastRay;
        private Health health;
        private ActionScheduler actionScheduler;
        
        private void Start()
        {
            health = GetComponent<Health>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            navMeshAgent.enabled = !health.IsDead();
            AnimationMove();
        }

        public void StartMoving(Vector3 destination, float speedFraction)
        {
            actionScheduler.ActionStart(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void AnimationMove()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            MoverSaveable data = new MoverSaveable();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveable data = (MoverSaveable)state;
            //navMeshAgent.enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            //navMeshAgent.enabled = true;
        }

        [System.Serializable]
        struct MoverSaveable
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation; 
        }
    }   
}