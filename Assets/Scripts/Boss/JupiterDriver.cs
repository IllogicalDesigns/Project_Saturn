using System;
using System.Collections.Generic;
using UnityEngine;

namespace Boss {
    public class JupiterDriver : MonoBehaviour {

        [SerializeField] private float timeBetweenPhases;
        [SerializeField] private float timeAfterAdds;
        
        [SerializeField] private GameObject deathBeam;
        [SerializeField] private float deathBeamDuration;
        
        private GroundDangerSpawner_Follow groundDangerFollower;
        private GroundDangerSpawner_Random groundDangerRandom;
        private AddSpawner adds;
        private BulletHellSpawner bulletHell;
        private float phaseTimer;

        private enum State {
            SpawningAdds,
            SpawningGroundDangerFollower,
            SpawningGroundDangerRandom,
            SpawningBulletHell,
            ShootingDeathBeam,
            Waiting
        }

        private State state = State.Waiting;
        private State lastState = State.Waiting;

        private void Start() {
            groundDangerFollower = gameObject.GetComponent<GroundDangerSpawner_Follow>();
            groundDangerRandom = gameObject.GetComponent<GroundDangerSpawner_Random>();
            adds = gameObject.GetComponent<AddSpawner>();
            bulletHell = gameObject.GetComponent<BulletHellSpawner>();
            EnterWaiting();
        }

        private void EnterWaiting() {
            state = State.Waiting;

            if (lastState == State.SpawningAdds) {
                phaseTimer = timeAfterAdds;
            }
            else {
                phaseTimer = timeBetweenPhases;
            }
        }

        private void Wait() {
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
                State.ShootingDeathBeam, State.SpawningBulletHell, State.SpawningGroundDangerFollower,
                State.SpawningGroundDangerRandom
            };
            var nextState = states[UnityEngine.Random.Range(0, states.Count)];

            switch (nextState) {
                case State.SpawningGroundDangerFollower:
                    EnterSpawnGroundDangerFollower();
                    break;
                case State.SpawningGroundDangerRandom:
                    EnterSpawnGroundDangerRandom();
                    break;
                case State.SpawningBulletHell:
                    EnterSpawnBulletHell();
                    break;
                case State.ShootingDeathBeam:
                    EnterDeathBeam();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void EnterSpawnGroundDangerFollower() {
            groundDangerFollower.StartSpawning();
            state = State.SpawningGroundDangerFollower;
            lastState = State.SpawningGroundDangerFollower;
        }
        
        private void SpawnGroundDangerFollower() {
            if (groundDangerFollower.IsSpawning) return;
            
            EnterWaiting();
        }
        
        private void EnterSpawnGroundDangerRandom() {
            groundDangerRandom.StartSpawning();
            state = State.SpawningGroundDangerRandom;
            lastState = State.SpawningGroundDangerRandom;
        }
        
        private void SpawnGroundDangerRandom() {
            if (groundDangerRandom.IsSpawning) return;
            
            EnterWaiting();
        }
        
        private void EnterSpawnAdds() {
            adds.SpawnAdds();
            state = State.SpawningAdds;
            lastState = State.SpawningAdds;
        }
        
        private void SpawnAdds() {
            if (adds.IsSpawning) return;
            
            EnterWaiting();
        }

        private void EnterSpawnBulletHell() {
            bulletHell.StartSpawning();
            state = State.SpawningBulletHell;
            lastState = State.SpawningBulletHell;
        }
        
        private void SpawnBulletHell() {
            if (bulletHell.IsSpawning) return;
            
            EnterWaiting();
        }
        
        private void EnterDeathBeam() {
            deathBeam.SetActive(true);
            phaseTimer = deathBeamDuration;
            state = State.ShootingDeathBeam;
            lastState = State.ShootingDeathBeam;
        }
        
        private void DeathBeam() {
            if (phaseTimer > 1e-4) {
                phaseTimer -= Time.deltaTime;
                return;
            }
            
            deathBeam.SetActive(false);
            EnterWaiting();
        }

        private void Update() {
            switch (state) {
                case State.SpawningGroundDangerFollower:
                    SpawnGroundDangerFollower();
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
    }
}