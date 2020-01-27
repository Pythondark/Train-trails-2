using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Combat;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] [Range(0, 1)] float patrolSpeedFraction = 0.2f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float chaseDistance = 5f;

        [SerializeField] bool chaseDistanceGizmo = true;


        Mover mover;
        Health health;
        GameObject player;
        Fighter fighter;

        Vector3 guardPosition;
        Vector3 lastKnownPlayerPosition;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                // Attack state
                AttackBehaviour();
            }
         
            UpdateTimers();
        }

        private void AttackBehaviour()
        {
            //timeSinceLastSawPlayer = 0f;
            fighter.AttackPlayer(player);
            lastKnownPlayerPosition = player.transform.position;
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
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }




        private bool AtWaypoint()
        {

            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
        }




        private void UpdateTimers()
        {
            //timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            if (chaseDistanceGizmo)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, chaseDistance);
            }            
        }

    }
    
}

