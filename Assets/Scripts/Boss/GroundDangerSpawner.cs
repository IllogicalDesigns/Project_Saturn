using System;
using UnityEngine;

namespace Boss {
    public abstract class GroundDangerSpawner : MonoBehaviour {

        [SerializeField] private GameObject groundDangerPrefab;
        [SerializeField] private float[] spawnInterval;

        [SerializeField] private float[] delay;
        [SerializeField] private int[] damage;
        [SerializeField] private int[] totalGroundDangersToSpawn;

        protected GameObject Player;
        private int numberToSpawn;
        private float intervalTracker;
        private int curDifficulty;

        public bool IsSpawning => numberToSpawn > 0;
        
        public void StartSpawning(int difficulty) {
            curDifficulty = difficulty;
            numberToSpawn = totalGroundDangersToSpawn[curDifficulty];
        }
        
        public void StopSpawning() {
            numberToSpawn = 0;
        }

        private void Start() {
            Player = GameObject.FindWithTag("Player");
        }

        private void Update() {
            if (!IsSpawning) return;
            
            if (intervalTracker > 1e-4) {
                intervalTracker -= Time.deltaTime;
            }
            else {
                DoSpawn();
            }
        }

        protected abstract void DoSpawn();

        protected void DoSpawn(Vector3 position) {
            var newDanger = Instantiate(groundDangerPrefab);
            newDanger.transform.position = position;
            
            var groundDanger = newDanger.GetComponent<GroundDanger>();
            groundDanger.delay = delay[curDifficulty];
            groundDanger.damage = damage[curDifficulty];

            intervalTracker = spawnInterval[curDifficulty];
            numberToSpawn--;
        }
    }
}