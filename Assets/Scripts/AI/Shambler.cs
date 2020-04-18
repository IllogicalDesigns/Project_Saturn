using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Shambler : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform target;
        private ShamblerState currentState = ShamblerState.Standing;

        private enum ShamblerState {
            Standing,
            Attacking,
        }

        private void Standing() {
            agent.isStopped = true;
            currentState = ShamblerState.Standing;
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 5) {
                Attacking();
            }
        }

        private void Attacking() {
            currentState = ShamblerState.Attacking;
            agent.isStopped = false;

            var targetPos = target.position;
            agent.SetDestination(targetPos);

            float distance = Vector3.Distance(transform.position, targetPos);
            if (distance >= 5) {
                Standing();
            }
        }

        void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
            agent.SetDestination(target.position);
        }

        void Update() {
            // agent.SetDestination(target.position);
            // var pos = transform.position;
            // float distance = Vector3.Distance(pos, pos);
            switch (currentState) {
                case ShamblerState.Standing:
                    Standing();
                    break;
                case ShamblerState.Attacking:
                    Attacking();
                    break;
            }
        }
    }
}
