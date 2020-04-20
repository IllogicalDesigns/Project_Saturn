using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Boss {
    public class MarsDriver : MonoBehaviour {

        [SerializeField] private float timeBetweenPhases;
        [SerializeField] private float timeAfterAdds;
        [SerializeField] private Vector3[] returnPositions;

        [SerializeField] private GameObject pilumPrefab;
        [SerializeField] private float[] pilaInterval;
        [SerializeField] private float[] pilaDuration;
        [SerializeField] private float[] pilumDamage;
        [SerializeField] private float[] pilumSpeed;
        [SerializeField] private Transform firePoint;
        
        [SerializeField] private GameObject deathBeam;
        [SerializeField] private float[] deathBeamDuration;
        [SerializeField] private float[] deathBeamRotationSpeed;
        
        [SerializeField] private float[] movespeed;

        
        private GroundDangerSpawner_Random groundDangerRandom;
        private AddSpawner adds;
        private BulletHellSpawner bulletHell;
        private Health health;
        private NavMeshAgent agent;
        private float phaseTimer;
        private float pilaTimer;
        private int curDifficulty;
        private bool inStumble;
        private Vector3 chargeDir;
        private GameObject player;
        private Vector3 moveTarget;

        private enum State {
            SpawningAdds,
            SpawningGroundDangerRandom,
            SpawningBulletHell,
            ShootingDeathBeam,
            ThrowingPila,
            MovingToPosition,
            Waiting
        }

        private State state = State.Waiting;
        private State lastState = State.Waiting;
        private State lastStateNonAdds = State.Waiting;

        private void Start() {
            groundDangerRandom = gameObject.GetComponent<GroundDangerSpawner_Random>();
            adds = gameObject.GetComponent<AddSpawner>();
            bulletHell = gameObject.GetComponent<BulletHellSpawner>();
            health = gameObject.GetComponent<Health>();
            agent = gameObject.GetComponent<NavMeshAgent>();
            agent.speed = movespeed[0];
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
                agent.speed = movespeed[curDifficulty];
            }else if (curDifficulty == 1 && health.PercentHp <= 0.33) {
                curDifficulty = 2;
                agent.speed = movespeed[curDifficulty];
            }

            if (lastState == State.SpawningAdds) {
                phaseTimer = timeAfterAdds;
            }
            else {
                phaseTimer = timeBetweenPhases;
            }
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
                EnterRandomState();
            }
        }

        private void EnterRandomState() {
            var states = new List<State>{
                State.ShootingDeathBeam, State.SpawningBulletHell, State.ThrowingPila,
                State.SpawningGroundDangerRandom
            };
            states.Remove(lastStateNonAdds);
            var nextState = states[UnityEngine.Random.Range(0, states.Count)];

            switch (nextState) {
                case State.SpawningGroundDangerRandom:
                    EnterSpawnGroundDangerRandom();
                    break;
                case State.SpawningBulletHell:
                    EnterSpawnBulletHell();
                    break;
                case State.ShootingDeathBeam:
                    EnterDeathBeam();
                    break;
                case State.ThrowingPila:
                    EnterThrowPila();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void EnterSpawnGroundDangerRandom() {
            groundDangerRandom.StartSpawning(curDifficulty);
            state = State.SpawningGroundDangerRandom;
            lastState = State.SpawningGroundDangerRandom;
            lastStateNonAdds = State.SpawningGroundDangerRandom;
        }
        
        private void SpawnGroundDangerRandom() {
            agent.isStopped = true;
            if (groundDangerRandom.IsSpawning) return;
            
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
        
        private void EnterDeathBeam() {
            deathBeam.SetActive(true);
            phaseTimer = deathBeamDuration[curDifficulty];
            state = State.ShootingDeathBeam;
            lastState = State.ShootingDeathBeam;
            lastStateNonAdds = State.ShootingDeathBeam;
        }
        
        private void DeathBeam() {
            agent.isStopped = true;
            if (phaseTimer > 1e-4) {
                phaseTimer -= Time.deltaTime;
                transform.Rotate(Vector3.up, deathBeamRotationSpeed[curDifficulty] * Time.deltaTime);
                return;
            }
            
            deathBeam.SetActive(false);
            EnterMoveToPosition();
        }

        private void EnterThrowPila() {
            phaseTimer = pilaDuration[curDifficulty];
            state = State.ThrowingPila;
            lastState = State.ThrowingPila;
            lastStateNonAdds = State.ThrowingPila;
            pilaTimer = 0;
        }

        private void ThrowPila() {
            agent.isStopped = true;
            var position = player.transform.position;
            transform.LookAt(new Vector3(position.x, 0f, position.y));

            if (pilaTimer > 1e-4) {
                pilaTimer -= Time.deltaTime;
            }
            else {
                var pos = firePoint.position;
                var newArrow = Instantiate(pilumPrefab, pos, firePoint.rotation);
                newArrow.transform.LookAt(new Vector3(position.x, transform.position.y,
                    position.z));
                newArrow.SendMessage("SetOwner", -1);
                newArrow.SendMessage("SetDmg", pilumDamage[curDifficulty]);
                newArrow.SendMessage("SetSpeed", pilumSpeed[curDifficulty]);
                pilaTimer = pilaInterval[curDifficulty];
            }

            if (phaseTimer > 1e-4) {
                phaseTimer -= Time.deltaTime;
            }
            else {
                EnterMoveToPosition();
            }
            
        }

        private void Update() {
            switch (state) {
                case State.MovingToPosition:
                    MoveToPosition();
                    break;
                
                case State.ThrowingPila:
                    ThrowPila();
                    break;
                
                case State.SpawningGroundDangerRandom:
                    SpawnGroundDangerRandom();
                    break;
                
                case State.SpawningAdds:
                    SpawnAdds();
                    break;
                
                case State.SpawningBulletHell:
                    SpawnBulletHell();
                    break;
                
                case State.ShootingDeathBeam:
                    DeathBeam();
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
            groundDangerRandom.StopSpawning();
            bulletHell.StopSpawning();
            deathBeam.SetActive(false);
            EnterWaiting();
        }

        private void ExitedStumble() {
            inStumble = false;
        }
    }
}