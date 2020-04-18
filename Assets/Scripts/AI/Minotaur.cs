using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Minotaur : MonoBehaviour {
      [SerializeField] private NavMeshAgent agent;
      [SerializeField] private Transform player;
      private Vector3 target;
      

      private enum State { Wander, Chasing, Charging };
      State currState = State.Wander;

      [SerializeField] private float chaseDist = 15, attackDist = 1, maxWanderDist = 10, timeInBetweenAttacks = 1;

      private void Start() {
          agent = gameObject.GetComponent<NavMeshAgent>();
      }

      private void Wander() {
          if (Vector3.Distance(transform.position, player.position) < chaseDist)
              currState = State.Chasing;

          if (Vector3.Distance(transform.position, target) < 1) {
              WanderInDirection();
          }

          agent.isStopped = false;
      }

      private void WanderInDirection() {
          target = WanderPoint(maxWanderDist);
      }
      
      Vector3 WanderPoint(float wanderDist) {
          Vector3 randPoint = Random.insideUnitSphere* wanderDist +transform.position;
          NavMeshHit hit; // NavMesh Sampling Info Container

          // from randomPos find a nearest point on NavMesh surface in range of maxDistance
          NavMesh.SamplePosition(randPoint, out hit, wanderDist, NavMesh.AllAreas);
          return hit.position;
      }

    }
}