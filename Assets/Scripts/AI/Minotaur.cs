using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Minotaur : MonoBehaviour {
      [SerializeField] private NavMeshAgent agent;
      [SerializeField] private Transform player;
      private Vector3 target;
      
      private enum State { Wander, Charging, Recovering };
      State currState = State.Wander;

      [SerializeField] private float chargeDist = 15, attackDist = 1, maxWanderDist = 10;

      private void Start() {
          agent = gameObject.GetComponent<NavMeshAgent>();
      }

      private void Wander() {
          if (Vector3.Distance(transform.position, player.position) < chargeDist)
              currState = State.Charging;

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

          // from randomPos find a nearest point on NavMesh surface in range of maxDistance
          NavMesh.SamplePosition(randPoint, out var hit, wanderDist, NavMesh.AllAreas);
          return hit.position;
      }

      private void Update() {
          switch (currState) {
              case State.Charging:
                  break;
              case State.Recovering:
                  break;
              case State.Wander:
                  Wander();
                  break;
              default:
                  Wander();
                  break;
          }
      }

    }
}