using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Archer : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform player;
        Vector3 target;

        Vector3 origArea;

        private enum state {
            wander,
            chasing,
            attacking
        };

        state currState = state.wander;

        [SerializeField] float chaseDist = 15f, attackDist = 10f, maxWanderDist = 10f, timeInBetweenAttacks = 1f;

        [SerializeField] GameObject arrowPrefab;
        [SerializeField] Transform firePoint;

        float spread = 0.15f;

        float attackTimer = 0f;

        private void Start() {
            origArea = transform.position;
            agent = gameObject.GetComponent<NavMeshAgent>();
            WanderInDirection();
        }

        private void Wander() {
            if (Vector3.Distance(transform.position, player.position) < chaseDist)
                currState = state.chasing;

            if (Vector3.Distance(transform.position, target) < 1) {
                WanderInDirection();
            }

            agent.isStopped = false;
        }

        private void WanderInDirection() {
            target = wanderPoint(maxWanderDist); //Get new wander point origArea
            agent.SetDestination(new Vector3(target.x, transform.position.y, target.z));
        }

        Vector3 wanderPoint(float wanderDist) {
            Vector3 randPoint = Random.insideUnitSphere * wanderDist + origArea;
            NavMeshHit hit; // NavMesh Sampling Info Container

            // from randomPos find a nearest point on NavMesh surface in range of maxDistance
            NavMesh.SamplePosition(randPoint, out hit, wanderDist, NavMesh.AllAreas);
            return hit.position;
        }

        private void Chasing() {
            if (Vector3.Distance(transform.position, player.position) < attackDist)
                currState = state.attacking;

            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        private void Attacking() {
            agent.isStopped = true;

            if (Vector3.Distance(transform.position, player.position) > attackDist)
                currState = state.chasing;

            if (attackTimer < 0f) {
                Vector3 pos = (firePoint.position + (firePoint.right * Random.Range(-spread, spread)));
                GameObject newArrow = Instantiate(arrowPrefab, pos, firePoint.rotation) as GameObject;
                newArrow.transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                newArrow.SendMessage("SetOwner", -1); //TODO fucking remove hardcode
                newArrow.SendMessage("SetDmg", 25); //TODO this is not sexy
                newArrow.SendMessage("SetSpeed", 15);
                attackTimer = timeInBetweenAttacks;
            }
            else {
                attackTimer -= Time.deltaTime;
            }
        }

        private void Update() {
            switch (currState) {
                case state.chasing:
                    Chasing();
                    break;
                case state.attacking:
                    Attacking();
                    break;
                default:
                    Wander();
                    break;

            }
        }
    }
}