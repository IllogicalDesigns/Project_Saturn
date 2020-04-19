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
        [SerializeField] private float shieldBashSpeed = 3f;

        private enum State { Wander, Chasing, Attacking, Bashing, Stumbled };
        State currState = State.Wander;

        [SerializeField] float chaseDist = 15f, attackDist = 1f, maxWanderDist = 10f, timeInBetweenAttacks = 1f;
        
        float attackTimer = 0f;
        private float bashTimer;

        private void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
            WanderInDirection();
            oldShieldPos = shield.localPosition;
            animator = gameObject.GetComponentInChildren<Animator>();
        }

        private void Wander() {
            if (Vector3.Distance(transform.position, player.position) < chaseDist)
                currState = State.Chasing;

            if (Vector3.Distance(transform.position, target) < 1) {
                WanderInDirection();
            }

            Unstopped();
        }

        private void WanderInDirection() {
            target = WanderPoint(maxWanderDist); //Get new wander point origArea
            agent.SetDestination(new Vector3(target.x, transform.position.y, target.z));
        }

        Vector3 WanderPoint(float wanderDist) {
            Vector3 randPoint = Random.insideUnitSphere * wanderDist +transform.position;
            NavMeshHit hit; // NavMesh Sampling Info Container

            // from randomPos find a nearest point on NavMesh surface in range of maxDistance
            NavMesh.SamplePosition(randPoint, out hit, wanderDist, NavMesh.AllAreas);
            return hit.position;
        }

        private void Chasing() {
            if (Vector3.Distance(transform.position, player.position) < attackDist)
                currState = State.Attacking;

            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        

        private void Attacking() {
            Stopped();
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist > attackDist)
                currState = State.Chasing;

            if(attackTimer < 0f && dist < attackDist) {
                EnterBashing();
            }
            else {
                attackTimer -= Time.deltaTime;
            }
        }

        private void EnterBashing() {
            bashTimer = 0.5f;
            currState = State.Bashing;
        }

        private void Bashing() {
            //TODO pre bash sound
            bashTimer -= Time.deltaTime;

            if (bashTimer > 0.4f) return;
            
            if (bashTimer <= 0.4f && bashTimer > 0.2f) {
                shieldDamager.SetActive(true);
                shield.Translate(Vector3.up * (shieldBashSpeed * Time.deltaTime), Space.Self);
            }
            else if (bashTimer <= 0.2f && bashTimer > 0.0f) {
                shieldDamager.SetActive(false);
                shield.Translate(Vector3.down * (shieldBashSpeed * Time.deltaTime), Space.Self);
            }
            else {
                shield.localPosition = oldShieldPos;
                attackTimer = timeInBetweenAttacks + 0.5f;
                currState = State.Wander;
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
                case State.Chasing:
                    Chasing();
                    break;
                case State.Attacking:
                    Attacking();
                    break;
                case State.Bashing:
                    Bashing();
                    break;
                case State.Stumbled:
                    Stumbled();
                    break;
                default:
                    Wander();
                    break;

            }
        }

        private void EnteredStumble() {
            currState = State.Stumbled;
            shieldDamager.SetActive(false);
        }

        private void ExitedStumble() {
            currState = State.Wander;
            shieldDamager.SetActive(true);
        }
    }
}