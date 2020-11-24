using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        public float drawGizmosRadius = 0.1f;
        private void OnDrawGizmos() 
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWaypointIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), drawGizmosRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextWaypointIndex(int i)
        {
            if (i == transform.childCount - 1) return 0;
            return i + 1;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}