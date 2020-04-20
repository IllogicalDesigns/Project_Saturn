using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Boss {
    public class DianaDriver : MonoBehaviour {

        [SerializeField] private float timeBetweenPhases;
        [SerializeField] private float timeAfterAdds;
        [SerializeField] private Vector3[] returnPositions;

        [SerializeField] private GameObject bite;
        [SerializeField] private float[] chasingDuration;

        [SerializeField] private float[] acceleration;
        [SerializeField] private float[] angularSpeed;
        [SerializeField] private float[] movespeed;

        private GroundDangerSpawner_Follow groundDangerFollow;
        private AddSpawner adds;
        private BulletHellSpawner bulletHell;
        private Health health;
        private NavMeshAgent agent;
        private float phaseTimer;
        private int curDifficulty;
        private bool inStumble;
        private Vector3 chargeDir;
        private GameObject player;
        private Vector3 moveTarget;

        private enum State {
            SpawningAdds,
            SpawningGroundDangerFollow,
            SpawningBulletHell,
            ChasingPlayer,
            MovingToPosition,
            Waiting
        }

        private State state = State.Waiting;
        private State lastState = State.Waiting;
        private State lastStateNonAdds = State.Waiting;

        private void Start() {
            groundDangerFollow = gameObject.GetComponent<GroundDangerSpawner_Follow>();
            adds = gameObject.GetComponent<AddSpawner>();
            bulletHell = gameObject.GetComponent<BulletHellSpawner>();
            health = gameObject.GetComponent<Health>();
            agent = gameObject.GetComponent<NavMeshAgent>();
            UpdateAgent();
            player = GameObject.FindWithTag("Player");
            EnterMoveToPosition();
        }

        private void EnterMoveToPosition() {
            moveTarget = returnPositions[UnityEngine.Random.Range(0, returnPositions.Length)];
            state = State.MovingToPosition;
            agent.SetDestination(moveTarget);
        }

        private void MoveToPosition() {
            agent.isStopped = false;

            if (Vector3.Distance(transform.position, moveTarget) > 1) return;
            EnterWaiting();
        }

        private void EnterWaiting() {
            state = State.Waiting;
            
            // Update difficulty
            if (curDifficulty == 0 && health.PercentHp <= 0.66) {
                curDifficulty = 1;
            }else if (curDifficulty == 1 && health.PercentHp <= 0.33) {
                curDifficulty = 2;
            }
            
            UpdateAgent();

            if (lastState == State.SpawningAdds) {
                phaseTimer = timeAfterAdds;
            }
            else {
                phaseTimer = timeBetweenPhases;
            }
        }

        private void UpdateAgent() {
            agent.speed = movespeed[curDifficulty];
            agent.acceleration = acceleration[curDifficulty];
            agent.angularSpeed = angularSpeed[curDifficulty];
        }

        private void Wait() {
            if (inStumble) return;
            
            if (phaseTimer > 1e-4) {
                phaseTimer -= Time.deltaTime;
                return;
            }

            // Always spawn adds between phases
            if (lastState != State.SpawningAdds) {
                EnterSpawnAdds();
            }
            else {
                EnterFollowState();
            }
        }

        private void EnterFollowState() {
            var states = new List<State>{
                State.SpawningBulletHell, State.ChasingPlayer, State.SpawningGroundDangerFollow
            };
            states.Remove(lastStateNonAdds);
            var nextState = states[UnityEngine.Random.Range(0, states.Count)];

            switch (nextState) {
                case State.SpawningGroundDangerFollow:
                    EnterSpawnGroundDangerFollow();
                    break;
                case State.SpawningBulletHell:
                    EnterSpawnBulletHell();
                    break;
                case State.ChasingPlayer:
                    EnterChasingPlayer();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EnterSpawnGroundDangerFollow() {
            groundDangerFollow.StartSpawning(curDifficulty);
            state = State.SpawningGroundDangerFollow;
            lastState = State.SpawningGroundDangerFollow;
            lastStateNonAdds = State.SpawningGroundDangerFollow;
        }
        
        private void SpawnGroundDangerFollow() {
            agent.isStopped = true;
            if (groundDangerFollow.IsSpawning) return;
            
            EnterMoveToPosition();
        }
        
        private void EnterSpawnAdds() {
            adds.SpawnAdds();
            state = State.SpawningAdds;
            lastState = State.SpawningAdds;
        }
        
        private void SpawnAdds() {
            agent.isStopped = true;
            if (adds.IsSpawning) return;
            
            EnterWaiting();
        }

        private void EnterSpawnBulletHell() {
            bulletHell.StartSpawning(curDifficulty);
            state = State.SpawningBulletHell;
            lastState = State.SpawningBulletHell;
            lastStateNonAdds = State.SpawningBulletHell;
        }
        
        private void SpawnBulletHell() {
            agent.isStopped = true;
            if (bulletHell.IsSpawning) return;
            
            EnterMoveToPosition();
        }

        private void EnterChasingPlayer() {
            state = State.ChasingPlayer;
            lastState = State.ChasingPlayer;
            lastStateNonAdds = State.ChasingPlayer;
            phaseTimer = chasingDuration[curDifficulty];
            bite.SetActive(true);
        }

        private void ChasePlayer() {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);

            if (phaseTimer > 1e-4) {
                phaseTimer -= Time.deltaTime;
            }
            else {
                EnterMoveToPosition();
                bite.SetActive(false);
            }
        }

        private void Update() {
            switch (state) {
                case State.MovingToPosition:
                    MoveToPosition();
                    break;

                case State.SpawningGroundDangerFollow:
                    SpawnGroundDangerFollow();
                    break;
                
                case State.SpawningAdds:
                    SpawnAdds();
                    break;
                
                case State.SpawningBulletHell:
                    SpawnBulletHell();
                    break;
                
                case State.ChasingPlayer:
                    ChasePlayer();
                    break;
                
                case State.Waiting:
                    Wait();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EnteredStumble() {
            inStumble = true;
            agent.isStopped = true;
            groundDangerFollow.StopSpawning();
            bulletHell.StopSpawning();
            EnterWaiting();
        }

        private void ExitedStumble() {
            inStumble = false;
            agent.isStopped = false;
            EnterMoveToPosition();
        }
    }
}