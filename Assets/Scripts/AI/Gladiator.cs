using Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AI {
    public class Gladiator : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        private Transform player;
        Vector3 target;
        Vector3 oldShieldPos;
        [SerializeField] private GameObject meleeDamager;
        [SerializeField] private Animator animator;
        [SerializeField] private float dodgeForce;
        [SerializeField] private float dodgeCooldown;

        private enum State { Wander, Chasing, Attacking, Meleeing, Stumbled };
        State currState = State.Wander;

        [SerializeField] float chaseDist = 15f, attackDist = 1f, maxWanderDist = 10f, timeInBetweenAttacks = 1f;

        private Rigidbody rb;
        float attackTimer = 0f;
        private float meleeTimer;
        private float dodgeTimer;
        private bool dodging;
        private Vector3 dodgeVector;

        private void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
            rb = gameObject.GetComponent<Rigidbody>();
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
            meleeTimer = 0.15f;
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
            if (dodgeTimer > 1e-4) {
                dodgeTimer -= Time.deltaTime;
            }
            
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

        private void FixedUpdate() {
            if (!dodging) return;
            
            rb.AddForce(dodgeVector, ForceMode.Impulse);
            dodging = false;
        }

        private void EnteredStumble() {
            currState = State.Stumbled;
        }

        private void ExitedStumble() {
            currState = State.Wander;
        }

        private void OnTriggerEnter(Collider other) {
            if (dodgeTimer > 1e-4 || currState == State.Stumbled ||
                !other.TryGetComponent(typeof(Bullet), out var comp) || !(comp is Bullet bullet) ||
                bullet.owner != 1) return;
            
            Debug.Log("Dodging");

            var trans = transform;
            var localBullet = trans.InverseTransformPoint(other.transform.position);
            dodgeTimer = dodgeCooldown;
            dodgeVector = trans.right * (-Mathf.Sign(localBullet.x) * dodgeForce);
            dodging = true;
        }
    }
}