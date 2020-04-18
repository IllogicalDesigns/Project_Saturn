using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Enemy : MonoBehaviour {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform target;

        private void Start() {
            agent = gameObject.GetComponent<NavMeshAgent>();
        }

        private void Update() {
            agent.SetDestination(target.position);
        }
    }
}