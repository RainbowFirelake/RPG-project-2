using UnityEngine;

using RPG.Movement;
using RPG.Combat;
using RPG.Core;
using RPG.Resources;


namespace RPG.Control
{
    public class AIContoller : MonoBehaviour
    { 
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float waypointDwellTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [Range(0, 1f)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [Range(0, 1f)]
        [SerializeField] float moveSpeedFractionWhileAttacking = 0.8f;

        public bool showGizmos = true;

        private GameObject player;
        private Fighter fighter;
        private Health health;
        private Mover mover;
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private int currentWaypointIndex = 0;

        private void Start() 
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsPlayerOnAttackRange() && fighter.CanAttack(player))
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player, moveSpeedFractionWhileAttacking);
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoving(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {   
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private bool IsPlayerOnAttackRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        private void OnDrawGizmos() 
        {
            if (showGizmos)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, chaseDistance);
            }
        }
    }
}