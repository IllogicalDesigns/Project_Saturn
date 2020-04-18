using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace AI {
    public class Minotaur : MonoBehaviour {
      [SerializeField] private NavMeshAgent agent;
      [SerializeField] private Transform player;
      [SerializeField] private float chargeDist = 15f, maxWanderDist = 10f, attackDamageDealt = 20f;
      Vector3 target;
      private State currState = State.Wander;
      private float baseSpeed, knockBackForce = 40f;
      
      private enum State { Wander, Preparing, Charging, Recovering };

      private void Start() {
          agent = gameObject.GetComponent<NavMeshAgent>();
          baseSpeed = agent.speed;
          WanderInDirection();
      }

      private void Wander() {
          if (Vector3.Distance(transform.position, player.position) < chargeDist) {
              currState = State.Preparing;
              return;
          }

          if (Vector3.Distance(transform.position, target) < 1) {
              WanderInDirection();
          }

          agent.isStopped = false;
      }

      private void WanderInDirection() {
          target = WanderPoint(maxWanderDist);
          agent.SetDestination(new Vector3(target.x, transform.position.y, target.z));
      }
      
      Vector3 WanderPoint(float wanderDist) {
          Vector3 randPoint = Random.insideUnitSphere * wanderDist + transform.position;

          // from randomPos find a nearest point on NavMesh surface in range of maxDistance
          NavMesh.SamplePosition(randPoint, out var hit, wanderDist, NavMesh.AllAreas);
          return hit.position;
      }

      private void Preparing() {
          agent.isStopped = true;
          StartCoroutine(PrepareToCharge());
          target = player.position;
          agent.SetDestination(target);
          currState = State.Charging;
      }
      
      private void Charging() {
          agent.isStopped = false;
          agent.speed *= 3;
          if (Vector3.Distance(transform.position, target) < 1) {
              currState = State.Recovering;
          }
      }

      private IEnumerator PrepareToCharge() {
          yield return new WaitForSeconds(1);
      }

      private IEnumerator RecoverFromCharge() {
          yield return new WaitForSeconds(3);
      }
      
      private void OnCollisionEnter(Collision other) {
          if (!other.gameObject.CompareTag("Player")) return;
          if (currState != State.Charging) return;
          
          other.gameObject.SendMessage("ApplyDamage", attackDamageDealt);
          other.gameObject.SendMessage("ApplyKnockback", new Vector4(transform.position.x, transform.position.y, transform.position.z, knockBackForce));
          currState = State.Recovering;
      }

      /*private void ApplyKnockback(Vector4 pos) {
          var posFrom = new Vector3(pos.x, pos.y, pos.z);
          var force = pos.w;
          var dir = (posFrom - transform.position).normalized;
          var ridge = GetComponent<Rigidbody>();
          ridge.AddForce(-dir * force, ForceMode.Impulse);
      }*/

      private void Recovering() {
          agent.isStopped = true;
          agent.speed = baseSpeed;
          StartCoroutine(RecoverFromCharge());
          currState = State.Wander;
      }
      
      private void Update() {
          switch (currState) {
              case State.Charging:
                  Charging();
                  break;
              case State.Recovering:
                  Recovering();
                  break;
              case State.Preparing:
                  Preparing();
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