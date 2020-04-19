using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Shambler : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        private State currentState = State.Standing;
        
        private Transform player;

        private enum State {
            Standing,
            Attacking,
            Stumbled,
        }

        private void Standing() {
            agent.isStopped = true;
            currentState = State.Standing;
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < 5) {
                Attacking();
            }
        }

        private void Attacking() {
            currentState = State.Attacking;
            agent.isStopped = false;

            var targetPos = player.position;
            agent.SetDestination(targetPos);

            float distance = Vector3.Distance(transform.position, targetPos);
            if (distance >= 5) {
                Standing();
            }
        }

        private void Stumbled() {
            agent.isStopped = true;
        }

        void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
            agent.SetDestination(player.position);
        }

        void Update() {
            // agent.SetDestination(target.position);
            // var pos = transform.position;
            // float distance = Vector3.Distance(pos, pos);
            switch (currentState) {
                case State.Standing:
                    Standing();
                    break;
                case State.Attacking:
                    Attacking();
                    break;
                case State.Stumbled:
                    Stumbled();
                    break;
            }
        }

        private void EnteredStumble() {
            currentState = State.Stumbled;
        }

        private void ExitedStumble() {
            currentState = State.Standing;
        }
    }
}
