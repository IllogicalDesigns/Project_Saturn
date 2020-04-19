using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Legionary : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        private Transform player;
        Vector3 target;

        private enum state { wander, chasing, attacking };
        state currState = state.wander;

        [SerializeField] float chaseDist = 15f, attackDist = 1f, maxWanderDist = 10f, timeInBetweenAttacks = 1f;
        
        float attackTimer = 0f;

        private void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
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
            Vector3 randPoint = Random.insideUnitSphere * wanderDist +transform.position;
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

            if(attackTimer < 0f) {
                //TODO Melee attack
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