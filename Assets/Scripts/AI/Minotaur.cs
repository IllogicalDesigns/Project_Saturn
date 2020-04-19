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
      [SerializeField] private float chargeForce;
      Vector3 target;
      private Vector3 chargeDir;
      private State currState = State.Wander;
      private float baseSpeed, knockBackForce = 40f;
      private Rigidbody rb;
      
      private enum State { Wander, Preparing, Charging, Recovering, Stumbled };

      private void Start() {
          agent = gameObject.GetComponent<NavMeshAgent>();
          rb = GetComponent<Rigidbody>();
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
          rb.velocity = new Vector3(0, 0, 0);
          StartCoroutine(PrepareToCharge());
      }
      
      private void Charging() {
          agent.isStopped = true;
          rb.AddForce(chargeDir * chargeForce);
      }
      
      private IEnumerator PrepareToCharge() {
          yield return new WaitForSeconds(0.5f);
          agent.isStopped = false;
          var position = player.position;
          chargeDir = (position - transform.position).normalized;
          transform.LookAt(position);
          currState = State.Charging;
      }

      private IEnumerator RecoverFromCharge() {
          agent.isStopped = true;
          agent.speed = baseSpeed;
          rb.velocity = new Vector3(0, 0, 0);
          yield return new WaitForSeconds(0.3f);
          currState = (Vector3.Distance(transform.position, player.position) < chargeDist) 
                    ? State.Preparing : State.Wander;
      }
      
      private void OnCollisionEnter(Collision other) {
          if (currState != State.Charging) return;
          currState = State.Recovering;
              
          if (!other.gameObject.CompareTag("Player")) return;
          
          other.gameObject.SendMessage("ApplyDamage", attackDamageDealt);
          var pos = transform.position;
          other.gameObject.SendMessage("ApplyKnockback", new Vector4(pos.x, pos.y, pos.z, knockBackForce));
      }

      private void Recovering() {
          StartCoroutine(RecoverFromCharge());
      }

      private void Stumbled() {
          agent.isStopped = true;
          rb.velocity = new Vector3(0, 0, 0);
      }
      
      private void FixedUpdate() {
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
          currState = State.Wander;
      }
      

    }
}