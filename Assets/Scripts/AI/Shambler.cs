using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Shambler : MonoBehaviour {
        [SerializeField] private GameObject meleeDamager;
        private State currState = State.Wandering;

        private float attackTimer;
        private float meleeTimer;
        
        private Vector3 target;
        
        [SerializeField] float chaseDist = 15f, attackDist = 1f, maxWanderDist = 10f, timeInBetweenAttacks = 1f;
        
        private NavMeshAgent agent;
        private Transform player;
        private Animator animator;

        private enum State {
            Wandering,
            Chasing,
            Attacking,
            Meleeing,
            Stumbled,
        }

        private void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
            WanderInDirection();
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
            var pos = player.position;
            transform.LookAt(new Vector3(pos.x, 0, pos.z));
            float dist = Vector3.Distance(transform.position, pos);

            if (dist > attackDist)
                currState = State.Chasing;

            if(attackTimer < 0f && dist < attackDist) {
                EnterMeleeing();
            }
            else {
                attackTimer -= Time.deltaTime;
            }
        }

        private void EnterMeleeing() {
            meleeTimer = 0.3f;
            currState = State.Meleeing;
        }

        private void Meleeing() {
            //TODO pre melee sound
            meleeTimer -= Time.deltaTime;

            if (meleeTimer > 0.1f) return;
            
            if (meleeTimer <= 0.1f && meleeTimer > 0f) {
                meleeDamager.SetActive(true);
            }
            else {
                meleeDamager.SetActive(false);
                attackTimer = timeInBetweenAttacks;
                currState = State.Wandering;
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
                case State.Meleeing:
                    Meleeing();
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
        }

        private void ExitedStumble() {
            currState = State.Wandering;
        }
    }
}
