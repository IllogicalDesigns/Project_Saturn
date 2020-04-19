using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Legionary : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        private Transform player;
        Vector3 target;
        Vector3 oldShieldPos;
        [SerializeField] private Transform shield;
        [SerializeField] private GameObject shieldDamager;
        [SerializeField] private Animator animator;

        private enum state { wander, chasing, attacking, stumbled };
        state currState = state.wander;

        [SerializeField] float chaseDist = 15f, attackDist = 1f, maxWanderDist = 10f, timeInBetweenAttacks = 1f;
        
        float attackTimer = 0f;

        private void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
            WanderInDirection();
            oldShieldPos = shield.localPosition;
            animator = gameObject.GetComponentInChildren<Animator>();
        }

        private void Wander() {
            if (Vector3.Distance(transform.position, player.position) < chaseDist)
                currState = state.chasing;

            if (Vector3.Distance(transform.position, target) < 1) {
                WanderInDirection();
            }

            Unstopped();
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

        IEnumerator ShieldBash() {
            //TODO pre bash sound
            yield return new WaitForSeconds(0.1f);
            shield.localPosition = shield.transform.forward * 0.5f;
            yield return new WaitForSeconds(0.3f);
            shield.localPosition = oldShieldPos;
        }

        private void Chasing() {
            if (Vector3.Distance(transform.position, player.position) < attackDist)
                currState = state.attacking;

            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        

        private void Attacking() {
            Stopped();
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > attackDist)
                currState = state.chasing;

            if(attackTimer < 0f && dist < 4f) {
                StartCoroutine(ShieldBash());
                attackTimer = timeInBetweenAttacks + 0.4f;
            }
            else {
                attackTimer -= Time.deltaTime;
            }
        }

        private void Stumbled() {
            Stopped();
        }

        private void Stopped() {
            agent.isStopped = true;
            animator.SetBool("Walking", false);
        }

        private void Unstopped() {
            agent.isStopped = false;
            animator.SetBool("Walking", true);
        }

        private void Update() {
            switch (currState) {
                case state.chasing:
                    Chasing();
                    break;
                case state.attacking:
                    Attacking();
                    break;
                case state.stumbled:
                    Stumbled();
                    break;
                default:
                    Wander();
                    break;

            }
        }

        private void EnteredStumble() {
            currState = state.stumbled;
            shieldDamager.SetActive(false);
        }

        private void ExitedStumble() {
            currState = state.wander;
            shieldDamager.SetActive(true);
        }
    }
}