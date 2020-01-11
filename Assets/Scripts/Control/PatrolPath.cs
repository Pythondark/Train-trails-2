using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] [Range(0, 2)] float wayPointSize = 0.2f;


        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                if (i == 0)
                {
                    Gizmos.color = Color.green;
                }
                else if (i == transform.childCount - 1)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.grey;
                }
                Gizmos.DrawSphere(GetWaypoint(i), wayPointSize);
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if ((i + 1) == transform.childCount)
            {
                return 0;
            }
            else
            {
                return i + 1;
            }

        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
